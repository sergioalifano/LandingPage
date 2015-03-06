using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(Models.ContactForm contactForm)
        {
            //add the form to the db
            try
            {
                //set the date
                contactForm.DateCreated = DateTime.Now;

                //add something to db
                //1.create data context
                Models.ContactFormEntities db = new Models.ContactFormEntities();

                //2. Add obj to the table
                db.ContactForms.Add(contactForm);

                //3.save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

            //mail us a copy

            MailMessage mail = new MailMessage();
            mail.To.Add(contactForm.Email);

            mail.From = new MailAddress(contactForm.Email);

            mail.Subject = "Test Mail";

            mail.Body = contactForm.MessageBox;

            SmtpClient SmtpServer = new SmtpClient("mail.dustinkraft.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("postmaster@dustinkraft.com", "techIsFun1");
            SmtpServer.EnableSsl = false;

            SmtpServer.Send(mail);
            //redirect the user to the thank you page
            return RedirectToAction("contact");
            //return View();
        }
    }
}