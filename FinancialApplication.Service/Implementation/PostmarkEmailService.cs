public class PostmarkDetails
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

public class PostmarkEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initiate Sendgrid email notifications
        /// </summary>
        public PostmarkEmailService(IConfiguration configuration)
        {
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }