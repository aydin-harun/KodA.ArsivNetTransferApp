using KodA.ArsivNetTransferApp.ArsivPlanlamaSvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace KodA.ArsivNetTransferApp
{
    public static class ServiceHelper
    {
        public static KullaniciSvc.kullaniciGirisSonuc wcfLoginResult = null;
        public static KullaniciSvc.KullaniciServiceClient kullaniciServiceClient = null;
        public static ArsivPlanlamaSvc.ArsivPlanlamaServiceClient arsivPlanlamaServiceClient = null;
        public static ArsivMalzemeSvc.ArsivMalzemeServiceClient arsivMalzemeServiceClient = null;

        static ServiceHelper()
        {
            kullaniciServiceClient = new KullaniciSvc.KullaniciServiceClient();
            kullaniciServiceClient.InnerChannel.OperationTimeout = new TimeSpan(0, 15, 0);
            arsivPlanlamaServiceClient = new ArsivPlanlamaSvc.ArsivPlanlamaServiceClient();
            arsivPlanlamaServiceClient.InnerChannel.OperationTimeout = new TimeSpan(0, 15, 0);
            arsivMalzemeServiceClient = new ArsivMalzemeSvc.ArsivMalzemeServiceClient();
            arsivMalzemeServiceClient.InnerChannel.OperationTimeout = new TimeSpan(0, 15, 0);
        }

        public static OperationContextScope GetOperationContextScope(IContextChannel channel)
        {
            try
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["ServiceUserName"];
                string userPwd = System.Configuration.ConfigurationManager.AppSettings["ServiceUserPwd"];
                string pwdHash = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{userName}:{userPwd}"));
                OperationContextScope operationContextScope = new OperationContextScope(channel);
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["Authorization"] = "Basic " + pwdHash;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                return operationContextScope;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool LoginService()
        {
            try
            {
                using (GetOperationContextScope(kullaniciServiceClient.InnerChannel))
                {
                    var res = kullaniciServiceClient.kullaniciGiris();
                    if (res.success && res.dtoList.Length > 0)
                    {
                        wcfLoginResult = ((KullaniciSvc.kullaniciGirisSonuc)res.dtoList[0]);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizini> GetArchiveFolder(int archiveFolderId)
        {
            try
            {
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizini> arsivDiziniCocuklariniListeleSonucs =
                        new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizini>();
                    var methodParams = new ArsivPlanlamaSvc.arsivDizinleriniListeleParametre();
                    methodParams.dizinId = archiveFolderId;
                    methodParams.dizinIdSpecified = true;
                    methodParams.dizinAdi = string.Empty;
                    methodParams.dizinKodu = string.Empty;
                    methodParams.altDizinSeviyesi = ArsivPlanlamaSvc.altDizinSeviyesiEnum.TUM;
                    methodParams.altDizinSeviyesiSpecified = true;
                    var res = arsivPlanlamaServiceClient.arsivDiziniListele(methodParams);
                    if (res.success && res.dtoList?.Length > 0)
                    {
                        foreach (object obj in res.dtoList)
                        {
                            arsivDiziniCocuklariniListeleSonucs.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizini)obj);
                        }
                    }
                    return arsivDiziniCocuklariniListeleSonucs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object[] ReadArchiveFolder(int archiveFolderId)
        {
            try
            {
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    var methodParams = new ArsivPlanlamaSvc.arsivDiziniOkuParametre();
                    methodParams.arsivPlaniKodu = "0";
                    var res = arsivPlanlamaServiceClient.arsivDiziniOku(methodParams);
                    if (res.success && res.dtoList.Length > 0)
                    {
                        return res.dtoList;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> GetArchiveFolderChildren(int parentFolderId)
        {
            try
            {
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                    var methodParams = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                    methodParams.id = parentFolderId;
                    methodParams.idSpecified = true;
                    //methodParams.start = 0;
                    //methodParams.limit = 50; 
                    var res = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                    if (res.success)
                    {
                        if (res.dtoList != null)
                        {
                            foreach (object obj in res.dtoList)
                            {
                                arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                            }
                        }
                    }
                    return arsivDizinis;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> GetArchiveYearFolders(int parentFolderId, string sdpKodu, string departmanKodu)
        {
            try
            {
                int lastId = parentFolderId;
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                    var methodParams = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                    methodParams.id = lastId;
                    methodParams.idSpecified = true;
                    //methodParams.start = 0;
                    //methodParams.limit = 50; 
                    var res = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                    if (res.success)
                    {
                        if (res.dtoList != null)
                        {
                            foreach (object obj in res.dtoList)
                            {
                                arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                            }
                        }
                    }
                    if (arsivDizinis.Count == 0)
                        return null;
                    lastId = arsivDizinis[0].id;
                    arsivDizinis.Clear();
                    // ikinci katman
                    using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                    {
                        List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis1 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                        var methodParams1 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                        methodParams.id = lastId;
                        methodParams.idSpecified = true;
                        //methodParams.start = 0;
                        //methodParams.limit = 50; 
                        var res1 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                        if (res1.success)
                        {
                            if (res1.dtoList != null)
                            {
                                foreach (object obj in res1.dtoList)
                                {
                                    arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                }
                            }
                        }
                        if (arsivDizinis.Count == 0)
                            return null;
                        //// burada sdp kodundan ilgili dizini buluyoruz
                        if (arsivDizinis.Where(d => d.dizinKodu == sdpKodu).Count() == 0)
                            return null;
                        lastId = arsivDizinis.Where(d => d.dizinKodu == sdpKodu).First().id;

                        arsivDizinis.Clear();
                        ///// dördüncü katman
                        using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                        {
                            List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis3 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                            var methodParams3 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                            methodParams.id = lastId;
                            methodParams.idSpecified = true;
                            //methodParams.start = 0;
                            //methodParams.limit = 50; 
                            var res3 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                            if (res3.success)
                            {
                                if (res3.dtoList != null)
                                {
                                    foreach (object obj in res3.dtoList)
                                    {
                                        arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                    }
                                }
                            }
                            if (arsivDizinis.Count == 0)
                                return null;
                            lastId = arsivDizinis[0].id;
                            arsivDizinis.Clear();
                            ///// beşinci katman
                            using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                            {
                                List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis4 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                                var methodParams4 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                methodParams.id = lastId;
                                methodParams.idSpecified = true;
                                //methodParams.start = 0;
                                //methodParams.limit = 50; 
                                var res4 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                if (res4.success)
                                {
                                    if (res4.dtoList != null)
                                    {
                                        foreach (object obj in res4.dtoList)
                                        {
                                            arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                        }
                                    }
                                }
                                if (arsivDizinis.Count == 0)
                                    return null;
                                lastId = arsivDizinis[0].id;
                                arsivDizinis.Clear();
                                ///// altıncı katman
                                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                                {
                                    List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis5 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                                    var methodParams5 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                    methodParams.id = lastId;
                                    methodParams.idSpecified = true;
                                    //methodParams.start = 0;
                                    //methodParams.limit = 50; 
                                    var res5 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                    if (res5.success)
                                    {
                                        if (res5.dtoList != null)
                                        {
                                            foreach (object obj in res5.dtoList)
                                            {
                                                arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                            }
                                        }
                                    }
                                    if (arsivDizinis.Count == 0)
                                        return null;
                                    /// burada departman kodunun ilk 3 hanesine göre koddan filtre yapılıp uygun dizin bulanacak
                                    /// 
                                    if (arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 3)).Count() == 0)
                                        return null;
                                    else
                                        lastId = arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 3)).First().id;
                                    arsivDizinis.Clear();
                                    ///// yedinci katman
                                    using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                                    {
                                        List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis6 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                                        var methodParams6 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                        methodParams.id = lastId;
                                        methodParams.idSpecified = true;
                                        //methodParams.start = 0;
                                        //methodParams.limit = 50; 
                                        var res6 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                        if (res6.success)
                                        {
                                            if (res6.dtoList != null)
                                            {
                                                foreach (object obj in res6.dtoList)
                                                {
                                                    arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                                }
                                            }
                                        }
                                        if (arsivDizinis.Count == 0)
                                            return null;
                                        /// burada departman kodunun ilk 6 hanesine göre koddan filtre yapılıp uygun dizin bulanacak
                                        /// 
                                        if (arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 6)).Count() == 0)
                                            return null;
                                        else
                                            lastId = arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 6)).First().id;
                                        arsivDizinis.Clear();
                                        ///// sekizinci katman yıl klasörünün bir üstü dönüyor
                                        using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                                        {
                                            List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis7 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                                            var methodParams7 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                            methodParams.id = lastId;
                                            methodParams.idSpecified = true;
                                            //methodParams.start = 0;
                                            //methodParams.limit = 50; 
                                            var res7 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                            if (res7.success)
                                            {
                                                if (res7.dtoList != null)
                                                {
                                                    foreach (object obj in res7.dtoList)
                                                    {
                                                        arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                                    }
                                                }
                                            }
                                            if (arsivDizinis.Count == 0)
                                                return null;
                                            /// burada departman kodunun ilk 9 hanesine göre koddan filtre yapılıp uygun dizin bulanacak
                                            /// 
                                            if (arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 9)).Count() == 0)
                                                return null;
                                            else
                                                lastId = arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 9)).First().id;
                                            arsivDizinis.Clear();
                                            ///// dokuzuncu katman yıl klasörü dönüyor
                                            using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                                            {
                                                var methodParams8 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                                methodParams.id = lastId;
                                                methodParams.idSpecified = true;
                                                //methodParams.start = 0;
                                                //methodParams.limit = 50; 
                                                var res8 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                                if (res8.success)
                                                {
                                                    if (res8.dtoList != null)
                                                    {
                                                        foreach (object obj in res8.dtoList)
                                                        {
                                                            arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                                        }
                                                    }
                                                }
                                                if (arsivDizinis.Count == 0)
                                                    return null;
                                                return arsivDizinis;
                                            }
                                        }

                                    }

                                }
                            }
                        }
                    }


                    //return arsivDizinis;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizini GetArchiveFolder(string fullDizinPath)
        {
            try
            {   
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizinleriniListeleParametre pp = new ArsivPlanlamaSvc.arsivDizinleriniListeleParametre();
                    pp.dizinId = Convert.ToInt32(fullDizinPath);
                    pp.dizinIdSpecified = true;
                    pp.altDizinSeviyesi = altDizinSeviyesiEnum.HIC;
                    pp.altDizinSeviyesiSpecified = true;
                    var resP = arsivPlanlamaServiceClient.arsivDiziniListele(pp);
                    if(resP.success)
                    {
                        if (resP.dtoList != null &&resP.dtoList.Length > 0 )
                        {
                            return resP.dtoList.FirstOrDefault() as KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDizini;                            
                        }
                        else 
                        { 
                            return null; 
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetArchiveYearFoldersDiaFoto(int parentFolderId,
            string sdpKodu, string departmanKodu)
        {
            try
            {
                int lastId = parentFolderId;
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                    var methodParams = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                    methodParams.id = lastId;
                    methodParams.idSpecified = true;
                    //methodParams.start = 0;
                    //methodParams.limit = 50; 
                    var res = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                    if (res.success)
                    {
                        if (res.dtoList != null)
                        {
                            foreach (object obj in res.dtoList)
                            {
                                arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                            }
                        }
                    }
                    if (arsivDizinis.Count == 0)
                        return 0;
                    lastId = arsivDizinis[0].id;
                    arsivDizinis.Clear();
                    // ikinci katman
                    using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                    {
                        List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis1 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                        var methodParams1 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                        methodParams.id = lastId;
                        methodParams.idSpecified = true;
                        //methodParams.start = 0;
                        //methodParams.limit = 50; 
                        var res1 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                        if (res1.success)
                        {
                            if (res1.dtoList != null)
                            {
                                foreach (object obj in res1.dtoList)
                                {
                                    arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                }
                            }
                        }
                        if (arsivDizinis.Count == 0)
                            return 0;
                        //// burada sdp kodundan ilgili dizini buluyoruz
                        if (arsivDizinis.Where(d => d.dizinKodu == sdpKodu).Count() == 0)
                            return 0;
                        lastId = arsivDizinis.Where(d => d.dizinKodu == sdpKodu).First().id;

                        arsivDizinis.Clear();
                        ///// dördüncü katman kök dizin
                        using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                        {
                            List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis3 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                            var methodParams3 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                            methodParams.id = lastId;
                            methodParams.idSpecified = true;
                            //methodParams.start = 0;
                            //methodParams.limit = 50; 
                            var res3 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                            if (res3.success)
                            {
                                if (res3.dtoList != null)
                                {
                                    foreach (object obj in res3.dtoList)
                                    {
                                        arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                    }
                                }
                            }
                            if (arsivDizinis.Count == 0)
                                return 0;
                            lastId = arsivDizinis[0].id;
                            arsivDizinis.Clear();
                            /////// beşinci katman
                            using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                            {
                                List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis5 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                                var methodParams5 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                methodParams.id = lastId;
                                methodParams.idSpecified = true;
                                //methodParams.start = 0;
                                //methodParams.limit = 50; 
                                var res5 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                if (res5.success)
                                {
                                    if (res5.dtoList != null)
                                    {
                                        foreach (object obj in res5.dtoList)
                                        {
                                            arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                        }
                                    }
                                }
                                if (arsivDizinis.Count == 0)
                                    return 0;
                                /// burada departman kodunun ilk 3 hanesine göre koddan filtre yapılıp uygun dizin bulanacak
                                /// 
                                if (arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 3)).Count() == 0)
                                    return 0;
                                else
                                    lastId = arsivDizinis.Where(d => d.dizinKodu == departmanKodu.Substring(0, 3)).First().id;
                                arsivDizinis.Clear();
                                ///// yedinci katman
                                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                                {
                                    List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinis6 = new List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();
                                    var methodParams6 = new ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleParametre();
                                    methodParams.id = lastId;
                                    methodParams.idSpecified = true;
                                    //methodParams.start = 0;
                                    //methodParams.limit = 50; 
                                    var res6 = arsivPlanlamaServiceClient.arsivDiziniCocuklariListele(methodParams);
                                    if (res6.success)
                                    {
                                        if (res6.dtoList != null)
                                        {
                                            foreach (object obj in res6.dtoList)
                                            {
                                                arsivDizinis.Add((KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc)obj);
                                            }
                                        }
                                    }
                                    if (arsivDizinis.Count == 0)
                                        return 0;
                                    /// burada departman kodunun ilk 6 hanesine göre koddan filtre yapılıp uygun dizin bulanacak
                                    /// 
                                    if (arsivDizinis.Where(d => d.dizinKodu == departmanKodu).Count() == 0)
                                        return 0;
                                    else
                                        lastId = arsivDizinis.Where(d => d.dizinKodu == departmanKodu).First().id;
                                    arsivDizinis.Clear();
                                    return lastId;
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object[] CreateArchiveChild(ArsivPlanlamaSvc.arsivDiziniEkleParametre arsivDiziniEkleParametre)
        {
            try
            {
                using (GetOperationContextScope(arsivPlanlamaServiceClient.InnerChannel))
                {
                    var res = arsivPlanlamaServiceClient.arsivDiziniEkle(arsivDiziniEkleParametre);
                    if (res.success && res.dtoList.Length > 0)
                    {
                        return res.dtoList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object[] CreateArchiveChildAuth(KullaniciSvc.kullaniciDizinYetkisiEkleParametre arsivDiziniYetkisiEkleParametre)
        {
            try
            {
                using (GetOperationContextScope(kullaniciServiceClient.InnerChannel))
                {
                    var res = kullaniciServiceClient.kullaniciDizinYetkisiEkle(arsivDiziniYetkisiEkleParametre);
                    if (res.success && res.dtoList.Length > 0)
                    {
                        return res.dtoList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CreateArchiveChildAuth2(KullaniciSvc.kullaniciDizinVeDizinUreticiYetkisiGuncelleParametre arsivDiziniYetkisiEkleParametre)
        {
            try
            {
                using (GetOperationContextScope(kullaniciServiceClient.InnerChannel))
                {
                    var res = kullaniciServiceClient.kullaniciDizinVeDizinUreticiYetkisiGuncelle(arsivDiziniYetkisiEkleParametre);
                    return res.success;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static object[] CreateArsivMalzeme(ArsivMalzemeSvc.arsivMalzemesiEkleMTOMParametre arsivMalzeme)
        {
            try
            {
                using (GetOperationContextScope(arsivMalzemeServiceClient.InnerChannel))
                {
                    var res = arsivMalzemeServiceClient.arsivMalzemesiEkle(arsivMalzeme);
                    if (res.success && res.dtoList.Length > 0)
                    {
                        return res.dtoList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CreateArsivMalzemeOcr(ArsivMalzemeSvc.arsivMalzemesiDetayOCRIcerikYazMTOMParametre arsivMalzemeOcr)
        {
            try
            {
                using (GetOperationContextScope(arsivMalzemeServiceClient.InnerChannel))
                {
                    var res = arsivMalzemeServiceClient.arsivMalzemesiDetayOcrIcerikYaz(arsivMalzemeOcr);
                    return res.success;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool LoginService()
        //{
        //    try
        //    {
        //        string pwdHash = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("edys:123"));
        //        using (new OperationContextScope(kullaniciServiceClient.InnerChannel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers["Authorization"] = "Basic " + pwdHash;
        //            //requestMessage.Headers["Authorization"] = "Basic ZWR5czoxMjM=";                    
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
        //            var res = kullaniciServiceClient.kullaniciGiris();
        //            if (res.success && res.dtoList.Length > 0)
        //            {
        //                wcfLoginResult = ((KullaniciSvc.kullaniciGirisSonuc)res.dtoList[0]);
        //                return true;
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
