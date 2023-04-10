# Barefoot Media Transcriber
Automatic English audio transcriber frontend used by Barefoot Invader team.

## System Requirements
This program can only run on Windows 10+ (x64) due to the system requirements of WhisperNet library.

## Usage
```
= Parameters =

BarefootMediaTranscriber /transcribe <SourceMediaFileName> [<OutputSubtitleFileName>] [<ModelType>]
- Transcribe media file

BarefootMediaTranscriber /download [<ModelType>]
- Download model file to current directory

= Notes =

- Output will be saved to current directory as SRT subtitle file
- If media file is a video, the first audio track will be used
- The following model types are available:
    tiny[.en], base[.en], small[.en], medium[.en], large
  The default model type is small.en if not specified
```

## Development Prerequisites
* Visual Studio 2022+ w/ .NET 6

Before the build,
* `version.txt` needs to be created with a version number `major.minor` as the contents.
* `generate-build-number.sh` needs to be executed in a Git / Bash shell to generate build information code file (`Directory.Build.props`).

## External Sources
Based on [WhisperNet](https://github.com/Const-me/Whisper) library, which is licensed under [Mozilla Public License 2.0](https://github.com/Const-me/Whisper/blob/master/LICENSE).
