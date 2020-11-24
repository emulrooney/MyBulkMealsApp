using MyBulkMealsApp.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    public static class MBMEmailHandler
    {
        private static readonly string emailSource = "mbmadmin@emudev.site";
        private static readonly string emailFrom = "MyBulkMeals Admin";
        public static AppSecrets AppSecrets { get; set; }

        public async static Task<Response> SendRecipeNotificationEmail(ApplicationUser user, string callbackUrl, int quantity)
        {
            var apiKey = AppSecrets.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailSource, emailFrom);
            var subject = $"You have ${quantity} recipes in the verification queue.";
            var to = new EmailAddress(user.Email, user.FirstName + " " + user.LastName);
            var plainTextContent = $"Please log in soon to clear the queue.";
            var htmlContent = $"Please log in <b>soon</b> to clear the queue.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            return await client.SendEmailAsync(msg);
        }

        public async static Task<Response> SendConfirmationEmail(ApplicationUser user, string callbackUrl)
        {
            var apiKey = AppSecrets.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailSource, emailFrom);
            var subject = "Confirm your MyBulkMeals account";
            var to = new EmailAddress(user.Email, user.FirstName + " " + user.LastName); //Input.Email
            var plainTextContent = $"Please confirm your account by copy and pasting this URL into your browser: {HtmlEncoder.Default.Encode(callbackUrl)}";
            var htmlContent = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            return await client.SendEmailAsync(msg);
        }


        /// <summary>
        /// Send email for password resets via SendGrid. Requires the intended user and a callback URL to give to the user
        /// to actaully reset their account.
        /// </summary>
        /// <param name="user">User who forgot their password</param>
        /// <param name="callbackUrl">URL for reset</param>
        /// <returns></returns>
        public async static Task<Response> SendPasswordResetEmail(ApplicationUser user, string callbackUrl)
        {
            var apiKey = AppSecrets.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailSource, emailFrom);
            var subject = "Reset MyBulkMealsApp Password";
            var to = new EmailAddress(user.Email, user.FirstName + " " + user.LastName); //Input.Email
            var plainTextContent = $"Please reset your password by copying and pasting the following into your address bar: {HtmlEncoder.Default.Encode(callbackUrl)}";
            var htmlContent = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            return response;

        }

    }
}