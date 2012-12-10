using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace $safeprojectname$.ViewModels
{

    [DataContract]
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性


        public String Title
        {
            get { return m_TitleLocator(this).Value; }
            set { m_TitleLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property String Title Setup
        protected Property<String> m_Title =
          new Property<String> { LocatorFunc = m_TitleLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<String>> m_TitleLocator =
            RegisterContainerLocator<String>(
                "Title",
                model =>
                {
                    model.m_Title =
                        model.m_Title
                        ??
                        new Property<String> { LocatorFunc = m_TitleLocator };
                    return model.m_Title.Container =
                        model.m_Title.Container
                        ??
                        new ValueContainer<String>("Title", model);
                });
        #endregion




    }

}

