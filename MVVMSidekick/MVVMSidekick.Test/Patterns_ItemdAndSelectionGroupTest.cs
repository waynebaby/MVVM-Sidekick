using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
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


        [TestMethod]
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
    }
}
