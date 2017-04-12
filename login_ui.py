# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'login.ui'
#
# Created by: PyQt5 UI code generator 5.8
#
# WARNING! All changes made in this file will be lost!

from PyQt5 import QtCore, QtGui, QtWidgets

class Ui_Form(object):
    def setupUi(self, Form):
        Form.setObjectName("Form")
        Form.resize(400, 300)
        self.passwordEdt = QtWidgets.QLineEdit(Form)
        self.passwordEdt.setGeometry(QtCore.QRect(84, 118, 254, 30))
        self.passwordEdt.setEchoMode(QtWidgets.QLineEdit.Password)
        self.passwordEdt.setObjectName("passwordEdt")
        self.loginBtn = QtWidgets.QPushButton(Form)
        self.loginBtn.setGeometry(QtCore.QRect(170, 191, 75, 36))
        self.loginBtn.setObjectName("loginBtn")
        self.usernameEdt = QtWidgets.QLineEdit(Form)
        self.usernameEdt.setGeometry(QtCore.QRect(84, 65, 254, 30))
        self.usernameEdt.setObjectName("usernameEdt")

        self.retranslateUi(Form)
        QtCore.QMetaObject.connectSlotsByName(Form)

    def retranslateUi(self, Form):
        _translate = QtCore.QCoreApplication.translate
        Form.setWindowTitle(_translate("Form", "登录"))
        self.passwordEdt.setPlaceholderText(_translate("Form", "请输入密码"))
        self.loginBtn.setText(_translate("Form", "登录"))
        self.usernameEdt.setPlaceholderText(_translate("Form", "请输入用户名"))

