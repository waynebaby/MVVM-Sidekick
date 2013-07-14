using System;
#if NETFX_CORE

using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif WINDOWS_PHONE_8
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Windows.Controls;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Windows.Controls;
#endif
using MVVMSidekick.Collections;
using System.Collections.Generic;
using MVVMSidekick.Patterns;
using MVVMSidekick.Patterns.ItemsAndSelection;
using System.Collections.ObjectModel;



namespace MVVMSidekick.Test
{
    [TestClass]
    public class Patterns_ItemdAndSelectionGroupTest
    {
        ItemsAndSelectionGroup<string > CreateSampleGroup()
        {
            return new ItemsAndSelectionGroup<string>
            {
              Items=   new System.Collections.ObjectModel.ObservableCollection<string> 
                { 
                    "1~~",
                    "2~",
                    "3~~~"
                }
            };
                
             

        }

        ItemsAndSelectionGroup<int> CreateSampleGroupInt()
        {
            return new ItemsAndSelectionGroup<int>()
            {
                Items = new System.Collections.ObjectModel.ObservableCollection<int> 
                { 
                    1,2,3,4
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
            listbox.SetValue(ElementBinder.BinderProperty , list.Binder );
            list.SelectionMode = SelectionMode.Multiple;
            list.SelectedIndex = 0;

            Assert.AreEqual(listbox.Items.Count, 3);
            Assert.AreEqual(list.SelectionMode, listbox.SelectionMode);
            Assert.AreEqual(list.SelectedItem, listbox.SelectedItem);
            listbox.SelectedIndex = 1;
            Assert.AreEqual(list.SelectedIndex, listbox.SelectedIndex);
            Assert.AreEqual(list.SelectedItem, listbox.SelectedItem);
            var list2 = CreateSampleGroupInt();
            listbox.SetValue(ElementBinder.BinderProperty, list2.Binder );
            Assert.AreEqual(listbox.Items.Count, 4);
            list2.SelectedIndex = 0;
            Assert.AreEqual(list2.SelectedItem, listbox.SelectedItem);
            Assert.AreEqual(list2.SelectedItem, 1);
            Assert.IsNotNull(list2.SelectedItems);
        }

#if NETFX_CORE

                [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute]
#else
        [TestMethod]
#endif
        public void TestWPComboFBinding()
        {
            var list = CreateSampleGroup();
            var combo = new ComboBox();
            combo.SetValue(ElementBinder.BinderProperty, list.Binder);

            list.SelectedIndex = 0;

            Assert.AreEqual(combo.Items.Count, 3);

            Assert.AreEqual(list.SelectedItem, combo.SelectedItem);
            combo.SelectedIndex = 1;
            Assert.AreEqual(list.SelectedIndex, combo.SelectedIndex);
            Assert.AreEqual(list.SelectedItem, combo.SelectedItem);



        }


    }
}
