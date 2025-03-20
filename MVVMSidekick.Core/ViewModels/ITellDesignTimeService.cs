using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    public interface ITellDesignTimeService
    {
        bool IsInDesignMode
        {
            get;
        }
        
    }
    public class InDesignTime : ITellDesignTimeService
    {
        public bool IsInDesignMode => true;
    }

    public class InRuntime : ITellDesignTimeService
    {
        public bool IsInDesignMode => false;
    }
}
