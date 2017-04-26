namespace ClientSendTest
{
    partial class MainFrom
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_Send = new System.Windows.Forms.Button();
            this.cb_EncryptionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Keys = new System.Windows.Forms.TextBox();
            this.tb_Body = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_Receive = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_IV = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Code = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_Pwd = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_loginName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_login = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SelectFIle = new System.Windows.Forms.Button();
            this.tb_CciePath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_IP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Port = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_SessionID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_SessionKeys = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(625, 60);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 23);
            this.btn_Send.TabIndex = 13;
            this.btn_Send.Text = "发送报文";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // cb_EncryptionType
            // 
            this.cb_EncryptionType.FormattingEnabled = true;
            this.cb_EncryptionType.Location = new System.Drawing.Point(92, 22);
            this.cb_EncryptionType.Name = "cb_EncryptionType";
            this.cb_EncryptionType.Size = new System.Drawing.Size(151, 20);
            this.cb_EncryptionType.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "加密方式：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "默认密钥：";
            // 
            // tb_Keys
            // 
            this.tb_Keys.Location = new System.Drawing.Point(422, 19);
            this.tb_Keys.Name = "tb_Keys";
            this.tb_Keys.Size = new System.Drawing.Size(151, 21);
            this.tb_Keys.TabIndex = 10;
            this.tb_Keys.Text = "FAJA2LFA9WR9FSASF92FMMAF";
            // 
            // tb_Body
            // 
            this.tb_Body.Location = new System.Drawing.Point(79, 33);
            this.tb_Body.Multiline = true;
            this.tb_Body.Name = "tb_Body";
            this.tb_Body.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Body.Size = new System.Drawing.Size(820, 153);
            this.tb_Body.TabIndex = 6;
            this.tb_Body.Text = "{\r\n    \"SerialNo\": \"\",\r\n    \"ExchCode\": \"C999\",\r\n    \"oper_flag\": 1,\r\n    \"UserID" +
                "\": \"0000000205\",\r\n    \"RspCode\": \"\",\r\n    \"RspMsg\": \"\",\r\n    \"rsp_encrypt_mode\":" +
                "\"1\"\r\n}";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "报文体：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 380);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "接收报文：";
            // 
            // lb_Receive
            // 
            this.lb_Receive.ContextMenuStrip = this.contextMenuStrip1;
            this.lb_Receive.FormattingEnabled = true;
            this.lb_Receive.HorizontalScrollbar = true;
            this.lb_Receive.ItemHeight = 12;
            this.lb_Receive.Location = new System.Drawing.Point(93, 371);
            this.lb_Receive.Name = "lb_Receive";
            this.lb_Receive.Size = new System.Drawing.Size(844, 268);
            this.lb_Receive.TabIndex = 15;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "复制";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // tb_IV
            // 
            this.tb_IV.Location = new System.Drawing.Point(422, 57);
            this.tb_IV.Name = "tb_IV";
            this.tb_IV.Size = new System.Drawing.Size(151, 21);
            this.tb_IV.TabIndex = 12;
            this.tb_IV.Text = "8A402C31";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(360, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "IV向量：";
            // 
            // tb_Code
            // 
            this.tb_Code.Location = new System.Drawing.Point(93, 63);
            this.tb_Code.Name = "tb_Code";
            this.tb_Code.Size = new System.Drawing.Size(151, 21);
            this.tb_Code.TabIndex = 11;
            this.tb_Code.Text = "C000";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 21;
            this.label10.Text = "证书编号：";
            // 
            // tb_Pwd
            // 
            this.tb_Pwd.Location = new System.Drawing.Point(345, 117);
            this.tb_Pwd.Name = "tb_Pwd";
            this.tb_Pwd.Size = new System.Drawing.Size(151, 21);
            this.tb_Pwd.TabIndex = 5;
            this.tb_Pwd.Text = "b6153be014b1af1811c85aa770c4cc46";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(298, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "密码：";
            // 
            // tb_loginName
            // 
            this.tb_loginName.Location = new System.Drawing.Point(112, 120);
            this.tb_loginName.Name = "tb_loginName";
            this.tb_loginName.Size = new System.Drawing.Size(151, 21);
            this.tb_loginName.TabIndex = 4;
            this.tb_loginName.Text = "1080179310";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(47, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 23;
            this.label12.Text = "用户名：";
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(112, 156);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 6;
            this.btn_login.Text = "登陆";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(940, 359);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(932, 333);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "登陆";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_SelectFIle);
            this.groupBox2.Controls.Add(this.tb_CciePath);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tb_loginName);
            this.groupBox2.Controls.Add(this.btn_login);
            this.groupBox2.Controls.Add(this.tb_Pwd);
            this.groupBox2.Controls.Add(this.tb_IP);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.tb_Port);
            this.groupBox2.Location = new System.Drawing.Point(25, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(889, 195);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "登陆信息";
            // 
            // btn_SelectFIle
            // 
            this.btn_SelectFIle.Location = new System.Drawing.Point(527, 34);
            this.btn_SelectFIle.Name = "btn_SelectFIle";
            this.btn_SelectFIle.Size = new System.Drawing.Size(75, 23);
            this.btn_SelectFIle.TabIndex = 34;
            this.btn_SelectFIle.Text = "选择证书";
            this.btn_SelectFIle.UseVisualStyleBackColor = true;
            this.btn_SelectFIle.Click += new System.EventHandler(this.btn_SelectFIle_Click);
            // 
            // tb_CciePath
            // 
            this.tb_CciePath.Location = new System.Drawing.Point(112, 34);
            this.tb_CciePath.Name = "tb_CciePath";
            this.tb_CciePath.Size = new System.Drawing.Size(384, 21);
            this.tb_CciePath.TabIndex = 1;
            this.tb_CciePath.Text = "\\\\SSL\\\\server.crt";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 32;
            this.label9.Text = "服务器公钥：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "链接IP：";
            // 
            // tb_IP
            // 
            this.tb_IP.Location = new System.Drawing.Point(112, 78);
            this.tb_IP.Name = "tb_IP";
            this.tb_IP.Size = new System.Drawing.Size(151, 21);
            this.tb_IP.TabIndex = 2;
            this.tb_IP.Text = "117.141.138.101";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(274, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "链接端口：";
            // 
            // tb_Port
            // 
            this.tb_Port.Location = new System.Drawing.Point(345, 78);
            this.tb_Port.Name = "tb_Port";
            this.tb_Port.Size = new System.Drawing.Size(151, 21);
            this.tb_Port.TabIndex = 3;
            this.tb_Port.Text = "41901";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.tb_SessionID);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.tb_SessionKeys);
            this.groupBox1.Location = new System.Drawing.Point(25, 248);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(889, 79);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登陆后得到信息";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(534, 33);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(281, 12);
            this.label15.TabIndex = 41;
            this.label15.Text = "会话ID和会话密钥有时间限制，过期后请重新登录！";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(53, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 38;
            this.label13.Text = "会话ID：";
            // 
            // tb_SessionID
            // 
            this.tb_SessionID.Location = new System.Drawing.Point(112, 28);
            this.tb_SessionID.Name = "tb_SessionID";
            this.tb_SessionID.Size = new System.Drawing.Size(151, 21);
            this.tb_SessionID.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(274, 33);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 40;
            this.label14.Text = "会话密钥：";
            // 
            // tb_SessionKeys
            // 
            this.tb_SessionKeys.Location = new System.Drawing.Point(345, 30);
            this.tb_SessionKeys.Name = "tb_SessionKeys";
            this.tb_SessionKeys.Size = new System.Drawing.Size(151, 21);
            this.tb_SessionKeys.TabIndex = 8;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(932, 333);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测试";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tb_Body);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(3, 107);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(905, 201);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "报文信息";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_Keys);
            this.groupBox3.Controls.Add(this.cb_EncryptionType);
            this.groupBox3.Controls.Add(this.btn_Send);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tb_IV);
            this.groupBox3.Controls.Add(this.tb_Code);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(7, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(901, 95);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "加密信息";
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(91, 642);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(491, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "服务器主动断开连接情况：1、会话ID无效！2、请求加密方式不正确！3、请求报文不正确！";
            // 
            // MainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 660);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lb_Receive);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "渠道终端测试程序";
            this.Load += new System.EventHandler(this.MainFrom_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.ComboBox cb_EncryptionType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Keys;
        private System.Windows.Forms.TextBox tb_Body;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lb_Receive;
        private System.Windows.Forms.TextBox tb_IV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Code;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_Pwd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_loginName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tb_SessionID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_SessionKeys;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_IP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Port;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_SelectFIle;
        private System.Windows.Forms.TextBox tb_CciePath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label15;
    }
}

