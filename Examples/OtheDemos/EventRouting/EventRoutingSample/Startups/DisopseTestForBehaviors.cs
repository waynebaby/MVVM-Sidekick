using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventRoutingSample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick;
using MVVMSidekick.ViewModels;

namespace EventRoutingSample.Startups
{
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal  Action<MVVMSidekickOptions> DisopseTestForBehaviorsConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<DisopseTestForBehaviors, DisopseTestForBehaviors_Model>());
    }
}
