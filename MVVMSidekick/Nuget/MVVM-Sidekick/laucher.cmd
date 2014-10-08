@echo off
set sver=%time%
set "sver=%sver::=%"
set "sver=%sver:.=%"

set bver=%date%
set "bver=%bver::=%"
set "bver=%bver:/=%"
set "bver=%bver:.=%"
set "bver=%bver:~0,8%"

set ver=1.3.%bver%.%sver%
echo %ver%
xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 
BuildPublishPackage.cmd MVVM-Sidekick %ver%


                                         