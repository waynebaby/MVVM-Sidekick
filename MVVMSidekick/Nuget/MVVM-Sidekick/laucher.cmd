rem @echo off

xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 

cd..\..\
commoncode\commonCode UPVer nuget\mvvm-sidekick\MVVM-Sidekick.nuspec

commoncode\commonCode DPEXT commonCode\ProjectsForNugetPackages.xml  VSExtensions2017   

echo commoncode\commonCode DPTML VSExtensions2017

cd nuget\mvvm-sidekick\ 
BuildPublishPackage.cmd MVVM-Sidekick 