# IMPORTANT: Make sure that the path to msbuild is correct!  
$msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
if ((Test-Path $msbuild) -eq $false) {
    Write-Host "Cannot find msbuild at '$msbuild'."
    Break
}

# Load useful functions.
. .\Resources\PowershellFunctions.ps1

# Keep track of the 'build' folder location - it's the root of everything else.
# We can also build paths to the key locations we'll use.
$scriptParentPath = Split-Path -parent $MyInvocation.MyCommand.Definition
$folderReleaseRoot = $scriptParentPath
$folderSourceRoot = Split-Path -parent $folderReleaseRoot
$folderSolutionsRoot = Join-Path $folderSourceRoot "source"
$folderNuspecRoot = Join-Path $folderSourceRoot "build\nuspec"

# Part 1 - Build the main libraries.
Write-Host "Preparing to build solution..."
$solutionCoreLibraries = Join-Path $folderSolutionsRoot "ConsoleControl.sln"
. $msbuild $solutionCoreLibraries /p:Configuration=Release /verbosity:minimal

# Part 2 - Get the version number of the core library, use this to build the destination release folder.
$folderWinFormsBinaries = Join-Path $folderSolutionsRoot "ConsoleControl\bin\Release"
$folderWPFBinaries = Join-Path $folderSolutionsRoot "ConsoleControl.WPF\bin\Release"
$releaseVersion = [Reflection.Assembly]::LoadFile((Join-Path $folderWinFormsBinaries "ConsoleControl.dll")).GetName().Version
Write-Host "Built Solution. Release Version: $releaseVersion"

# Part 3 - Copy the libraries to the nuget package folders.
$folderNugetWinForms = Join-Path $folderNuspecRoot "ConsoleControl\lib\net40"
$folderNugetWPF= Join-Path $folderNuspecRoot "ConsoleControl.WPF\lib\net40"
Remove-Item -Force $folderNugetWinForms
Remove-Item -Force $folderNugetWPF
CopyItems (Join-Path $folderWinFormsBinaries "*.*") $folderNugetWinForms
CopyItems (Join-Path $folderWPFBinaries "*.*") $folderNugetWPF

# Part 4 - Build the Nuget Package
$folderRelease = Join-Path $folderReleaseRoot $releaseVersion
EnsureEmptyFolderExists($folderRelease)
Write-Host "Preparing to build the Nuget Package..."
$nuget = Join-Path $scriptParentPath "Resources\nuget.exe"
. $nuget pack (Join-Path $folderNuspecRoot "ConsoleControl\ConsoleControl.nuspec") -Version $releaseVersion -OutputDirectory $folderRelease
. $nuget pack (Join-Path $folderNuspecRoot "ConsoleControl.WPF\ConsoleControl.WPF.nuspec") -Version $releaseVersion -OutputDirectory $folderRelease

# We're done!
Write-Host "Successfully built version: $releaseVersion"