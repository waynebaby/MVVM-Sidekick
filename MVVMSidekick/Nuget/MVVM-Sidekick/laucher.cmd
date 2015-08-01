@echo off

xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 

cd..\..\
commoncode\commonCode UPVer nuget\mvvm-sidekick\MVVM-Sidekick.nuspec
commoncode\commonCode DPEXT commonCode\ProjectsForNugetPackages.xml  VSExtensions2015  
commoncode\commonCode DPEXT commonCode\ProjectsForNugetPackages.xml  VSExtensions
commoncode\commonCode DPTML VSExtensions2015
commoncode\commonCode DPTML VSExtensions
cd nuget\mvvm-sidekick\ 
BuildPublishPackage.cmd MVVM-Sidekick 


                                         