namespace ConnectSphere.Application.Common.Settings
{
    public class SmtpSettings
    {
        public const string Section = "Smtp";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
    }
}
