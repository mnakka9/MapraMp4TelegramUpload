namespace MapraMp4TelegramUpload
{
    public class Settings
    {
        public Telegram? Telegram { get; set; }

        public string? EnginePath { get; set; }
        public string? TempPath { get; set; }
    }

    public class Telegram
    {
        public string? ChatName { get; set; }
        public string? ApiId { get; set; }
        public string? ApiHash { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
