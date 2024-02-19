using FFmpeg.NET;
using FFmpeg.NET.Events;

namespace MapraMp4TelegramUpload
{
    public class Converter
    {
        public async Task StartConverting (string input, string output, string enginePath, CancellationToken cancellationToken)
        {
            var inputFile = new InputFile(input);
            var outputFile = new OutputFile(output);

            var ffmpeg = new Engine(enginePath + "\\ffmpeg.exe");
            ffmpeg.Progress += OnProgress!;
            ffmpeg.Data += OnData!;
            ffmpeg.Error += OnError!;
            ffmpeg.Complete += OnComplete!;
            await ffmpeg.ConvertAsync(inputFile, outputFile, cancellationToken);
        }

        private void OnProgress (object sender, ConversionProgressEventArgs e)
        {
            Console.WriteLine("[{0} => {1}]", e.Input.Name, e.Output.Name);
            Console.WriteLine("Bitrate: {0}", e.Bitrate);
            Console.WriteLine("Fps: {0}", e.Fps);
            Console.WriteLine("Frame: {0}", e.Frame);
            Console.WriteLine("ProcessedDuration: {0}", e.ProcessedDuration);
            Console.WriteLine("Size: {0} kb", e.SizeKb);
            Console.WriteLine("TotalDuration: {0}\n", e.TotalDuration);
        }

        private void OnData (object sender, ConversionDataEventArgs e)
        {
            Console.WriteLine("[{0} => {1}]: {2}", e.Input.Name, e.Output.Name, e.Data);
        }

        private void OnComplete (object sender, ConversionCompleteEventArgs e)
        {
            Console.WriteLine("Completed conversion from {0} to {1}", e.Input.MetaData.FileInfo.FullName, e.Output.Name);
        }

        private void OnError (object sender, ConversionErrorEventArgs e)
        {
            Console.WriteLine("[{0} => {1}]: Error: {2}\n{3}", e.Input.MetaData.FileInfo.Name, e.Output.Name, e.Exception.ExitCode, e.Exception.InnerException);
        }
    }
}
