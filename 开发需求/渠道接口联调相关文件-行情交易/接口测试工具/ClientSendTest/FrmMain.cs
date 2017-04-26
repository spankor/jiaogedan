using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClientSendTest.Net;
using ClientSendTest.Util;
using System.Threading;


namespace ClientSendTest
{
    public partial class MainFrom : Form
    {

        #region 数据定义
        /// <summary>
        /// 循环终结标识
        /// </summary>
        bool isSotp = false;
        /// <summary>
        /// 测试循环变量
        /// </summary>
        int iCount = 0;
        /// <summary>
        /// 测试循环变量
        /// </summary>
        int iCount2 = 0;
        /// <summary>
        /// 加密方式
        /// </summary>
        int encryptType = -1;
        #endregion

        #region 构造函数

        public MainFrom()
        {
            InitializeComponent();
        }
        #endregion

        #region 事件


        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (!isYz())
            {
                return;
            }
             
            string str2 = this.tb_Body.Text.Trim().Replace("\r\n", "");
            //  string str2 = ClientSendTest.Util.TxtHelper.Read(@"G:\Log\QQ\895966408\FileRecv\C206.txt");
            string str = SocketSend(str2, this.cb_EncryptionType.SelectedIndex);
            AddListBox(str);

        }

        private void MainFrom_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectFIle_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.Filter = "数字证书 (*.crt)|*.crt|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.tb_CciePath.Text = fileDialog1.FileName;
                Constant.CCIEPATH = fileDialog1.FileName;
            }

        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_login_Click(object sender, EventArgs e)
        {
            if (this.tb_loginName.Text.Trim().Length <= 0)
            {
                MessageBox.Show("用户名称不能为空！");
                return;
            }
            if (this.tb_Pwd.Text.Trim().Length <= 0)
            {
                MessageBox.Show("密码不能为空！");
                return;
            }
            if (this.tb_IP.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请求IP不能为空！");
                return;
            }
            if (this.tb_Port.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请求端口不能为空！");
                return;
            }
            if (Constant.CCIEPATH.Trim().Length <= 0)
            {
                MessageBox.Show("请先选择数字证书！");
                return;
            }
           
            string str = TxtHelper.Read(System.Windows.Forms.Application.StartupPath + "\\login.txt").Replace("USERIDVALUE", this.tb_loginName.Text.Trim()).Replace("USERPWDVALUE", this.tb_Pwd.Text);
            //根据C004请求加密算法，设置加密方式为RSA算法
            str = SocketSend(str.Replace("\r\n", ""), 1);
            DataTable dt = JsonUtil.JsonToDataTable(str);
            try
            {
                if (dt.Rows.Count > 0 && dt.Rows[0]["RspCode"].ToString() == "00000000")
                {
                    this.tb_SessionID.Text = dt.Rows[0]["session_id"].ToString();
                    this.tb_SessionKeys.Text = dt.Rows[0]["session_key"].ToString();

                    ClientSendTest.Util.ConfigHelper ch = new ConfigHelper(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                    ch.SetValue("Info", "ID", this.tb_SessionID.Text);
                    ch.SetValue("Info", "Key", this.tb_SessionKeys.Text);
                }
                if (str != null)
                {
                    AddListBox(str);
                }
            }
            catch (Exception)
            {


            }

        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.lb_Receive.SelectedItem != null)
            {
                Clipboard.SetDataObject(lb_Receive.SelectedItem);
            }
        }
        #endregion

        #region  公用方法

        public bool isYz()
        {
            if (this.tb_IP.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请求IP不能为空！");
                return false;
            }
            if (this.tb_Port.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请求端口不能为空！");
                return false;
            }
            if (this.cb_EncryptionType.Text.Trim().Length <= 0)
            {
                MessageBox.Show("加密方式！");
                return false;
            }
            if (Constant.CCIEPATH.Trim().Length <= 0)
            {
                MessageBox.Show("请先选择数字证书！");
                return false;
            }
            if (tb_SessionID.Text.Length <= 0)
            {
                MessageBox.Show("请先登录！");
                return false;
            }

            if (tb_SessionKeys.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请先登录！");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            this.cb_EncryptionType.Items.Add("无加密");
            this.cb_EncryptionType.Items.Add("RSA算法加密");
            this.cb_EncryptionType.Items.Add("3DES加密（会话密钥）");
            this.cb_EncryptionType.Items.Add("3DES加密（默认密钥）");
            this.cb_EncryptionType.Items.Add("ZIP压缩");
            this.cb_EncryptionType.Items.Add("先ZIP压缩后再3DES加密(会话密钥)");
            this.cb_EncryptionType.Items.Add("先ZIP压缩后再3DES加密(默认密密钥)");
            this.cb_EncryptionType.SelectedIndex = 0;


            this.tb_SessionID.Text = Constant.SESSION_ID;
            this.tb_SessionKeys.Text = Constant.SESSION_KEY;

            ClientSendTest.Util.ConfigHelper ch = new ConfigHelper(System.Windows.Forms.Application.StartupPath + "\\config.ini");
            this.tb_SessionID.Text = ch.ReadInivalue("Info", "ID");
            this.tb_SessionKeys.Text = ch.ReadInivalue("Info", "Key");

            string strFile = System.Windows.Forms.Application.StartupPath + this.tb_CciePath.Text;
            if (System.IO.File.Exists(strFile))
            {
                this.tb_CciePath.Text = strFile;
                Constant.CCIEPATH = strFile;
            }
            else
            {
                this.tb_CciePath.Text = string.Empty;
            }
        }

        /// <summary>
        /// 写入返回值
        /// </summary>
        /// <param name="strText"></param>
        public void AddListBox(string strText)
        {
            this.lb_Receive.Items.Add(strText);
            this.lb_Receive.SelectedIndex = this.lb_Receive.Items.Count - 1;
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="EncryptionType"></param>
        /// <returns></returns>
        public string SocketSend(string strText, int EncryptionType)
        {
            Constant.IV_DEFAULT = this.tb_IV.Text;
            Constant.SESSION_KEY_DEFAULT = this.tb_Keys.Text;
            Constant.SESSION_KEYS = this.tb_SessionKeys.Text.Trim();
            Constant.SESSION_ID = this.tb_SessionID.Text.Trim();
            Constant.SESSION_CODE = this.tb_Code.Text.Trim();

            SocketChannel socketchannel = new SocketChannel(Constant.SOCKETTIME);
            bool bLinked = socketchannel.ConnectTransServer(this.tb_IP.Text, this.tb_Port.Text);
            string sRecvMsg = "";
            if (bLinked)
            {
                socketchannel.SendGoldMsg(strText.Replace("\r\n", ""), EncryptionType);
                //接收报文
                sRecvMsg = socketchannel.RecvGoldMsg();
            }
            else
            {
                return "Socket连接不正确,请检查IP和端口是否正确！";
            }

            //关闭socket链接
            if (socketchannel != null)
                socketchannel.CloseSocket();
            return sRecvMsg;
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="EncryptionType"></param>
        /// <returns></returns>
        public void SocketSend()
        {
            while (!isSotp)
            {

                Constant.IV_DEFAULT = this.tb_IV.Text;
                Constant.SESSION_KEY_DEFAULT = this.tb_Keys.Text;
                Constant.SESSION_KEYS = this.tb_SessionKeys.Text.Trim();
                Constant.SESSION_ID = this.tb_SessionID.Text.Trim();

                SocketChannel socketchannel = new SocketChannel(Constant.SOCKETTIME);
                bool bLinked = socketchannel.ConnectTransServer(this.tb_IP.Text, this.tb_Port.Text);
                string sRecvMsg = "";
                if (bLinked)
                {
                    string strBody = TxtHelper.Read(System.Windows.Forms.Application.StartupPath + "\\Test.txt");
                    iCount += 1;
                    socketchannel.SendGoldMsg(strBody.Replace("\r\n", ""), encryptType);

                    //接收报文
                    sRecvMsg = socketchannel.RecvGoldMsg();
                    if (sRecvMsg != null)
                    {
                        iCount2 += 1;
                    }
                }

                //关闭socket链接
                if (socketchannel != null)
                    socketchannel.CloseSocket();
            }


        }

        #endregion
       
        #region test

        //public void test()
        //{
        //    DateTime dt = DateTime.Now;

        //    for (int i = 0; i < int.Parse(this.tb_Num.Text); i++)
        //    {

        //        Thread t1 = new Thread(new ThreadStart(SocketSend));
        //        // t1.Name = "th" + i;
        //        t1.Start();
        //    }
        //}
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    this.timer1.Enabled = false;
        //    isSotp = true;

        //    AddListBox("压力测试执行结束：发送：" + iCount.ToString() + "接收：" + iCount2.ToString());
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (tb_SessionID.Text.Length <= 0)
        //    {
        //        MessageBox.Show("请先登录！");
        //        return ;
        //    }

        //    if (tb_SessionKeys.Text.Trim().Length <= 0)
        //    {
        //        MessageBox.Show("请先登录！");
        //        return ;
        //    }

        //    encryptType = this.cb_testType.SelectedIndex;
        //    AddListBox("正在执行压力测试,加密方式：" + encryptType + ",线程个数：" + this.tb_Num.Text + ",线程执行时间：" + this.numericUpDown1.Value + "毫秒");
        //    this.timer1.Interval = (int)this.numericUpDown1.Value;
        //    this.timer1.Enabled = true;
        //    test();
        //}
        #endregion

    }
}