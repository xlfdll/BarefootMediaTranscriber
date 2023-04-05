using BarefootAutoTranscriber;

Console.WriteLine("Barefoot Auto Transcriber");
Console.WriteLine("(C) 2023 Xlfdll Workstation");
Console.WriteLine();

if (args.Length < 2)
{
    Helper.ShowHelp();
}
else
{
    switch (args[0])
    {
        case "/download":
            await Helper.DownloadModel(args[1]);
            break;
        case "/transcribe":
            await Helper.Transcribe(args[1], args[2]);
            break;
        default:
            Helper.ShowHelp();
            break;
    }
}