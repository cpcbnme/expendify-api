using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http.Internal;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

public class SMTPDetails
{
    public string SMTPHost { get; set; }

    public int SMTPPort { get; set; }

    public bool EnableSSL { get; set; }

    public bool UseDefaultCredentials { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
    public string FromEmail { get; set; }
    public string FromDisplayName { get; set; }

}
public class SMTPMailService : IEmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initiate Sendgrid email notifications
    /// </summary>
    public SMTPMailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// Send mail via SMTP to a single recipient
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="toEmail"></param>
    /// <param name="body"></param>
    /// <param name="attachment"></param>
    /// <param name="cc"></param>
    /// <param name="bcc"></param>
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        var HttpReq = new HttpClient();
        // make POST to https://us-central1-pisces-main.cloudfunctions.net/email-notifiactions
        // with a json containing email, subject and message
        // make sure the Content-Type header is application/json

        HttpReq.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpReq.DefaultRequestHeaders.Add("User-Agent", "FinancialApplication");
        HttpReq.DefaultRequestHeaders.Add("Accept", "application/json");

        // recipient, subject, body
        var data = new
        {
            recipient = toEmail,
            subject = subject,
            body = message
        };

        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await HttpReq.PostAsync("https://us-central1-pisces-main.cloudfunctions.net/email-notifiactions", content);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private SMTPDetails SMPTPInforFromConfig
    {
        get
        {
            _ = int.TryParse(_configuration["SMTP:Port"], out int smtpPort);
            _ = bool.TryParse(_configuration["SMTP:EnableSSL"], out bool enableSSL);
            SMTPDetails smtpDetails = new()
            {
                SMTPHost = _configuration["SMTP:Host"],
                UseDefaultCredentials = false,
                SMTPPort = smtpPort,
                EnableSSL = enableSSL,
                Username = _configuration["SMTP:username"],
                Password = _configuration["SMTP:password"],
                FromDisplayName = _configuration["SMTP:DisplayName"],
                FromEmail = _configuration["SMTP:From"]
            };
            return smtpDetails;
        }
    }
}