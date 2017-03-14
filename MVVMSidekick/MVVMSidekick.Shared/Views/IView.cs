using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
//using Windows.UI.Xaml;

namespace MVVMSidekick.Views
{
    /// <summary>
    /// Interface IView
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        IViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the type of the view.
        /// </summary>
        /// <value>The type of the view.</value>
        ViewType ViewType { get; }

        /// <summary>
        /// Gets or sets the content object.
        /// </summary>
        /// <value>The content object.</value>
        Object ContentObject { get; set; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        Object Parent { get; }


    }


    /// <summary>
    /// Interface IView
    /// </summary>
    /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
    public interface IView<TViewModel> : IView, IDisposable where TViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the specific typed view model.
        /// </summary>
        /// <value>The specific typed view model.</value>
        TViewModel SpecificTypedViewModel { get; set; }
    }
}
