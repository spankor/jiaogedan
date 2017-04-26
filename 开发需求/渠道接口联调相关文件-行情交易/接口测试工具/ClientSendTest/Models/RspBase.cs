using System;
using System.Collections.Generic;
using System.Text;

namespace ClientSendTest.Models
{
    /// <summary>
    /// 响应报文体基类
    /// </summary>
    public class RspBase:MsgBase
    {
        /// <summary>
        /// 响应码,"00"表示成功，其它代表失败
        /// </summary>
        public string rsp_code = "";

        /// <summary>
        /// 响应信息
        /// </summary>
        public string rsp_msg = "";

        /// <summary>
        /// 回传字段
        /// </summary>
        public string client_code = "";

    }
}
