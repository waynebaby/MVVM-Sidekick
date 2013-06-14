using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using MVVMSidekick.Collections;
using System.Collections.Generic;
using MVVMSidekick.Patterns;
using MVVMSidekick.Patterns.Tree;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMSidekick.Test
{
    [TestClass]
    public class Patterns_TreeTest
    {
        TreeViewItemModel<string> CreateSampleGroup()
        {
            return new TreeViewItemModel<string>()
            {
                Children = new ObservableCollection<ITreeItem<object, TreeViewItemState>>
                (

                   Enumerable.Range(0, 15)
                   .Select(
                       i =>
                        new TreeViewItemModel<int> { Value = i }
                   )
                   .Cast<ITreeItem<object, TreeViewItemState>>()

                )


            };


        }
    }
}
