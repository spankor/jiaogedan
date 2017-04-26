using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Timers;
using System.Xml;
using GessMonitoring.Modles;

namespace GessMonitoring.Util
{
    public class EmailHelper
    {

        #region sendTheMail  
        /// <summary>
        /// 实现邮件发送的一个过程
        /// </summary>
        /// <param name="smtpserver">邮件服务器smtp.163.com表示网易邮箱服务器</param>
        /// <param name="smptport">端口号（通常网易和qq为25）</param>
        /// <param name="userName">发送端账号</param>
        /// <param name="pwd">发送端密码</param>
        /// <param name="strfrom">发送端账号</param>
        /// <param name="strto">注册的邮箱号</param>
        /// <param name="subj">邮箱的主题</param>
        /// <param name="bodys">发送的邮件正文</param>
        /// <returns></returns>
        protected bool sendTheMail(string smtpserver, string smptport, string userName, string pwd, string strfrom, string strto, string subj, string bodys)
        {
            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式  
            _smtpClient.Host = smtpserver;//指定SMTP服务器  
            _smtpClient.UseDefaultCredentials = true;   //获取或设置 Boolean 值，该值控制 DefaultCredentials 是否随请求一起发送。（放在下面的那一句之前）
            _smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);//用户名和密码  
            MailMessage _mailMessage = new MailMessage(strfrom, strto);
            _mailMessage.Subject = subj;//主题  
            _mailMessage.Body = bodys;//内容  
            _mailMessage.BodyEncoding = System.Text.Encoding.Default;//正文编码  
            _mailMessage.IsBodyHtml = true;//设置为HTML格式  
            _mailMessage.Priority = MailPriority.High;//优先级  

            try
            {
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region SendTEmail  
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Content">内容</param>
        /// <returns></returns>
        public bool SendTEmail(string Title, string Content)
        {
            string strList = TxtHelper.Read(AppDomain.CurrentDomain.BaseDirectory + "EmailConfig.txt");
            EmailClass myField = Newtonsoft.Json.JsonConvert.DeserializeObject<EmailClass>(strList);
            if (myField != null)
            {
                bool bl = sendTheMail(myField.SmtpServer, myField.SmpPort, myField.EmailLoginName, myField.EmailPwd, myField.EmailLoginName, myField.Emailreceive, Title, Content);
                return bl;
            }
            else
            {
                return false;
            }

        }
        #endregion
    }
}
