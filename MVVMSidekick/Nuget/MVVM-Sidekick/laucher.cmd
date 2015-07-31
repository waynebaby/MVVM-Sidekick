@echo off

xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 

cd..\..\
commoncode\commonCode UPVer nuget\mvvm-sidekick\MVVM-Sidekick.nuspec
commoncode\commonCode DPEXT commonCode\ProjectsForNugetPackages.xml  d:\Users\waywa\Documents\GitHub\MVVM-Sidekick\MVVMSidekick\VSExtensions2015  
commoncode\commonCode DPEXT commonCode\ProjectsForNugetPackages.xml  d:\Users\waywa\Documents\GitHub\MVVM-Sidekick\MVVMSidekick\VSExtensions

cd nuget\mvvm-sidekick\ 
BuildPublishPackage.cmd MVVM-Sidekick 


                                         