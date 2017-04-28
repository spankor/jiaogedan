from PyQt5 import QtWidgets
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from PyQt5.QtGui import *
from jiaogedan import *
import lscjcx
import login_ui
import sys
import os
import random
from functools import wraps

styleTypes = ('yq', 'js', 'zlc')
maiTypes = ('buy', 'sell')
kpTypes = ('kai', 'ping')
onStyle = "QPushButton{border-image:url(:/src/src/on.png);}"
offStyle = "QPushButton{border-image:url(:/src/src/off.png);}"
sellgreen = "rgb(76,170,7)"
buyred = "rgb(255,0,0)"
numgreen = "rgb(76,170,7)"
numred = "rgb(255,0,0)"
sizeIcon = ":/src/src/sizeSelected.png"
historyFile = "hist.db"

Pay = True
NeedLogin = False

def ignore_exceptions(func):
    @wraps(func)
    def _call(*args, **kwargs):
        try:
            result = func(*args, **kwargs)
        except Exception as e:
            print(e)

class LoginDialog(QDialog):
    loginState = pyqtSignal(int)
    def __init__(self, parent=None):
        super(LoginDialog, self).__init__(parent)
        self.ui = login_ui.Ui_Form()
        self.ui.setupUi(self)
        self.flag = 0
        self.ui.loginBtn.clicked.connect(self.login)
        
    def login(self):
        if self.ui.usernameEdt.text() == 'abc' and self.ui.passwordEdt.text() == 'qwezxc':
            self.flag = 1
            self.close()
        if self.ui.usernameEdt.text() == 'admin' and self.ui.passwordEdt.text() == 'corypi':
            self.flag = 1
            global Pay
            Pay = False
            self.close()
            
    def closeEvent(self, e):
        self.loginState.emit(self.flag)
        e.accept()

class LishiChengjiaoChaxun(QDialog):
    def __init__(self, parent=None):
        super(LishiChengjiaoChaxun, self).__init__(parent)
        self.ui = lscjcx.Ui_lscjcx()
        self.ui.setupUi(self)
        self.setWindowFlags(self.windowFlags()& ~Qt.WindowMaximizeButtonHint& ~Qt.WindowMinimizeButtonHint)
        self.ui.cjTbl.setShowGrid(False)
        self.cjMenu = QMenu()
        self.cjMenuAddAction = QAction("添加行", self)
        self.cjMenuAddAction.triggered.connect(self.cjTblAddARow)
        self.cjMenuDeleteAction = QAction("删除行", self)
        self.cjMenuDeleteAction.triggered.connect(self.cjTblDeleteARow)
        self.cjMenu.addAction(self.cjMenuAddAction)
        self.cjMenu.addAction(self.cjMenuDeleteAction)
        self.ui.cjTbl.customContextMenuRequested.connect(self.openCjTblMenu)

    def cjTblDeleteARow(self):
        currentRow = self.ui.cjTbl.currentRow()
        if currentRow < 0:
            return
        self.ui.cjTbl.removeRow(currentRow)
        self.updateStyleOfBdtbl(self.ui.cjTbl, currentRow)
    
    def cjTblAddARow(self):
        countRow = self.ui.cjTbl.rowCount()
        self.ui.cjTbl.insertRow(countRow)
        self.ui.cjTbl.setRowHeight(countRow, 21)
        for cln in range(self.ui.cjTbl.columnCount()):
            self.ui.cjTbl.setItem(countRow, cln, self.getTableItem(countRow, ""))
    
    def openCjTblMenu(self, pos):
        pos += self.ui.cjTbl.pos()
        self.cjMenu.exec_(self.mapToGlobal(pos))

    def updateStyleOfBdtbl(self, table, currentRow):
        for row in range(currentRow, table.rowCount()):
            for col in range(table.columnCount()):
                table.setItem(row, col, self.getTableItem(row, self.getItemTextFromTable(table, row, col)))

    def getTableItem(self, rowCount, txt):
        item = QTableWidgetItem(txt)
        if rowCount % 2 == 0:
            item.setBackground(QBrush(QPixmap(":/src/src/tbl_yellow.png")))
        else:
            item.setBackground(QBrush(QPixmap(":/src/src/tbl_white.png")))
        return item

    def getItemTextFromTable(self, table, row, column):
        return table.item(row, column).text()

class MainWindow(QMainWindow):
    def __init__(self, parent=None):
        super(MainWindow, self).__init__(parent)
        self.dialog = LoginDialog()
        self.dialog.loginState.connect(self.showOrNot)
        if NeedLogin:
            self.dialog.exec()
        self.init_ui()
        self.build_connections()
        self.styleType = styleTypes[0]
        self.maiType = maiTypes[0]
        self.kpType = kpTypes[0]
        self.show()
        
    def init_ui(self):
        self.setWindowFlags(Qt.FramelessWindowHint)
        self.ui = Ui_Form()
        self.ui.setupUi(self)
        if Pay:
            self.setStyleSheet("#Form{background:url(:/src/src/background1366.png)}")
            self.ui.frame00.setStyleSheet("#frame00{background:url(:/src/src/border.png);border: 1px solid rgb(160,160,160); }")
            self.ui.frame01.setStyleSheet("#frame01{background:url(:/src/src/border.png);border: 1px solid rgb(160,160,160); }")
            self.ui.frame10.setStyleSheet("#frame10{background:url(:/src/src/border.png);border: 1px solid rgb(160,160,160); }")
            self.ui.frame11.setStyleSheet("#frame11{background:url(:/src/src/border.png);border: 1px solid rgb(160,160,160); }")
        self.lscjcx = LishiChengjiaoChaxun()
        self.dragPosition = QPoint(0, 0)
        self.oldPos1 = 0
        self.mousePressedFlag = False
        self.ui.bdTbl.setColumnCount(11)
        headers  =  ( "合约代码", "交易类型", "委托价格",
                "委托数量", "未成交数量", "报单状态", "报单类型",
                "委托时间", "操作信息", "本地报单号", "报单号")  
        self.ui.bdTbl.setHorizontalHeaderLabels(headers)
        #self.ui.bdTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        self.ui.bdTbl.verticalHeader().setVisible(False)
        self.ui.bdTbl.setShowGrid(False)
        self.bdMenu = QMenu()
        self.bdMenuDeleteAction = QAction("删除", self)
        self.bdMenuDeleteAction.triggered.connect(self.bdTblDeleteARow)
        self.bdMenu.addAction(self.bdMenuDeleteAction)
        self.ui.bdTbl.customContextMenuRequested.connect(self.openBdTblMenu)

        self.ui.ymdTbl.setColumnCount(8)
        headers  =  ( "类型", "状态", "触发条件",
                "合约代码", "交易类型", "价格", "手数",
                "预埋时间")  
        self.ui.ymdTbl.setHorizontalHeaderLabels(headers)
        self.ui.ymdTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
        self.ui.ccTbl.setColumnCount(12)
        headers  =  ( "合约代码", "持仓方向", "可用仓",
                "开仓均价", "持仓均价", "持仓盈亏", "占用保证金",
                "总持仓", "昨仓", "今仓")  
        self.ui.ccTbl.setHorizontalHeaderLabels(headers)
#        self.ui.ccTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        self.ui.ccTbl.verticalHeader().setVisible(False)
        self.ui.ccTbl.setShowGrid(False)
        
        self.ui.kcTbl.setColumnCount(4)
        headers  =  ( "合约代码","当前库存", "可用库存", "交易冻结")  
        self.ui.kcTbl.setHorizontalHeaderLabels(headers)
        self.ui.kcTbl.verticalHeader().setVisible(False)
        self.ui.kcTbl.setShowGrid(False)
        
        self.ui.zjTbl.setColumnCount(4)
        headers  =  ( "当前余额", "可用金额", "交易冻结资金", "浮动盈亏")  
        self.ui.zjTbl.setHorizontalHeaderLabels(headers)
        self.ui.zjTbl.setShowGrid(False)
        self.ui.zjTbl.setRowHeight(0, 21)
        
    #成交单
        self.ui.cjTbl.setColumnCount(7)
        headers  =  ( "合约代码", "交易类型", "成交价格", "成交数量",
                "成交时间","报单号", "成交流水号")  
        self.ui.cjTbl.setHorizontalHeaderLabels(headers)
        self.ui.cjTbl.setColumnWidth(6, 137)
        #self.ui.cjTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        self.ui.cjTbl.verticalHeader().setVisible(False)
        self.ui.cjTbl.setShowGrid(False)
        self.cjMenu = QMenu()
        self.cjMenuDeleteAction = QAction("删除", self)
        self.cjMenuDeleteAction.triggered.connect(self.cjTblDeleteARow)
        self.cjMenu.addAction(self.cjMenuDeleteAction)
        self.cjMenuKcAction = QAction("累计到库存/记录数据/清空成交流水", self)
        self.cjMenuKcAction.triggered.connect(self.cjTblKcAndRecord)
        self.cjMenu.addAction(self.cjMenuKcAction)
        self.ui.cjTbl.customContextMenuRequested.connect(self.openCjTblMenu)
        
        buttons = [self.ui.s1,self.ui.s2,self.ui.s3,self.ui.s4,self.ui.s5,self.ui.b1,self.ui.b2,self.ui.b3,self.ui.b4,self.ui.b5]
        for i in range(0, 5):
            buttons[i].setStyleSheet("QPushButton{color:%s;} QPushButton:hover{color:black;}"%numgreen)
            buttons[i+5].setStyleSheet("QPushButton{color:%s;} QPushButton:hover{color:black;}"%numred)
        self.ui.closeBtn.setStyleSheet("background:transparent")
        self.ui.ccTab.setStyleSheet("background:transparent")
        self.ui.kcTab.setStyleSheet("background:transparent")
        self.ui.zjTab.setStyleSheet("background:transparent")
        self.ui.bdTab.setStyleSheet("background:transparent")
        self.ui.ymdTab.setStyleSheet("background:transparent")
        menu = QMenu(self)
        menu1 = QMenu("窗口宽度", self)
        self.sizeAction1 = QAction(QIcon(sizeIcon), "1366", self, triggered = self.resize1366)
        self.sizeAction2 = QAction(QIcon(), "1600", self, triggered = self.resize1600)
        self.sizeAction3 = QAction(QIcon(), "1920", self, triggered = self.resize1920)
        menu1.addAction(self.sizeAction1)
        menu1.addAction(self.sizeAction2)
        menu1.addAction(self.sizeAction3)
        menu.addMenu(menu1)
        menu.addAction(QAction("截图", self))
        if Pay:
            self.ui.czBtn.setMenu(menu)
       
        self.ui.ssEdit.hide()
        self.ui.zddEdit.hide()
        self.ui.zdxEdit.hide()
        self.sbls = [self.ui.s1l, self.ui.s2l, self.ui.s3l, self.ui.s4l, self.ui.s5l,
                    self.ui.b1l, self.ui.b2l, self.ui.b3l, self.ui.b4l, self.ui.b5l]
        sysCdMenu1 = QMenu("当日查询", self)
        sysCdMenu1.addAction(QAction("委托/申报", self))
        self.lscjcxAction = QAction("成交查询", self)
        self.lscjcxAction.triggered.connect(self.lscjcx.exec)
        sysCdMenu1.addAction(self.lscjcxAction)
        sysCdMenu1.addAction(QAction("延期持仓明细", self))
        sysCdMenu1.addAction(QAction("出入金明细", self))
        sysCdMenu2 = QMenu("历史查询", self)
        sysCdMenu3 = QMenu("风险查询", self)
        sysMenu = QMenu(self)
        sysMenu.addAction(QAction("客户资金库存及持仓", self))
        sysMenu.addAction(QAction("交收行情", self))
        sysMenu.addAction(QAction("显示交易状态", self))
        sysMenu.addAction(QAction("延期平仓试算", self))
        sysMenu.addAction(QAction("出入金", self))
        sysMenu.addAction(QAction("提货", self))
        sysMenu.addAction(QAction("登录密码修改", self))
        sysMenu.addAction(QAction("资金密码修改", self))
        sysMenu.addAction(QAction("会员公告查询", self))
        sysMenu.addMenu(sysCdMenu1)
        sysMenu.addMenu(sysCdMenu2)
        sysMenu.addMenu(sysCdMenu3)
        sysMenu.addAction(QAction("清除缓存数据", self))
        sysMenu.addSeparator()
        sysMenu.addAction(QAction("选项设置", self))
        sysMenu.addAction(QAction("使用说明", self))
        sysMenu.addSeparator()
        sysMenu.addAction(QAction("退出交易", self))
        self.ui.sysBtn.setMenu(sysMenu)

        self.init_params()
    
    def init_params(self):
        self.history = []
    
    def build_connections(self):
        self.ui.ssLbl.installEventFilter(self)
        self.ui.zddLbl.installEventFilter(self)
        # self.ui.zdxLbl.installEventFilter(self)
        self.ui.ssEdit.installEventFilter(self)
        self.ui.zddEdit.installEventFilter(self)
        self.ui.zdxEdit.installEventFilter(self)
        self.ui.dragLbl1.installEventFilter(self)
        self.ui.dragLbl2.installEventFilter(self)
        self.ui.windowDragLbl.installEventFilter(self)
        self.ui.closeBtn.clicked.connect(self.close)
        self.ui.yqBtn.clicked.connect(self.yqClicked)
        self.ui.jsBtn.clicked.connect(self.jsClicked)
        self.ui.zlcBtn.clicked.connect(self.zlcClicked)
        self.ui.buyBtn.clicked.connect(self.buyClicked)
        self.ui.sellBtn.clicked.connect(self.sellClicked)
        self.ui.kcBtn.clicked.connect(self.kcClicked)
        self.ui.pcBtn.clicked.connect(self.pcClicked)
        self.ui.zdjBtn.clicked.connect(self.zdjClicked)
        self.ui.ccTab.clicked.connect(self.ccTabClicked)
        self.ui.kcTab.clicked.connect(self.kcTabClicked)
        self.ui.zjTab.clicked.connect(self.zjTabClicked)
        self.ui.bdTab.clicked.connect(self.bdTabClicked)
        self.ui.ymdTab.clicked.connect(self.ymdTabClicked)
        self.ui.zdjSpin.valueChanged.connect(self.zdjSpinChanged)
        self.ui.hyCombo.currentIndexChanged.connect(self.hyComboChanged)
        self.ui.cdBtn.clicked.connect(self.cdBtnClicked)
        self.ui.qcBtn.clicked.connect(self.qcBtnClicked)
        self.ui.gdBtn.clicked.connect(self.gdBtnClicked)

        # self.ui.kdcBtn.clicked.connect(self.kdcBtnClicked)
        self.ui.pcBtn2.clicked.connect(self.pcBtn2Clicked)
        self.ui.fsBtn.clicked.connect(self.fsBtnClicked)
        self.ui.fxdBtn.clicked.connect(self.fxdBtnClicked)

    def showOrNot(self, flag):
        if flag == 0:
            sys.exit(0)

    def openCjTblMenu(self, pos):
        pos += (self.ui.frame11.pos() + self.ui.cjTbl.pos())
        self.cjMenu.exec_(self.mapToGlobal(pos))

    def cjTblDeleteARow(self):
        currentRow = self.ui.cjTbl.currentRow()
        if currentRow < 0:
            return
        self.ui.cjTbl.removeRow(currentRow)
        self.updateStyleOfBdtbl(self.ui.cjTbl, currentRow)

    def openBdTblMenu(self, pos):
        pos += (self.ui.frame01.pos() + self.ui.bdTbl.pos() + QPoint(0, 50))
        self.bdMenu.exec_(self.mapToGlobal(pos))

    def bdTblDeleteARow(self):
        currentRow = self.ui.bdTbl.currentRow()
        if currentRow < 0:
            return
        self.ui.bdTbl.removeRow(currentRow)
        self.updateStyleOfBdtbl(self.ui.bdTbl, currentRow)
        
    def clean_commo_from_text(self, text):
        nt = "".join(text.split(","))
        return nt
        
    def pcBtn2Clicked(self):
        currentRow = self.ui.ccTbl.currentRow()
        if currentRow < 0:
            return
        zc7 = int(self.clean_commo_from_text(self.ui.ccTbl.item(currentRow, 8).text()))
        ccjj8 = float(self.clean_commo_from_text(self.ui.ccTbl.item(currentRow, 4).text()))
        cjjg6 = float(self.clean_commo_from_text(self.ui.ccTbl.item(currentRow, 10).text()))
        cjsl2 = int(self.ui.ccTbl.item(currentRow, 11).text())
        kcjj9 = (zc7 * ccjj8 + cjjg6 * cjsl2) / (zc7 + cjsl2)
        jc4 = int(self.ui.ccTbl.item(currentRow, 9).text())
        zcc10 = cjsl2 + zc7 + jc4
        zybzj11 = 0
        ccyk12 = 0
        zdj14 = self.ui.zdjSpin.value()
        if self.ui.hyCombo.currentIndex() == 0:
            ccyk12 = zcc10 * (ccjj8 - zdj14) * 1
            zybzj11 = kcjj9* zcc10 * 0.13
        if self.ui.hyCombo.currentIndex() == 1:
            ccyk12 = zcc10 * (ccjj8 - zdj14) * 1000
            zybzj11 = kcjj9* zcc10  * 110
        if self.ui.hyCombo.currentIndex() == 2:
            ccyk12 = zcc10 * (ccjj8 - zdj14) * 100
            zybzj11 = kcjj9* zcc10 * 11
        self.ui.ccTbl.item(currentRow, 7).setText(str(zcc10))  
        self.ui.ccTbl.item(currentRow, 3).setText(self.baoliu(2, kcjj9))
        self.ui.ccTbl.item(currentRow, 6).setText(self.baoliu(2, zybzj11))
        self.ui.ccTbl.item(currentRow, 5).setText(self.baoliu(2, ccyk12))

    def fxdBtnClicked(self):
        try:
            kyje2 = float(self.clean_commo_from_text(self.ui.zjTbl.item(0, 1).text()))
            jydj3 = 0
            fdyk4 = 0
            for row in range(self.ui.ccTbl.rowCount()):
                zybzj11 = float(self.clean_commo_from_text(self.ui.ccTbl.item(row, 6).text()))
                ccyk12 = float(self.clean_commo_from_text(self.ui.ccTbl.item(row, 5).text()))
                jydj3 += zybzj11
                fdyk4 += ccyk12
            dqye1 = kyje2 + jydj3 + fdyk4
            fxd5 = int(jydj3 / dqye1 * 100)
            self.ui.zjTbl.setItem(0, 0, self.getTableItem(0, self.baoliu(2, dqye1)))
            self.ui.zjTbl.setItem(0, 1, self.getTableItem(0, self.baoliu(2, kyje2)))
            self.ui.zjTbl.setItem(0, 2, self.getTableItem(0, self.baoliu(2, jydj3)))
            self.ui.zjTbl.setItem(0, 3, self.getTableItem(0, self.baoliu(2, fdyk4)))
            self.ui.fxdBtn.setText(str(fxd5)+"%")
        except Exception as e:
            print(e)
        
    def fsBtnClicked(self):
        currentRow = self.ui.ccTbl.currentRow()
        if currentRow < 0:
    	    return
        self.ui.ccTbl.removeRow(currentRow)
        self.updateStyleOfBdtbl(self.ui.ccTbl, currentRow)
        
    def yqClicked(self):
        self.styleType = styleTypes[0]
        self.ui.jsBtn.setStyleSheet(offStyle)
        self.ui.yqBtn.setStyleSheet(onStyle)
        self.ui.zlcBtn.setStyleSheet(offStyle)
        self.ui.kcBtn.setEnabled(True)
        self.ui.pcBtn.setEnabled(True)
        self.ui.zdjBtn.setEnabled(True)
        self.ui.zdjSpin.setEnabled(True)
        self.ui.zdjBtn.setStyleSheet("QPushButton{border-image:url(:/src/src/zdj.png)} QPushButton:hover{color:red;}")
        if self.maiType == maiTypes[0] and self.kpType == kpTypes[0]:
            self.ui.kdcBtn.setText("开多仓")
        elif self.maiType == maiTypes[0] and self.kpType == kpTypes[1]:
            self.ui.kdcBtn.setText("平空仓")
        elif self.maiType == maiTypes[1] and self.kpType == kpTypes[0]:
            self.ui.kdcBtn.setText("开空仓")
        else:
            self.ui.kdcBtn.setText("平多仓")
        if self.kpType == kpTypes[0]:
            self.ui.kcBtn.setStyleSheet(onStyle)
            self.ui.pcBtn.setStyleSheet(offStyle)
        else:
            self.ui.pcBtn.setStyleSheet(onStyle)
            self.ui.kcBtn.setStyleSheet(offStyle)     
        
    def jsClicked(self):
        self.styleType = styleTypes[1]
        self.ui.yqBtn.setStyleSheet(offStyle)
        self.ui.jsBtn.setStyleSheet(onStyle)
        self.ui.zlcBtn.setStyleSheet(offStyle)
        self.ui.kcBtn.setEnabled(False)
        self.ui.pcBtn.setEnabled(False)
        self.ui.zdjBtn.setEnabled(False)
        self.ui.zdjSpin.setEnabled(False)
        self.ui.kcBtn.setStyleSheet("QPushButton{background:rgb(240,240,240);color:gray;}")
        self.ui.pcBtn.setStyleSheet("QPushButton{background:rgb(240,240,240);color:gray;}")
        self.ui.zdjBtn.setStyleSheet("QPushButton{background:rgb(240,240,240);color:gray;}")
        if self.maiType == maiTypes[0]:
            self.ui.kdcBtn.setText("延期收金")
        else:
            self.ui.kdcBtn.setText("延期交金")

    def zlcClicked(self):
        self.styleType = styleTypes[2]
        self.ui.jsBtn.setStyleSheet(offStyle)
        self.ui.zlcBtn.setStyleSheet(onStyle)
        self.ui.yqBtn.setStyleSheet(offStyle)
        self.ui.kcBtn.setEnabled(False)
        self.ui.pcBtn.setEnabled(False)
        self.ui.zdjBtn.setEnabled(False)
        self.ui.zdjSpin.setEnabled(False)
        self.ui.kcBtn.setStyleSheet("QPushButton{background:rgb(240,240,240);color:gray;}")
        self.ui.pcBtn.setStyleSheet("QPushButton{background:rgb(240,240,240);color:gray;}")
        self.ui.zdjBtn.setStyleSheet("QPushButton{background:rgb(240,240,240);color:gray;}")
        if self.maiType == maiTypes[0]:
            self.ui.kdcBtn.setText("中立仓收金")
        else:
            self.ui.kdcBtn.setText("中立仓交金")
            
    def buyClicked(self):
        self.maiType = maiTypes[0]
        self.ui.buyBtn.setStyleSheet("border-image:url(:/src/src/on.png);color:%s;"%buyred)
        self.ui.sellBtn.setStyleSheet("border-image:url(:/src/src/off.png);color:%s;"%sellgreen)
        if self.styleType == styleTypes[0]:
            if self.kpType == kpTypes[0]:
                self.ui.kdcBtn.setText("开多仓")
            else:
                self.ui.kdcBtn.setText("平空仓")
        elif self.styleType == styleTypes[1]:
            self.ui.kdcBtn.setText("延期收金")
        else:
            self.ui.kdcBtn.setText("中立仓收金")
    
    def sellClicked(self):
        self.maiType = maiTypes[1]
        self.ui.buyBtn.setStyleSheet("border-image:url(:/src/src/off.png);color:%s;"%buyred)
        self.ui.sellBtn.setStyleSheet("border-image:url(:/src/src/on.png);color:%s;"%sellgreen)
        if self.styleType == styleTypes[0]:
            if self.kpType == kpTypes[0]:
                self.ui.kdcBtn.setText("开空仓")
            else:
                self.ui.kdcBtn.setText("平多仓")
        elif self.styleType == styleTypes[1]:
            self.ui.kdcBtn.setText("延期交金")
        else:
            self.ui.kdcBtn.setText("中立仓交金")

    def kcClicked(self):
        self.kpType = kpTypes[0]
        self.ui.kcBtn.setStyleSheet(onStyle)
        self.ui.pcBtn.setStyleSheet(offStyle)
        if self.maiType == maiTypes[0]:
            self.ui.kdcBtn.setText("开多仓")
        else:
            self.ui.kdcBtn.setText("开空仓")
          
    def pcClicked(self):
        self.kpType = kpTypes[1]
        self.ui.pcBtn.setStyleSheet(onStyle)
        self.ui.kcBtn.setStyleSheet(offStyle)
        if self.maiType == maiTypes[0]:
            self.ui.kdcBtn.setText("平空仓")
        else:
            self.ui.kdcBtn.setText("平多仓")
    
    def zdjClicked(self):
        if self.ui.zdjBtn.text() == "指定价":
            self.ui.zdjBtn.setText("跟盘价")
        else:
            self.ui.zdjBtn.setText("指定价")
                
    def ccTabClicked(self):
        self.ui.tLbl.setPixmap(QtGui.QPixmap(":/src/src/cc.png"))
        self.ui.tStack.setCurrentIndex(0)
    
    def kcTabClicked(self):
        self.ui.tLbl.setPixmap(QtGui.QPixmap(":/src/src/kc.png"))
        self.ui.tStack.setCurrentIndex(1)
    
    def zjTabClicked(self):
        self.ui.tLbl.setPixmap(QtGui.QPixmap(":/src/src/zj.png"))
        self.ui.tStack.setCurrentIndex(2)
        
    def bdTabClicked(self):
        self.ui.sLbl.setPixmap(QtGui.QPixmap(":/src/src/bd.png"))
        self.ui.sStack.setCurrentIndex(0)
    
    def ymdTabClicked(self):
        self.ui.sLbl.setPixmap(QtGui.QPixmap(":/src/src/ymd.png"))
        self.ui.sStack.setCurrentIndex(1)

    def hyComboChanged(self, index):
        if index == 0:
            self.ui.zdjSpin.setDecimals(0)
        else:
            self.ui.zdjSpin.setDecimals(2)
        self.ui.zdjSpin.setValue(0)
        self.zdjSpinChanged(0)
    
    def getrnd(self, l):
        return str(random.randint(10*(l-1), int("9"*l)))
    
    def cdBtnClicked(self):
        hydm = self.ui.hyCombo.currentText()
        jylx = self.ui.yqBtn.text()+self.ui.kdcBtn.text()
        wtjg = self.baoliu(0 if self.ui.hyCombo.currentIndex()==0 else 2, self.ui.zdjSpin.value())
        # self.history.append([hydm, jylx, wtjg])
        countRow = self.ui.bdTbl.rowCount()
        self.ui.bdTbl.insertRow(countRow)
        self.ui.bdTbl.setRowHeight(countRow, 21)
        self.ui.bdTbl.setItem(countRow, 0, self.getTableItem(countRow, hydm))
        self.ui.bdTbl.setItem(countRow, 1, self.getTableItem(countRow, jylx))
        self.ui.bdTbl.setItem(countRow, 2, self.getTableItem(countRow, wtjg))
        self.ui.bdTbl.setItem(countRow, 3, self.getTableItem(countRow, str(self.ui.ssSpin.value())))
        self.ui.bdTbl.setItem(countRow, 4, self.getTableItem(countRow, str(self.ui.ssSpin.value())))
        self.ui.bdTbl.setItem(countRow, 5, self.getTableItem(countRow, '已报入'))
        self.ui.bdTbl.setItem(countRow, 6, self.getTableItem(countRow, '正常报单'))
        self.ui.bdTbl.setItem(countRow, 7, self.getTableItem(countRow, '13:34:41'))
        self.ui.bdTbl.setItem(countRow, 8, self.getTableItem(countRow, '报单成功'))
        self.ui.bdTbl.setItem(countRow, 9, self.getTableItem(countRow, self.getrnd(6)))
        self.ui.bdTbl.setItem(countRow, 10, self.getTableItem(countRow, '0'+self.getrnd(6)))
    
    def getTableItem(self, rowCount, txt):
        item = QTableWidgetItem(txt)
        if rowCount % 2 == 0:
            item.setBackground(QBrush(QPixmap(":/src/src/tbl_yellow.png")))
        else:
            item.setBackground(QBrush(QPixmap(":/src/src/tbl_white.png")))
        return item
    
    def qcBtnClicked(self):
        currentRow = self.ui.bdTbl.currentRow()
        if currentRow < 0:
            return
        countRow = self.ui.cjTbl.rowCount()
        self.ui.cjTbl.insertRow(countRow)
        self.ui.cjTbl.setRowHeight(countRow, 21)
        self.ui.cjTbl.setItem(countRow, 0, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 0)))
        self.ui.cjTbl.setItem(countRow, 1, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 1)))
        self.ui.cjTbl.setItem(countRow, 2, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 2)))
        self.ui.cjTbl.setItem(countRow, 3, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 3)))
        self.ui.cjTbl.setItem(countRow, 4, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 7)))
        self.ui.cjTbl.setItem(countRow, 5, self.getTableItem(countRow, '0'+self.getrnd(7)))
        self.ui.cjTbl.setItem(countRow, 6, self.getTableItem(countRow, '17'+self.getrnd(14)))
        self.ui.bdTbl.removeRow(currentRow)
        self.updateStyleOfBdtbl(self.ui.bdTbl, currentRow)

    def cjTblKcAndRecord(self):
        self.recordHistory()
        for row in range(self.ui.cjTbl.rowCount()):
            hy = self.getItemTextFromTable(self.ui.cjTbl, row, 0)
            kcRow = self.checkHyInTable(self.ui.kcTbl, hy)
            if kcRow == self.ui.kcTbl.rowCount():
                self.ui.kcTbl.insertRow(kcRow)
                self.ui.kcTbl.setRowHeight(kcRow, 21)
                self.ui.kcTbl.setItem(kcRow, 0, self.getTableItem(kcRow, self.getItemTextFromTable(self.ui.cjTbl, row, 0)))
                self.ui.kcTbl.setItem(kcRow, 1, self.getTableItem(kcRow, self.getItemTextFromTable(self.ui.cjTbl, row, 3)))
                self.ui.kcTbl.setItem(kcRow, 2, self.getTableItem(kcRow, self.getItemTextFromTable(self.ui.cjTbl, row, 3)))
                self.ui.kcTbl.setItem(kcRow, 3, self.getTableItem(kcRow, '0'))
            else:
                amt = int(self.getItemTextFromTable(self.ui.cjTbl, row, 3))
                currentKc = int(self.getItemTextFromTable(self.ui.kcTbl, kcRow, 1))
                self.ui.kcTbl.item(kcRow, 1).setText(str(amt+currentKc))
                self.ui.kcTbl.item(kcRow, 2).setText(str(amt+currentKc))
        while(self.ui.cjTbl.rowCount()):
            self.ui.cjTbl.removeRow(0)

    def gdBtnClicked(self):
        currentRow = self.ui.cjTbl.currentRow()
        if currentRow < 0:
            return
        hy = self.getItemTextFromTable(self.ui.cjTbl, currentRow, 0)
        jylx = self.getItemTextFromTable(self.ui.cjTbl, currentRow, 1)
        cjsl = self.getItemTextFromTable(self.ui.cjTbl, currentRow, 3)
        cjjg = self.getItemTextFromTable(self.ui.cjTbl, currentRow, 2)
        ccfx = jylx[3]
        jc = 0
        for row in range(self.ui.cjTbl.rowCount()):
            hy1 = self.getItemTextFromTable(self.ui.cjTbl, row, 0)
            jylx1 = self.getItemTextFromTable(self.ui.cjTbl, row, 1)
            if hy1 == hy and jylx1 == jylx:
                amt = int(self.getItemTextFromTable(self.ui.cjTbl, row, 3))
                jc += amt
        countRow = self.ui.ccTbl.rowCount()
        self.ui.ccTbl.insertRow(countRow)
        self.ui.ccTbl.setRowHeight(countRow, 21)
        self.ui.ccTbl.setItem(countRow, 0, self.getTableItem(countRow, hy))
        self.ui.ccTbl.setItem(countRow, 1, self.getTableItem(countRow, ccfx))
        self.ui.ccTbl.setItem(countRow, 2, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 3, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 4, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 5, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 6, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 7, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 8, self.getTableItem(countRow, '0'))
        self.ui.ccTbl.setItem(countRow, 9, self.getTableItem(countRow, str(jc)))
        self.ui.ccTbl.setItem(countRow, 10, self.getTableItem(countRow, cjjg))
        self.ui.ccTbl.setItem(countRow, 11, self.getTableItem(countRow, cjsl))

    def recordHistory(self):
        if not os.path.exists(historyFile):
            f = open(historyFile, "w")
            f.close()
        for row in range(self.ui.cjTbl.rowCount()):
            l = []
            for cnt in  range(self.ui.cjTbl.columnCount()):
                l.append(self.getItemTextFromTable(self.ui.cjTbl, row, cnt))
            txt = ",".join(l)
            f = open(historyFile, 'a')
            f.write(txt+'\n')
            f.close()

    def checkHyInTable(self, table, hy):
        for row in range(table.rowCount()):
            if self.getItemTextFromTable(table, row, 0) == hy:
                return row
        return table.rowCount()

    def updateStyleOfBdtbl(self, table, currentRow):
        for row in range(currentRow, table.rowCount()):
            for col in range(table.columnCount()):
                table.setItem(row, col, self.getTableItem(row, self.getItemTextFromTable(table, row, col)))
                
    
    def getItemTextFromTable(self, table, row, column):
        return table.item(row, column).text()
    
    def zdjSpinChanged(self, value):
        if self.ui.hyCombo.currentIndex() == 0:
            self.ui.zdjSpin.setDecimals(0)
            self.ui.s1.setText(self.baoliu(0, value))
            self.ui.s2.setText(self.baoliu(0, value + 1))
            self.ui.s3.setText(self.baoliu(0, value + 2))
            self.ui.s4.setText(self.baoliu(0, value + 3))
            self.ui.s5.setText(self.baoliu(0, value + 4))
            self.ui.b1.setText(self.baoliu(0, value - 1))
            self.ui.b2.setText(self.baoliu(0, value - 2))
            self.ui.b3.setText(self.baoliu(0, value - 3))
            self.ui.b4.setText(self.baoliu(0, value - 4))
            self.ui.b5.setText(self.baoliu(0, value - 5))
            for sb in self.sbls:
                sb.setText(str(random.randint(1000, 9999)))  
        else:
            self.ui.s1.setText(self.baoliu(2, value))
            self.ui.s2.setText(self.baoliu(2, value + 0.01))
            self.ui.s3.setText(self.baoliu(2, value + 0.02))
            self.ui.s4.setText(self.baoliu(2, value + 0.03))
            self.ui.s5.setText(self.baoliu(2, value + 0.04))
            self.ui.b1.setText(self.baoliu(2, value - 0.01))
            self.ui.b2.setText(self.baoliu(2, value - 0.02))
            self.ui.b3.setText(self.baoliu(2, value - 0.03))
            self.ui.b4.setText(self.baoliu(2, value - 0.04))
            self.ui.b5.setText(self.baoliu(2, value - 0.05))
            if self.ui.hyCombo.currentIndex() in (1,2):
                for sb in self.sbls:
                    sb.setText(str(random.randint(10, 99)))
            else:
                for sb in self.sbls:
                    sb.setText(str(random.randint(100, 999)))
            
    def resize1366(self):
        self.setStyleSheet("#Form{background:url(:/src/src/background1366.png)}")
        self.sizeAction1.setIcon(QIcon(sizeIcon))
        self.sizeAction2.setIcon(QIcon())
        self.sizeAction3.setIcon(QIcon())
        self.reshapeMainWindow(1366)
        
    def resize1600(self):
        self.setStyleSheet("#Form{background:url(:/src/src/background1600.png)}")
        self.sizeAction1.setIcon(QIcon())
        self.sizeAction2.setIcon(QIcon(sizeIcon))
        self.sizeAction3.setIcon(QIcon())
        self.reshapeMainWindow(1600)
        
    def resize1920(self):
        self.setStyleSheet("#Form{background:url(:/src/src/background1920.png)}")
        self.sizeAction1.setIcon(QIcon())
        self.sizeAction2.setIcon(QIcon())
        self.sizeAction3.setIcon(QIcon(sizeIcon))
        self.reshapeMainWindow(1920)
    
    def reshapeMainWindow(self, width):
        offset = width - self.width()
        self.resize(width, self.height())
        self.ui.frame01.resize(self.ui.frame01.width()+offset, self.ui.frame01.height())
        self.ui.sStack.resize(self.ui.sStack.width()+offset, self.ui.sStack.height())
        self.ui.frame11.resize(self.ui.frame11.width()+offset, self.ui.frame11.height())
        self.moveBtn(self.ui.cdBtn, offset)
        self.moveBtn(self.ui.qcBtn, offset)
        self.moveBtn(self.ui.gdBtn, offset)    
    
    def moveBtn(self, btn, offset):
        btn.setGeometry(btn.x() + offset, btn.y(), btn.width(), btn.height())
    
    def baoliu(self, n, f):
        if n == 2:
            return "%.2f"%f
        return "%i"%f

    def changeZddZdx(self, value):
        zdd = value
        zdx = value
        if self.ui.hyCombo.currentIndex() == 0:
            zdd = int(value * 1.08)
            zdx = int(value * 0.92)
        elif self.ui.hyCombo.currentIndex() in (1, 2):
            zdd = value * 1.06
            zdx = value * 0.94
        return (zdd, zdx)

    def eventFilter(self, target, event):
        if target == self.ui.dragLbl1 and Pay:
            if event.type() == QEvent.MouseButtonPress:
                self.oldPos1 = self.ui.dragLbl1.x()
                self.dragPosition = event.globalPos() - self.ui.dragLbl1.frameGeometry().topLeft()
            if event.type() == QEvent.MouseMove:
                self.ui.dragLbl1.move((event.globalPos() - self.dragPosition).x(), self.ui.dragLbl1.y())
            if event.type() == QEvent.MouseButtonRelease:
                offset = self.ui.dragLbl1.x() - self.oldPos1
                self.ui.frame01.setGeometry(self.ui.frame01.x()+offset, self.ui.frame01.y(), self.ui.frame01.width()-offset, self.ui.frame01.height())
                self.ui.sStack.setGeometry(self.ui.sStack.x(), self.ui.sStack.y(), self.ui.sStack.width()-offset, self.ui.sStack.height())
                self.ui.frame00.resize(self.ui.frame00.width()+offset, self.ui.frame00.height())
                self.moveBtn(self.ui.cdBtn, -offset)
                self.moveBtn(self.ui.qcBtn, -offset)
                self.moveBtn(self.ui.gdBtn, -offset)
                self.moveBtn(self.ui.czBtn, offset)
            if event.type() == QEvent.Enter:
                self.ui.dragLbl1.setStyleSheet("background:green;")
            if event.type() == QEvent.Leave:
                self.ui.dragLbl1.setStyleSheet("background:transparent;")
            event.accept()
        if target == self.ui.dragLbl2 and Pay:
            if event.type() == QEvent.MouseButtonPress:
                self.oldPos2 = self.ui.dragLbl2.x()
                self.dragPosition = event.globalPos() - self.ui.dragLbl2.frameGeometry().topLeft()
            if event.type() == QEvent.MouseMove:
                self.ui.dragLbl2.move((event.globalPos() - self.dragPosition).x(), self.ui.dragLbl2.y())
            if event.type() == QEvent.MouseButtonRelease:
                offset = self.ui.dragLbl2.x() - self.oldPos2
                self.ui.frame11.setGeometry(self.ui.frame11.x()+offset, self.ui.frame11.y(), self.ui.frame11.width()-offset, self.ui.frame11.height())
                self.moveBtn(self.ui.fLbl, offset)
                self.ui.tStack.setGeometry(self.ui.tStack.x(), self.ui.tStack.y(), self.ui.tStack.width()+offset, self.ui.tStack.height())
                self.ui.frame10.resize(self.ui.frame10.width()+offset, self.ui.frame10.height())
                self.ui.ccTbl.resize(self.ui.ccTbl.width()+offset, self.ui.ccTbl.height())
                self.moveBtn(self.ui.pcBtn2, offset)
                self.moveBtn(self.ui.fsBtn, offset)
            if event.type() == QEvent.Enter:
                self.ui.dragLbl2.setStyleSheet("background:green;")
            if event.type() == QEvent.Leave:
                self.ui.dragLbl2.setStyleSheet("background:transparent;")
            event.accept()
        if target == self.ui.windowDragLbl and Pay:
            if event.type() == QEvent.MouseButtonPress:
                self.dragPosition = event.globalPos() - self.frameGeometry().topLeft()
            if event.type() == QEvent.MouseMove:
                self.move(event.globalPos() - self.dragPosition)
            if event.type() == QEvent.Enter:
                self.ui.windowDragLbl.setStyleSheet("background:green;")
            if event.type() == QEvent.Leave:
                self.ui.windowDragLbl.setStyleSheet("background:transparent;")
            event.accept()
        if target == self.ui.ssLbl:
            if event.type() == QEvent.MouseButtonPress:
                self.ui.ssEdit.show()
            event.accept()
        if target == self.ui.ssEdit:
            if event.type() == QEvent.Leave:
                if self.ui.ssEdit.text():
                    self.ui.ssLbl.setText("<= %s"%self.ui.ssEdit.text())
                    self.ui.ssEdit.clear()
                self.ui.ssEdit.hide()
            event.accept()
        if target == self.ui.zddLbl:
            if event.type() == QEvent.MouseButtonPress:
                self.ui.zddEdit.show()
            event.accept()
        if target == self.ui.zddEdit:
            if event.type() == QEvent.Leave:
                if self.ui.zddEdit.text():
                    try:
                        v = float(self.ui.zddEdit.text())
                        zdd, zdx = self.changeZddZdx(v)
                        self.ui.zddLbl.setText("<= %s"%self.baoliu(2, zdd))
                        self.ui.zdxLbl.setText("<= %s"%self.baoliu(2, zdx))
                    except:
                        pass
                    self.ui.zddEdit.clear()
                self.ui.zddEdit.hide()
            event.accept()
        # if target == self.ui.zdxLbl:
        #     if event.type() == QEvent.MouseButtonPress:
        #         self.ui.zdxEdit.show()
        #     event.accept()
        # if target == self.ui.zdxEdit:
        #     if event.type() == QEvent.Leave:
        #         if self.ui.zdxEdit.text():
        #             try:
        #                 v = float(self.ui.zdxEdit.text())
        #                 self.ui.zdxLbl.setText(">= %.2f"%v)
        #             except:
        #                 pass
        #             self.ui.zdxEdit.clear()
        #         self.ui.zdxEdit.hide()
        #     event.accept()
        return False
        
if __name__ == "__main__":
    app = QtWidgets.QApplication(sys.argv)
    window = MainWindow()
    #window.show()
    sys.exit(app.exec_())    
