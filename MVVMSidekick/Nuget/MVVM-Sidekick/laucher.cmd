@echo off
set sver=%time%
set "sver=%sver::=%"
set "sver=%sver:.=%"


set bver=%date%
set "bver=%bver::=%"
set "bver=%bver:/=%"
set "bver=%bver:.=%"

set "bver=%bver:~0,8%"
set "bver=%bver: =%"

rem set ver=1.4.%bver%.%sver%
set ver=1.4.20150527.16500000
echo %ver%
xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 
BuildPublishPackage.cmd MVVM-Sidekick "%ver%"


                                         