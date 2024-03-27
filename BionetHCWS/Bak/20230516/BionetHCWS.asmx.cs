using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using baseCrypt;

namespace BionetHCWS
{
    /// <summary>
    ///BionetHCWS 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class BionetHCWS : System.Web.Services.WebService
    {       
        string pathLogFile =  ConfigurationManager.AppSettings["pathLogFile"] + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        string TodyMillisecond = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string aaa="=8ZcLDoTn3rk3afT60x4NuW0jqccK26PISqkCfqIl3eq";
        string HOS_INVOICE = string.Empty;
        string publicKey = string.Empty;
        string privateKey = string.Empty;
        [WebMethod]
        public string BionetWEBUH001(string USER_ID, string USER_PW, string AUTH_TOKEN,string JSONSTR)
        {
            //BionetWEBUH001寫入DB
            //string C1 = antiData(Eecrypt("檢驗系統介接400"));
            //string C2 = Decrypt(C1);
            //string C3 = Decrypt("f15O75fAh2ZRBjwIGu5f5RqkCfqIl3eq");

            ////=====建立新院所AUTH_TOKEN算法(先反轉中文,再加密)=======
            //baseCrypt.baseCrypt BaseCrypt = new baseCrypt.baseCrypt();  
            //string en_token = BaseCrypt.enc(baseCrypt.cryptMode.DefaultString, antiData("H000284檢驗系統介接284"));
            //de_token = antiData(BaseCrypt.dec(baseCrypt.cryptMode.DefaultString, "ExHCBzdPo4xw/UM5EyHr8/QsJADtPKufdupGtRpExJjyENJkwqY9eg=="));
            ////=======================================================

            //新增資料夾
            if (!Directory.Exists(ConfigurationManager.AppSettings["pathLogFile"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["pathLogFile"]);


            //寫LOG
            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            File.AppendAllText(pathLogFile, "\r\n" + TodyMillisecond + " 登入ID為：" + USER_ID + "");

            //反轉token值
            //string aaa = Data("qe3lIqfCkqSIP62Kccqj0WuN4x06Tfa3kr3nToDLcZ8=");

            string strstatus = string.Empty;

            //1.先用USER_ID、USER_PW 查出是否有這個合作醫院(HOSPITAL_COOPERATION)
            string Name = GetHospitalData(USER_ID, USER_PW, AUTH_TOKEN);

            //測試版
            //string Name = GetHospitalData("H20188", "1234", "7ZwrxozpIrYvYHKt+Dog4RqkCfqIl3eq");
            //1.1如果沒有，跳出訊息，結束
            if (Name == "")
            {
                strstatus = "9999";
                //寫LOG : 把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：" + strstatus);
            }
            //1.2如果有，接下去，解密JSON    
            //2.讀取JSON，做判別，是否寫入
            else
            {
                //20190417 傳明碼過來
                //JSONSTR = DecryptString(JSONSTR, Name);
                strstatus = step1(JSONSTR, USER_ID, AUTH_TOKEN);                      
            }
               
            return strstatus;
        }

         [WebMethod]
        public string BionetWEBSH001(string USER_ID, string USER_PW, string AUTH_TOKEN, string JSONSTR, string USE_HIS_DATA, string USE_PDF)
        {
             //BionetWEBSH001查詢資料

             //*********************
             //測試到時會拿掉(用來取得加密字串，以利進行測試用)
             //string AAA = antiData("qe3lIqfCkqSH4me2vZPjmHXMJKLPwivu");  //倒置字串
             //string AAA = EncryptString(JSONSTR, "92025801");            //加密JSON

//            string AAA = antiData("uviwPLKJMXHmjPZv2em4HSqkCfqIl3eqH001748");  //
//            string AAA = antiData("1OnmxqJR0zjW9m4B5nmuyE31WfqV6b/J1RQdDhVYSkw=");  //倒置
             string AAA = EncryptString(JSONSTR, "0601160016");     //童綜合資訊
//            string AAA = EncryptString(JSONSTR, "3512012595");     //送子鳥 產生加密JSON
//            string AAA = DecryptString(JSONSTR, "3512012595");     //送子鳥 將回傳string解密

             //test();
             //*********************;
            string strstatus = string.Empty;

            if (USER_ID == "")
            {
                strstatus = "9982";
            }

            if (USER_PW == "")
            {
                strstatus = "9981";
            }

            if (AUTH_TOKEN == "")
            {
                strstatus = "9980";
            }

            if (USE_HIS_DATA == "")
            {
                strstatus = "9979";
            }            

            if (USE_PDF == "")
            {
                strstatus = "9978";
            }

            if (USE_HIS_DATA == "0" & USE_PDF == "0")
            {
                strstatus = "9976";
            }

            if (JSONSTR == "")
            {
                strstatus = "9983";
            }

            if (strstatus == "")
            {
                //1.先用USER_ID、USER_PW 查出是否有這個合作醫院(HOSPITAL_COOPERATION)，看是否可查出(醫事機構代號/統一編號)
                HOS_INVOICE = GetHospitalData(USER_ID, USER_PW, AUTH_TOKEN);
                //1.1 如果沒有(醫事機構代號/統一編號)，跳出訊息，結束
                //1.2 如果有，接下去，解密JSON 
                if (HOS_INVOICE != "")
                {
                    //test用途========================================
                    //JSONSTR=明碼文字
                    //JSONSTR = EncryptString(JSONSTR, "3512012595"); 
                    //================================================
                    //STRJSON = antiData(Eecrypt("H000400檢驗系統介接400"));
                    //STRJSON = Decrypt(STRJSON);  //解密時會先反轉,再Decrypt
                    //================================================

                    string STRJSON = JSONSTR;

                    STRJSON = DecryptString(STRJSON, HOS_INVOICE);
                    //AUTH_TOKEN加密內容 = "檢驗系統介接" + HOS_ID

//                    string s = "uviwPLKJMXHmjPZv2em4HSqkCfqIl3eq";
//                    string strAUTH_TOKEN = Decrypt(s);

//                    string strAUTH_TOKEN = Decrypt(antiData(AUTH_TOKEN));
                    string strAUTH_TOKEN = Decrypt(AUTH_TOKEN);

                    //1.3 資料庫查出檢驗報告       
                    //2.  組JSON
                    strstatus = step2(STRJSON, USER_ID, strAUTH_TOKEN, USE_HIS_DATA, USE_PDF);
                }
                else
                {
                    strstatus = "9999";
                    //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：" + strstatus);

                }
            }

            return strstatus;
         }
        
        //取json值
        public string step1(string jsonStr, string USER_ID, string AUTH_TOKEN)
        {
           
            try
            {
                baseCrypt.baseCrypt BaseCrypt = new baseCrypt.baseCrypt();                                            
                string strstatus = string.Empty;
                string USER_ENCODE = string.Empty;
                string NHI_ID = string.Empty;
                string HOS_NAME = string.Empty;
                string CUS_ID = string.Empty;
                string CUS_NAME = string.Empty;
                string PATIENT_ID = string.Empty; 
                string CUS_BIRTH = string.Empty; 
                string DR_NAME = string.Empty;
                string HIS_TITLE = string.Empty;
                string HIS_TITLE_ID = string.Empty;
                string GET_BLOOD_D = string.Empty;
                string HIS_ID = string.Empty;
                string HIS_NAME = string.Empty;

                //20181217  KEN 增加
                string MEMO_TEACHER = string.Empty; //DR_NAME
                string USER_EMAIL = string.Empty;
                string USER_MOIBLE = string.Empty;
                string CUS_SEX = string.Empty; //???
                string USER_ADDR = string.Empty;
                string USER_UB_DATE = string.Empty;
                string HOS_ID = string.Empty;  

                                
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                /*載入JSON字串*/
                /*注意key有分大小寫*/
                //****************************
                //取JSON值
                //JObject obj = JsonConvert.DeserializeObject<JObject>(jsonStr);
                JArray array = JsonConvert.DeserializeObject<JArray>(jsonStr);
                foreach (JObject obj_results in array)
                {
                    USER_ENCODE = obj_results["USER_ENCODE"].ToString();

                    //解密                                      
                    string strUSER_ENCODE = BaseCrypt.dec(baseCrypt.cryptMode.DefaultString, antiData(USER_ENCODE));
                    if (strUSER_ENCODE == "")
                    {
                        //2.1讀取JSON的USER_ENCODE是否為(HOSPITAL_COOPERATION)的USER_ID+AUTH_TOKEN
                        strstatus += ", 9995";
                    }
                    else if (strUSER_ENCODE != Decrypt(AUTH_TOKEN))
                    {
                        //return strstatus;  這句註掉  因檢查完再RETURN  KEN  20180621
                        strstatus += ", 9997";
                    }

                    NHI_ID = obj_results["NHI_ID"].ToString();
                    if (NHI_ID == "") strstatus += ", 9994";

                    HOS_NAME = obj_results["HOS_NAME"].ToString();
                    if (HOS_NAME == "") strstatus += ", 9993";

                    CUS_ID = obj_results["CUS_ID"].ToString();
                    if (CUS_ID == "") strstatus += ", 9992";

                    CUS_NAME = obj_results["CUS_NAME"].ToString();
                    if (CUS_NAME == "") strstatus += ", 9991";

                    PATIENT_ID = obj_results["PATIENT_ID"].ToString();
                    if (PATIENT_ID == "") strstatus += ", 9990";

                    CUS_BIRTH = obj_results["CUS_BIRTH"].ToString();
                    if (CUS_BIRTH == "") strstatus += ", 9989";

                    DR_NAME = obj_results["DR_NAME"].ToString();
                    if (DR_NAME == "") strstatus += ", 9988";

                    //給MEMO_TEACHER = DR_NAME
                    MEMO_TEACHER = DR_NAME; 

                    GET_BLOOD_D = obj_results["GET_BLOOD_D"].ToString();
                    if (GET_BLOOD_D == "") strstatus = ", 9987";

                    HIS_TITLE_ID = obj_results["HIS_TITLE_ID"].ToString();
                    if (HIS_TITLE_ID == "") strstatus += ", 9986";

                    HIS_TITLE = obj_results["HIS_TITLE"].ToString();
                    if (HIS_TITLE == "") strstatus += ", 9985";

                    //20181217增加  KEN

                    USER_EMAIL = obj_results["USER_EMAIL"].ToString();
                    //if (USER_EMAIL == "") strstatus += ", 9802";

                    USER_MOIBLE = obj_results["USER_MOIBLE"].ToString();
                    //if (USER_MOIBLE == "") strstatus += ", 9803";

                    USER_ADDR = obj_results["USER_ADDR"].ToString();
                    //if (USER_ADDR == "") strstatus += ", 9805";

                    USER_UB_DATE = obj_results["USER_UB_DATE"].ToString();
                    //if (USER_UB_DATE == "") strstatus += ", 9806";


                    //=======以下不是必填欄位規格, 以null判斷

                    if (obj_results["CUS_SEX"] != null)
                        CUS_SEX = obj_results["CUS_SEX"].ToString();

                    if (obj_results["HOS_ID"] != null)
                        HOS_ID = obj_results["HOS_ID"].ToString();
             
                    //==========================================

                    JArray HisData = (JArray)obj_results["HIS_DATA"];
                    foreach (JObject obj_hisData in HisData)
                    {
                        HIS_ID = obj_hisData["HIS_ID"].ToString();
                        if (HIS_ID == "") strstatus += ", 9984";

                        HIS_NAME = obj_hisData["HIS_NAME"].ToString();
                        if (HIS_NAME == "") strstatus += ", 9983";
                    }                  
                }      
                                                                                                  
                //****************************
                //1.3取ExlInfoNo值(HC_EXLINFO), 有無協定該院所可做交換
                string ExlInfoNo = GetProductList(NHI_ID);
                if (ExlInfoNo == "")
                {
                    strstatus += ", 9800";
                }

                if (strstatus != "")
                {
                    //寫LOG : 把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：" + strstatus);
                    return strstatus; //有錯誤跳出
                }


                //InsertData(ExlInfoNo, jsonStr);
                strstatus = Insert_HcJsondata(ExlInfoNo, jsonStr, CUS_ID, HIS_TITLE, CUS_NAME, GET_BLOOD_D, CUS_BIRTH
                                  , PATIENT_ID, MEMO_TEACHER, USER_EMAIL, USER_MOIBLE, CUS_SEX, USER_ADDR, USER_UB_DATE, HOS_ID);

                //3.寫入成功，回傳OK
                return strstatus;
            }         
            catch (Exception ex)
            {
                //3.1寫入失敗，回傳ERROR
                //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：9999 \r\n" + ex.Message);
                return "9999";
            }                                       
        }

        //組json值
        public string step2(string JSONSTR, string USER_ID, string AUTH_TOKEN, string USE_HIS_DATA, string USE_PDF)
        {
            string strJsonValue = string.Empty;

            try
            {
                JArray array = JsonConvert.DeserializeObject<JArray>(JSONSTR);
                foreach (JObject obj_results in array)
                {
                    string USER_ENCODE = string.Empty;
                    string NHI_ID = string.Empty;
                    string IS_NEW_REPORT = string.Empty;
                    string PATIENT_ID = string.Empty;
                    string CUS_ID = string.Empty;
                    string CUS_CT_ID = string.Empty;
                    string CUS_BIRTH = string.Empty;
                    string S_GETBLOOD_D = string.Empty;
                    string E_GETBLOOD_D = string.Empty;
                    string S_REPORT_DATE = string.Empty;
                    string E_REPORT_DATE = string.Empty;
                    string S_PrintDate = string.Empty;
                    string E_PrintDate = string.Empty;
                    string strBASE64_CONTENT = string.Empty;                                      

                    USER_ENCODE = Decrypt(obj_results["USER_ENCODE"].ToString());
                    NHI_ID = obj_results["NHI_ID"].ToString();
                    IS_NEW_REPORT = obj_results["IS_NEW_REPORT"].ToString();
                    CUS_CT_ID = obj_results["CUS_CT_ID"].ToString();
                    S_GETBLOOD_D = obj_results["S_GET_BLOOD_D"].ToString();
                    E_GETBLOOD_D = obj_results["E_GET_BLOOD_D"].ToString();
                    CUS_BIRTH = obj_results["CUS_BIRTH"].ToString();
                    CUS_ID = obj_results["CUS_ID"].ToString();
                    S_REPORT_DATE = obj_results["S_REPORT_DATE"].ToString();
                    E_REPORT_DATE = obj_results["E_REPORT_DATE"].ToString();
                    S_PrintDate = obj_results["S_PrintDate"].ToString();
                    E_PrintDate = obj_results["E_PrintDate"].ToString();

                    if (NHI_ID == "")
                    {
                        strJsonValue = "9975";
                    }

                    if (IS_NEW_REPORT == "")
                    {
                        strJsonValue = "9974";
                    }

                    //要解析USER_ENCODE 是否 = USER_ID + AUTH_TOKEN
                    //USER_ENCODE
                    if (USER_ENCODE == USER_ID + AUTH_TOKEN)
                    {
                        //檢驗結果回傳(主要)                 
                        DataTable DT = GetCustomerData(NHI_ID, IS_NEW_REPORT, CUS_CT_ID, S_GETBLOOD_D, E_GETBLOOD_D, CUS_BIRTH, CUS_ID, S_REPORT_DATE, E_REPORT_DATE, S_PrintDate, E_PrintDate);
                        //檢驗結果回傳(HisData)
                        DataTable HisData = new DataTable();
                        //要輸出的變數
                        StringWriter sw = new StringWriter();
                        //建立JsonTextWriter
                        JsonTextWriter writer = new JsonTextWriter(sw);
                        string PDFFlag = "N";

                        if (DT.Rows.Count != 0)
                        {
                            writer.WriteStartArray();

                            for (int f = 0; f < DT.Rows.Count; f++)
                            {
                                //writer.WriteStartArray();
                                writer.WriteStartObject();
                                writer.WritePropertyName("NHI_ID"); writer.WriteValue(DT.Rows[f]["NHI_ID"].ToString());
                                writer.WritePropertyName("HOS_NAME"); writer.WriteValue(DT.Rows[f]["HOS_NAME"].ToString());
                                writer.WritePropertyName("CUS_ID"); writer.WriteValue(DT.Rows[f]["CUS_ID"].ToString());
                                writer.WritePropertyName("PATIENT_ID"); writer.WriteValue(DT.Rows[f]["PATIENT_ID"].ToString());
                                writer.WritePropertyName("CUS_BIRTH"); writer.WriteValue(DT.Rows[f]["CUS_BIRTH"].ToString());
                                writer.WritePropertyName("DR_NAME"); writer.WriteValue(DT.Rows[f]["DR_NAME"].ToString());
                                writer.WritePropertyName("GET_BLOOD_D"); writer.WriteValue(DT.Rows[f]["GET_BLOOD_D"].ToString());

                                //2018.03.07 HIS_TITLE_ID 要改用大項

                                //2018.06.21 KEN補述  
                                //撈取VW_BIONET_WEBSH001檢驗結果回傳的"HIS_TITLE_ID" (DT.Rows[f][""HIS_TITLE_ID].ToString())
                                //是源自CUSTOMER_SMA.Producttype
                                writer.WritePropertyName("HIS_TITLE_ID"); writer.WriteValue(GetHisTitleCode(DT.Rows[f]["HOS_ID"].ToString(), DT.Rows[f]["HIS_TITLE_ID"].ToString()));
                                //writer.WritePropertyName("HIS_TITLE_ID"); writer.WriteValue(DT.Rows[f]["HIS_TITLE_ID"].ToString());

                                writer.WritePropertyName("HIS_TITLE"); writer.WriteValue(DT.Rows[f]["HIS_TITLE"].ToString());
                                writer.WritePropertyName("HOS_ID"); writer.WriteValue(DT.Rows[f]["HOS_ID"].ToString());
                                writer.WritePropertyName("MEMO"); writer.WriteValue("");

                                //附件檔的內容
                                strBASE64_CONTENT = Base64Encode(DT.Rows[f]["CUS_CT_ID"].ToString(), DT.Rows[f]["PRODUCTTYPE"].ToString(), DT.Rows[f]["CMPNAME"].ToString());

                                #region HIS_DATA的JSON陣列
                                //USE_HIS_DATA 是否要顯示資料
                                if (USE_HIS_DATA == "1")
                                {
                                    writer.WritePropertyName("HIS_DATA");
                                    //檢驗結果回傳(索引)
                                    DataTable tble = GetHisData(DT.Rows[f]["HOS_ID"].ToString(), DT.Rows[f]["PRODUCTTYPE"].ToString());

                                    writer.WriteStartArray();
                                    for (int i = 0; i < tble.Rows.Count; i++)
                                    {
                                        //2018.03.22 要Mproject不等於Y 不是大項時才輸出HisData
                                        if (tble.Rows[i]["Mproject"].ToString() != "Y")
                                        {
                                            //SMA_41只有大項，移到else，但不確定未來會否有SMA類別，這裡先保留
                                            if (tble.Rows[i]["TABLENAME"].ToString() == "CUSTOMER_SMA")
                                            {
                                                HisData = GetHisDataA(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                            }
                                            else if (tble.Rows[i]["TABLENAME"].ToString() == "CUSTOMER_SMA_DETAIL")
                                            {
                                                HisData = GetHisDataB(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                            }
                                            else if (tble.Rows[i]["TABLENAME"].ToString() == "ANALYSE")
                                            {
                                                HisData = GetHisDataC(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                            }
                                            else if (tble.Rows[i]["TABLENAME"].ToString() == "PGD_MAIN")
                                            {
                                                HisData = GetHisDataD(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                            }
                                        }
                                        else 
                                        {
                                            //SMA_41只有大項，所以大項時要出檢驗結果
                                            if (tble.Rows[i]["TABLENAME"].ToString() == "CUSTOMER_SMA")
                                            {
                                                HisData = GetHisDataA(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                            }

                                        }

                                        if (strBASE64_CONTENT != "")
                                        {PDFFlag = "Y";} 
                                        else
                                        { PDFFlag = "N"; }

                                        for (int k = 0; k < HisData.Rows.Count; k++)
                                        {
                                            writer.WriteStartObject();
                                            writer.WritePropertyName("HIS_ID"); writer.WriteValue(tble.Rows[i]["Test_Code"].ToString());
                                            writer.WritePropertyName("HIS_NAME"); writer.WriteValue(tble.Rows[i]["Test_Name"].ToString());

                                            if (tble.Rows[i]["COLNAME"] == "")
                                            {
                                                writer.WritePropertyName("HIS_VALUE"); writer.WriteValue("");
                                            }
                                            else
                                            {
                                                writer.WritePropertyName("HIS_VALUE"); writer.WriteValue(HisData.Rows[k][tble.Rows[i]["COLNAME"].ToString()].ToString());
                                            }                                            
                                            writer.WritePropertyName("HIS_UNIT"); writer.WriteValue(GetHISData_Unit(DT.Rows[f]["HIS_TITLE_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString()));
                                            writer.WritePropertyName("HIS_MEMO"); writer.WriteValue("");
                                            writer.WriteEndObject();
                                        }
                                    }
                                    writer.WriteEndArray();
                                }
                                #endregion

                                if (USE_PDF == "1")
                                {
                                    writer.WritePropertyName("FILE_DATA");
                                    writer.WriteStartArray();

                                    if (PDFFlag == "Y") 
                                    {
                                        if (strBASE64_CONTENT!="")
                                        {
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("BASE64_CONTENT"); writer.WriteValue(strBASE64_CONTENT);
                                    //writer.WritePropertyName("BASE64_CONTENT"); writer.WriteValue("測試用PDF密文");
                                    writer.WritePropertyName("FILE_TYPE"); writer.WriteValue("PDF");
                                    writer.WritePropertyName("DIGEST_VALUE"); writer.WriteValue("");
                                    writer.WriteEndObject();
                                        }
                                    }
                                    writer.WriteEndArray();
                                }

                                writer.WriteEndObject();
                                //writer.WriteEndArray();
                                //2018.02.21 移至此處(為了應對可能不只一筆時)

                                string tCusID = DT.Rows[f]["CUS_ID"].ToString();
                                //2018.03.22 有抓到PDF檔時才進行註記
                                if ((strBASE64_CONTENT != "") && (IS_NEW_REPORT=="1"))
                                {
                                    //2018.06.21 KEN  HIS_TITLE_ID是源自CUSTOMER_SMA.Producttype
                                    UpdateFlag(tCusID, DT.Rows[f]["HIS_TITLE_ID"].ToString());
                                }
                            }
                            //輸出結果
//                            HttpContext.Current.Response.Write(sw.ToString());
                            writer.WriteEndArray();
                            //2.1 加密後回傳          
                            strJsonValue = EncryptString(sw.ToString(), HOS_INVOICE);
                            //不加密回傳(測試用)
                            //strJsonValue = sw.ToString();

                        }
                        else
                        {
                            //DT.Rows.Count = 0
                            if (IS_NEW_REPORT =="1") 
                            {
                                strJsonValue = "9972";
                            }
                            if (IS_NEW_REPORT == "0")
                            {
                                strJsonValue = "9973";
                            }
                        }                       
                    }
                    else
                    {
                        strJsonValue = "9977";
                        //寫LOG
                        //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                        File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：9977");
                    }
                    //變更位置
                    //UpdateFlag(CUS_ID);
                }
                return strJsonValue;
            }
            catch (Exception ex)
            {
                //3.1寫入失敗，回傳ERROR
                //把內容寫到目的檔案LOG，若檔案存在則附加在原本內容之後(換行)
                File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：9999--" + ex.Message);
           
                return "9999";
            }   
          
           
        }

        public void test()
        {
            string aaa = Base64Encode("FFDO10600014", "", "BIONET");
            ReBase64Encode(aaa);
        }

        //報告的附件(BASE64_CONTENT)
        public string Base64Encode(string cusCtId, string productType, string cmpName)
        {
            //2018.03.22 送子鳥的資料都是GGA，增加GGA也要
            if (cmpName == "BIONET" || cmpName == "GGA")
            {
                int len1 = cusCtId.Length;
                string folder = cusCtId.Substring(0, len1 - 5);
//                string path = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
                string path = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
//                string path = @"D:\out\" + folder + @"\" + cusCtId + ".pdf";
                if (System.IO.File.Exists(path))
                {
                    var result = System.IO.File.ReadAllBytes(path);
                    return Convert.ToBase64String(result);
                }
                else
                {
                    return "";
                }

            }
            if (productType == "FXS")
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\ggalis\IN\" + folder + @"\" + cusCtId + ".pdf";
                if (System.IO.File.Exists(path2))
                {
                    var result2 = System.IO.File.ReadAllBytes(path2);
                    return Convert.ToBase64String(result2);
                }
                else
                {
                    return "";
                }

            }
            if (productType.Contains("SNP"))
            {
                string folder = cusCtId.Substring(0, 4);
                string path2 = @"\\ggalis\IN\" + folder + @"\SNP_TEST\" + cusCtId + "_OK.pdf";
                if (System.IO.File.Exists(path2))
                {
                    var result2 = System.IO.File.ReadAllBytes(path2);
                    return Convert.ToBase64String(result2);
                }
                else
                {
                    return "";
                }

            }
            if (productType.Contains("PGS"))
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\ggalis\IN\" + folder + @"\國外\" + cusCtId + ".pdf";
                string path3 = @"\\ggalis\IN\" + folder + @"\" + cusCtId + ".pdf";
                if (System.IO.File.Exists(path2))
                {
                    var result2 = System.IO.File.ReadAllBytes(path2);
                    return Convert.ToBase64String(result2);
                }
                else if (System.IO.File.Exists(path3))
                {
                    var result3 = System.IO.File.ReadAllBytes(path3);
                    return Convert.ToBase64String(result3);
                }
                else
                {
                    return "";
                }

            }
            return "";
            //string path2 = @"\\ggalis\IN_GGA\AF14\" + cusCtId + ".pdf";
            //var result2 = System.IO.File.ReadAllBytes(path2);
            //return Convert.ToBase64String(result2);
        }

        //還原附件pdf檔
        public void ReBase64Encode(string strValue)
        {
            byte[] bytes = Convert.FromBase64String(strValue);
            File.WriteAllBytes(@"D:\FolderPath\pdfFileName.pdf", bytes);
        }

        //LOG寫入
        public void writeLog()
        {

        }

        //在資料被調用時，將HC_JSONDATA的REPORT_UPLOAD變更為Y
        public void UpdateFlag(string CusID,string ProductType)
        {
            string sql = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "update HC_JSONDATA set REPORT_UPLOAD='Y' ";
            sql += " WHERE 1=1 ";
            sql += " and CUS_ID = '" + CusID + "'";
            sql += " and ProductType = '" + ProductType + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            int eff = cmd.ExecuteNonQuery();
            conn.Close();

            //寫LOG
            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "將身分證號：" + CusID + "的Report_Upload=Y ");
        }

        //檢體報告單位值
        public string GetHISData_Unit(string TITLE_ID, string COLNAME)
        {
            string strUnit = string.Empty;

            if (TITLE_ID == "SDS")
            {
                if (COLNAME == "SMN1")
                {
                    strUnit = "ng/ml";
                }
                else if (COLNAME == "SMN1_2")
                {
                    strUnit = "mIU/ml";
                }
                else if (COLNAME == "SMN1_3")
                {
                    strUnit = "ng/ml";
                }
                else if (COLNAME == "SMN1_4")
                {
                    strUnit = "pg/ml";
                }
                else if (COLNAME == "SMN2")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "SMN2_2")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "SMN2_3")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "SMN2_4")
                {
                    strUnit = "MoM";
                }
                else 
                {
                    strUnit = "";
                }
            }
            else if (TITLE_ID == "FTS")
            {
                if (COLNAME == "CHECK_RESULT_7")
                {
                    strUnit = "IU/L";
                }
                else if (COLNAME == "CHECK_RESULT_8")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "CHECK_RESULT_9")
                {
                    strUnit = "IU/L";
                }
                else if (COLNAME == "CHECK_RESULT_10")
                {
                    strUnit = "MoM";
                }
                else
                {
                    strUnit = "";
                }
            }
            else if (TITLE_ID == "PES1")
            {
                if (COLNAME == "CHECK_RESULT_9")
                {
                    strUnit = "IU/L";
                }
                else if (COLNAME == "CHECK_RESULT_10")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "CHECK_RESULT_11")
                {
                    strUnit = "pg/ml";
                }
                else if (COLNAME == "CHECK_RESULT_12")
                {
                    strUnit = "MoM";
                }
                else
                {
                    strUnit = "";
                }
            }
            else if (TITLE_ID == "PES2")
            {
                if (COLNAME == "CHECK_RESULT_7")
                {
                    strUnit = "IU/L";
                }
                else if (COLNAME == "CHECK_RESULT_8")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "CHECK_RESULT_9")
                {
                    strUnit = "IU/L";
                }              
                else if (COLNAME == "CHECK_RESULT_10")
                {
                    strUnit = "MoM";
                }
                else if (COLNAME == "CHECK_RESULT_11")
                {
                    strUnit = "pg/ml";
                }
                else if (COLNAME == "CHECK_RESULT_12")
                {
                    strUnit = "MoM";
                }
                else
                {
                    strUnit = "";
                }

            }
            else if (TITLE_ID == "FDS")
            {
                if (COLNAME == "SMN1")
                {
                    strUnit = "IU/L";
                }
                else if (COLNAME == "SMN2")
                {
                    strUnit = "IU/L";
                }              
                else
                {
                    strUnit = "";
                }
            }
            else
            {
                strUnit = "";
            }

            return strUnit;
        }

        //查出是否有這個合作醫院(HOSPITAL_COOPERATION)
        public string GetHospitalData(string USER_ID, string USER_PW, string AUTH_TOKEN)
        {
            string strName = string.Empty;
            string sql = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT HOS_INVOICE FROM HOSPITAL_COOPERATION WHERE 1 = 1 ";
            sql += "AND USER_ID ='" + USER_ID + "'";
            sql += "AND USER_PW ='" + USER_PW + "'";
            sql += "AND AUTH_TOKEN ='" + AUTH_TOKEN + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    strName = reader.GetString(0).ToString();
                }
            }
            conn.Close();

            //寫LOG
            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查出是否有這個合作醫院(HOSPITAL_COOPERATION) SQL語法:" + sql);

            return strName;
        }

        //檢驗結果回傳(主要)
        public DataTable GetCustomerData(string NHI_ID, string IS_NEW_REPORT, string CUS_CT_ID, string S_GETBLOOD_D, string E_GETBLOOD_D, string CUS_BIRTH, string CUS_ID, string S_REPORT_D, string E_REPORT_D, string S_PRINT_D, string E_PRINT_D)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT distinct * FROM VW_BIONET_WEBSH001 A ";            
            sql += " WHERE 1=1  ";            
            //這裡開始都是帶條件式
            sql += " AND A.NHI_ID = '" + NHI_ID + "'";

            //只查詢最新完成的報告0:否;1:是
            //若使用此條件(IS_NEW_REPORT == "1")，則條件改為PRINTDATE<= T-1
            if (IS_NEW_REPORT == "1")
            {
//                sql += " AND A.PRINTDATE > '" + DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd") + "'";
                sql += " AND A.PRINTDATE >= '" + DateTime.Now.AddDays(-14).ToString("yyyy/MM/dd 00:00:00") + "'";
                sql += " AND A.PRINTDATE <= '" + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd 23:59:59") + "'";
                //是否已被查詢傳輸資料(Y:已送 N:未送)
                //sql += " AND A.REPORT_UPLOAD is null ";
                sql += " AND A.REPORT_UPLOAD='N' ";
            }
            else if (IS_NEW_REPORT == "0")
            {
                if (CUS_CT_ID != "")
                {
                    sql += " AND A.CUS_CT_ID = '" + CUS_CT_ID + "'";
                }
                if (S_GETBLOOD_D != "" & E_GETBLOOD_D != "")
                {
                    sql += " AND A.GET_BLOOD_D BETWEEN '" + S_GETBLOOD_D + "' AND '" + E_GETBLOOD_D + "'";
                }
                if (CUS_BIRTH != "") //CUS_BIRTH  EX: 1968/01/08
                {
                    sql += " AND A.CUS_BIRTH = '" + CUS_BIRTH + "'";
                }
                if (CUS_ID != "")
                {
                    sql += " AND A.CUS_ID = '" + CUS_ID + "'";
                }
                if (S_REPORT_D != "" & E_REPORT_D != "") //REPORT_DATE  EX:2018/06/05
                {
                    sql += " AND A.REPORT_DATE BETWEEN '" + S_REPORT_D + "' AND '" + E_REPORT_D + "'";
                }
                if (S_PRINT_D != "" & E_PRINT_D != "")
                {
                    sql += " AND A.PRINTDATE BETWEEN '" + S_PRINT_D + " 00:00:00' AND '" + E_PRINT_D + " 23:59:59'";
                }
                //是否已被查詢傳輸資料(Y:已送 N:未送)
                //sql += " AND A.REPORT_UPLOAD='N' ";
            }
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
         
            conn.Close();

            return dt;
        }

        //2018.03.07 Albert：His_Title_ID改用大項Test_Code
        public string GetHisTitleCode(string HOS_ID, string PROJECTTYPE)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            sql = "SELECT Test_Code FROM ACER_CLOUD_CODE_LIST ";
            sql += " WHERE 1=1  ";
            sql += " AND HOS_ID = '" + HOS_ID + "'";
            sql += " AND PROJECTTYPE = '" + PROJECTTYPE + "'";
            sql += " AND Mproject='Y' ";

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            string Code = "";
            Code = dt.Rows.Count.ToString();
            if (dt.Rows.Count != 0)
            {
                Code = cmd.ExecuteScalar().ToString();
            }
            else
            {
                Code = "";
            }
            conn.Close();
            return Code;
        }

        //檢驗結果回傳(索引)        
        public DataTable GetHisData(string HOS_ID, string PROJECTTYPE)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT * FROM ACER_CLOUD_CODE_LIST ";
            sql += " WHERE 1=1  ";
            sql += " AND HOS_ID = '" + HOS_ID + "'";
            sql += " AND PROJECTTYPE = '" + PROJECTTYPE + "'";
            //2018.02.06 Albert：資料表裡有中項，其沒有ColName的資料，以not null篩掉
            //2018.03.22 因有大項Mproject=Y，送子鳥測試2018/3/18~2018/3/20兩筆資料會被篩掉，故先拿掉
            sql += " and ColName is not null ";

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容A)
        public DataTable GetHisDataA(string CUSCTID, string ColName)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            //2018.06.07.Albert
            //因為FXS_2的ColName=SMN1;SMN2，會導致SQL出錯
            //以及有些資料存放在GGADB的問題，修正SQL
            sql = "SELECT " + ColName.Replace(";", "+':'+") + " as '" + ColName + "' FROM CUSTOMER_SMA ";
            sql += " WHERE 1=1  ";
            sql += " AND Cus_CT_ID ='" + CUSCTID + "' ";
            sql += " union ";
            sql += "SELECT " + ColName.Replace(";", "+':'+") + " as '" + ColName + "' FROM GGADB.dbo.CUSTOMER_SMA ";
            sql += " WHERE 1=1 ";
            sql += " AND Cus_CT_ID ='" + CUSCTID + "' ";

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容B)
        public DataTable GetHisDataB(string CUSCTID,string ColName)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT " + ColName + " FROM CUSTOMER_SMA_DETAIL ";
            sql += " WHERE 1=1  ";
            sql += " AND Cus_CT_ID ='' = '" + CUSCTID + "'";  

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容C)
        public DataTable GetHisDataC(string CUSCTID,string ColName)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["GGA"];
            sql = "SELECT " + ColName + " FROM  ANALYSE ";
            sql += " WHERE 1=1  ";
            sql += " AND GGA_FULL_NO = '" + CUSCTID + "'";          

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容D)
        public DataTable GetHisDataD(string CUSCTID,string ColName)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["GGA"];
            sql = "SELECT " + ColName + " FROM  ANALYSE ";
            sql += " WHERE 1=1  ";
            sql += " AND GGA_FULL_NO = '" + CUSCTID + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            conn.Close();
            return dt;
        }

        //取HC_EXLINFO中所有資料
        public string GetProductList(string strHOS_ID)
        {
            string sql = string.Empty;
            string No = string.Empty;            
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT NO FROM HC_EXLINFO WHERE isEnabled = 1 ";
            sql += "AND Hos_Invoice ='" + strHOS_ID + "'";
            //sql += "AND HOS_ID ='" + strHOS_ID + "'";    
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    No = reader.GetInt32(0).ToString();                    
                }
            }
            conn.Close();

            //寫LOG
            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  取HC_EXLINFO中所有資料 SQL語法:" + sql);

            return No;
        }

        //檢查該檔是否轉入過
        public void RepeatData()
        {

        }

        //寫入HC_JSONDATA
        public void InsertData(string ExlInfoNo,string jsonStr)
        {
            string sql = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "INSERT INTO HC_JSONDATA(ExlInfoNo, JsonData, Src_FileName,CRE_DATE) VALUES(@param1,@param2,@param3,@param4)";

            cmd.Parameters.AddWithValue("@param1", ExlInfoNo);
            cmd.Parameters.AddWithValue("@param2", jsonStr);
            cmd.Parameters.AddWithValue("@param3", "");
            cmd.Parameters.AddWithValue("@param4", DateTime.Today);
            cmd.ExecuteNonQuery();
            conn.Close();

            //寫LOG
            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  寫入HC_JSONDATA SQL語法:" + cmd.CommandText);
        }

        //寫入HC_JSONDATA (2018.02.13.新增身分證號及Flag狀態)
        public string Insert_HcJsondata(string ExlInfoNo, string jsonStr, string CusID, string ProductType, string CUS_NAME, string GET_BLOOD_D
                                    , string CUS_BIRTH, string PATIENT_ID, string MEMO_TEACHER, string USER_EMAIL, string USER_MOIBLE, string CUS_SEX
                                    , string USER_ADDR, string USER_UB_DATE, string HOS_ID)
        {
            string errtxt = "";
            string errcode = "";
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["BBCMS"]);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            try
            {
                SqlTransaction sTrans = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                //cmd = conn.CreateCommand();
                cmd.Transaction = sTrans; //任何錯誤則Rollback, 取消二邊insert動作

                //(1)記錄資料交換
                cmd.CommandText = "INSERT INTO HC_JSONDATA(ExlInfoNo, JsonData, Src_FileName,CRE_DATE,CUS_ID,REPORT_UPLOAD,ProductType) " +
                                  "VALUES (@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                cmd.CommandText += ";SELECT IDENT_CURRENT ('HC_JSONDATA') AS JSON_SN; ";

                cmd.Parameters.AddWithValue("@param1", ExlInfoNo);
                cmd.Parameters.AddWithValue("@param2", jsonStr);
                cmd.Parameters.AddWithValue("@param3", "");
                cmd.Parameters.AddWithValue("@param4", DateTime.Now);
                cmd.Parameters.AddWithValue("@param5", CusID);
                cmd.Parameters.AddWithValue("@param6", "N");
                cmd.Parameters.AddWithValue("@param7", ProductType);

                int JSON_SN = Convert.ToInt16(cmd.ExecuteScalar());
                File.AppendAllText(pathLogFile, "\r\n SQL：" + cmd.CommandText);


                //(2)記錄該人===================================================
                SqlConnection conn2 = new SqlConnection(ConfigurationManager.AppSettings["BBCMS"]);
                conn2.Open();
                SqlTransaction sTrans2 = conn2.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlCommand cmd2 = conn2.CreateCommand();
                cmd2.Transaction = sTrans2;
                //cmd.Parameters.Clear();  //不知是否這句話的問題???  因為沒有清除上面 Parameters, 導致取MAX(SN)常常取錯而Pkey重複? 
                cmd2.CommandText = " SELECT MAX(SN) CUS_SN FROM HC_CUS_DATA ";
                int CUS_SN = Convert.ToInt16(cmd2.ExecuteScalar());
                CUS_SN += 1;
                conn2.Close(); 
                
                File.AppendAllText(pathLogFile, "\r\n  CUS_SN: " + CUS_SN + "\r\n SQL：" + cmd2.CommandText);
                //==============================================================


                cmd.CommandText = "INSERT INTO HC_CUS_DATA(SN, JSON_SN, ExlInfoNo, Cus_Name, Cus_CID, GetBlood_D, Cus_Birth" +
                                  ", Patient_ID, Memo_Teacher, EMail, Moible, Cus_Sex, Addr, UB_DATE, HOS_ID, CreateDate) " +
                                  "VALUES (@SN, @JSON_SN, @ExlInfoNo, @Cus_Name, @Cus_CID, @GetBlood_D, @Cus_Birth " +
                                  ",@Patient_ID, @Memo_Teacher, @EMail, @Moible, @Cus_Sex, @Addr, @UB_Date, @HOS_ID, @CreateDate)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SN", CUS_SN);
                cmd.Parameters.AddWithValue("@JSON_SN", JSON_SN);
                cmd.Parameters.AddWithValue("@ExlInfoNo", ExlInfoNo);
                cmd.Parameters.AddWithValue("@Cus_Name", CUS_NAME);
                cmd.Parameters.AddWithValue("@Cus_CID", CusID);
                cmd.Parameters.AddWithValue("@GetBlood_D", GET_BLOOD_D);
                cmd.Parameters.AddWithValue("@Cus_Birth", CUS_BIRTH);
                cmd.Parameters.AddWithValue("@Patient_ID", PATIENT_ID);
                cmd.Parameters.AddWithValue("@Memo_Teacher", MEMO_TEACHER);
                cmd.Parameters.AddWithValue("@EMail", USER_EMAIL);
                cmd.Parameters.AddWithValue("@Moible", USER_MOIBLE);
                cmd.Parameters.AddWithValue("@Cus_Sex", CUS_SEX);
                cmd.Parameters.AddWithValue("@Addr", USER_ADDR);
                cmd.Parameters.AddWithValue("@UB_Date", USER_UB_DATE);
                cmd.Parameters.AddWithValue("@HOS_ID", HOS_ID);
                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //Cus_CT_ID
                //CASE_ID

                //--------SQL成不成功都要LOG----------------------------------
                string p = "";  //LOG要寫入Parameters.Value
                for (int i = 0; i <= cmd.Parameters.Count - 1; i++)
                    p += "; " + cmd.Parameters[i].Value.ToString();

                File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  寫入HC_JSONDATA SQL語法:" + cmd.CommandText + "(" + p.Substring(1) + ")");
                //------------------------------------------------------------


                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();

                errcode = "0000";
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                errtxt = ex.Message;
            }
            finally
            {
                conn.Close();
            }


            //寫LOG : 把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            if (errtxt == "")
            {
                File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 寫入HC_JSONDATA OK!");
            }
            else
            {
                File.AppendAllText(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 寫入HC_JSONDATA 失敗! \r\n" + errtxt);
                errcode = "9991";
            }

            return errcode;
        }

        //字串反轉
        public string antiData(string str)
        {
            char[] temp = str.ToCharArray();
            Array.Reverse(temp);
            return new string(temp);            
        }

        //字串(auth_token)解密
        public string Decrypt(string PASSWORD)
        {
            string USER_VALUE = string.Empty;
            baseCrypt.baseCrypt BaseCrypt = new baseCrypt.baseCrypt();
            USER_VALUE = BaseCrypt.dec(baseCrypt.cryptMode.DefaultString, antiData(PASSWORD));
            return USER_VALUE;
        }

        //加密
        public string Eecrypt(string StrValue)
        {
            string USER_VALUE = string.Empty;
            baseCrypt.baseCrypt BaseCrypt = new baseCrypt.baseCrypt();
            USER_VALUE = BaseCrypt.enc(baseCrypt.cryptMode.DefaultString, StrValue);
            return USER_VALUE;
        }

        ////產生公鑰及私鑰
        //public void GenerateRSAKeys()
        //{
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    publicKey = rsa.ToXmlString(false);
        //    privateKey = rsa.ToXmlString(true);
        //}

        ////加密字串
        //public string Encrypt(string publicKey, string content)
        //{
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.FromXmlString(publicKey);
        //    var encryptString = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(content), false));
        //    return encryptString;
        //}

        ////解密字串
        //public string Decrypt(string privateKey, string encryptedContent)
        //{
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.FromXmlString(privateKey);
        //    string decryptString = Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(encryptedContent), false));
        //    return decryptString;
        //}
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">要加密的字串</param>
        /// <param name="key">加密KEY</param>
        /// <returns>加密後的字串</returns>
        public static string EncryptString(string plainText, string key)
        {
            //密碼轉譯一定都是用byte[] 所以把string都換成byte[]
            byte[] plainTextByte = Encoding.UTF8.GetBytes(plainText);
            byte[] keyByte = Encoding.UTF8.GetBytes(key);

            //加解密函數的key通常都會有固定的長度 而使用者輸入的key長度不定 因此用hash過後的值當做key
            MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
            byte[] md5Byte = provider_MD5.ComputeHash(keyByte);

            //產生加密實體
            RijndaelManaged aesProvider = new RijndaelManaged();
            ICryptoTransform aesEncrypt = aesProvider.CreateEncryptor(md5Byte, md5Byte);

            //output就是加密過後的結果
            byte[] output = aesEncrypt.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);

            //	將加密後的位元組轉成16進制字串
            return BitConverter.ToString(output).Replace("-", "");
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="chipherText">加密後的密文</param>
        /// <param name="key">解密KEY</param>
        /// <returns>解密後的明文</returns>
        public static string DecryptString(string chipherText, string key)
        {
            byte[] chipherTextByte = new byte[chipherText.Length / 2];
            int j = 0;

            for (int i = 0; i < chipherText.Length / 2; i++)
            {
                chipherTextByte[i] = Byte.Parse(chipherText[j].ToString() + chipherText[j + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                j += 2;
            }

            //密碼轉譯一定都是用byte[] 所以把string都換成byte[]
            byte[] keyByte = Encoding.UTF8.GetBytes(key);

            //加解密函數的key通常都會有固定的長度 而使用者輸入的key長度不定 因此用hash過後的值當做key
            MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
            byte[] md5Byte = provider_MD5.ComputeHash(keyByte);

            //產生解密實體
            RijndaelManaged aesProvider = new RijndaelManaged();
            ICryptoTransform aesDecrypt = aesProvider.CreateDecryptor(md5Byte, md5Byte);

            //string_secretContent就是解密後的明文
            byte[] plainTextByte = aesDecrypt.TransformFinalBlock(chipherTextByte, 0, chipherTextByte.Length);
            string plainText = Encoding.UTF8.GetString(plainTextByte);
            return plainText;
        }


    }

}
