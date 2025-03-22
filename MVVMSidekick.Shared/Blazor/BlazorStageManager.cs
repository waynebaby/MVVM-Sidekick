#if BLAZOR
namespace MVVMSidekick.Views
{
    using Microsoft.AspNetCore.Components;
    using MVVMSidekick.ViewModels;
    using System;

    /// <summary>
    /// The abstract  for frame/contentcontrol. VM can access this class to Show other vm and vm's mapped view.
    /// </summary>
    public class BlazorStageManager : IStageManager
    {

        public BlazorStageManager(IStage stage)
        {
            DefaultStage = stage;
        }
        public IStage this[string beaconKey] => throw new NotImplementedException();

        public IViewModel ViewModel { get; set; }
        public IView CurrentBindingView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

 
        public IStage DefaultStage { get; private set; }

        public void InitParent(Func<object> parentLocator)
        {

        }
    }
}

#endif
