call "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild" src\mysqlbackuper.sln /t:build /p:Configuration="Release" /m
@IF %ERRORLEVEL% NEQ 0 PAUSE