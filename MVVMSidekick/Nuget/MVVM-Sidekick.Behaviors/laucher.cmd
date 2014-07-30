set ver=0.3.0.3
xcopy lib\*.*  ..\..\packages\MVVM-Sidekick.Behaviors.%ver%\lib  /s /i /y 
BuildPublishPackage.cmd MVVM-Sidekick.Behaviors %ver%

