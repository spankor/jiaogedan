using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
 
using ClientSendTest.Models;

namespace ClientSendTest.Net
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class Global
    {
         
       


        /// <summary>
        /// 连接方式 1：认证  2：直连
        /// </summary>
        public static string CONNECT_TYPE = "1";



        /// <summary>
        /// 所有交易共享的请求报文头
        /// </summary>
        public static ReqHead GReqHead = new ReqHead();

        /// <summary>
        /// 加密模式：1：不加密 2：使用默认密钥加密 3：使用认证临时密钥加密
        /// </summary>
        public static int ENCRYPT_MODEL = 2;

        /// <summary>
        /// 交易日期
        /// </summary>
        public static string GExchDate = "";

        /// <summary>
        /// 服务器时间
        /// </summary>
        public static string GSysDate = "";

        /// <summary>
        /// 系统状态
        /// </summary>
        public static string GSysStat = "";

        /// <summary>
        /// 安全校验码
        /// </summary>
        public static string GCheckCode = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        public static string GPassWord = "";

        /// <summary>
        /// 心跳包超时时间（毫秒）
        /// </summary>
        public static int GHeartOverTime = 3000;
       

       
    }
}
