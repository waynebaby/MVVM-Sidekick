using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
//using Windows.UI.Xaml;

namespace MVVMSidekick.Views 
{
    /// <summary>
    /// 视图接口，定义视图的基本功能和属性
    /// Interface IView - Defines basic functionality and properties for views
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 获取或设置视图模型
        /// Gets or sets the view model.
        /// </summary>
        /// <value>视图模型 / The view model.</value>
        IViewModel ViewModel { get; set; }

        /// <summary>
        /// 获取或设置视图内容对象
        /// Gets or sets the content object.
        /// </summary>
        /// <value>视图内容对象 / The content object.</value>
        Object ViewContentObject { get; set; }

        /// <summary>
        /// 获取视图对象
        /// Gets the view object
        /// </summary>
        Object ViewObject { get; }
        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        object  Parent { get; }

        void SelfClose();
    }
    
}
