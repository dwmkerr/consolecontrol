# Run msbuild on the solution, in release mode.
$msbuild ="${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
$args = "ConsoleControl.sln /t:Rebuild /p:Configuration=Release"

# Run the command.
Write-Host "Running: ""$msbuild"" $args"
& "$msbuild" /p:Configuration=Release /t:Clean,Build ConsoleControl.sln
