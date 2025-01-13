﻿using System.Net.Mail;
using System.Net;

namespace EmailBodyMessage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fromAddress = new MailAddress("sender@example.com", "Sender Name");
            var toAddress = new MailAddress("receiver@example.com", "Receiver Name");
            const string subject = "Willkommen! - Testmail";
            const string messageAsString = "Hallo, \n\n erster Abschnitt der Nachricht.";

            // SMTP-Server-Einstellungen für Ionos 
            var smtpClient = new SmtpClient("smtp.ionos.de")
            {
                Port = 587,
                Credentials = new NetworkCredential("sender@example.com", "password"),
                EnableSsl = true
            };

            string htmlFooter = @"
                    <html>
                        <body>
                            <p>Hallo,</p>
                            <p>Dies ist der HTML-Teil der Nachricht.</p>

                            <footer>
                                <p>Mit freundlichen Grüßen,<br/>Ihr Team</p>
                        <p>Dies ist die HTML-Version der Nachricht mit einem eingebetteten Bild.</p>
                            <img src='cid:EmbImg' alt='Embedded Image' />    
                            <footer <footer style='font-size: 10px;'>
                                <p>Unsere Adresse: Musterstraße 123, 20149 Hamburg</p>
                            </footer>
                        </body>
                    </html>
                ";

            // E-Mail-Nachricht
            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,  // Wichtig, damit der Inhalt als HTML behandelt wird
                Body = messageAsString
            };

            var plainTextView = AlternateView.CreateAlternateViewFromString(messageAsString, null, "text/plain");
            mailMessage.AlternateViews.Add(plainTextView);

            // HTML-Version erstellen und hinzufügen (AlternateView)
            var htmlView = AlternateView.CreateAlternateViewFromString(htmlFooter, null, "text/html");
            mailMessage.AlternateViews.Add(htmlView);

            var imgPath = @"C:\Users\Work\...\imag1-test.jpg";

            // Einbinden eines eingebetteten Bildes
            var inlineImage = new Attachment(imgPath)
            {
                ContentDisposition = { Inline = true, FileName = "imag1-test.jpg" },
                ContentId = "EmbImg",
                ContentType = { MediaType = "image/jpeg" }
            };

            mailMessage.Attachments.Add(inlineImage);

            // Senden der E-Mail
            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("E-Mail erfolgreich gesendet!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der E-Mail: {ex.Message}");
            }
        }
    }
    }
