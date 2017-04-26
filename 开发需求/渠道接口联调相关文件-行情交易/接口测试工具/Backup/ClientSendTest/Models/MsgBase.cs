using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ClientSendTest.Util;
using ClientSendTest.Net;

namespace ClientSendTest.Models
{
    /// <summary>
    /// �ƽ��׶���ϵͳ���Ļ���
    /// </summary>
    public class MsgBase
    {
        

        /// <summary>
        /// ���ɱ����ַ���
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((char)Constant.SEPARATOR_SEGMENT);
            foreach (FieldInfo field in this.GetType().GetFields())
            {
                string str = "";
                object obj = field.GetValue(this);
                if (obj != null)
                    str = obj.ToString();
                if (str != null && str.Length > 0)
                {
                    sb.Append(field.Name);
                    sb.Append("=");
                    sb.Append(str);
                    sb.Append((char)Constant.SEPARATOR_SEGMENT);
                }
            }
            return sb.ToString();
        }

        
         
    }
}
