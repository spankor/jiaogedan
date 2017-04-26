using System;
using System.Collections.Generic;
using System.Text;
using ClientSendTest.Util;

namespace ClientSendTest.Models
{
    /// <summary>
    /// 响应报文头
    /// </summary>
    class RspHead
    {
        /// <summary>
        /// 报文长度
        /// </summary>
        public string msg_len = "";
        /// <summary>
        /// 流水号
        /// </summary>
        public string seq_no = "";
        /// <summary>
        /// 报文类型
        /// </summary>
        public string msg_type = "";
        /// <summary>
        /// 交易方向
        /// </summary>
        public string exch_code = "";
        /// <summary>
        /// 报文标识
        /// </summary>
        public string msg_flag = "";
        /// <summary>
        /// 终端来源
        /// </summary>
        public string term_type = "";
        /// <summary>
        /// 用户类型
        /// </summary>
        public string user_type = "";
        /// <summary>
        /// 用户ID
        /// </summary>
        public string user_id = "";
        /// <summary>
        /// 地区代码
        /// </summary>
        public string area_code = "";
        /// <summary>
        /// 代理机构
        /// </summary>
        public string branch_id = "";
        /// <summary>
        /// 响应代码
        /// </summary>
        public string rsp_code = "";
        /// <summary>
        /// 响应信息
        /// </summary>
        public string rsp_msg = "";


        /// <summary>
        /// 解析响应报文头
        /// </summary>
        /// <param name="strMs"></param>
        public void Parse(string v_sMsg)
        {
            int[] iLens = new int[] { 8, 8, 1, 4, 1, 2, 2, 10, 4, 12,8,10 };
            string[] sValues = CommUtil.SplitFastString(v_sMsg, iLens);
            this.msg_len = sValues[0];
            this.seq_no = sValues[1];
            this.msg_type = sValues[2];
            this.exch_code = sValues[3];
            this.msg_flag = sValues[4];
            this.term_type = sValues[5];
            this.user_type = sValues[6];
            this.user_id = sValues[7];
            this.area_code = sValues[8];
            this.branch_id = sValues[9];
            this.rsp_code = sValues[10];
            this.rsp_msg = sValues[11];
        }
    }
}
