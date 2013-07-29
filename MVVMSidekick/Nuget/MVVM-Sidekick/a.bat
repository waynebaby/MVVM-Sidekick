cd  D:\Users\Wayne\Documents\GitHub\MVVM-Sidekick\MVVMSidekick\
rd Samples_SL5\bin /s /q
rd Samples_SL5\obj /s /q
rd Samples_WP8\bin /s /q
rd Samples_WP8\obj /s /q
rd Samples_WPF\bin /s /q
rd Samples_WPF\obj /s /q
rd Samples_WinRT\bin /s /q
rd Samples_WinRT\obj /s /q
rd Samples_ViewModels\bin /s /q
rd Samples_ViewModels\obj /s /q
rd Samples_ViewModels.Test\bin /s /q
rd Samples_ViewModels.Test\obj /s /q




cd D:\Users\Wayne\Documents\GitHub\MVVM-Sidekick\MVVMSidekick\Nuget\MVVM-Sidekick\
rd Samples /s /q
md Samples
cd Samples
xcopy ..\..\..\Samples_SL5  Samples_SL5   /s  /i  /y
xcopy ..\..\..\Samples_WP8  Samples_WP8  /s  /i  /y
xcopy ..\..\..\Samples_WPF  Samples_WPF /s  /i /y
xcopy ..\..\..\Samples_WinRT Samples_WinRT  /s  /i  /y
xcopy ..\..\..\Samples_ViewModels Samples_ViewModels  /s  /i /y
xcopy ..\..\..\Samples_ViewModels.Test Samples_ViewModels.Test  /s  /i /y
xcopy ..\..\..\Samples.sln   .
xcopy ..\..\..\.nuget .nuget  /s  /i  /y
xcopy ..\..\..\packages\Microsoft.Bcl.Build*  packages  /s /i /y
cd ..

del src\samples.zip
7z a src\Samples.zip  Samples
pause
