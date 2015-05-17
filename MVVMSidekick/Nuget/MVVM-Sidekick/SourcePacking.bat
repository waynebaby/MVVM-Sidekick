cd %ProjectDir%

rd Samples /s /q
rem md Samples
rem cd Samples
rem xcopy ..\..\..\Samples_SL5  Samples_SL5   /s  /i  /y
rem xcopy ..\..\..\Samples_WP8  Samples_WP8  /s  /i  /y
rem xcopy ..\..\..\Samples_WPF  Samples_WPF /s  /i /y
rem xcopy ..\..\..\Samples_WinRT Samples_WinRT  /s  /i  /y
rem xcopy ..\..\..\Samples_ViewModels Samples_ViewModels  /s  /i /y
rem xcopy ..\..\..\Samples_ViewModels.Test Samples_ViewModels.Test  /s  /i /y
rem 
rem 
rem 
rem rd Samples_SL5\bin /s /q
rem rd Samples_SL5\obj /s /q
rem rd Samples_WP8\bin /s /q
rem rd Samples_WP8\obj /s /q
rem rd Samples_WPF\bin /s /q
rem rd Samples_WPF\obj /s /q
rem rd Samples_WinRT\bin /s /q
rem rd Samples_WinRT\obj /s /q
rem rd Samples_ViewModels\bin /s /q
rem rd Samples_ViewModels\obj /s /q
rem rd Samples_ViewModels.Test\bin /s /q
rem rd Samples_ViewModels.Test\obj /s /q
rem 
rem 
rem xcopy ..\..\..\Samples.sln   .
rem xcopy ..\..\..\.nuget .nuget  /s  /i  /y
rem rem xcopy ..\..\..\packages\Microsoft.Bcl.Build*  packages  /s /i /y
rem cd ..
rem 
rem del src\samples.zip
rem 7z a src\Samples.zip  Samples




cd %ProjectDir%

rd MVVMSidekick /s /q
md MVVMSidekick
cd MVVMSidekick

xcopy ..\..\..\MVVMSidekick.Shared  MVVMSidekick.Shared     /s  /i  /y
xcopy ..\..\..\MVVMSidekick			MVVMSidekick            /s  /i  /y
xcopy ..\..\..\MVVMSidekick_Metro	MVVMSidekick_Metro      /s  /i  /y
xcopy ..\..\..\MVVMSidekick_net40	MVVMSidekick_net40      /s  /i  /y
xcopy ..\..\..\MVVMSidekick_Sl		MVVMSidekick_Sl         /s  /i  /y
xcopy ..\..\..\MVVMSidekick_Wp8		MVVMSidekick_Wp8        /s  /i  /y
xcopy ..\..\..\MVVMSidekick_win81	MVVMSidekick_win81        /s  /i  /y






rd MVVMSidekick\bin				/s /q	
rd MVVMSidekick_Metro\bin		/s /q
rd MVVMSidekick_net40\bin		/s /q
rd MVVMSidekick_Sl\bin			/s /q
rd MVVMSidekick_Wp8\bin			/s /q
	
rd MVVMSidekick\obj				/s /q
rd MVVMSidekick_Metro\obj		/s /q
rd MVVMSidekick_net40\obj		/s /q
rd MVVMSidekick_Sl\obj			/s /q
rd MVVMSidekick_Wp8\obj			/s /q


cd ..

del src\MVVMSidekick.zip
7z a src\MVVMSidekick.zip  MVVMSidekick


