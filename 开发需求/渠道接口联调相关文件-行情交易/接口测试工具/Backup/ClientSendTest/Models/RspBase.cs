using System;
using System.Collections.Generic;
using System.Text;

namespace ClientSendTest.Models
{
    /// <summary>
    /// ��Ӧ���������
    /// </summary>
    public class RspBase:MsgBase
    {
        /// <summary>
        /// ��Ӧ��,"00"��ʾ�ɹ�����������ʧ��
        /// </summary>
        public string rsp_code = "";

        /// <summary>
        /// ��Ӧ��Ϣ
        /// </summary>
        public string rsp_msg = "";

        /// <summary>
        /// �ش��ֶ�
        /// </summary>
        public string client_code = "";

    }
}
