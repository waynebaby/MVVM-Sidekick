@echo off
set sver=%time%
set "sver=%sver::=%"
set "sver=%sver:.=%"

set bver=%date%
set "bver=%bver::=%"
set "bver=%bver:/=%"
set "bver=%bver:.=%"
set "bver=%bver:~0,8%"

set ver=0.4.0.0
xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.Behaviors.%ver%\lib  /s /i /y 
BuildPublishPackage.cmd MVVM-Sidekick.Behaviors %ver%

