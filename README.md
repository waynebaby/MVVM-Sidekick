
MVVM-Sidekick
=============
MVVM密友
===================


A Modern light-weight MVVM framework based on RX and TPL await

轻量级MVVM框架,基于RX与 await等新技术 



The aim of this project: Using new techs offered by .Net 4.5 and Windows Runtime, design a suitable foundation for new tech environment, based on a cool ViewModelBase and ReactiveCommand.

目的是集合Reactive Command, Prism 等框架的优点，应对.Net 4.5 和 Windows Runtime带来的变化，为新技术环境量身打造一套以ViewModelBase/ReactiveCommand为核心的基础。



MVVM-Sidekick is design on Windows 8 Modern Style Apps, and we got that ambition to cover all modern XAML runtime. 

从设计开始就以Windows 8 Style App作为运行环境进行测试，野心覆盖所有XAML运行环境。



What’s good.
============
功能特色
=======
1.	Full support for Data Contract Serializing. You can keep your status that VM got into JSON and XML. Easy steps after restore the data to make vm works again.

	全面支持DataContract序列化 可以将一个VM的全部状态用任何方式保存为JSon/XML反序列化后只需要简单操作就可以恢复工作



2.	This is light-weight one. Dll or code can works in you projects either. You just need install Reactive Extensions with nuget.

	轻量级代码级框架，不必安装全部DLL或者引用工程，只需要将指定代码文件加入你的工程切安装Reactive Extensions就可以用。



3.	Model Members in MVVM-Sidekick are communicated amount each other with events. Linq-Like code with RX can filter/subscribe events easily, and subscription could be disposed with model involved.

	Model所有的成员之间用事件序列驱动交互，只需要用 Linq-Like 语法进行配置和订阅，订阅在VM 销毁时自动取消。



4.	You can configure business logic of your properties and commands right at where they were declared. This will reduce your jumping between different parts in one VM file. (This kills me when I was working with other frameworks:you cannot configure a DelayCommand at the property/field declaration part because “this” instance is not ready yet .)

	可以在声明property的代码处配置property的业务细节，可以在声明command的代码处配置command的业务细节,这样你就不用在一个vm里面不同的代码段跳来跳去了。（用别的框架可累死我了，声明个command 还不能在声明原地初始化，因为this还没有实例化）



5.	You can also separate the business logic to your VM declaration into a decorator or factory something, to manage all code involved a USE CASE together, with the documenting organizing sequence, and you can also easily add more business everywhere if you like.

	可以将VM的业务细节配置与VM的创建时机分离，不但可以在实体外用装饰模式进行批量配置(这样可以让代码与需求文档的组织顺序高度统一，便于维护)，也可以根据需要临时装饰增加VM的功能。




Performance
===========

性能亮点
========

Propery access supports JIT inline and also support Property Name-Value Access. 

比起一般的字典内核与字段内核， MVVMSidekick 的VMBase对于属性访问采用可内联的直接寻址方式访问提高速度，且仍能保持字段名字典访问


How To Use project template ?
===================
如何使用MVVMSidekick项目模板？
===================

1.	Enter MVVMSidekick Folder

	进入MVVMSidekick目录

2.	Open MVVMSidekickVSIX.vsix , install the extension.

	打开 MVVMSidekickVSIX.vsix按照提示安装

3.	Create New Project using  MVVMSidekick template.

	创建新项目，在c#项目中找到 MVVMSidekick 模板

4.	Create MVVMSidekick Items in project.

	在创建好的项目中用MVVM Sidekick 代码模板 创建DataModel和View/ViewModel文件




Code snippets is needed when you use MVVMSidekick.
==============

本框架需要安装如下代码块辅助开发
==============


Install-Package MVVMSidekick.Snippet 


Supported snippets:

支持如下常用代码块：

	propvm  	New Propery In Model

	propvm  	在MVVMSidekick Binable/ViewModel 中增加属性

	vmcmd 		New Command In Model

	vmcmd 		在MVVMSidekick Binable/ViewModel 中增加命令

example:

	propvm +tab +tab





[微博] [twitter] [Mail]

[微博]: http://www.weibo.com/waynebabywang "韦恩卑鄙"
[twitter]: http://twitter.com/waynebaby "Waynebaby"
[Mail]: mailto:blackshaman_wayne@hotmail.com "MSN Skype"