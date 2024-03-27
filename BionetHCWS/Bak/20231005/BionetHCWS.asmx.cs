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

            //馨蕙馨
            //string C1 = antiData(Eecrypt("檢驗系統介接547"));
            //string C2 = Decrypt(C1);
            //string C3 = Decrypt("pBt71XExR0LlZ2Cd8Kly1TqkCfqIl3eq");

            //博愛
            //string C1 = antiData(Eecrypt("檢驗系統介接2004"));
            //string C2 = Decrypt(C1);
            //string C3 = Decrypt("3JbCkvhN0fMQrd2nchpfsQqkCfqIl3eq");
            //string S3 = antiData(Eecrypt("H002004檢驗系統介接2004"));

            ////宋俊宏婦幼醫院
            //string S1 = antiData(Eecrypt("檢驗系統介接251"));
            //string S2 = Decrypt(C1);
            //string SS3 = Decrypt("3JbCkvhN0fMQrd2nchpfsQqkCfqIl3eq");
            //string CC3 = antiData(Eecrypt("H000251檢驗系統介接251"));

            //天主教聖功醫療財團法人聖功醫院
            //string S1 = antiData(Eecrypt("檢驗系統介接545"));
            //string S2 = Decrypt(S1);
            //string SS3 = Decrypt("3JbCkvhN0fMQrd2nchpfsQqkCfqIl3eq");
            //string CC3 = antiData(Eecrypt("H000545檢驗系統介接545"));

            //沙爾德聖保祿修女會醫療財團法人聖保祿醫院
            //string S1 = antiData(Eecrypt("檢驗系統介接252"));
            //string S2 = Decrypt(S1);
            //string SS3 = Decrypt("3JbCkvhN0fMQrd2nchpfsQqkCfqIl3eq");
            //string CC3 = antiData(Eecrypt("H000252檢驗系統介接252"));


            //以上是各院所加解密部分-------------------------------------------------------------------------------------------

            WriteLog(pathLogFile, "\r\n[".PadRight(50, '*'));

            string strstatus = string.Empty;

            ////=====建立新院所AUTH_TOKEN算法(先反轉中文,再加密)(解密時:先解密TOKEN,再反轉)=======
            //baseCrypt.baseCrypt BaseCrypt = new baseCrypt.baseCrypt();  
            //string en_token = BaseCrypt.enc(baseCrypt.cryptMode.DefaultString, antiData("H000284檢驗系統介接284"));
            //de_token = antiData(BaseCrypt.dec(baseCrypt.cryptMode.DefaultString, "ExHCBzdPo4xw/UM5EyHr8/QsJADtPKufdupGtRpExJjyENJkwqY9eg=="));
            ////=======================================================
            //反轉token值
            //string aaa = Data("qe3lIqfCkqSIP62Kccqj0WuN4x06Tfa3kr3nToDLcZ8=");


            //新增資料夾
            if (!Directory.Exists(ConfigurationManager.AppSettings["pathLogFile"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["pathLogFile"]);

            //寫LOG
            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
            WriteLog(pathLogFile, "\r\n" + TodyMillisecond + " BionetWEBUH001寫入 登入ID：" + USER_ID + ", PW：" + USER_PW + " \r\n " + JSONSTR);


            //1.先用USER_ID、USER_PW 查出是否有這個合作醫院(HOSPITAL_COOPERATION)
            string HOS_Invoice = GetHospitalData(USER_ID, USER_PW, AUTH_TOKEN);

            //測試版
            //string Name = GetHospitalData("H20188", "1234", "7ZwrxozpIrYvYHKt+Dog4RqkCfqIl3eq");
            //1.1如果查無院所，跳出9999結束
            if (HOS_Invoice == "")
            {
                strstatus = "9999";
                //寫LOG : 把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：" + strstatus + ",無此合作醫院!");
            }
            //1.2如果有，接下去，解密JSON    
            //2.讀取JSON，做判別，是否寫入
            else
            {
                //20190417 傳明碼過來
                //JSONSTR = DecryptString(JSONSTR, Name);
                strstatus = step1(JSONSTR, USER_ID, AUTH_TOKEN);                      
            }

            WriteLog(pathLogFile, "\r\n" + "]".PadLeft(50, '*'));

            return strstatus;
        }

         [WebMethod]
        public string BionetWEBSH001(string USER_ID, string USER_PW, string AUTH_TOKEN, string JSONSTR, string USE_HIS_DATA, string USE_PDF)
        {
             //BionetWEBSH001查詢資料
            WriteLog(pathLogFile, "\r\n[".PadRight(50, '-'));

            WriteLog(pathLogFile, "\r\n" + TodyMillisecond + "BionetWEBSH001查詢 登入ID：" + USER_ID + ",PW：" + USER_PW + ",USE_HIS_DATA：" + USE_HIS_DATA + ",USE_PDF：" + USE_PDF + " \r\n" + JSONSTR + "\r\n");

             //*********************
             //測試到時會拿掉(用來取得加密字串，以利進行測試用)
             //string AAA = antiData("qe3lIqfCkqSH4me2vZPjmHXMJKLPwivu");  //倒置字串
             //string AAA = EncryptString(JSONSTR, "92025801");            //加密JSON

//            string AAA = antiData("uviwPLKJMXHmjPZv2em4HSqkCfqIl3eqH001748");  //
//            string AAA = antiData("1OnmxqJR0zjW9m4B5nmuyE31WfqV6b/J1RQdDhVYSkw=");  //倒置
//            string AAA = EncryptString(JSONSTR, "0601160016");     //童綜合資訊
//            string AAA = EncryptString(JSONSTR, "3512012595");     //送子鳥 產生加密JSON
//            string AAA = DecryptString(JSONSTR, "3512012595");     //送子鳥 將回傳string解密

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

            //string STRJSONxx = Decrypt("f15O75fAh2ZRBjwIGu5f5RqkCfqIl3eq");                            //檢驗系統介接400(寫入insert)
            //string USERENCODE = Decrypt("");   //H000400檢驗系統介接400


            //string STRJSONxx = Decrypt("uviwPLKJMXHmjPZv2em4HSqkCfqIl3eq");                            //檢驗系統介接1748(寫入insert)
            //string USERENCODE = Decrypt("==QqXgPHJIwipiHwNxCA6dMrpE86zoQsHnesgrQ3S5JjJWYF39oDbz8f");   //H001748檢驗系統介接1748

            //string S1 = antiData(Eecrypt("H000400檢驗系統介接400"));      //童綜合  (==AOecv7SH3J2ygZqWzCsqKbO7Ac7CNrIVbYb1zlFRTJMqO0X8fu3mIO )

            //string S2 = antiData(Eecrypt("檢驗系統介接284"));             //秉坤： AUTH_TOKEN = 檢驗系統介接284  (ALYEvkLPzB1ZlbtBDpZvVSqkCfqIl3eq)
            //string S3 = antiData(Eecrypt("H000284檢驗系統介接284"));      //秉坤： USER_ENCODE = H000284 + 檢驗系統介接284  (==AUJedxAS/YbNtPWjuQU9OUtv3waI21voESRDAaXsJDzoO0X8fu3mIO)

            //string Ucode1 = Decrypt(antiData("ExHCBzdPo4xw/UM5EyHr8/QsJADtPKufdupGtRpExJjyENJkwqY9eg=="));  //482接介統系驗檢482000H (秉坤,應該有誤,應是中文加密再反轉, 不是中文反轉再加密)

            //string STRJSONxx3 = EncryptString("[{'USER_ENCODE':'==QqXgPHJIwipiHwNxCA6dMrpE86zoQsHnesgrQ3S5JjJWYF39oDbz8f','NHI_ID':'3512012595','IS_NEW_REPORT':'1','PATIENT_ID':'','CUS_ID':'','CUS_CT_ID':'','CUS_BIRTH':'','S_GET_BLOOD_D':'','E_GET_BLOOD_D':'','S_REPORT_DATE':'','E_REPORT_DATE':'','S_PrintDate':'','E_PrintDate':''}]", "0936060016");
            //string STRJSONxx2 = DecryptString("46ECBC6C815BE4BA5B1299C34C3492BD2E477FF51F8E81A29B4F702F661A847643C6D395EC96971D139E531F717B9CD13D607B018DF55DD9DE38D93E2F32D13BE2BD9CD8966623D6DFE074A8CFDFE47E374F00B8FCC5EC1FF0B8CE88C969B444715AEB99272C89F6383BC80E7AC1D1CAFC95336D738DB9178D912FB362D7F6305DF231DBE5326F4FA69F742FB0F2284D2ACD12EB0C100328FF0CBAB9E1B54B63EC5FAE2FE75A6AAD8EC1360F62FFAF849B885D724E3D02127340B24A1AE78589666B9123AC460238008CEAB0A0A6FAE8F350855CE191C939B04DB5C9388F2F26F6B97E42DA7561189F28472AB4D63BF1EF1EBEAD30DD0A4D97DFD6527C7EB62E8691E9DC6A69EF6CC058EC15131AB55DC831FCA5BEBDB3A16C0D288E180842F8", "3512012595");
            //string STRJSONxx4 = DecryptString("7B6837F61663DDDE7D2E1364850256C0E53B00CB2C95CB69D547E02E2F729559338867DD6425AF90FF8D1E0FA403E44E75F0D4882304AC9E036AAC260B944C143FEB4626A45BEE373BB5B4D5313B8BD51F836A59F3EC98A90000CB5265D1A2860441419D160EE594BC2951F7B30D863B4DEB5EFC1417E87461E3289CA4234FE619CA124947C78B0DE03FEECD61E3D98FA37201610FC7C01AEC8934272B2EF4F7F9080B3CF34FE0440A15BD99D0A374AE6F76DBB87195374B8160243048FA7CD69A7A4918BD815594F2C90BD0D112C5838800E724D2B02255B606D8F1739D0D23770F2E76142F6EDC868590CD4E2EBEC7C8C705EF9F1D8CD2374E9A4AC4D73AEB", "0936060016");

             ////string STRJSONxx31 = DecryptString("[{USER_ENCODE:f15O75fAh2ZRBjwIGu5f5RqkCfqIl3eq,NHI_ID:0936060016}]", "0936060016");


            if (strstatus == "")
            {

                //1.先用USER_ID、USER_PW 查出是否有這個合作醫院(HOSPITAL_COOPERATION)，看是否可查出(醫事機構代號/統一編號)
                HOS_INVOICE = GetHospitalData(USER_ID, USER_PW, AUTH_TOKEN);
                //1.1 如果沒有(醫事機構代號/統一編號)，跳出訊息，結束
                //1.2 如果有，接下去，解密JSON 
                if (HOS_INVOICE != "")
                {

                    //test用途========================================
                    //AUTH_TOKEN(來源為  "檢驗系統介接" + HOS_ID : EX: "檢驗系統介接1748" => 先BaseCrypt加密,再反轉 )
                    //AUTH_TOKEN = antiData(Eecrypt("檢驗系統介接400"));
                    //STRJSON = Decrypt(AUTH_TOKEN);  //Decrypt會先反轉AUTH_TOKEN,再解密
                    //================================================
                    //JSONSTR=明碼文字
                    //JSONSTR = EncryptString(JSONSTR, "0936060016"); //3512012595
                    //================================================
                    //JSON中USER_ENCODE
                    //EX: H001748檢驗系統介接1748  => 先BaseCrypt加密,再反轉
                    //STEP2 中 = USER_ID + AUTH_TOKEN(加密前明文字) 
                    //string USER_ENCODE = antiData(Eecrypt("H000400檢驗系統介接400"));
                    //================================================


                    string STRJSON = JSONSTR;
                    STRJSON = DecryptString(STRJSON, HOS_INVOICE);

                    string strAUTH_TOKEN = Decrypt(AUTH_TOKEN);  //baseCrypt

                    //測試用
                    //STRJSON = "[{'USER_ENCODE':'==AUJedxAS/YbNtPWjuQU9OUtv3waI21voESRDAaXsJDzoO0X8fu3mIO','NHI_ID':'1532101117','IS_NEW_REPORT':'1','PATIENT_ID':'','CUS_ID':'','CUS_CT_ID':'','CUS_BIRTH':'','S_GET_BLOOD_D':'','E_GET_BLOOD_D':'','S_REPORT_DATE':'','E_REPORT_DATE':'','S_PrintDate':'','E_PrintDate':''}]";

                    //1.3 資料庫查出檢驗報告       
                    //2.  組JSON
                    strstatus = step2(STRJSON, USER_ID, strAUTH_TOKEN, USE_HIS_DATA, USE_PDF);
                }
                else
                {
                    strstatus = "9999";
                    //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息9999：" + strstatus);

                }
            }

            WriteLog(pathLogFile, "\r\n" + "]".PadLeft(50, '-'));

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
                        string ASE = Decrypt(AUTH_TOKEN);
                        //return strstatus;  這句註掉  因檢查完再RETURN  KEN  20180621
                        strstatus += ", 9997";
                    }

                    //=======必填欄位==============================================================
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
                    //================================================================================
                    //2023/01/04 jeff 新增 (美兆，由於不提供身分證所以身分證與病歷號一樣都是病歷號。如果不一致擋下來!)
                    if (NHI_ID == "3507330451" || NHI_ID == "3503270758" || NHI_ID == "3501184197")
                    {
                        if (CUS_ID != PATIENT_ID) strstatus += ", 9970";
                    }

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
                //1.3取ExlInfoNo值(SELECT NO FROM HC_EXLINFO), 院所[NO]會記錄表示哪間醫院
                string ExlInfoNo = GetProductList(NHI_ID);
                if (ExlInfoNo == "")
                {
                    strstatus += ", 9800";
                }

                if (strstatus != "")
                {
                    //寫LOG : 把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：" + strstatus);
                    return strstatus; //有錯誤跳出
                }
                string TurnHIS_TITLE = string.Empty;
                if (NHI_ID == "1502031095" || NHI_ID == "1507300022")
                {//2021/09/08 jeff 只針對547,2004
                    //2021/08/09 jeff 新增產品別轉換器
                    TurnHIS_TITLE = HIS_TITLETurn(NHI_ID, HIS_TITLE);
                }                
                else
                {//原本
                    TurnHIS_TITLE = HIS_TITLE;
                }
                //InsertData(ExlInfoNo, jsonStr);
                strstatus = Insert_HcJsondata(ExlInfoNo, jsonStr, CUS_ID, TurnHIS_TITLE, CUS_NAME, GET_BLOOD_D, CUS_BIRTH
                                  , PATIENT_ID, MEMO_TEACHER, USER_EMAIL, USER_MOIBLE, CUS_SEX, USER_ADDR, USER_UB_DATE, HOS_ID);

                //3.寫入成功，回傳OK
                return strstatus;
            }         
            catch (Exception ex)
            {
                //3.1寫入失敗，回傳ERROR
                //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：9999 \r\n" + ex.Message);
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
                    string strHis = string.Empty;
                    string snn = string.Empty;

                    USER_ENCODE = Decrypt(obj_results["USER_ENCODE"].ToString());   //USER_ENCODE 做 Decrypt
                    NHI_ID = obj_results["NHI_ID"].ToString();
                    IS_NEW_REPORT = obj_results["IS_NEW_REPORT"].ToString();
                    CUS_CT_ID = obj_results["CUS_CT_ID"].ToString();
                    S_GETBLOOD_D = obj_results["S_GET_BLOOD_D"].ToString();
                    E_GETBLOOD_D = obj_results["E_GET_BLOOD_D"].ToString();
                    CUS_BIRTH = obj_results["CUS_BIRTH"].ToString();
                    CUS_ID = obj_results["CUS_ID"].ToString();
                    S_REPORT_DATE = obj_results["S_REPORT_DATE"].ToString();
                    E_REPORT_DATE = obj_results["E_REPORT_DATE"].ToString();

                    S_PrintDate = obj_results["S_PrintDate"].ToString();  //注意S_PrintDate,這裡有分大小寫
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
                        //**** FROM VW_BIONET_WEBSH001 檢驗[項目]回傳(主要)   先查有無設定該(院所)與(項目)               
                        DataTable DT = GetCustomerData(NHI_ID, IS_NEW_REPORT, CUS_CT_ID, S_GETBLOOD_D, E_GETBLOOD_D, CUS_BIRTH, CUS_ID, S_REPORT_DATE, E_REPORT_DATE, S_PrintDate, E_PrintDate);
                        //DT.DefaultView.RowFilter = "Cus_CT_ID= 'FFDO10904176' ";
                        //DT = DT.DefaultView.ToTable();  


                        //檢驗結果回傳(HisData)
                        DataTable HisData = new DataTable();
                        //要輸出的變數
                        StringWriter sw = new StringWriter();
                        //建立JsonTextWriter
                        JsonTextWriter writer = new JsonTextWriter(sw);
                        //string PDFFlag = "N";


                        if (DT.Rows.Count == 0)
                        {
                            //無(院所)簽約(項目)
                            if (IS_NEW_REPORT == "1") strJsonValue = "9972(IS_NEW_REPORT=1) 查無報告記錄";
                            if (IS_NEW_REPORT == "0") strJsonValue = "9973(IS_NEW_REPORT=0) 查無報告記錄";

                        }
                        else if (DT.Rows.Count > 0)
                        {
                            //有該(院所)與(項目)
                            writer.WriteStartArray();

                            for (int f = 0; f < DT.Rows.Count; f++)
                            {

                                //================================================
                                //(一)取客戶的檢驗資料
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
                                //撈取VW_BIONET_WEBSH001檢驗結果, 回傳的"HIS_TITLE_ID"是源自CUSTOMER_SMA.Producttype (DT.Rows[f][""HIS_TITLE_ID].ToString())

                                //------ACER_CLOUD_CODE_LIST  WHERE  HOS_ID + HIS_TITLE_ID--------
                                //這是找Mproject='Y' 的Test_Code

                                if (DT.Rows[f]["PRODUCTTYPE"].ToString() == "CS_H" || DT.Rows[f]["PRODUCTTYPE"].ToString() == "CS_C")
                                {
                                    writer.WritePropertyName("HIS_TITLE_ID"); writer.WriteValue(DT.Rows[f]["PRODUCTTYPE"].ToString());  //如果是康癌系列直接帶檢體代號
                                }
                                else
                                {
                                    writer.WritePropertyName("HIS_TITLE_ID"); writer.WriteValue(GetHisTitleCode(DT.Rows[f]["HOS_ID"].ToString(), DT.Rows[f]["HIS_TITLE_ID"].ToString()));  //==>換成院所[主碼]
                                }
                                   
                                writer.WritePropertyName("HIS_TITLE"); writer.WriteValue(DT.Rows[f]["HIS_TITLE"].ToString());
                                //-----------------------------------------------------------------

                                writer.WritePropertyName("HOS_ID"); writer.WriteValue(DT.Rows[f]["HOS_ID"].ToString());
                                writer.WritePropertyName("MEMO"); writer.WriteValue("");


                                //(二)附件檔(USE_PDF=1)的內容(傳回Bytes[])-------
                                strBASE64_CONTENT = Base64Encode(DT.Rows[f]["CUS_CT_ID"].ToString(), DT.Rows[f]["PRODUCTTYPE"].ToString(), DT.Rows[f]["CMPNAME"].ToString());
                                //==================================================

                                DataTable CStable = new DataTable();
                                string NOCS = string.Empty;
                                #region HIS_DATA的JASON陣列
                                //數值需要調整(欄位加工)20230515 ED
                                string strproducttype = string.Empty;
                                string strhos_id = string.Empty;
                                string strcolname = string.Empty;
                                //USE_HIS_DATA 是否要顯示資料
                                if (USE_HIS_DATA == "1")
                                {   // 2022/09/21 新增康知因、癌知因的Json格式
                                    if (DT.Rows[f]["PRODUCTTYPE"].ToString() == "CS_H" || DT.Rows[f]["PRODUCTTYPE"].ToString() == "CS_C")
                                    {
                                        writer.WritePropertyName("HIS_DATA");

                                        //該ProductType的檢驗細項回傳(索引)--CUS_SMA_XLS
                                        DataTable tble = GetHisDataG(DT.Rows[f]["CUS_CT_ID"].ToString(), DT.Rows[f]["PRODUCTTYPE"].ToString());

                                        writer.WriteStartArray();                            
                                       
                                            if (tble.Rows.Count == 1)
                                            {//只有一筆可能是全正常 or 一項異常
                                                if (string.IsNullOrEmpty(tble.Rows[0]["CS_GENE"].ToString()) && string.IsNullOrEmpty(tble.Rows[0]["CS_RESULT"].ToString()) && string.IsNullOrEmpty(tble.Rows[0]["CS_VARIENT"].ToString()))
                                                {//如果三項數值欄位都是NULL or 空白，就代表全正常
                                                     CStable = GetHisDataCS(DT.Rows[0]["HIS_TITLE"].ToString(), DT.Rows[0]["PRODUCTTYPE"].ToString());
                                                }
                                                else
                                                {//如果有數值就代表這項有異常，其他正常
                                                    CStable = GetHisDataCS2(DT.Rows[0]["HIS_TITLE"].ToString(), DT.Rows[0]["PRODUCTTYPE"].ToString(), DT.Rows[0]["CUS_CT_ID"].ToString());
                                                }
                                            }
                                            else
                                            {//多筆異常
                                                CStable = GetHisDataCS2(DT.Rows[0]["HIS_TITLE"].ToString(), DT.Rows[0]["PRODUCTTYPE"].ToString(), DT.Rows[0]["CUS_CT_ID"].ToString());
                                            }


                                            for (int k = 0; k < CStable.Rows.Count; k++)
                                            {
                                                writer.WriteStartObject();
                                                writer.WritePropertyName("HIS_ID"); writer.WriteValue(CStable.Rows[k]["Test_Code"].ToString());
                                                writer.WritePropertyName("HIS_NAME"); writer.WriteValue(CStable.Rows[k]["Test_Name"].ToString());

                                            if (string.IsNullOrEmpty(tble.Rows[0]["CS_GENE"].ToString()) && string.IsNullOrEmpty(tble.Rows[0]["CS_RESULT"].ToString()) && string.IsNullOrEmpty(tble.Rows[0]["CS_VARIENT"].ToString()))
                                            {//如果三項數值欄位都是NULL or 空白，就代表全正常
                                                    writer.WritePropertyName("HIS_RESULT"); writer.WriteValue("未檢出");
                                                    writer.WritePropertyName("HIS_RESULT2"); writer.WriteValue("");
                                                    writer.WritePropertyName("HIS_RESULT3"); writer.WriteValue("");
                                                    writer.WritePropertyName("HIS_VALUE"); writer.WriteValue("");
                                                }
                                                else
                                                {
                                                    writer.WritePropertyName("HIS_RESULT"); writer.WriteValue("檢出異常");
                                                    writer.WritePropertyName("HIS_RESULT2"); writer.WriteValue(CStable.Rows[k]["Attribution"].ToString());//遺傳模式
                                                    writer.WritePropertyName("HIS_RESULT3"); writer.WriteValue(CStable.Rows[k]["CS_RESULT"].ToString());//異型合子
                                                    writer.WritePropertyName("HIS_VALUE"); writer.WriteValue(CStable.Rows[k]["CS_VARIENT"].ToString());//數值
                                                }
                                               
                                                //取Unit值
                                                writer.WritePropertyName("HIS_UNIT"); writer.WriteValue("");

                                                if (string.IsNullOrEmpty(tble.Rows[0]["CS_GENE"].ToString()) && string.IsNullOrEmpty(tble.Rows[0]["CS_RESULT"].ToString()) && string.IsNullOrEmpty(tble.Rows[0]["CS_VARIENT"].ToString()))
                                                {//如果三項數值欄位都是NULL or 空白，就代表全正常
                                                    writer.WritePropertyName("HIS_MEMO"); writer.WriteValue("");
                                                }
                                                else
                                                {
                                                    writer.WritePropertyName("HIS_MEMO"); writer.WriteValue(tble.Rows[0]["CS_SN"].ToString());//多筆異常筆數
                                                }
                                                    
                                                writer.WriteEndObject();
                                            }
                                        

                                        writer.WriteEndArray();

                                    }
                                    else //下面原本的架構
                                    {

                                        writer.WritePropertyName("HIS_DATA");

                                        //該ProductType的檢驗細項回傳(索引)--ACER_CLOUD_CODE_LIST
                                        DataTable tble = GetHisData(DT.Rows[f]["HOS_ID"].ToString(), DT.Rows[f]["PRODUCTTYPE"].ToString());
                                        //數值需要調整(欄位加工)20230515 ED
                                        strproducttype = DT.Rows[f]["PRODUCTTYPE"].ToString();
                                        strhos_id = DT.Rows[f]["HOS_ID"].ToString();

                                        writer.WriteStartArray();
                                        //2021/10/27  jeff 新增 聖功客製化Json                                 



                                        for (int i = 0; i < tble.Rows.Count; i++)
                                        {
                                            //2018.03.22 要Mproject不等於Y 不是大項時才輸出HisData
                                            if (tble.Rows[i]["Mproject"].ToString() != "Y")
                                            {
                                                //SMA_41只有大項，移到else，但不確定未來會否有SMA類別，這裡先保留

                                                switch (tble.Rows[i]["TABLENAME"].ToString())
                                                {
                                                    case "CUSTOMER_SMA":
                                                        HisData = GetHisDataA(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                                        break;

                                                    case "CUSTOMER_SMA_DETAIL":
                                                        HisData = GetHisDataB(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());

                                                        //數值需要調整(欄位加工)20230515 ED
                                                        strcolname = tble.Rows[i]["COLNAME"].ToString();

                                                        break;//以後工程註記，附加報告細項上版。  施工方向: 資料庫ACER_CLOUD_CODE_LIST加上附加細項，之後再下面放值前先用 switch..case 把
                                                        //附加報告的每一項數值都判別成高低風險在輸出

                                                    case "ANALYSE":
                                                        HisData = GetHisDataC(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                                        break;

                                                    case "PGD_MAIN":
                                                        HisData = GetHisDataD(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                                        break;

                                                    case "Customer_SMA_HCWS": //2021/10/29 jeff 新增 組合細項    2022/07/11 jeff修改帶入參數Attribution
                                                        HisData = GetHisDataE(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["Attribution"].ToString());
                                                        strHis = HisData.Rows[0][0].ToString();
                                                        break;
                                                }

                                            }
                                            else
                                            {
                                                //SMA_41只有大項，所以遇到大項時要轉出檢驗結果
                                                if (tble.Rows[i]["TABLENAME"].ToString() == "CUSTOMER_SMA")
                                                {
                                                    HisData = GetHisDataA(DT.Rows[f]["CUS_CT_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString());
                                                }

                                            }



                                            //if (strBASE64_CONTENT != "")    //放這幹嘛? 萬一沒設定取DATA, 連PDF也找不到了
                                            //{ PDFFlag = "Y"; }
                                            //else
                                            //{ PDFFlag = "N"; }

                                            for (int k = 0; k < HisData.Rows.Count; k++)
                                            {
                                                writer.WriteStartObject();
                                                writer.WritePropertyName("HIS_ID"); writer.WriteValue(tble.Rows[i]["Test_Code"].ToString());
                                                writer.WritePropertyName("HIS_NAME"); writer.WriteValue(tble.Rows[i]["Test_Name"].ToString());

                                                if (tble.Rows[i]["COLNAME"].ToString() == "")
                                                {
                                                    //COLNAME空白, 則無傳回值
                                                    writer.WritePropertyName("HIS_VALUE"); writer.WriteValue("");
                                                }
                                                else
                                                {
                                                    //writer.WritePropertyName("HIS_VALUE"); writer.WriteValue(HisData.Rows[k][tble.Rows[i]["COLNAME"].ToString()].ToString());
                                                    writer.WritePropertyName("HIS_VALUE");
                                                    //20210713 jeff 補上Suggest5內有多項
                                                    if (tble.Rows[i]["COLNAME"].ToString() == "Suggest5")
                                                    {
                                                        string Suggest5 = GETSuggest5(HisData.Rows[k][tble.Rows[i]["COLNAME"].ToString()].ToString(), tble.Rows[i]["Test_Name"].ToString());
                                                        writer.WriteValue(Suggest5);
                                                    }
                                                    else if (tble.Rows[i]["TABLENAME"].ToString() == "Customer_SMA_HCWS")
                                                    {
                                                        writer.WriteValue(strHis);
                                                    }
                                                    else
                                                    {
                                                        //原來的
                                                        //writer.WriteValue(HisData.Rows[k][tble.Rows[i]["COLNAME"].ToString()].ToString());
                                                        //數值需要調整(欄位加工)20230515 ED
                                                        writer.WriteValue(outcome(strproducttype, strcolname, HisData.Rows[k][tble.Rows[i]["COLNAME"].ToString()].ToString(), strhos_id));
                                                        
                                                    }
                                                }

                                                //取Unit值
                                                writer.WritePropertyName("HIS_UNIT"); writer.WriteValue(GetHISData_Unit(DT.Rows[f]["HIS_TITLE_ID"].ToString(), tble.Rows[i]["COLNAME"].ToString()));
                                                writer.WritePropertyName("HIS_MEMO"); writer.WriteValue("");
                                                writer.WriteEndObject();
                                            }
                                        }

                                        writer.WriteEndArray();

                                    }


                                   
                                }
                                #endregion
                               // snn = sw.ToString();


                                if (USE_PDF == "1")
                                {
                                    writer.WritePropertyName("FILE_DATA");
                                    writer.WriteStartArray();

                                    if (strBASE64_CONTENT != "")  //表示有PDF
                                    {
                                        WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " :: 有PDF");
                                        writer.WriteStartObject();
                                        writer.WritePropertyName("BASE64_CONTENT"); writer.WriteValue(strBASE64_CONTENT);
                                        //writer.WritePropertyName("BASE64_CONTENT"); writer.WriteValue("測試用PDF密文");
                                        writer.WritePropertyName("FILE_TYPE"); writer.WriteValue("PDF");
                                        writer.WritePropertyName("DIGEST_VALUE"); writer.WriteValue("");
                                        writer.WriteEndObject();
                                    }

                                    writer.WriteEndArray();
                                }

                                writer.WriteEndObject();
                                //writer.WriteEndArray();
                                //2018.02.21 移至此處(為了應對可能不只一筆時)

                                //=======================================================================================
                                string tCusID = DT.Rows[f]["CUS_ID"].ToString();

                                //2018.03.22 每天查詢新完成報套(IS_NEW_REPORT =1), 
                                //*** 必有抓到PDF檔時才進行註記 *****
                                if ((strBASE64_CONTENT != "") && (IS_NEW_REPORT == "1"))
                                {
                                    //2018.06.21 KEN  HIS_TITLE_ID是源自CUSTOMER_SMA.Producttype
                                    UpdateFlag(tCusID, DT.Rows[f]["HIS_TITLE_ID"].ToString());  //REPORT_UPLOAD='Y'
                                }
                                //=======================================================================================

                            }
                            //輸出結果
                            //                            HttpContext.Current.Response.Write(sw.ToString());
                            writer.WriteEndArray();


                            //不加密回傳(測試用)
                            //strJsonValue = sw.ToString();
                            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Return結果(不加密)：" + sw.ToString());


                            //2.1 加密後回傳          
                            strJsonValue = EncryptString(sw.ToString(), HOS_INVOICE);
                            string endstr = DecryptString(strJsonValue, HOS_INVOICE);
                        }
                     
                    }
                    else
                    {
                        strJsonValue = "9977";
                        //寫LOG
                        //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                        WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：9977");
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
                WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 錯誤訊息：9999--" + ex.Message);
           
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

            //有特定篩選的項目擺前面判斷, 通用的放後段
            if (productType == "FXS")
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
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
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
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
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
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
            //2021/07/30 jeff 新增品項
            if (productType.Contains("FFD_P"))
            {
                string folder = cusCtId.Substring(0, 7);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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
            if (productType.Contains("FFD_F"))
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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
            if (productType.Contains("FFD_T"))
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FFD_15"))
            {
                string folder = cusCtId.Substring(0, 7);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FFD_8"))
            {
                string folder = cusCtId.Substring(0, 7);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FFD_M"))
            {
                string folder = cusCtId.Substring(0, 7);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FDS"))
            {
                string folder = cusCtId.Substring(0, 6);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FXS_1"))
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FXS_2"))
            {
                string folder = cusCtId.Substring(0, 5);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("PES2"))
            {
                string folder = cusCtId.Substring(0, 6);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("SDS"))
            {
                string folder = cusCtId.Substring(0, 6);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("SMA_41"))
            {
                string folder = cusCtId.Substring(0, 4);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("FFD_O"))
            {
                string folder = cusCtId.Substring(0, 7);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            if (productType.Contains("PES1"))
            {
                string folder = cusCtId.Substring(0, 6);
                string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
                string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
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

            //if (productType.Contains("CSC"))
            //{
            //    string folder = cusCtId.Substring(0, 6);
            //    string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
            //    string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
            //    if (System.IO.File.Exists(path2))
            //    {
            //        var result2 = System.IO.File.ReadAllBytes(path2);
            //        return Convert.ToBase64String(result2);
            //    }
            //    else if (System.IO.File.Exists(path3))
            //    {
            //        var result3 = System.IO.File.ReadAllBytes(path3);
            //        return Convert.ToBase64String(result3);
            //    }
            //    else
            //    {
            //        return "";
            //    }

            //}
            ////補上康癌取報告(未使用)
            //if (productType.Contains("CSH"))
            //{
            //    string folder = cusCtId.Substring(0, 6);
            //    string path2 = @"\\sma\out\" + folder + @"\" + cusCtId + ".pdf";
            //    string path3 = @"\\lis\IN\" + folder + @"\" + cusCtId + ".pdf";
            //    if (System.IO.File.Exists(path2))
            //    {
            //        var result2 = System.IO.File.ReadAllBytes(path2);
            //        return Convert.ToBase64String(result2);
            //    }
            //    else if (System.IO.File.Exists(path3))
            //    {
            //        var result3 = System.IO.File.ReadAllBytes(path3);
            //        return Convert.ToBase64String(result3);
            //    }
            //    else
            //    {
            //        return "";
            //    }

            //}

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

            return "";  //都沒有就空值

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
            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "將身分證號：" + CusID + "的Report_Upload=Y. eff=" + eff);
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
            sql = "SELECT HOS_INVOICE FROM HOSPITAL_COOPERATION \r\n";
            sql += "WHERE USER_ID ='" + USER_ID + "'  \r\n";
            sql += "AND USER_PW ='" + USER_PW + "'  \r\n";
            sql += "AND AUTH_TOKEN ='" + AUTH_TOKEN + "'  \r\n";
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
            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " SQL 是否有這合作醫院(HOSPITAL_COOPERATION) : \r\n" + sql);

            return strName;
        }

        //檢驗結果回傳(主要)
        public DataTable GetCustomerData(string NHI_ID, string IS_NEW_REPORT, string CUS_CT_ID, string S_GETBLOOD_D, string E_GETBLOOD_D, string CUS_BIRTH, string CUS_CID, string S_REPORT_D, string E_REPORT_D, string S_PRINT_D, string E_PRINT_D)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            ////取出院所HOS_ID, 以在SQL中限制只傳出有設定的產品
            //string t1 = " SELECT HOS_ID  FROM  HC_EXLINFO  WHERE  isEnabled = 1  AND HOS_INVOICE= '" + NHI_ID + "' ";
            //cmd.CommandText = t1;
            //2022/07/18 新增用NHI_ID取得 ExlInfoNo
            string EX_NO = GET_EX_NO(NHI_ID);

            //=====這是特例===秉坤做FXS_2(組合), 把FXS1修正為FXS_2. 單獨FXS1不動=======
            if (NHI_ID == "1532101117")
            {
                sql = "  SELECT  A.SN, A.ProductType, C1.ProductType, C2.ProductType, B.Cus_CID, B.Cus_Name, B.GetBlood_D  \r\n"; //A.REPORT_UPLOAD 
                sql += " FROM   dbo.HC_JSONDATA A  JOIN  HC_CUS_DATA B ON A.SN = B.JSON_SN  \r\n";
                sql += " LEFT   JOIN   GGADB..CUSTOMER_SMA  C1 ON  B.Cus_CID = C1.Cus_CID  AND  A.ProductType = C1.ProductType   \r\n";
                sql += " LEFT   JOIN   GGADB..CUSTOMER_SMA  C2 ON  B.Cus_CID = C2.Cus_CID  AND  B.GetBlood_D = C2.GetBlood_D  AND C2.ProductType LIKE 'FXS%'  \r\n";
                sql += " WHERE  B.ExlInfoNo =26  \r\n";       //26=秉坤
                sql += " AND  A.ProductType ='FXS_1'  AND  C1.ProductType IS NULL  AND  C2.ProductType ='FXS_2'   \r\n";  //表示上傳FXS_1, 但CUSTOMER_SMA =FXS_2

                DataTable ta1 = new DataTable();
                cmd.CommandText = sql;
                SqlDataAdapter dap1 = new SqlDataAdapter(cmd);
                dap1.Fill(ta1);

                DataView DV1 =ta1.DefaultView ;
                for (int i = 0; i <= DV1.Count - 1; i++)
                {
                    sql = " UPDATE  HC_JSONDATA    \r\n";
                    sql += " SET  ProductType = 'FXS_2'  \r\n";
                    sql += " , RMK ='秉坤FXS1=>FXS2'  \r\n";
                    sql += " WHERE  SN ='" + DV1[i]["SN"].ToString() + "'  \r\n";
                    cmd.CommandText = sql;
                    int ef1 = cmd.ExecuteNonQuery();
                }

                //PES1 => PES2
                sql = "  SELECT  A.SN, A.ProductType, C1.ProductType, C2.ProductType, B.Cus_CID, B.Cus_Name, B.GetBlood_D  \r\n";
                sql += " FROM   dbo.HC_JSONDATA  A  JOIN  HC_CUS_DATA B ON A.SN = B.JSON_SN  \r\n";
                sql += " LEFT   JOIN   BBCMS..CUSTOMER_SMA  C1 ON  B.Cus_CID = C1.Cus_CID  AND  A.ProductType = C1.ProductType     \r\n";
                sql += " LEFT   JOIN   BBCMS..CUSTOMER_SMA  C2 ON  B.Cus_CID = C2.Cus_CID  AND  B.GetBlood_D = C2.GetBlood_D AND C2.ProductType LIKE 'PES%'  \r\n";
                sql += " WHERE  B.ExlInfoNo = 26  \r\n";       //秉坤
                sql += " AND  A.ProductType ='PES1'  AND  C1.ProductType IS NULL  AND  C2.ProductType ='PES2'  \r\n";

                DataTable ta2 = new DataTable();
                cmd.CommandText = sql;
                SqlDataAdapter dap2 = new SqlDataAdapter(cmd);
                dap2.Fill(ta2);

                DataView DV2 = ta2.DefaultView;
                for (int i = 0; i <= DV2.Count - 1; i++)
                {
                    sql = " UPDATE  HC_JSONDATA    \r\n";
                    sql += " SET  ProductType = 'PES2'  \r\n";
                    sql += " , RMK ='秉坤PES1=>PES2'  \r\n";
                    sql += " WHERE  SN ='" + DV2[i]["SN"].ToString() + "'  \r\n";
                    cmd.CommandText = sql;
                    int ef1 = cmd.ExecuteNonQuery();
                }
            }
            else if (NHI_ID == "1502031095" || NHI_ID == "1507300022" || NHI_ID == "1107350015")//2021/10/06 jeff  新增 兩間慧馨 FXS， 把FXS_1修正為FXS_2. 單獨FXS_1不動=======
            {
                sql = "  SELECT  A.SN, A.ProductType, C1.ProductType, C2.ProductType, B.Cus_CID, B.Cus_Name, B.GetBlood_D  \r\n"; //A.REPORT_UPLOAD 
                sql += " FROM   dbo.HC_JSONDATA A  JOIN  HC_CUS_DATA B ON A.SN = B.JSON_SN  \r\n";
                sql += " LEFT   JOIN   GGADB..CUSTOMER_SMA  C1 ON  B.Cus_CID = C1.Cus_CID  AND  A.ProductType = C1.ProductType   \r\n";
                sql += " LEFT   JOIN   GGADB..CUSTOMER_SMA  C2 ON  B.Cus_CID = C2.Cus_CID  AND  B.GetBlood_D = C2.GetBlood_D  AND C2.ProductType LIKE 'FXS%'  \r\n";
                //2022/07/18 修改用各院所ExlInfoNo去撈資料
                sql += " WHERE  B.ExlInfoNo in ('" + EX_NO + "')  \r\n";       //34 = 馨蕙馨醫院 , 38 = 博愛蕙馨醫院                
                sql += " AND  A.ProductType ='FXS_1'  AND  C1.ProductType IS NULL  AND  C2.ProductType ='FXS_2'   \r\n";  //表示上傳FXS_1, 但CUSTOMER_SMA =FXS_2

                DataTable ta1 = new DataTable();
                cmd.CommandText = sql;
                SqlDataAdapter dap1 = new SqlDataAdapter(cmd);
                dap1.Fill(ta1);

                DataView DV1 = ta1.DefaultView;
                for (int i = 0; i <= DV1.Count - 1; i++)
                {
                    sql = " UPDATE  HC_JSONDATA    \r\n";
                    sql += " SET  ProductType = 'FXS_2'  \r\n";
                    sql += " , RMK ='FXS_1=>FXS_2'  \r\n";
                    sql += " WHERE  SN ='" + DV1[i]["SN"].ToString() + "'  \r\n";
                    cmd.CommandText = sql;
                    int ef1 = cmd.ExecuteNonQuery();
                }
            }
            //=======================================================================

            //下句SQL是從VW_BIONET_WEBSH001 改寫
            //distinct拿掉   20200818
            sql = "SELECT  * FROM  \r\n";
            sql += " (   \r\n";
            sql += "  SELECT  A.Cus_CT_ID, A.CmpName, A.ProductType, A.PrintDate, A.HOS_ID, B.NHI_ID, B.HOS_NAME, \r\n";
            sql += "  A.Cus_CID AS CUS_ID, A.Patient_ID, A.Cus_Birth, C.DR_NAME, A.GetBlood_D AS GET_BLOOD_D,  \r\n";
            sql += "  A.Report_Date, D.ProductType AS HIS_TITLE_ID, D.ProductDesc AS HIS_TITLE, '' AS MEMO,  E.REPORT_UPLOAD \r\n";

            sql += "  FROM   dbo.CUSTOMER_SMA AS A LEFT  JOIN   dbo.HOSPITAL AS B ON A.HOS_ID = B.HOS_ID  \r\n";
            sql += "  LEFT   JOIN  dbo.DOCTOR AS C ON A.Doc_ID = C.DR_ID   \r\n";
            sql += "  LEFT   JOIN  dbo.ProductList AS D ON A.ProductType = D.ProductType  \r\n";

            //GetBlood_D規格書寫必填, 子句SQL可再WHERE縮小
            sql += "  INNER  JOIN  dbo.HC_JSONDATA AS E ON A.Cus_CID = E.CUS_ID AND A.ProductType = E.ProductType  \r\n";
            //sql += "  INNER  JOIN  (SELECT A.SN, B.Cus_CID, B.Cus_Name, B.GetBlood_D, A.REPORT_UPLOAD  FROM dbo.HC_JSONDATA A JOIN HC_CUS_DATA B ON A.SN =B.JSON_SN ) AS E ON A.Cus_CID = E.CUS_CID AND A.GetBlood_D =E.GetBlood_D ";

            sql += "  UNION  \r\n";
            sql += "  SELECT  A.Cus_CT_ID, A.CmpName, A.ProductType, A.PrintDate, A.HOS_ID, B.NHI_ID, B.HOS_NAME,  \r\n";
            sql += "  A.Cus_CID, A.Patient_ID, A.Cus_Birth, C.DR_NAME, A.GetBlood_D AS GET_BLOOD_D,  \r\n";
            sql += "  A.Report_Date, D.ProductType AS HIS_TITLE_ID, D.ProductDesc AS HIS_TITLE, '' AS MEMO,  E.REPORT_UPLOAD  \r\n";

            sql += "  FROM   GGADB.dbo.CUSTOMER_SMA AS  A   LEFT   JOIN  dbo.HOSPITAL AS B ON A.HOS_ID = B.HOS_ID  \r\n";
            sql += "  LEFT   JOIN  dbo.DOCTOR AS C ON A.Doc_ID = C.DR_ID  \r\n";
            sql += "  LEFT   JOIN  dbo.ProductList AS D ON A.ProductType = D.ProductType  \r\n";

            sql += "  INNER  JOIN  dbo.HC_JSONDATA AS E ON A.Cus_CID = E.CUS_ID AND A.ProductType = E.ProductType  \r\n";
            //sql += "  INNER  JOIN  (SELECT A.SN, B.Cus_CID, B.Cus_Name, B.GetBlood_D, A.REPORT_UPLOAD  FROM dbo.HC_JSONDATA A JOIN HC_CUS_DATA B ON A.SN =B.JSON_SN ) AS E ON A.Cus_CID = E.CUS_CID AND A.GetBlood_D =E.GetBlood_D ";
            sql += " )  A  \r\n"; 

            
            //這裡開始都是帶條件式
            sql += " WHERE  A.PrintDate <> ''  \r\n";

            sql += " AND   A.NHI_ID = '" + NHI_ID + "'  \r\n";


            //限定只傳有設定的產品  20200810  ==>  先不限制, 因為限制後會PDF也拿不到(頂多拿不到傳回資料值, 但可以拿到PDF)  20200811
            //sql += " AND   A.ProductType IN(SELECT DISTINCT  ProjectType  FROM  ACER_CLOUD_CODE_LIST  WHERE  HOS_ID = '" + HOS_ID + "') ";


            //只查詢最新完成的報告0:否;1:是
            //若使用此條件(IS_NEW_REPORT == "1")，則條件改為PRINTDATE<= T-1
            if (IS_NEW_REPORT == "1")
            {
                //sql += " AND A.PRINTDATE > '" + DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd") + "'";
                sql += " AND A.PRINTDATE >= '" + DateTime.Now.AddDays(-14).ToString("yyyy/MM/dd 00:00:00") + "'   \r\n";
                sql += " AND A.PRINTDATE <= '" + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd 23:59:59") + "'   \r\n";
                //是否已被查詢傳輸資料(Y:已送 N:未送)
                //sql += " AND A.REPORT_UPLOAD is null ";
                sql += " AND A.REPORT_UPLOAD='N' ";
            }
            else if (IS_NEW_REPORT == "0")
            {
                if (CUS_CT_ID != "")
                {
                    sql += " AND A.CUS_CT_ID = '" + CUS_CT_ID + "'   \r\n";
                }
                if (S_GETBLOOD_D != "" & E_GETBLOOD_D != "")
                {
                    sql += " AND A.GET_BLOOD_D BETWEEN '" + S_GETBLOOD_D + "' AND '" + E_GETBLOOD_D + "'   \r\n";
                }
                if (CUS_BIRTH != "") //CUS_BIRTH  EX: 1968/01/08
                {
                    //先轉成datetime, 再轉成文字EX: 1968/01/08, 因為CUSTOMER_SMA的CUS_BIRTH(文字), 有時不會格式化EX: 2001/8/12
                    sql += " AND  CONVERT(varchar, CONVERT(datetime, A.CUS_BIRTH), 111) = '" + CUS_BIRTH + "'   \r\n";
                }
                if (CUS_CID != "")
                {
                    sql += " AND A.CUS_ID = '" + CUS_CID + "'   \r\n";   //注意這是別名(CUS_CID ==> CUS_ID), 因後面讀資料是CUS_ID, 為求簡便這樣做  20201208
                }
                if (S_REPORT_D != "" & E_REPORT_D != "") //REPORT_DATE  EX:2018/06/05
                {
                    sql += " AND A.REPORT_DATE BETWEEN '" + S_REPORT_D + "' AND '" + E_REPORT_D + "'   \r\n";
                }
                if (S_PRINT_D != "" & E_PRINT_D != "")
                {
                    sql += " AND A.PRINTDATE BETWEEN '" + S_PRINT_D + " 00:00:00' AND '" + E_PRINT_D + " 23:59:59'   \r\n";
                }
                //是否已被查詢傳輸資料(Y:已送 N:未送)
                //sql += " AND A.REPORT_UPLOAD='N' ";
            }

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetCustomerData：" + sql);


            cmd.CommandText = sql;
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

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisTitleCode：" + sql);

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
            {//2021/10/15 jeff 新增 兩家蕙馨FXS客製化
                if (HOS_ID == "547" || HOS_ID == "2004")
                {
                    if (PROJECTTYPE == "FXS_2")
                    {
                        Code = "FXS";
                    }
                    else
                    {
                        Code = cmd.ExecuteScalar().ToString();
                    }
                }
                else
                {
                    Code = cmd.ExecuteScalar().ToString();
                }
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

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisData：" + sql);

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

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataA：" + sql);

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
            sql += " AND Cus_CT_ID = '" + CUSCTID + "'";

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataB：" + sql);

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

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataC：" + sql);

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

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataD：" + sql);

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容E)
        public DataTable GetHisDataE(string CUSCTID, string Attribution)
        {// 2021/1/01 jeff 新增
            DataTable dt = new DataTable();
            DataTable HisData = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            string strGA = string.Empty;
            string str = string.Empty;
           SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT TableName,ColName FROM Customer_SMA_HCWS ";
            sql += " WHERE 1=1  ";
            sql += " AND Attribution = '" + Attribution + "'";

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataE：" + sql);

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            conn.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {//以迴圈的方式判斷細項內有沒有高風險
                switch (dt.Rows[i]["TABLENAME"].ToString())
                {
                    case "CUSTOMER_SMA":
                        HisData = GetHisDataA(CUSCTID, dt.Rows[i]["COLNAME"].ToString());
                        strGA = dt.Rows[i]["COLNAME"].ToString();
                        str = HisData.Rows[0][strGA].ToString(); //str 以下用來判斷有沒有高風險                      
                        break;

                    case "CUSTOMER_SMA_DETAIL":
                        HisData = GetHisDataB(CUSCTID, dt.Rows[i]["COLNAME"].ToString());
                        strGA = dt.Rows[i]["COLNAME"].ToString();
                        str = HisData.Rows[0][strGA].ToString(); //str 以下用來判斷有沒有高風險                       
                        break;                    
                }
                if (str == "低風險")
                {//如果是低風險繼續下一種細項
                }
                else if (str == "高風險")
                {//如果是高風險直接離開 回傳HisData為高風險
                    break;
                }
            }
                return HisData;
        }

        //檢驗結果回傳(檢驗內容G)
        public DataTable GetHisDataG(string CUSCTID, string ColName)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT * FROM  CUS_SMA_XLS ";
            sql += " WHERE 1=1  ";
            sql += " AND CUS_CT_ID = '" + CUSCTID + "'";

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataG：" + sql);

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容CS)
        public DataTable GetHisDataCS(string HOS_ID, string PROJECTTYPE)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT * FROM  ACER_CLOUD_CODE_LIST ";
            sql += " WHERE 1=1  ";
            sql += " AND HOS_ID = '" + HOS_ID + "'";
            sql += " AND Mproject is null";
            sql += " order by Test_Name desc";

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataCS：" + sql);

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            conn.Close();
            return dt;
        }

        //檢驗結果回傳(檢驗內容CS異常)
        public DataTable GetHisDataCS2(string HOS_ID, string PROJECTTYPE, string CUS_CT_ID)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Address = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];
            sql = "SELECT * FROM  ACER_CLOUD_CODE_LIST a ";
            sql += " left join CUS_SMA_XLS x on x.CS_GENE = a.ProjectType and cus_ct_id = '"+ CUS_CT_ID + "'  ";
            sql += " WHERE 1=1  ";
            sql += " AND a.HOS_ID = '" + HOS_ID + "'";
            sql += " AND Mproject is null";
            sql += " order by Test_Name desc";

            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetHisDataCS異常：" + sql);

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
            sql = "SELECT NO FROM HC_EXLINFO WHERE isEnabled = 1 \r\n";
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
            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  取HC_EXLINFO中院所NO：" + No + " \r\n" + sql);

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
            WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  寫入HC_JSONDATA SQL語法:" + cmd.CommandText);
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
                cmd.CommandText += ";SELECT IDENT_CURRENT ('HC_JSONDATA') AS JSON_SN ";

                cmd.Parameters.AddWithValue("@param1", ExlInfoNo);
                cmd.Parameters.AddWithValue("@param2", jsonStr);
                cmd.Parameters.AddWithValue("@param3", "");
                cmd.Parameters.AddWithValue("@param4", DateTime.Now);
                cmd.Parameters.AddWithValue("@param5", CusID);
                cmd.Parameters.AddWithValue("@param6", "N");
                cmd.Parameters.AddWithValue("@param7", ProductType);

                long JSON_SN = Convert.ToInt64(cmd.ExecuteScalar());    //拉大成long, 20210219
                WriteLog(pathLogFile, "\r\n  JSON_SN：" + JSON_SN + " SQL：" + cmd.CommandText);

                //(2)記錄該人===================================================
                SqlConnection conn2 = new SqlConnection(ConfigurationManager.AppSettings["BBCMS"]);
                conn2.Open();
                SqlTransaction sTrans2 = conn2.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlCommand cmd2 = conn2.CreateCommand();
                cmd2.Transaction = sTrans2;
                //cmd.Parameters.Clear();  //不知是否這句話的問題???  因為沒有清除上面 Parameters, 導致取MAX(SN)常常取錯而Pkey重複? 
                cmd2.CommandText = " SELECT MAX(SN) CUS_SN FROM HC_CUS_DATA ";
                long CUS_SN = Convert.ToInt64(cmd2.ExecuteScalar());
                CUS_SN += 1;
                conn2.Close();

                WriteLog(pathLogFile, "\r\n  CUS_SN：" + CUS_SN + " SQL：" + cmd2.CommandText);
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
                cmd.Parameters.AddWithValue("@Cus_Sex",CUS_SEX );
                cmd.Parameters.AddWithValue("@Addr", USER_ADDR);
                cmd.Parameters.AddWithValue("@UB_Date", USER_UB_DATE);
                cmd.Parameters.AddWithValue("@HOS_ID", HOS_ID);
                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //Cus_CT_ID
                //CASE_ID
                //cmd.Parameters.Clear();
                //--------SQL成不成功都要LOG----------------------------------
                string p = "";  //LOG要寫入Parameters.Value
                for (int i = 0; i <= cmd.Parameters.Count - 1; i++)
                    p += "; " + cmd.Parameters[i].Value.ToString();

                WriteLog(pathLogFile, "\r\n \r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 寫入HC_JSONDATA SQL語法:" + cmd.CommandText + " \r\n (" + p.Substring(1) + ")");
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
                WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 寫入HC_JSONDATA 成功! \r\n \r\n");
            }
            else
            {
                WriteLog(pathLogFile, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 寫入HC_JSONDATA 失敗! \r\n \r\n" + errtxt);
                errcode = "9996";
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
        //2021/08/09 jeff 新增產品別轉換器
        public string HIS_TITLETurn(string NHI_ID,string HIS_TITLE)
        {
            DataTable dt = new DataTable();
            string HIS_TITLENEW = string.Empty;
            string strSQL = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            strSQL = " select DISTINCT ProjectType ";
            strSQL += " from ACER_CLOUD_CODE_LIST  ";
            strSQL += " where Test_Code ='" + HIS_TITLE + "' ";          
            cmd = new SqlCommand(strSQL, conn);
            conn.Open();
            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            conn.Close();

            HIS_TITLENEW = dt.Rows[0]["ProjectType"].ToString();

            return HIS_TITLENEW;
        }

        public string GETSuggest5(string str, string name)
        {
            string Suggest5 = string.Empty;
            if (name == "透納氏症") {
                if (str == "低風險")
                {
                    Suggest5 = "低風險";
                }
                else
                {
                    Suggest5 = "高風險";
                }
                
            }
            else if (name == "三X染色體症候群")
            {
                if (str == "低風險")
                {
                    Suggest5 = "低風險";
                }
                else                {
                    Suggest5 = "高風險";
                }
            }
            else if (name == "柯林菲特氏症")
            {
                if (str == "低風險")
                {
                    Suggest5 = "低風險";
                }
                else
                {
                    Suggest5 = "高風險";
                }
            }
            else if (name == "47,XYY症候群")
            {
                if (str == "低風險")
                {
                    Suggest5 = "低風險";
                }
                else
                {
                    Suggest5 = "高風險";
                }
            }
            else if (name == "48,XXXX症候群")
            {
                if (str == "低風險")
                {
                    Suggest5 = "低風險";
                }
                else
                {
                    Suggest5 = "高風險";
                }
            }
            else if (name == "性染色體數目異常")
            {//以上幾項的組合項(5合1 or 4合1)
                if (str == "低風險")
                {//如果(5合1 or 4合1)都是低風險，必為低風險
                    Suggest5 = "低風險";
                }
                else
                {//(5合1 or 4合1)其中有不是低風險的，那就是高風險
                    Suggest5 = "高風險";
                }
            }
            else if (name == "低風險")
            {                
                    Suggest5 = "低風險";               
            }
            return Suggest5;
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

            string returnStr = "";
            if (md5Byte != null)
            {
                for (int i = 0; i < md5Byte.Length; i++)
                {
                    returnStr += md5Byte[i].ToString("X2");
                }
            }



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


        //LOG寫入
        public void WriteLog(string LogFilePath, string Msg)
        {
            try
            {
                File.AppendAllText(LogFilePath, Msg);
            }
            catch (Exception ex)
            {
                string ErrLogDir = @"C:\Hospital_Log\LogError";
                Directory.CreateDirectory(ErrLogDir);

                File.AppendAllText(ErrLogDir + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", Msg + " \r\n \r\n " + ex.Message);
            }
        }
        public string GET_EX_NO(string NHI_ID)
        {//2022/07/18 新增用NHI_ID取得 ExlInfoNo
            DataTable dt = new DataTable();
            string EXNO = string.Empty;
            string strSQL = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings["BBCMS"];

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            strSQL = " SELECT NO,*  FROM  HC_EXLINFO ";            
            strSQL += " where HOS_INVOICE = '" + NHI_ID + "' ";
            cmd = new SqlCommand(strSQL, conn);
            conn.Open();
            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            conn.Close();

            EXNO = dt.Rows[0]["NO"].ToString();
            return EXNO;
        }

        //結果值加工函式==================================2023.05.09 by ED
        public string outcome(string producttype, string colume, string value, string Hos_ID)
        {
            string strValue = string.Empty;
            if (producttype == "MT")
            {
                if (colume == "CHECK_RESULT_6")
                {
                    if (value.IndexOf("無") >= 0)
                    {
                        value = "不具";
                    }
                    else if (value == "")
                    {
                        value = "";
                    }
                    else
                    {
                        value = "具";
                    }
                }
            }
            else if (producttype == "RTD")
            {
                if (colume == "CHECK_RESULT_6")
                {
                    if (value.IndexOf("有-同型") >= 0)
                    {
                        value = "具";
                    }
                    else if (value.IndexOf("有-異型") >= 0)
                    {
                        value = "具";
                    }
                    else if (value.IndexOf("無") >= 0)
                    {
                        value = "不具";
                    }
                    else if (value == "")
                    {
                        value = "";
                    }
                }
            }
            else if (producttype == "SNP_750K")
            {
                //========================================
                //2023.05.09 調整
                //院所聖功(545)希望SNP系列，如果分析結果有其他這個字眼，內容值改成功院所本身希望的罐頭訊息
                //========================================
                if (colume == "EMBRYO_BLANK")
                {
                    if (Hos_ID == "545")
                    {
                        if (value.IndexOf("其他") >= 0)
                        {
                            value = "此客戶檢驗結果疑似高風險，請詳閱PDF報告";
                        }
                    }
                }
            }
            else if (producttype == "SNP_750K_P")
            {
                //========================================
                //2023.05.09 調整
                //院所聖功(545)希望SNP系列，如果分析結果有其他這個字眼，內容值改成功院所本身希望的罐頭訊息
                //========================================
                if (colume == "EMBRYO_BLANK")
                {
                    if (Hos_ID == "545")
                    {
                        if (value.IndexOf("其他") >= 0)
                        {
                            value = "此客戶檢驗結果疑似高風險，請詳閱PDF報告";
                        }
                    }
                }
            }
            else if (producttype == "SNP_HD_B")
            {
                //========================================
                //2023.05.09 調整
                //院所聖功(545)希望SNP系列，如果分析結果有其他這個字眼，內容值改成功院所本身希望的罐頭訊息
                //========================================
                if (colume == "EMBRYO_BLANK")
                {
                    if (Hos_ID == "545")
                    {
                        if (value.IndexOf("其他") >= 0)
                        {
                            value = "此客戶檢驗結果疑似高風險，請詳閱PDF報告";
                        }
                    }
                }
            }
            else if (producttype == "PES1")
            {
                if (colume == "CHECK_RESULT_45")
                {
                    if (Hos_ID == "545")
                    {
                        if (value == "")
                        {
                            value = "N/A";
                        }
                    }
                }
            }
            else if (producttype == "PES2")
            {
                if (colume == "CHECK_RESULT_45")
                {
                    if (Hos_ID == "545")
                    {
                        if (value == "")
                        {
                            value = "N/A";
                        }
                    }
                }
            }
            else
            {
                strValue = value;
            }


            return value;
        }
    }

}
