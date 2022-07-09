using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EnviarCorresoCarreraV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string email = "iglesiacentralnav@gmail.com";
            string password = "kbzhcmtmzjquiyoh";
            string log = "";
            var text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"DATA\", "data.json"));
            StreamReader file = File.OpenText(Path.Combine(Environment.CurrentDirectory, @"DATA\", "data.json"));
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                Dictionary<string, string> o2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                foreach (var item in o2)
                {
                    try
                    {
                        log += "--------------------------------------------\n";
                        log += "ITEM - " + item.Value + "\n";
                        string fileName = item.Value;
                        int pos = 0;
                        for (int i = 0; i < fileName.Length; i++)
                        {
                            if (fileName[i] == ',')
                                pos = i;
                        }
                        int posStart = pos + 1;
                        int posEnd = fileName.Length - posStart;

                        string name = fileName.Substring(0, pos);
                        string emailTo = fileName.Substring(posStart, posEnd).Replace('+', '@');

                        log += "NOMBRE - " + name + "\n";
                        log += "CORREO - " + emailTo + "\n";

                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email, "Iglesia Adventista Distrito de Navojoa");
                        msg.To.Add(new MailAddress(emailTo));
                        msg.Subject = "Correo de prueba";
                        msg.Body = name + ". Este es un correo de prueba. No es el diseño final.";
                        msg.IsBodyHtml = true;

                        System.Net.Mail.Attachment attachment;
                        string path = Path.Combine(Environment.CurrentDirectory, @"IMG\", fileName+".png");
                        attachment = new System.Net.Mail.Attachment(path);

                        msg.Attachments.Add(attachment);
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                        log += "CORREO ENVIADO\n";
                    }
                    catch (Exception ex)
                    {
                        log += "ERROR - " + ex.Message + "\n";
                    }
                }
            }
            File.WriteAllText("log.txt", log);
        }
    }
}
