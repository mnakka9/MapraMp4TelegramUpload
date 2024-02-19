# MapraMp4TelegramUpload

Using @WTelegram package and FFMPEG.NET package

Login to Telegram, Upload mp4 files to a channel from Base directory.
MKV files are converted to mp4 to support streaming in telegram.

**For example:**

Base Directory

--> Movie 1
   
   --> Movie1.mp4
   
   --> Movie1.srt

--> Movie 3
  
  --> Movie 3.mkv
  
  --> Movie 3.srt

files are uploaded to a channel like

Message:

Movie 1
File attachment .mp4
File attachment .srt

Message:

Movie 3
File attachment .mp4
File attachment .srt

**Settings.json needs to be filled with API Id, API hash, Phone Number for Telegram connection**
**EnginePath is the path for FFMPEG exe files**

