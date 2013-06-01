using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MVVMSidekick.Test
{
    [TestClass]
    public class ObservableItemdAndSelectionGroupsTest
    {
        MVVMSidekick.Collections.ObservableItemsAndSelectionGroup<string> CreateSampleGroup()
        {
            return new Collections.ObservableItemsAndSelectionGroup<string>()
            {
                Items = new System.Collections.ObjectModel.ObservableCollection<string> 
                { 
                    "1",
                    "2",
                    "3"
                }
            };


        }


        [TestMethod]
        public void TestWPListBoxFBinding()
        {

        }
    }
}
