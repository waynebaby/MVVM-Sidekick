using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;





namespace MVVMSidekick.Views
{
    public class EmptyStageManager : IStageManager
    {

        public IStage this[string beaconKey]
        {
            get => null;
        }

        public IView CurrentBindingView
        {
            get; set;
        }

        public IStage DefaultStage
        {
            get => null;

            set { }
        }

        public IViewModel ViewModel { get; set; }

        public void InitParent(Func<object> parentLocator)
        {

        }
    }
}
