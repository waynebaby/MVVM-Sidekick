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
    public class $safeitemrootname$ : ViewModelBase<$safeitemrootname$>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

    
        [DataMember]
        public string SomePropertySample
        {
            get { return m_SomePropertySampleLocator(this).Value; }
            set { m_SomePropertySampleLocator(this).SetValueAndTryNotify(value); }
        }


        #region Property string SomePropertySample Setup

        protected Property<string> m_SomePropertySample =
          new Property<string> { LocatorFunc = m_SomePropertySampleLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<string>> m_SomePropertySampleLocator =
            RegisterContainerLocator<string>(
                "SomePropertySample",
                model =>
                {
                    model.m_SomePropertySample =
                        model.m_SomePropertySample
                        ??
                        new Property<string> { LocatorFunc = m_SomePropertySampleLocator };
                    return model.m_SomePropertySample.Container =
                        model.m_SomePropertySample.Container
                        ??
                        new ValueContainer<string>("SomePropertySample", model);
                });

        #endregion



    }
	
}

