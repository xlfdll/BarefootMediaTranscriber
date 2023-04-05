using BarefootAutoTranscriber;

Console.WriteLine("Barefoot Auto Transcriber");
Console.WriteLine("(C) 2023 Xlfdll Workstation");
Console.WriteLine();

if (args.Length < 1 || args[0] == "/?")
{
    Helper.ShowHelp();
}
else
{
    switch (args[0])
    {
        case "/download":
            if (args.Length > 1)
            {
                await Helper.DownloadModel(args[1]);
            }
            else
            {
                await Helper.DownloadModel();
            }

            break;
        case "/transcribe":
            if (args.Length > 2)
            {
                await Helper.Transcribe(args[1], args[2]);
            }
            else
            {
                await Helper.Transcribe(args[1]);
            }

            break;
        default:
            Helper.ShowHelp();
            break;
    }

    Console.WriteLine("Done.");
    Console.WriteLine();
}