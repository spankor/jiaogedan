from PyQt5 import QtWidgets
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from PyQt5.QtGui import *
from jiaogedan import *
import sys
import random

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

Pay = False

class MainWindow(QMainWindow):
    def __init__(self, parent=None):
        super(MainWindow, self).__init__(parent)
        self.init_ui()
        self.build_connections()
        self.styleType = styleTypes[0]
        self.maiType = maiTypes[0]
        self.kpType = kpTypes[0]
        
    def init_ui(self):
        self.setWindowFlags(Qt.FramelessWindowHint)
        self.ui = Ui_Form()
        self.ui.setupUi(self)
        if Pay:
            self.setStyleSheet("#Form{background:url(:/src/src/background1366.png)}")
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
        
        self.ui.ymdTbl.setColumnCount(8)
        headers  =  ( "类型", "状态", "触发条件",
                "合约代码", "交易类型", "价格", "手数",
                "预埋时间")  
        self.ui.ymdTbl.setHorizontalHeaderLabels(headers)
        self.ui.ymdTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
        self.ui.ccTbl.setColumnCount(10)
        headers  =  ( "合约代码", "持仓方向", "可用仓",
                "开仓均价", "持仓均价", "持仓盈亏", "占用保证金",
                "总持仓", "昨仓", "今仓")  
        self.ui.ccTbl.setHorizontalHeaderLabels(headers)
        self.ui.ccTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
        self.ui.kcTbl.setColumnCount(4)
        headers  =  ( "合约代码","当前库存", "可用库存", "交易冻结")  
        self.ui.kcTbl.setHorizontalHeaderLabels(headers)
        self.ui.kcTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
        self.ui.zjTbl.setColumnCount(4)
        headers  =  ( "当前余额", "可用金额", "交易冻结资金", "浮动盈亏")  
        self.ui.zjTbl.setHorizontalHeaderLabels(headers)
        self.ui.zjTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
        self.ui.cjTbl.setColumnCount(7)
        headers  =  ( "合约代码", "交易类型", "成交价格", "成交数量",
                "成交时间","报单号", "成交流水号")  
        self.ui.cjTbl.setHorizontalHeaderLabels(headers)
        self.ui.cjTbl.setColumnWidth(6, 137)
        #self.ui.cjTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        self.ui.cjTbl.verticalHeader().setVisible(False)
        self.ui.cjTbl.setShowGrid(False)
        
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
        self.init_params()
    
    def init_params(self):
        self.history = []
    
    def build_connections(self):
        self.ui.ssLbl.installEventFilter(self)
        self.ui.zddLbl.installEventFilter(self)
        self.ui.zdxLbl.installEventFilter(self)
        self.ui.ssEdit.installEventFilter(self)
        self.ui.zddEdit.installEventFilter(self)
        self.ui.zdxEdit.installEventFilter(self)
        self.ui.dragLbl1.installEventFilter(self)
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
        if currentRow >= 0:
            countRow = self.ui.cjTbl.rowCount()
            self.ui.cjTbl.insertRow(countRow)
            self.ui.cjTbl.setRowHeight(countRow, 21)
            self.ui.cjTbl.setItem(countRow, 0, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 0)))
            self.ui.cjTbl.setItem(countRow, 1, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 1)))
            self.ui.cjTbl.setItem(countRow, 2, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 2)))
            self.ui.cjTbl.setItem(countRow, 3, self.getTableItem(countRow, self.getItemTextFromTable(self.ui.bdTbl, currentRow, 3)))
            self.ui.cjTbl.setItem(countRow, 4, self.getTableItem(countRow, '13:43:41'))
            self.ui.cjTbl.setItem(countRow, 5, self.getTableItem(countRow, '0'+self.getrnd(7)))
            self.ui.cjTbl.setItem(countRow, 6, self.getTableItem(countRow, '17'+self.getrnd(14)))
            self.ui.bdTbl.removeRow(currentRow)
            self.updateStyleOfBdtbl(currentRow)
    
    def updateStyleOfBdtbl(self, currentRow):
        for row in range(currentRow, self.ui.bdTbl.rowCount()):
            for col in range(self.ui.bdTbl.columnCount()):
                self.ui.bdTbl.setItem(row, col, self.getTableItem(row, self.getItemTextFromTable(self.ui.bdTbl, row, col)))
                
    
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
        self.moveBtn(self.ui.cdBtn, offset)
        self.moveBtn(self.ui.qcBtn, offset)
        self.moveBtn(self.ui.gdBtn, offset)    
    
    def moveBtn(self, btn, offset):
        btn.setGeometry(btn.x() + offset, btn.y(), btn.width(), btn.height())
    
    def baoliu(self, n, f):
        if n == 2:
            return "%.2f"%f
        return "%i"%f
    
    def eventFilter(self, target, event):
        if target == self.ui.dragLbl1:
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
                        self.ui.zddLbl.setText("<= %.2f"%v)
                    except:
                        pass
                    self.ui.zddEdit.clear()
                self.ui.zddEdit.hide()
            event.accept()
        if target == self.ui.zdxLbl:
            if event.type() == QEvent.MouseButtonPress:
                self.ui.zdxEdit.show()
            event.accept()
        if target == self.ui.zdxEdit:
            if event.type() == QEvent.Leave:
                if self.ui.zdxEdit.text():
                    try:
                        v = float(self.ui.zdxEdit.text())
                        self.ui.zdxLbl.setText(">= %.2f"%v)
                    except:
                        pass
                    self.ui.zdxEdit.clear()
                self.ui.zdxEdit.hide()
            event.accept()
        return False
        
if __name__ == "__main__":
    app = QtWidgets.QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec_())    
