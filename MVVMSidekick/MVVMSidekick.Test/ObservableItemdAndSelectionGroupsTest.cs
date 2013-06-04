using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using MVVMSidekick.Collections;
using System.Collections.Generic;

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
                    "1~~",
                    "2~",
                    "3~~~"
                }
            };


        }


        [TestMethod]
        public void TestWPListBoxFBinding()
        {
            var list = CreateSampleGroup();
            var listbox = new ListBox();
            listbox.SetValue(ObservableItemsAndSelectionGroup.ItemSelectionGroupProperty, list);
            list.SelectionMode = SelectionMode.Multiple;
            list.SelectedIndex = 0;

            Assert.AreEqual(listbox.Items.Count, 3);
            Assert.AreEqual(list.SelectionMode, listbox.SelectionMode);
            Assert.AreEqual(list.SelectedItem, listbox.SelectedItem);
            listbox.SelectedIndex = 1;
            Assert.AreEqual(list.SelectedIndex, listbox.SelectedIndex);
            Assert.AreEqual(list.SelectedItem, listbox.SelectedItem);



        }
    }
}
