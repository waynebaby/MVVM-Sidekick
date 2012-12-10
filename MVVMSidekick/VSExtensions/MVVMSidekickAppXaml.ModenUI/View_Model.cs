using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace $rootnamespace$.ViewModels
{

	[DataContract]
	public class MainPage_Model : ViewModelBase<MainPage_Model>
	{
		// If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
		// 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

	
		

        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup        
        protected Property<String> _Title = new Property<String>{ LocatorFunc = _TitleLocator};
        static Func<BindableBase,ValueContainer<String>> _TitleLocator= RegisterContainerLocator<String>("Title", model =>model.Initialize("Title",ref model._Title, ref _TitleLocator,_TitleDefaultValueFactory));         
        static Func<String> _TitleDefaultValueFactory = null;
        #endregion


	 

		


	}
	
}

