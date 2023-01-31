using YouYou.Business.Interfaces;

namespace YouYou.Api.Extensions
{
    public class EmailSettings : IEmailSettings
    {
        public string From { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string SMTPAddress { get; private set; }
        public string Port { get; private set; }

        public EmailSettings(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            From = emailSettings.GetSection(nameof(EmailSettings.From)).Value;
            User = emailSettings.GetSection(nameof(EmailSettings.User)).Value;
            Password = emailSettings.GetSection(nameof(EmailSettings.Password)).Value;
            SMTPAddress = emailSettings.GetSection(nameof(EmailSettings.SMTPAddress)).Value;
            Port = emailSettings.GetSection(nameof(EmailSettings.Port)).Value;
        }
    }
}
