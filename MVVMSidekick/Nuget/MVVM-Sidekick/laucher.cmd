@echo off
set sver=%time%
set "sver=%sver::=%"
set "sver=%sver:.=%"

set bver=%~t0
set "bver=%bver::=%"
set "bver=%bver:.=%"
set "bver=%bver: =%"
set ver=1.3.20140906.%sver%
echo %ver%
xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 
BuildPublishPackage.cmd MVVM-Sidekick %ver%


                                         