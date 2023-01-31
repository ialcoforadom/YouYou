namespace YouYou.Business.Interfaces
{
    public interface IEmailSettings
    {
        string From { get; }

        string User { get; }

        string Password { get; }

        string SMTPAddress { get; }

        string Port { get; }
    }
}
