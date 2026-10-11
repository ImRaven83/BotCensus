# Builds Bot Census (Release) and auto-deploys BotCensus.dll to BepInEx\plugins\BotCensus.
# Uses the user-local .NET SDK if present, otherwise whatever `dotnet` is on PATH.
#   .\build.ps1          SPT 4.1 (BepInEx 5, Mono)
#   .\build.ps1 -Spt 5   SPT 5.0 (BepInEx 6, IL2CPP) - draft, see README
param([ValidateSet(4, 5)][int]$Spt = 4)
$ErrorActionPreference = "Stop"
$dotnet = if (Test-Path "$env:USERPROFILE\dotnet\dotnet.exe") { "$env:USERPROFILE\dotnet\dotnet.exe" } else { "dotnet" }
& $dotnet build "$PSScriptRoot\BotCensus.csproj" -c Release "-p:Spt=$Spt"
