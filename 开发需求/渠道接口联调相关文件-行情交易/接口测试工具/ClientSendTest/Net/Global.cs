using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
 
using ClientSendTest.Models;

namespace ClientSendTest.Net
{
    /// <summary>
    /// ȫ�ֱ���
    /// </summary>
    public class Global
    {
         
       


        /// <summary>
        /// ���ӷ�ʽ 1����֤  2��ֱ��
        /// </summary>
        public static string CONNECT_TYPE = "1";



        /// <summary>
        /// ���н��׹����������ͷ
        /// </summary>
        public static ReqHead GReqHead = new ReqHead();

        /// <summary>
        /// ����ģʽ��1�������� 2��ʹ��Ĭ����Կ���� 3��ʹ����֤��ʱ��Կ����
        /// </summary>
        public static int ENCRYPT_MODEL = 2;

        /// <summary>
        /// ��������
        /// </summary>
        public static string GExchDate = "";

        /// <summary>
        /// ������ʱ��
        /// </summary>
        public static string GSysDate = "";

        /// <summary>
        /// ϵͳ״̬
        /// </summary>
        public static string GSysStat = "";

        /// <summary>
        /// ��ȫУ����
        /// </summary>
        public static string GCheckCode = "";

        /// <summary>
        /// ��¼����
        /// </summary>
        public static string GPassWord = "";

        /// <summary>
        /// ��������ʱʱ�䣨���룩
        /// </summary>
        public static int GHeartOverTime = 3000;
       

       
    }
}
