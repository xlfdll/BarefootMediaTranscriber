using System.Globalization;
using System.Text;

using Whisper;

namespace BarefootAutoTranscriber
{
    public static class Helper
    {
        public static void ShowHelp()
        {
            Console.WriteLine("= Parameters =");
            Console.WriteLine();
            Console.WriteLine("BarefootAutoTranscriber /transcribe <MediaFileName> [<ModelType>]");
            Console.WriteLine("- Transcribe media file");
            Console.WriteLine();
            Console.WriteLine("BarefootAutoTranscriber /download [<ModelType>]");
            Console.WriteLine("- Download model file to current directory");
            Console.WriteLine();

            Console.WriteLine("= Notes =");
            Console.WriteLine();
            Console.WriteLine("- Output will be saved to current directory as SRT subtitle file");
            Console.WriteLine("- If media file is a video, the first track will be used");
            Console.WriteLine("- The following model types are available:");
            Console.WriteLine("    tiny[.en], base[.en], small[.en], medium[.en], large");
            Console.WriteLine("- When transcribing, the default model type is small.en if not specified");
            Console.WriteLine();
        }

        public static async Task DownloadModel(String modelType = ModelType.SmallEnglish)
        {
            // https://huggingface.co/ggerganov/whisper.cpp/tree/main
            String url = $"https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-{modelType}.bin";
            String fileName = $"ggml-{modelType}.bin";

            if (!String.IsNullOrEmpty(url))
            {
                Console.WriteLine($"Downloading model ({modelType})...");

                await DownloadFile(url, fileName);
            }
        }

        private static async Task DownloadFile(String url, String fileName)
        {
            using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public static async Task Transcribe(String mediaFileName, String modelType = ModelType.SmallEnglish)
        {
            String modelFileName = $"ggml-{modelType}.bin";

            if (!File.Exists(modelFileName))
            {
                await DownloadModel(modelType);
            }

            Console.WriteLine($"Processing using {modelType} model...");
            Console.WriteLine(mediaFileName);
            Console.WriteLine();

            var cancellationToken = new CancellationToken();
            var library = await Library.loadModelAsync(modelFileName, cancellationToken);
            var context = library.createContext();
            var mf = Library.initMediaFoundation();

            using iAudioReader reader = mf.openAudioFile(mediaFileName, true);

            context.runFull(reader, progress =>
            {
                Int32 p = (Int32)(progress * 100);

                if (p % 10 == 0)
                {
                    Console.WriteLine($"{p}% processed.");
                }
            });

            WriteResultsToSRT(context, mediaFileName);
        }

        public static void WriteResultsToSRT(Context context, String audioFileName)
        {
            StringBuilder sb = new StringBuilder();
            var segments = context.results(eResultFlags.Timestamps).segments;

            for (int i = 0; i < segments.Length; i++)
            {
                sb.AppendLine((i + 1).ToString());

                String begin = PrintTimeWithComma(segments[i].time.begin);
                String end = PrintTimeWithComma(segments[i].time.end);

                sb.AppendLine($"{begin} --> {end}");
                sb.AppendLine(segments[i].text);
                sb.AppendLine();
            }

            File.WriteAllText(Path.ChangeExtension(audioFileName, ".srt"), sb.ToString());
        }

        private static string PrintTimeWithComma(TimeSpan ts)
            => ts.ToString("hh':'mm':'ss','fff", CultureInfo.InvariantCulture);

        private static HttpClient httpClient
            => new HttpClient();
    }
}