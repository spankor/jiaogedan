# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'lscjcx.ui'
#
# Created by: PyQt5 UI code generator 5.8
#
# WARNING! All changes made in this file will be lost!

from PyQt5 import QtCore, QtGui, QtWidgets

class Ui_lscjcx(object):
    def setupUi(self, lscjcx):
        lscjcx.setObjectName("lscjcx")
        lscjcx.setWindowModality(QtCore.Qt.NonModal)
        lscjcx.resize(1222, 372)
        lscjcx.setAutoFillBackground(True)
        lscjcx.setStyleSheet("")
        self.frame = QtWidgets.QFrame(lscjcx)
        self.frame.setGeometry(QtCore.QRect(-1, -1, 1323, 373))
        self.frame.setStyleSheet("#frame{background:url(:/src/src/lscx_background.png)}")
        self.frame.setFrameShape(QtWidgets.QFrame.StyledPanel)
        self.frame.setFrameShadow(QtWidgets.QFrame.Plain)
        self.frame.setObjectName("frame")
        self.comboBox = QtWidgets.QComboBox(self.frame)
        self.comboBox.setGeometry(QtCore.QRect(83, 16, 135, 24))
        font = QtGui.QFont()
        font.setFamily("Small Fonts")
        font.setPointSize(8)
        self.comboBox.setFont(font)
        self.comboBox.setObjectName("comboBox")
        self.comboBox.addItem("")
        self.comboBox.addItem("")
        self.comboBox.addItem("")
        self.comboBox.addItem("")
        self.comboBox.addItem("")
        self.comboBox.addItem("")
        self.comboBox.addItem("")
        self.lineEdit_2 = QtWidgets.QLineEdit(self.frame)
        self.lineEdit_2.setGeometry(QtCore.QRect(315, 46, 135, 24))
        self.lineEdit_2.setText("")
        self.lineEdit_2.setObjectName("lineEdit_2")
        self.cjTbl = QtWidgets.QTableWidget(self.frame)
        self.cjTbl.setGeometry(QtCore.QRect(13, 88, 963, 239))
        self.cjTbl.setContextMenuPolicy(QtCore.Qt.CustomContextMenu)
        self.cjTbl.setFrameShape(QtWidgets.QFrame.NoFrame)
        self.cjTbl.setFrameShadow(QtWidgets.QFrame.Plain)
        self.cjTbl.setGridStyle(QtCore.Qt.NoPen)
        self.cjTbl.setRowCount(0)
        self.cjTbl.setColumnCount(12)
        self.cjTbl.setObjectName("cjTbl")
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(0, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(1, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(2, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(3, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(4, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(5, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(6, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(7, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(8, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(9, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(10, item)
        item = QtWidgets.QTableWidgetItem()
        self.cjTbl.setHorizontalHeaderItem(11, item)
        self.cjTbl.horizontalHeader().setDefaultSectionSize(80)
        self.cjTbl.verticalHeader().setVisible(False)
        self.cjTbl.verticalHeader().setDefaultSectionSize(21)
        self.cjTbl.verticalHeader().setHighlightSections(False)
        self.lineEdit = QtWidgets.QLineEdit(self.frame)
        self.lineEdit.setGeometry(QtCore.QRect(83, 46, 135, 24))
        self.lineEdit.setObjectName("lineEdit")
        self.label = QtWidgets.QLabel(self.frame)
        self.label.setGeometry(QtCore.QRect(415, 49, 30, 19))
        self.label.setStyleSheet("border-image:url(:/src/src/rl.png)")
        self.label.setText("")
        self.label.setObjectName("label")
        self.label_3 = QtWidgets.QLabel(self.frame)
        self.label_3.setGeometry(QtCore.QRect(183, 49, 30, 19))
        self.label_3.setStyleSheet("border-image:url(:/src/src/rl.png)")
        self.label_3.setText("")
        self.label_3.setObjectName("label_3")

        self.retranslateUi(lscjcx)
        QtCore.QMetaObject.connectSlotsByName(lscjcx)

    def retranslateUi(self, lscjcx):
        _translate = QtCore.QCoreApplication.translate
        lscjcx.setWindowTitle(_translate("lscjcx", "历史成交查询"))
        self.comboBox.setItemText(0, _translate("lscjcx", "Ag(T+D)"))
        self.comboBox.setItemText(1, _translate("lscjcx", "Au(T+D)"))
        self.comboBox.setItemText(2, _translate("lscjcx", "mAu(T+D)"))
        self.comboBox.setItemText(3, _translate("lscjcx", "Au99.99"))
        self.comboBox.setItemText(4, _translate("lscjcx", "Au100g"))
        self.comboBox.setItemText(5, _translate("lscjcx", "Au99.95"))
        self.comboBox.setItemText(6, _translate("lscjcx", "Pt99.95"))
        item = self.cjTbl.horizontalHeaderItem(0)
        item.setText(_translate("lscjcx", "成交日期"))
        item = self.cjTbl.horizontalHeaderItem(1)
        item.setText(_translate("lscjcx", "成交时间"))
        item = self.cjTbl.horizontalHeaderItem(2)
        item.setText(_translate("lscjcx", "成交流水"))
        item = self.cjTbl.horizontalHeaderItem(3)
        item.setText(_translate("lscjcx", "报单号"))
        item = self.cjTbl.horizontalHeaderItem(4)
        item.setText(_translate("lscjcx", "交易市场"))
        item = self.cjTbl.horizontalHeaderItem(5)
        item.setText(_translate("lscjcx", "合约代码"))
        item = self.cjTbl.horizontalHeaderItem(6)
        item.setText(_translate("lscjcx", "交易类型"))
        item = self.cjTbl.horizontalHeaderItem(7)
        item.setText(_translate("lscjcx", "成交价格"))
        item = self.cjTbl.horizontalHeaderItem(8)
        item.setText(_translate("lscjcx", "成交数量"))
        item = self.cjTbl.horizontalHeaderItem(9)
        item.setText(_translate("lscjcx", "买卖方向"))
        item = self.cjTbl.horizontalHeaderItem(10)
        item.setText(_translate("lscjcx", "成交金额"))
        item = self.cjTbl.horizontalHeaderItem(11)
        item.setText(_translate("lscjcx", "保证金"))

import jgd_rc
