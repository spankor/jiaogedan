using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Net;
using ClientSendTest.Util;
using System.Data;
using System.Collections;
using System.Runtime.InteropServices;
using ClientSendTest.Models;
using ClientSendTest.Net;

namespace ClientSendTest.Util
{
    /// <summary>
    /// 工具类 
    /// </summary>
    public class CommUtil
    {
        private static string s_WriteToFileLock = "0";

        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入一个配置参数值
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <param name="value">参数值</param>
        public static void WriteIniValue(string section, string key, string value, string filePath)
        {
            int i = WritePrivateProfileString(section, key, value, filePath);
        }

        /// <summary>
        /// 读取一个配置参数值
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <returns>参数的值</returns>
        public static string ReadIniValue(string section, string key, string def, string filePath)
        {
            StringBuilder buff = new StringBuilder(1000);
            int i = GetPrivateProfileString(section, key, def, buff, 1000, filePath);
            return buff.ToString().Trim();
        }

        /// <summary>
        /// 读取一个配置参数值
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <returns>参数的值</returns>
        public string ReadIniValue(string section, string key, string filePath)
        {
            StringBuilder buff = new StringBuilder(1000);
            int i = GetPrivateProfileString(section, key, "", buff, 1000, filePath);
            return buff.ToString();
        }

        /// <summary>
        /// 将字节数组转换成字符串
        /// </summary>
        /// <param name="v_bMsgs">原始字节数组</param>
        /// <returns></returns>
        public static string ConvertString(byte[] v_bMsg)
        {
            if (v_bMsg == null)
                return "";
            else
                return Encoding.Default.GetString(v_bMsg);
        }

        /// <summary>
        /// 将字符串转换为字节数组
        /// </summary>
        /// <param name="v_sMsg">原始字符串</param>
        /// <returns></returns>
        public static byte[] ConvertBytes(string v_sMsg)
        {
            if (v_sMsg == null)
                v_sMsg = "";
            return Encoding.Default.GetBytes(v_sMsg);
        }

        /// <summary>
        /// 计算字符串的字节长度，汉字为两个字节长度
        /// </summary>
        /// <param name="v_sMsg">原始字符串</param>
        /// <returns></returns>
        public static int GetBytesLen(string v_sMsg)
        {
            if (v_sMsg == null)
                return 0;
            else
                return Encoding.Default.GetByteCount(v_sMsg);
        }
        /// <summary>
        /// 判断字符串是否是由数字和字母组成
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumberOrLetter(string value)
        {
            return new Regex(@"^[A-Za-z0-9]+$").IsMatch(value);
        }
        /// <summary>
        /// 字符串如果NULL返回""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceNull(string str)
        {
            if (null == str)
            {
                return "";
            }
            else
                return str;
        }

        /// <summary>
        /// 填充字符串
        /// </summary>
        /// <param name="v_sSrc">原始字符串</param>
        /// <param name="v_cFill">填充字符</param>
        /// <param name="v_iLen">填充后整体长度</param>
        /// <param name="v_cDire">填充方向,'L/R'</param>
        /// <returns></returns>
        public static string Fill(string v_sSrc, char v_cFill, int v_iLen, char v_cDire)
        {

            int iSrcLen = Encoding.Default.GetByteCount(v_sSrc);

            if (iSrcLen > v_iLen)
            {
                return v_sSrc;
            }
            else
            {
                string sFile = "";
                for (int i = 0; i < v_iLen - iSrcLen; i++)
                {
                    sFile += v_cFill;
                }

                if (v_cDire == 'L')
                {
                    return sFile + v_sSrc;
                }
                else
                {
                    return v_sSrc + sFile;
                }
            }
        }

        /// <summary>
        /// 从sMsg字符串中获取变量名为sFieldKey的值  
        /// </summary>
        /// <param name="v_strMsg">报文字符串</param>
        /// <param name="v_strFieldKey">字段名</param>
        /// <returns></returns>
        public static string GetMsgFieldValue(string v_strMsg, string v_strFieldKey, char[] v_cSeparator)
        {
            v_strFieldKey += "=";
            int index = v_strMsg.IndexOf(v_strFieldKey);
            if (index >= 0)
            {
                String temp = v_strMsg.Substring(index + v_strFieldKey.Length);
                String[] splitA = temp.Split(v_cSeparator);
                if (splitA.Length >= 1)
                    return splitA[0];
            }
            return "";
        }

        /// <summary>
        /// 获得广播消息字段
        /// </summary>
        /// <param name="v_strMsg"></param>
        /// <param name="v_strFieldKey"></param>
        /// <returns></returns>
        public static string GetGsiMsgFieldValue(string v_strMsg, string v_strFieldKey)
        {
            return GetMsgFieldValue(v_strMsg, v_strFieldKey, new char[] { Constant.GSI_EOFLD,Constant.GSI_EOREC });
        }

        /// <summary>
        /// 从sMsg字符串中获取变量名为sFieldKey的值  
        /// </summary>
        /// <param name="v_strMsg">报文字符串</param>
        /// <param name="v_strFieldKey">字段名</param>
        /// <returns></returns>
        public static string GetMsgFieldValue(string v_strMsg , string v_strFieldKey,char v_cSeparator)
        {
            //modify by csl 2009.7.5 支持参数值中包括 “#”和“=”，但如果字段的值中同时包括“#”和“=”则撞墙死了算了。

            if (v_strMsg.Length <= 0)
                return "";

            v_strFieldKey = v_cSeparator + v_strFieldKey + "=";            
            int index = v_strMsg.IndexOf(v_strFieldKey);
            if (index >= 0)
            {
                string sAferMsg = v_strMsg.Substring(index + v_strFieldKey.Length);

                int idx1 = sAferMsg.IndexOf(v_cSeparator);
                if (idx1 == -1)
                    return sAferMsg;
                else
                {
                    int idx2 = sAferMsg.IndexOf('=', idx1);
                    if (idx2 == -1)
                    {
                        int idx3 = sAferMsg.LastIndexOf(v_cSeparator);
                        if (idx3 == -1)
                            return sAferMsg;
                        else
                            return sAferMsg.Substring(0, idx3);
                    }
                    else
                    {
                        string sTempValue = sAferMsg.Substring(0, idx2 + 1);
                        int idx3 = sTempValue.LastIndexOf(v_cSeparator);
                        if (idx3 == -1)
                        {
                            return sAferMsg;
                        }
                        else
                        {
                            return sTempValue.Substring(0, idx3);
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// /// 从sMsg字符串中获取变量名为sFieldKey的值  
        /// </summary>
        /// <param name="v_sMsg"></param>
        /// <param name="v_sFieldKey"></param>
        /// <returns></returns>
        public static string GetMsgFieldValue(string v_sMsg, string v_sFieldKey)
        {
            return GetMsgFieldValue(v_sMsg, v_sFieldKey, Constant.SEPARATOR_SEGMENT);
        }

   
        
        /// <summary>
        /// 获得异常的详细信息
        /// </summary>
        /// <param name="v_exc"></param>
        /// <returns></returns>
        public static string GetExceptionMsg(Exception v_exc)
        {
            return "Exception.Message：" + v_exc.Message + "\n"
                + "Exception.HelpLink： " + v_exc.HelpLink + "\n"
                + "Exception.Source：" + v_exc.Source + "\n"
                + "Exception.StackTrace：" + v_exc.StackTrace + "\n"
                + "Exception.TargetSite：" + v_exc.TargetSite;
        }

        /// <summary>
        /// 解析定长报文 
        /// </summary>
        /// <param name="v_strFixedMsg">定长报文</param>
        /// <param name="v_lens">依次存放每个域的长度的整型数组</param>
        /// <returns></returns>
        public static string[] SplitFastString(string v_strFixedMsg, int[] v_lens)
        {
            int      iIndex = 0;
            string[] arrValue   = new string[v_lens.Length];
            for (int i = 0; i < v_lens.Length; i++)
            {
                arrValue[i] = v_strFixedMsg.Substring(iIndex, v_lens[i]);
                iIndex += v_lens[i];
            }
            return arrValue;
        }
      

        
        /// <summary>
        /// 从对象中获取字段的值
        /// </summary>
        /// <param name="v_obj"></param>
        /// <param name="v_sFieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue(object v_obj, string v_sFieldName)
        {
            FieldInfo finfo = v_obj.GetType().GetField(v_sFieldName);
            if (finfo != null)
                return finfo.GetValue(v_obj);
            return null;
        }


        
        /// <summary>
        /// 判断一个类型是否为另一个类型的子类
        /// </summary>
        /// <param name="v_subType">子类型</param>
        /// <param name="v_parentType">父类型</param>
        /// <returns></returns>
        public static bool CheckSubType(Type v_subType, Type v_parType)
        {
            if (v_subType.BaseType == null)
                return false;

            if (v_subType == v_parType)
                return true;

            return CheckSubType(v_subType.BaseType, v_parType);

        }

      

      

        /// <summary>
        /// 检查字符串是否为数字整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckStringIsInteger(string str)
        {
            if (str != null && str.Length > 0)
            {
                foreach (char c in str)
                {
                    if ( c < '0' || c > '9' )
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查字符串是否为符点型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckStringIsDouble(string str)
        {
            if (str != null && str.Length > 0)
            {
                foreach (char c in str)
                {
                    if ( ( c < '0' || c > '9') && c != '.')
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将字符串写入到文件中
        /// </summary>
        /// <param name="v_sFileName">全路径的文件名</param>
        /// <param name="v_str">字符串</param>
        /// <param name="v_bIsNewLine">是否在尾末添加换行符</param>
        /// <param name="v_bIsAppend">是否是追加模式</param>
        public static void WriteToFile(string v_sFileName, string v_str, bool v_bIsNewLine, bool v_bIsAppend)
        {
            lock (s_WriteToFileLock)
            {
                FileStream fs = null;
                try
                {
                    if (v_bIsAppend)
                        fs = new FileStream(v_sFileName, FileMode.Append);
                    else
                        fs = new FileStream(v_sFileName, FileMode.OpenOrCreate);
                    byte[] bytes = CommUtil.ConvertBytes(v_str);
                    fs.Write(bytes, 0, bytes.Length);

                    if (v_bIsNewLine)
                    {
                        fs.WriteByte(13);
                        fs.WriteByte(10);
                    }
                }
                catch (Exception e)
                {
                   
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
        }


        /// <summary>
        /// 将字符串写入到文件中
        /// </summary>
        /// <param name="v_sFileName">全路径的文件名</param>
        /// <param name="v_str">字符串数组</param>
        /// <param name="v_bIsNewLine">是否在尾末添加换行符</param>
        /// <param name="v_bIsAppend">是否是追加模式</param>
        public static void WriteToFile(string v_sFileName, string[] v_strs, bool v_bIsNewLine, bool v_bIsAppend)
        {
            lock (s_WriteToFileLock)
            {
                FileStream fs = null;
                try
                {

                    if (v_bIsAppend)
                        fs = new FileStream(v_sFileName, FileMode.Append);
                    else
                        fs = new FileStream(v_sFileName, FileMode.OpenOrCreate);

                    foreach (string sLine in v_strs)
                    {
                        byte[] bytes = CommUtil.ConvertBytes(sLine);
                        fs.Write(bytes, 0, bytes.Length);

                        if (v_bIsNewLine)
                        {
                            fs.WriteByte(13);
                            fs.WriteByte(10);
                        }
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
        }

        /// <summary>
        /// 将string类型转化为bool型
        /// </summary>
        /// <param name="v_sString"></param>
        /// <returns></returns>
        public static bool ConvertStringToBool(string v_sString)
        {
            bool bIsCheck = false;

            if (v_sString == "1" || v_sString.ToUpper() == "TRUE")
                bIsCheck = true;

            return bIsCheck;
        }

 

        /// <summary>
        /// 将小写的人民币转换为大写
        /// </summary>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static string GetCapitalization(double Money)
        {
            string CN_ZERO = "零";
            string CN_ONE = "壹";
            string CN_TWO = "贰";
            string CN_THREE = "叁";
            string CN_FOUR = "肆";
            string CN_FIVE = "伍";
            string CN_SIX = "陆";
            string CN_SEVEN = "柒";
            string CN_EIGHT = "捌";
            string CN_NINE = "玖";
            string CN_TEN = "拾";
            string CN_HUNDRED = "佰";
            string CN_THOUSAND = "仟";
            string CN_TEN_THOUSAND = "万";
            string CN_HUNDRED_MILLION = "亿";
            string CN_SYMBOL = "";//人民币
            string CN_YUAN = "元";
            string CN_TEN_CENT = "角";
            string CN_CENT = "分";
            string CN_INTEGER = "整";
            string CN_Negative = "负";
            //变量
            string integral; //整数部分
            string Float; //小数部分
            string outputCharacters; //大写金额
            long zeroCount;
            int i;
            long p;
            string d;
            long quotient;
            long modulus;

            string currencyDigits = Money.ToString("0.00");
            string[] parts = currencyDigits.Split('.');
            integral = parts[0];
            Float = parts[1];

            string[] digits = { CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE };
            string[] radices = { "", CN_TEN, CN_HUNDRED, CN_THOUSAND };
            string[] bigRadices = { "", CN_TEN_THOUSAND, CN_HUNDRED_MILLION, };
            string[] decimals = { CN_TEN_CENT, CN_CENT };

            //开始转换
            outputCharacters = "";
            //整数的转换
            if (long.Parse(integral) > 0)
            {
                zeroCount = 0;
                for (i = 0; i < integral.Length; i++)
                {
                    p = integral.Length - i - 1;
                    d = integral.Substring(i, 1);
                    quotient = p / 4;
                    modulus = p % 4;
                    if (d == "0")
                    {
                        zeroCount++;
                    }
                    else
                    {
                        if (zeroCount > 0)
                        {
                            outputCharacters += digits[0];
                        }
                        zeroCount = 0;
                        outputCharacters += digits[int.Parse(d)] + radices[modulus];
                    }
                    if (modulus == 0 && zeroCount < 4)
                    {
                        outputCharacters += bigRadices[quotient];
                    }
                }
                outputCharacters += CN_YUAN;
            }
            else if (int.Parse(integral) < 0)
            {
                zeroCount = 0;
                outputCharacters += CN_Negative;
                integral = integral.Substring(1, integral.Length);
                for (i = 0; i < integral.Length; i++)
                {
                    p = integral.Length - i - 1;
                    d = integral.Substring(i, 1);
                    quotient = p / 4;
                    modulus = p % 4;
                    if (d == "0")
                    {
                        zeroCount++;
                    }
                    else
                    {
                        if (zeroCount > 0)
                        {
                            outputCharacters += digits[0];
                        }
                        zeroCount = 0;
                        outputCharacters += digits[int.Parse(d)] + radices[modulus];
                    }
                    if (modulus == 0 && zeroCount < 4)
                    {
                        outputCharacters += bigRadices[quotient];
                    }
                }
                outputCharacters += CN_YUAN;
            }
            //小数的转换
            if (Float != "")
            {
                for (i = 0; i < Float.Length; i++)
                {
                    d = Float.Substring(i, 1);
                    if (d != "0")
                    {
                        outputCharacters += digits[int.Parse(d)] + decimals[i];
                    }
                }
            }
            if (outputCharacters == "")
            {
                outputCharacters = CN_ZERO + CN_YUAN;
            }

            if (int.Parse(Float) == 0)
            {
                outputCharacters += CN_INTEGER;
            }
            outputCharacters = CN_SYMBOL + outputCharacters;
            return outputCharacters;
        }

        /// <summary>
        /// 将小写的人民币转换为大写
        /// </summary>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static string GetCapitalization(string sMoney)
        {
            if (sMoney.Trim().Length > 0 && sMoney.Trim().Length < 16 && double.Parse(sMoney.Trim()) < 1000000000000.00)               
            {
                return GetCapitalization(double.Parse(sMoney));
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 转换金额的显示格式
        /// </summary>
        /// <param name="v_sFundValue"></param>
        /// <returns></returns>
        public static string FormatFund(string v_sFundValue)
        {
            if (v_sFundValue != null && v_sFundValue.Trim().Length > 0)
            {
                try
                {
                    v_sFundValue = string.Format("{0:N}", Convert.ToDouble(v_sFundValue));
                }
                catch (Exception ex)
                {
                    
                }
            }
            return v_sFundValue;
        }

        /// <summary>
        /// 转换重量的显示格式   i是保留小数点后面的位数  
        /// i(1-6)  超出范围则默认是空
        /// </summary>
        /// <param name="v_sFundValue"></param>
        /// <returns></returns>
        public static string FormatWeight(string v_sWeightValue,int i)
        {
            string num = i.ToString();
            if (v_sWeightValue != null && v_sWeightValue.Trim().Length > 0)
            {
                try
                {
                    if(i<1||i>6)
                        num="";
                    v_sWeightValue = string.Format("{0:N" + num + "}", Convert.ToDouble(v_sWeightValue));
                }
                catch (Exception ex)
                {
                   
                }
            }
            return v_sWeightValue;
        }
        

        /// <summary>
        /// 转换八位字符日期显示格式
        /// </summary>
        /// <param name="v_sDateTime"></param>
        /// <returns></returns>
        public static string FormatDateTime(string v_sDateTime)
        {
            string strYear = "0000";
            string strMonth = "00";
            string strDay = "00";

            if (v_sDateTime.Length == 8)
            {
                strYear = v_sDateTime.Substring(0, 4);
                strMonth = v_sDateTime.Substring(4, 2);
                strDay = v_sDateTime.Substring(6, 2);
            }
            else return v_sDateTime;

            return strYear + "-" + strMonth + "-" + strDay;
        }

        /// <summary>
        /// 转换六位数字字符时间显示格式
        /// </summary>
        /// <param name="v_sTime"></param>
        /// <returns></returns>
        public static string FormatTime(string v_sTime)
        {
            if (v_sTime.Length == 6)
                return v_sTime.Substring(0, 2) + ":" + v_sTime.Substring(2, 2) + ":" + v_sTime.Substring(4, 2);
            else
                return v_sTime;
        }

        /// <summary>
        /// 过滤字符串开头的*号和结尾的：号
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string FilterBlot(string sValue)
        {
            if (sValue.Substring(0, 1) == "*")
            {
                sValue = sValue.Substring(1);
            }
            if (sValue.Substring(sValue.Length - 1, 1) == "：")
            {
                sValue = sValue.Substring(0, sValue.Length - 1);
            }
            return sValue;
        }

        /// <summary>
        /// 将yyyyMMdd格式的日期字符串转化成yyyy-MM-dd的日期
        /// yyyyMMdd一定要是8位
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string strDate)
        {
            string strYear = "0000";
            string strMonth = "00";
            string strDay = "00";

            if (strDate.Length == 8)
            {
                strYear = strDate.Substring(0, 4);
                strMonth = strDate.Substring(4, 2);
                strDay = strDate.Substring(6, 2);   
            }

            return Convert.ToDateTime(strYear + "-" + strMonth + "-" + strDay);
        }

      
         

       

        /// <summary>
        /// 判断字符串是否可以转换为DateTime
        /// </summary>
        /// <param name="s">要判断的字符串</param>
        /// <returns>bool true=可以转换; false=不可转换</returns>
        public static bool IsDateTime(string s)
        {
            try
            {
                System.DateTime.Parse(s);
            }
            catch
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 判断字符串是否可以转换为数字
        /// </summary>
        /// <param name="value">要判断的字符串</param>
        /// <returns>true=可以转换;false=不可转换</returns>
        public static bool IsNumeric(object value)
        {
            try
            {
                double i = Convert.ToDouble(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// 替换字符串中的（"#","="）
        /// </summary>
        /// <param name="v_sValue">字符串</param>
        /// <returns></returns>
        public static string StrReplace(string v_sValue)
        {
            string strReturn = v_sValue;

            strReturn = strReturn.Replace("#", "＃");
            strReturn = strReturn.Replace("=", "＝");
            strReturn = strReturn.Replace("∧", "^");
            strReturn = strReturn.Replace("｜", "|");
            strReturn = strReturn.Replace("，", ",");
            //strReturn = strReturn.Replace("·", "。");//姓名中的点是不可过滤的
            strReturn = strReturn.Replace("\\", "＼");
            strReturn = strReturn.Replace("'", "＇");

            return strReturn;
        }

         

        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="v_sFilePath">文件夹名</param>
        public static void ClearPathFile(string v_sFilePath)
        {
            string[] stemparr = null;

            //得到所有文件
            stemparr = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + v_sFilePath);

            for (int i = 0; i < stemparr.Length; i++)
            {
                File.Delete(stemparr[i]);
            }
        }

        /// <summary>
        /// 检查电话号码
        /// </summary>
        /// <param name="v_sTelephone">电话号</param>
        /// <param name="v_sType">号码类型 1、固话 2、传真 3、手机</param>
        /// <returns></returns>
        public static bool CheckTelephone(string v_sTelephone,string v_sType)
        {
            string sContains = "";

            switch (v_sType)
            {
                case "1":
                    sContains = "0123456789()+-";                    
                    break;
                case "2":
                    sContains = "0123456789()+-";
                    break;
                case "3":
                    sContains = "0123456789";
                    break;
                default:
                    return true;
                    break;
            }

            return CheckStrContains(v_sTelephone, sContains);

        }

        /// <summary>
        /// 检查字符串中是否包含设定之外的字符
        /// </summary>
        /// <param name="v_sStr">待检查字符串</param>
        /// <param name="v_sContains">字符范围</param>
        /// <returns></returns>
        private static bool CheckStrContains(string v_sStr, string v_sContains) 
        {
            if (v_sStr != null && v_sStr.Length > 0)
            {
                foreach (char c in v_sStr)
                {
                    if (!v_sContains.Contains(c.ToString()))
                        return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断字符串中是否含有汉字
        /// </summary>
        /// <param name="words">待判断的字符串</param>
        /// <returns>true含有汉字 || false不含汉字</returns>
        public static bool WordsIScn(string words)
        {
            bool b = false;

            for (int i = 0; i < words.Length; i++)
            {
                Regex rx = new Regex("^[\u4e00-\u9fa5]$");
                if (rx.IsMatch(words[i].ToString()))
                {
                    b = true;
                    break;
                }
            }

            return b;
        }
        
        /// <summary>
        /// 截取节点名称字符 如：[2001]abcd截取为abcd
        /// </summary>
        /// <param name="NodeText"></param>
        /// <returns></returns>
        public static string NodeTextSubString(string NodeText)
        {
            string ReturnValue = NodeText;
            int iStart=NodeText.LastIndexOf("[");
            int iEnd=NodeText.LastIndexOf("]");
            int iLengthFirst=NodeText.Length;
            int iLengthLast=NodeText.Replace("[","").Replace("]","").Length;
            if (iStart > -1 && iEnd > -1 && iEnd > iStart && iLengthFirst - iLengthLast == 2)
            {
                ReturnValue = NodeText.Substring(iEnd + 1);
            }
            return ReturnValue;
        }


        //======================================================
        //   风险控制
        //======================================================

        /// <summary>
        /// 保留2位小数
        /// </summary>
        /// <param name="inpString"></param>
        /// <returns></returns>
        public static string FormatDecimal(string inpString)
        {
            if (inpString.Trim() == string.Empty)
                return "";
            else
                return Convert.ToDecimal(inpString).ToString("0.00");
        }

        /// <summary>
        /// 整数
        /// </summary>
        /// <param name="inpString"></param>
        /// <returns></returns>
        public static string FormatInt(string inpString)
        {
            if (inpString.Trim() == string.Empty)
                return "0";
            else
                return Convert.ToInt32(inpString).ToString();
        }
 
        /// <summary>
        /// 转化成百分数
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string FormatPercent(object o)
        {
            if(o.GetType()==Type.GetType("System.Double"))
            {
                return (Convert.ToDouble(o) * 100).ToString()+"%";
            }
            else if(o.GetType()==Type.GetType("System.Decimal"))
            {
                return (Convert.ToDecimal(o) * 100).ToString()+"%";
            }
            else if (o.GetType() == Type.GetType("System.String"))
            {
                try
                {
                    decimal dValue = Convert.ToDecimal(o);
                    return FormatPercent(dValue);
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// 百分数转换成Decimal
        /// </summary>
        /// <returns></returns>
        public static string PercentRevert(string inpString)
        {
            if (inpString.EndsWith("%"))
            {
                try
                {
                    decimal dValue = Convert.ToDecimal(inpString.Replace("%", ""));
                    return (dValue / 100).ToString();
                }
                catch (Exception ex)
                {
                   
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获得首选的本地IPv4的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPv4Address()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            foreach (IPAddress ip in ips)
            {
                string sIp = ip.ToString();

                if (sIp.IndexOf('.') != -1)
                    return sIp;

            }

            return "127.0.0.1";
        }

        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="vSrcBuff">复制的源字节数组</param>
        /// <param name="vStartIndex">起始复制的位置</param>
        /// <param name="vCopyLen">复制内容的长度</param>
        /// <returns>返回复制结果</returns>
        public static byte[] ArrayCopy(byte[] vSrcBuff, int vStartIndex, int vCopyLen)
        {
            byte[] destBuff = null;
            if ((vStartIndex + vCopyLen) > vSrcBuff.Length)
                destBuff = new byte[vSrcBuff.Length - vStartIndex];
            else
                destBuff = new byte[vCopyLen];
            for (int i = 0; i < vCopyLen && (i + vStartIndex) < vSrcBuff.Length; i++)
            {
                destBuff[i] = vSrcBuff[i + vStartIndex];
            }
            return destBuff;
        }


        /// <summary>
        /// 判断密码是不是连续数字
        /// </summary>
        /// <param name="strPwd"></param>
        public static bool PwdIsContinue(string strPwd)
        {
            //首先密码要是纯数字
            if (IsNum(strPwd))
            {
                return IsIncrease(strPwd) || IsReduce(strPwd);
            }
            return false;
        }

        /// <summary>
        /// 递增
        /// </summary>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        private static bool IsIncrease(string strPwd)
        {
            for (int i = 0; i < strPwd.Length - 1; i++)
            {
                int iChar1 = Convert.ToInt32(strPwd[i]);
                int iChar2 = Convert.ToInt32(strPwd[i + 1]);
                if (iChar2 - iChar1 != 1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        private static bool IsReduce(string strPwd)
        {
            for (int i = 0; i < strPwd.Length - 1; i++)
            {
                int iChar1 = Convert.ToInt32(strPwd[i]);
                int iChar2 = Convert.ToInt32(strPwd[i + 1]);
                if (iChar2 - iChar1 != -1)
                    return false;
            }
            return true;
        }

        private static bool IsNum(string value)
        {
            foreach (char c in value)
            {
                if (!Char.IsNumber(c))
                    return false;
            }
            return true;
        }

        
        /// <summary>
        /// 判断输入的字符串是否由数字和字母组成
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumAndLetter(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg.IsMatch(str);
        }

        /// <summary>
        /// 将整型转换为4位字节数组 
        /// </summary>
        /// <param name="iNum">整型</param>
        /// <returns></returns>
        public static byte[] intToByte(int iNum)
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[4 - 1 - i] = (byte)(iNum >> 8 * i & 0xFF);
            }
            return bytes;
        }

       /// <summary>
        /// 将报文中指定位置的无符号型字节数组转换为整型
       /// </summary>
        /// <param name="arrLfvMsg">原始报文</param>
        /// <param name="iOffset">起始偏移</param>
        /// <param name="iLen">长度</param>
       /// <returns></returns>
        public static int byteToInt(byte[] arrLfvMsg, int iOffset, int iLen)
        {
            int num = 0;
            for (int i = iOffset; i < iOffset + iLen; i++)
            {
                num += (arrLfvMsg[i] & 0xFF) << (8 * (iLen - 1 - (i - iOffset)));
            }
            return num;
        }
    }
}
