set ver=1.1.8.4
xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.%ver%\lib  /s /i /y 
BuildPublishPackage.cmd MVVM-Sidekick %ver%
