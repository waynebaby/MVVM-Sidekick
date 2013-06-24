using System;
#if NETFX_CORE

using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Windows.Controls;
#endif
using MVVMSidekick.Collections;
using System.Collections.Generic;
using MVVMSidekick.Patterns;
using MVVMSidekick.Patterns.ItemsAndSelection;



namespace MVVMSidekick.Test
{
    [TestClass]
    public class Patterns_ItemdAndSelectionGroupTest
    {
        ItemsAndSelectionGroup<string> CreateSampleGroup()
        {
            return new ItemsAndSelectionGroup<string>()
            {
                Items = new System.Collections.ObjectModel.ObservableCollection<string> 
                { 
                    "1~~",
                    "2~",
                    "3~~~"
                }
            };


        }

        #if NETFX_CORE

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute]
#else
        [TestMethod]
#endif
        public void TestWPListBoxFBinding()
        {
            var list = CreateSampleGroup();
            var listbox = new ListBox();
            listbox.SetValue(ItemsAndSelectionGroup.ItemsAndSelectionGroupProperty, list);
            list.SelectionMode = SelectionMode.Multiple;
            list.SelectedIndex = 0;

            Assert.AreEqual(listbox.Items.Count, 3);
            Assert.AreEqual(list.SelectionMode, listbox.SelectionMode);
            Assert.AreEqual(list.SelectedItem, listbox.SelectedItem);
            listbox.SelectedIndex = 1;
            Assert.AreEqual(list.SelectedIndex, listbox.SelectedIndex);
            Assert.AreEqual(list.SelectedItem, listbox.SelectedItem);



        }

#if NETFX_CORE

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute]
#else
        [TestMethod]
#endif
        public void TestWPComboFBinding()
        {
            var list = CreateSampleGroup();
            var combo = new ComboBox ();
            combo.SetValue(ItemsAndSelectionGroup.ItemsAndSelectionGroupProperty, list);
  
            list.SelectedIndex = 0;

            Assert.AreEqual(combo.Items.Count, 3);

            Assert.AreEqual(list.SelectedItem, combo.SelectedItem);
            combo.SelectedIndex = 1;
            Assert.AreEqual(list.SelectedIndex, combo.SelectedIndex);
            Assert.AreEqual(list.SelectedItem, combo.SelectedItem);



        }


    }
}
