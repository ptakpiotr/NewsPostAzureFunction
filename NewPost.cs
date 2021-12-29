using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FluentEmail.Smtp;
using System.Net.Mail;
using FluentEmail.Core;
using System.Net;

namespace NotifyNewPost
{
    public static class NewPost
    {
        static NewPost()
        {
            var sender = new SmtpSender(new SmtpClient(" smtp.ethereal.email ")
            {
                EnableSsl = true,
                Port= 587,
                Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("Email"), Environment.GetEnvironmentVariable("Password"))
            });

            Email.DefaultSender = sender;
        }

        [FunctionName("NewPost")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string mess = await new StreamReader(req.Body).ReadToEndAsync();
            RequestModel request = JsonConvert.DeserializeObject<RequestModel>(mess);

            var res = await Email.From("tess.stokes89@ethereal.email").To(request.Email).Subject("New post!").Body(request.Message).SendAsync();

            if (res.Successful)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
