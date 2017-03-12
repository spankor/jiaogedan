from PyQt5 import QtWidgets
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from PyQt5.QtGui import *
from jiaogedan import *
import sys

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
        self.setStyleSheet("#Form{background:url(:/src/src/background1366.png)}")
        self.dragPosition = QPoint(0, 0)
        self.oldPos1 = 0
        self.mousePressedFlag = False
        self.ui.bdTbl.setColumnCount(11)
        headers  =  ( "合约代码", "交易类型", "委托价格",
                "委托数量", "未成交数量", "报单状态", "报单类型",
                "委托时间", "操作信息", "本地报单号", "报单号")  
        self.ui.bdTbl.setHorizontalHeaderLabels(headers)
        self.ui.bdTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
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
        self.ui.cjTbl.setEditTriggers(QAbstractItemView.NoEditTriggers)
        
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
        self.ui.czBtn.setMenu(menu)
        # mainSplitter=QSplitter(Qt.Vertical,self) 
        # upSplitter = QSplitter(Qt.Horizontal,mainSplitter)
        # upSplitter.addWidget(self.ui.frame00)
        # upSplitter.addWidget(self.ui.frame01)
        # downSplitter = QSplitter(Qt.Horizontal,mainSplitter) 
        # downSplitter.addWidget(self.ui.frame10)
        # downSplitter.addWidget(self.ui.frame11)
        # self.setCentralWidget(mainSplitter)
    
    def build_connections(self):
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
        
    # def mousePressEvent(self, event):
        # self.mousePressedFlag = True
        # self.dragPosition = event.globalPos() - self.frameGeometry().topLeft()
        # event.accept()
 
    # def mouseReleaseEvent(self, event):
        # self.mousePressedFlag = False
    
    def mouseMoveEvent(self, event):
        if self.mousePressedFlag:
            self.move(event.globalPos() - self.dragPosition)
            event.accept()
           
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
        if target == self.ui.windowDragLbl:
            if event.type() == QEvent.MouseButtonPress:
                self.dragPosition = event.globalPos() - self.frameGeometry().topLeft()
            if event.type() == QEvent.MouseMove:
                self.move(event.globalPos() - self.dragPosition)
            if event.type() == QEvent.Enter:
                self.ui.windowDragLbl.setStyleSheet("background:green;")
            if event.type() == QEvent.Leave:
                self.ui.windowDragLbl.setStyleSheet("background:transparent;")
            event.accept()
        return False
        
if __name__ == "__main__":
    app = QtWidgets.QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec_())    
