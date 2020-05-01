using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace MantisReportService
{
    internal class SendMail
    {
        public static string IsProduction = ConfigurationManager.AppSettings["IS_PRODUCTION"];

        /* PRODUCTION MAIL INFORMATION */
        private static readonly string To1 = ConfigurationManager.AppSettings["RECEIVER_TO_1"];
        private static readonly string To2 = ConfigurationManager.AppSettings["RECEIVER_TO_2"];
        private static readonly string To3 = ConfigurationManager.AppSettings["RECEIVER_TO_3"];
        private static readonly string To4 = ConfigurationManager.AppSettings["RECEIVER_TO_4"];
        private static readonly string To5 = ConfigurationManager.AppSettings["RECEIVER_TO_5"];
        private static readonly string To6 = ConfigurationManager.AppSettings["RECEIVER_TO_6"];
        private static readonly string To7 = ConfigurationManager.AppSettings["RECEIVER_TO_7"];
        private static readonly string To8 = ConfigurationManager.AppSettings["RECEIVER_TO_8"];
        private static readonly string To9 = ConfigurationManager.AppSettings["RECEIVER_TO_9"];
        private static readonly string To10 = ConfigurationManager.AppSettings["RECEIVER_TO_10"];
        private static readonly string To11 = ConfigurationManager.AppSettings["RECEIVER_TO_11"];
        private static readonly string To12 = ConfigurationManager.AppSettings["RECEIVER_TO_12"];
        private static readonly string To13 = ConfigurationManager.AppSettings["RECEIVER_TO_13"];
        private static readonly string To14 = ConfigurationManager.AppSettings["RECEIVER_TO_14"];
        private static readonly string To15 = ConfigurationManager.AppSettings["RECEIVER_TO_15"];

        private static readonly string Cc1 = ConfigurationManager.AppSettings["RECEIVER_CC_1"];
        private static readonly string Cc2 = ConfigurationManager.AppSettings["RECEIVER_CC_2"];
        private static readonly string Cc3 = ConfigurationManager.AppSettings["RECEIVER_CC_3"];
        private static readonly string Cc4 = ConfigurationManager.AppSettings["RECEIVER_CC_4"];
        private static readonly string Cc5 = ConfigurationManager.AppSettings["RECEIVER_CC_5"];
        private static readonly string Bcc1 = ConfigurationManager.AppSettings["RECEIVER_BCC_1"];
        private static readonly string Bcc2 = ConfigurationManager.AppSettings["RECEIVER_BCC_2"];

        /* DEVELOPMENT MAIL INFORMATION */
        public static string DevTo1 = "burak.kaya@optiim.com";

        public void SendAnEmail(bool isHtml, string body, string sender, string senderPass, string host, bool enableSsl, int port)
        {
            try
            {
                Console.WriteLine("Mail işlemi başlatılıyor.");
                var message = new MailMessage { From = new MailAddress(sender) };
                
                //If environment is production
                if (IsProduction == "1")
                {
                    if (To1 != "") message.To.Add(new MailAddress(To1));
                    if (To2 != "") message.To.Add(new MailAddress(To2));
                    if (To3 != "") message.To.Add(new MailAddress(To3));
                    if (To4 != "") message.To.Add(new MailAddress(To4));
                    if (To5 != "") message.To.Add(new MailAddress(To5));
                    if (To6 != "") message.To.Add(new MailAddress(To6));
                    if (To7 != "") message.To.Add(new MailAddress(To7));
                    if (To8 != "") message.To.Add(new MailAddress(To8));
                    if (To9 != "") message.To.Add(new MailAddress(To9));
                    if (To10 != "") message.To.Add(new MailAddress(To10));
                    if (To11 != "") message.To.Add(new MailAddress(To11));
                    if (To12 != "") message.To.Add(new MailAddress(To12));
                    if (To13 != "") message.To.Add(new MailAddress(To13));
                    if (To14 != "") message.To.Add(new MailAddress(To14));
                    if (To15 != "") message.To.Add(new MailAddress(To15));

                    if (Cc1 != "") message.CC.Add(new MailAddress(Cc1));
                    if (Cc2 != "") message.CC.Add(new MailAddress(Cc2));
                    if (Cc3 != "") message.CC.Add(new MailAddress(Cc3));
                    if (Cc4 != "") message.CC.Add(new MailAddress(Cc4));
                    if (Cc5 != "") message.CC.Add(new MailAddress(Cc5));

                    if (Bcc1 != "") message.Bcc.Add(new MailAddress(Bcc1));
                    if (Bcc2 != "") message.Bcc.Add(new MailAddress(Bcc2));
                }
                else //If environment is development
                {
                    if (DevTo1 != "") { message.To.Add(new MailAddress(DevTo1)); }
                }

                message.Subject = "Mantis Statü Raporu " + DateTime.Now.ToString("dd.MM.yyy");
                message.Body = body;
                message.IsBodyHtml = isHtml;

                Console.WriteLine("Mail tanımlamaları tamamlandı.");
                Console.WriteLine("SMTP işlemi başlatılıyor.");

                var smtpClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(sender, senderPass),
                    Host = host,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = enableSsl,
                    Port = port
                };
                Console.WriteLine("SMTP tanımlamaları tamamlandı.");
                Console.WriteLine("Mail gönderme işlemi başlamıştır.");
                try
                {
                    smtpClient.SendAsync(message, message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine("Mail gönderme işlemi tamamlandı.");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
