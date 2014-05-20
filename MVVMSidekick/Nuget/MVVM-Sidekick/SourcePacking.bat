cd %ProjectDir%

rd Samples /s /q
md Samples
cd Samples
xcopy ..\..\..\Samples_SL5  Samples_SL5   /s  /i  /y
xcopy ..\..\..\Samples_WP8  Samples_WP8  /s  /i  /y
xcopy ..\..\..\Samples_WPF  Samples_WPF /s  /i /y
xcopy ..\..\..\Samples_WinRT Samples_WinRT  /s  /i  /y
xcopy ..\..\..\Samples_ViewModels Samples_ViewModels  /s  /i /y
xcopy ..\..\..\Samples_ViewModels.Test Samples_ViewModels.Test  /s  /i /y



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


xcopy ..\..\..\Samples.sln   .
xcopy ..\..\..\.nuget .nuget  /s  /i  /y
rem xcopy ..\..\..\packages\Microsoft.Bcl.Build*  packages  /s /i /y
cd ..

del src\samples.zip
7z a src\Samples.zip  Samples




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


