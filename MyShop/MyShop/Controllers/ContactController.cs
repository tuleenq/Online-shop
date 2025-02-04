using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using MyShop.Domain.Entities;

public class ContactController : Controller
{
    [HttpPost]
    public IActionResult Contact(ContactFormModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var toEmail = "soundcloudinstruments@gmail.com";
                var subject = "Message from Contact Form";
                var body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage:\n{model.Message}";

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("tuleenqaisi@gmail.com", "lacl ppkg opbc ftar");
                    smtp.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("tuleenqaisi@gmail.com", "My Shop"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(toEmail);
                    mailMessage.ReplyToList.Add(new MailAddress(model.Email));

                    smtp.Send(mailMessage);
                }

                TempData["SuccessMessage"] = "Your message was sent successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                TempData["ErrorMessage"] = "There was an error sending your message. Please try again later.";
            }
        }

        return View(model);
    }

}
