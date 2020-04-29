
﻿using dm_backend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace dm_backend.Logics
{
    public class sendMail
    {
         //MultipleNotifications item
<<<<<<< HEAD
        public async Task<string>  sendNotification(string  email , string  body  ,  string sbject)
=======
        public async Task<string>  sendNotification(string  email , string  body, string subject  )
>>>>>>> a863ec828b5afd4e934ea9351b625a90c3c36df6
        {
          
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("sjangra@ex2india.com", "Admin");
                mail.To.Add(email);
                mail.IsBodyHtml = true;

<<<<<<< HEAD
                mail.Subject = sbject;
=======
                mail.Subject = subject;
>>>>>>> a863ec828b5afd4e934ea9351b625a90c3c36df6
                mail.Body = body;
                SmtpServer.Port = 587;
                string ans = Dec("SmVmZmhhcmR5QDYxOQ==");
                SmtpServer.Credentials = new System.Net.NetworkCredential("sjangra@ex2india.com ", ans);
                SmtpServer.EnableSsl = true;
                await   SmtpServer.SendMailAsync(mail);
                return null;
                       
        }
        private string Dec(string v)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(v);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
                Console.WriteLine(fe);
            }
            return decrypted;
        }

    }




}
