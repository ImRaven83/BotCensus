# Builds Bot Census and zips it in the layout SPT mods ship in, so the archive extracts straight over the
# game folder:  BepInEx/plugins/BotCensus/BotCensus.dll
#   .\package.ps1          SPT 4.1  ->  dist\BotCensus-<version>-SPT4.1.zip
#   .\package.ps1 -Spt 5   SPT 5.0  ->  dist\BotCensus-<version>-SPT5.0.zip  (draft build, ship as a pre-release)
# Builds with SkipDeploy, so packaging never touches the live install.
param([ValidateSet(4, 5)][int]$Spt = 4)
$ErrorActionPreference = "Stop"
$dotnet = if (Test-Path "$env:USERPROFILE\dotnet\dotnet.exe") { "$env:USERPROFILE\dotnet\dotnet.exe" } else { "dotnet" }
$project = "$PSScriptRoot\BotCensus.csproj"

& $dotnet build $project -c Release "-p:Spt=$Spt" -p:SkipDeploy=true
if ($LASTEXITCODE -ne 0) { throw "Build failed." }

$version = (& $dotnet msbuild $project "-p:Spt=$Spt" -getProperty:Version).Trim()
$dll = (& $dotnet msbuild $project -p:Configuration=Release "-p:Spt=$Spt" -getProperty:TargetPath).Trim()
if (-not (Test-Path $dll)) { throw "Built DLL not found at $dll" }

$label = if ($Spt -eq 5) { "SPT5.0" } else { "SPT4.1" }
$dist = "$PSScriptRoot\dist"
$stage = "$dist\stage-$label"
$zip = "$dist\BotCensus-$version-$label.zip"

if (Test-Path $stage) { Remove-Item $stage -Recurse -Force }
$plugin = New-Item -ItemType Directory -Force "$stage\BepInEx\plugins\BotCensus"
Copy-Item $dll $plugin
Copy-Item "$PSScriptRoot\README.md", "$PSScriptRoot\LICENSE" $plugin

if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path "$stage\BepInEx" -DestinationPath $zip
Remove-Item $stage -Recurse -Force

Write-Host "Packaged $zip"
