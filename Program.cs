using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KodA.ArsivNetTransferApp
{
    class Program
    {
        //static KullaniciSvc.kullaniciGirisSonuc wcfLoginResult = null;
        //static void Main(string[] args)
        //{
        //    try
        //    {
        //        string pwdHash = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("edys:123"));
        //        KullaniciSvc.KullaniciServiceClient kullaniciServiceClient = new KullaniciSvc.KullaniciServiceClient();
        //        using (new OperationContextScope(kullaniciServiceClient.InnerChannel))
        //        {                    
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers["Authorization"] = "Basic "+ pwdHash;
        //            //requestMessage.Headers["Authorization"] = "Basic ZWR5czoxMjM=";                    
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
        //            var res = kullaniciServiceClient.kullaniciGiris();
        //            if(res.success && res.dtoList.Length>0)
        //            {
        //                wcfLoginResult = ((KullaniciSvc.kullaniciGirisSonuc)res.dtoList[0]);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process[] processes = Process.GetProcessesByName(Application.ProductName);
            Application.Run(new FormMain());

        }
    }
}
