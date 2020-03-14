using MVVMSidekick.ViewModels;
using System;





namespace MVVMSidekick.Views
{
    public interface IStageManager
    {
        IStage this[string beaconKey] { get; }
        IViewModel ViewModel { get; set; }
        IView CurrentBindingView { get; set; }
        IStage DefaultStage { get; }

        void InitParent(Func<object> parentLocator);
    }


}