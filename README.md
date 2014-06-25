

 ------------------

MVVM-Sidekick, You play the hero, and I play cute!
=============

> MVVM跟班儿,你英雄酷霸,我总萌萌哒
> ===================

![MVVM-Sidekick！ You play the hero, and I play cute!](https://github.com/waynebaby/MVVM-Sidekick/raw/master/MVVMSidekick/Documents/icon.png)


- MVVM-Sidekick: A Modern light-weight MVVM framework based on RX and TAP await. The CORE of this framework is ViewModelBase Type which you can use it with this framework, or use it with other framework, as well.
> 本项目是一个基于RX与 await等新技术的轻量级MVVM框架。其核心是ViewModelBase类型，你甚至可以把它拿出去和别的框架一起使用。


- The aim of this project: This project has learned a lot of good ideas from Prism and Reactive UI/Command project, and it is using new techs offered by .Net 4.5 and Windows Run-time. It offers a suitable foundation for new tech environment, based on a cool ViewModelBase and ReactiveCommand. 
> 项目的目的：集合 Reactive UI/Command, Prism 等框架的优点，应对.Net 4.5 和 Windows Run-time 带来的变化，为新技术环境量身打造一套以 ViewModelBase/ReactiveCommand 为核心的基础。


- MVVM-Sidekick is design on Windows 8 Modern Style Apps, and we got that ambition to cover all modern XAML run-time. 
> 本框架从设计开始就以Windows 8 Style App作为运行环境进行测试，野心覆盖所有XAML运行环境。


---

Resources
=========

### F.A.Q. goes [HERE]
[HERE]:    https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/0.FAQ.md
>### F.A.Q. 请戳 [这边]
[这边]:  https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/0.FAQ.md

---
###[Chinese course] provided by community  - Thanks [@黑芝麻的糊] for your contribution.
>###[中文教程]感谢[@黑芝麻的糊] 的贡献


[@黑芝麻的糊]: http://weibo.com/yanxiaodi1983
[中文教程]: http://www.cnblogs.com/yanxiaodi/p/3800767.html
[Chinese course]: http://www.cnblogs.com/yanxiaodi/p/3800767.html

---
### Projects in Real-World
>###案例



 - Open source projects
	>开源项目

   	[WP-ShanghaiFilmCenters]  by  [Edi_Wang] 		
	>上海影城查询
 		
 - Close source projects
  	>闭源项目
	
	
	[百思不得姐] by [mohoo-cc]

[WP-ShanghaiFilmCenters]: https://github.com/EdiWang/WP-ShanghaiFilmCenters
[Edi_Wang]:      http://diaosbook.com/

[百思不得姐]:http://www.windowsphone.com/en-us/store/app/百思不得姐/c7725093-2b0b-4765-9df8-9214b1e3e12d
[mohoo-cc]:http://weibo.com/mohoostudio

----------

What make difference?
============
> 功能特色
> =======

- Full support for Data Contract Serializing. You can easily save your status of View Model to JSON or XML stream, and easily restore from, too.
>全面支持Data Contract序列化 可以将一个VM的全部状态用任何方式保存为JSON/XML,反序列化后只需要简单操作就可以恢复工作


- This is light-weight framework. You can use our dll or use code file, either way you just need to install Reactive Extensions with Nuget.
>轻量级框架，不必安装全部DLL或者引用工程，只需要将指定代码文件加入你的工程切安装Reactive Extensions就可以用。


- Each of Model Properties in MVVM-Sidekick has it's own event container, can subscribe or broadcast it self. LinQ-Like code with RX can be used.
>Model所有的成员都有自己的事件容器，可以独立与其他事件订阅与广播。可以使用 LinQ-Like 语法进行配置和订阅。



- You can configure business logic of your properties and commands where they were declared. This will avoid your jumping between different parts in one View Model code file. (This kills me when I was working with other frameworks: for example you cannot configure a `DelayCommand` at the ***Property/Field declaration*** because “***this***” instance is not ready yet .)
>您可以在声明property的代码处配置property的业务细节，可以在声明command的代码处配置command的业务细节,这样你就不用在一个VM里面不同的代码段跳来跳去了。（用别的框架可累死我了，声明个command 还不能在声明原地初始化，因为this还没有实例化）



- You can also separate the business logic to your View Model declaration into a decorator factory or something else, to manage ***all code involved one same USE CASE together***, with the sequence same as document you a following. You can also easily add more business logic anywhere you like.
>您也可以将VM的业务细节配置与VM的创建时机分离，不但可以在实体外用装饰模式进行批量配置(这样可以让代码与需求文档的组织顺序高度统一，便于维护)，也可以根据需要临时装饰增加VM的功能。




----------

Samples
===========

>示例
>===========



In Nuget folder 

`\src\Samples.zip`

or [here](https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Nuget/MVVM-Sidekick/src/Samples.zip?raw=true)

----------

How To Use project template ?
===================
如何使用MVVMSidekick项目模板？
===================

 See document: [Hello world](https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/1.HelloWorld.md)


 >详情见文档  [跟班问世篇](https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/1.HelloWorld.md)




#Documents TOC
>#文档目录

1. [Hello world](https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/1.HelloWorld.md) 	跟班问世篇
2. [View Models (Part 1)](https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/2.ViewModels.md) 	View Models(第1部分)

3.  [View Models (Part 2)](https://github.com/waynebaby/MVVM-Sidekick/blob/master/MVVMSidekick/Documents/3.ViewModels_2.md) 	View Models(第2部分)

----------

Owner: Waynebaby


Ping Me：
 
[微博]  
[twitter]  
 [Mail]

[微博]: http://www.weibo.com/waynebabywang "WaynebabyWang"

[twitter]: http://twitter.com/waynebaby "Waynebaby"

[Mail]: mailto:blackshaman_wayne@hotmail.com "MSN Skype"




-----------------------------


