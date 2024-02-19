
using MapraMp4TelegramUpload;

using Microsoft.Extensions.Configuration;

using WTelegram;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("settings.json")
    .AddEnvironmentVariables()
    .Build();

Settings? settings = config.GetRequiredSection("Settings").Get<Settings>();

if (settings is null || settings.Telegram is null || settings.EnginePath is null)
{
    Console.WriteLine("Settings not found");
    return;
}

Client telegramClient = new(int.Parse(settings.Telegram.ApiId!), settings.Telegram.ApiHash!);

string what = await telegramClient.Login(settings.Telegram.PhoneNumber);

if (what != null)
{
    Console.WriteLine("Enter the telegram code");
    var code = Console.ReadLine();
    what = await telegramClient.Login(code);

    if (what != null)
    {
        Console.WriteLine("Telegram login failed");
        return;
    }
}

if (telegramClient.User == null)
{
    Console.WriteLine("Telegram user not found");
    return;
}

var channels = await telegramClient.Messages_GetAllChats();

var channel = channels.chats.FirstOrDefault(x => x.Value.Title == settings.Telegram.ChatName!);

if (channel.Value is null)
{
    Console.WriteLine("Channel not found");
    return;
}

Console.WriteLine($"Channel {channel.Value.Title} found with Chat Id:{channel.Key}");

pth: Console.WriteLine("Enter the folder path");

var path = Console.ReadLine();

if (!string.IsNullOrEmpty(path))
{
    DirectoryInfo directory = new(path);

    if (!directory.Exists)
    {
        Console.WriteLine("Folder not found");
        goto pth;
    }


    var dirs = directory.GetDirectories();

    Console.WriteLine($"Found {dirs.Length} folders");


    foreach (var dir in dirs)
    {
        Console.WriteLine($"Uploading {dir.Name} folder");
        var mkvFiles = dir.GetFiles("*.mkv");
        var mp4Files = dir.GetFiles("*.mp4");
        var srtFiles = dir.GetFiles("*.srt");

        foreach (var mkvFile in mkvFiles)
        {
            Console.WriteLine($"Converting {mkvFile.Name} file");

            var mp4Path = mkvFile.Name + ".mp4";
            Converter converter = new();
            await converter.StartConverting(mkvFile.FullName, mp4Path, settings.EnginePath!, CancellationToken.None);

            Console.WriteLine($"Converted {mkvFile.Name} file");

            Console.WriteLine($"Uploading {mp4Path} file");
            var telegramFile = await telegramClient.UploadFileAsync(mp4Path);

            Console.WriteLine($"File uploaded to Telegram with Id:{telegramFile.Name}");

            await telegramClient.SendMediaAsync(channel.Value, dir.Name, telegramFile);
            Console.WriteLine($"Sent file to Telegram with Id:{telegramFile.Name}");

            File.Delete(mp4Path);
        }

        foreach (var mp4File in mp4Files)
        {
            Console.WriteLine($"Uploading {mp4File.Name} file");
            var telegramFile = await telegramClient.UploadFileAsync(mp4File.FullName);

            Console.WriteLine($"File uploaded to Telegram with Id:{telegramFile.Name}");

            await telegramClient.SendMediaAsync(channel.Value, dir.Name, telegramFile);
            Console.WriteLine($"Sent file to Telegram with Id:{telegramFile.Name}");
        }

        foreach (var srtFile in srtFiles)
        {
            var telegramFile = await telegramClient.UploadFileAsync(srtFile.FullName);

            Console.WriteLine($"File uploaded to Telegram with Id:{telegramFile.Name}");

            await telegramClient.SendMediaAsync(channel.Value, string.Empty, telegramFile);
        }
    }
}
Console.WriteLine("Done");
Console.ReadLine();