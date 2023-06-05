@SET PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin";%PATH%
@SET OUTPUT_PATH="%CD%\bin"
@SET CONFIGURATION=Release

MSBuild src\BorderlessGaming.sln /p:OutputPath="%OUTPUT_PATH%" /t:Clean;Rebuild /p:Configuration=%CONFIGURATION%;Platform="Any CPU"

PAUSE
