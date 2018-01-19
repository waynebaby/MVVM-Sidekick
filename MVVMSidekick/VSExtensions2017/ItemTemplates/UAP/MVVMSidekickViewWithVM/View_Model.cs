using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
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
    public class $safeitemrootname$ : ViewModelBase<$safeitemrootname$>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public String Title
        {
            get => _TitleLocator(this).Value;
            set => _TitleLocator(this).SetValueAndTryNotify(value);
        }
        #region Property String Title Setup        
        protected Property<String> _Title = new Property<String>(_TitleLocator);
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator(nameof(Title), model => model.Initialize(nameof(Title), ref model._Title, ref _TitleLocator, () => default(String)));
        #endregion

    }
}

