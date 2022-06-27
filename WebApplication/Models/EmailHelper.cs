using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Contacts.Dto;

namespace WebApplication.Models
{
    public class EmailHelper
    {
        public bool SendEmailPasswordReset( string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tranriemann5298@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));
            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;
            
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("tranriemann5298@gmail.com", "vxgqhybroiuznkzy");
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            //client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool SendEmailContact(ContactRequest request)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("yourmailtosend");
            mailMessage.To.Add(new MailAddress(request.Email));
            mailMessage.Subject = "Contact was sent";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = request.Message;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("yourmailtosend", "mkud");
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            //client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool SendEmailContact(ContactProcessRequest request)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("emailtosend");
            mailMessage.To.Add(new MailAddress(request.Email));
            mailMessage.Subject = request.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = request.MessageResponse;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("emailtosend", "vxgqhybroiuznkzy");
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            //client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
