using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using MVVMSidekick.EventRouting ;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

using MVVMSidekick.Utilities;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Primitives;


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;


#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;

#endif

namespace MVVMSidekick
{

    namespace Patterns
    {
        /// <summary>
        /// A model that can bind to a ItemsControl just like ListBox or List View.  
        /// Use ItemSelectionGroupProperty to bind it with single set.
        /// </summary>
        public static class ObservableItemsAndSelectionGroup
        {


            public static IObservableItemsAndSelectionGroup<object, ICollection, IList> GetItemSelectionGroup(DependencyObject obj)
            {
                return (IObservableItemsAndSelectionGroup<object, ICollection, IList>)obj.GetValue(ItemsAndSelectionGroupProperty);
            }

            public static void SetItemSelectionGroup(DependencyObject obj, IObservableItemsAndSelectionGroup<object, ICollection, IList> value)
            {
                obj.SetValue(ItemsAndSelectionGroupProperty, value);
            }

            public static readonly DependencyProperty ItemsAndSelectionGroupProperty =
                DependencyProperty.RegisterAttached("ItemsAndSelectionGroup", typeof(IObservableItemsAndSelectionGroup<object, ICollection, IList>), typeof(ObservableItemsAndSelectionGroup), new PropertyMetadata(null,
                    (o, s) =>
                    {
                        var ls = o as ItemsControl;
                        if (ls == null)
                        {
                            return;
                        }
                        var vm = s.NewValue as IObservableItemsAndSelectionGroup<object, ICollection, IList>;
                        if (vm == null)
                        {
                            return;
                        }

                        vm.BindedTo = ls;
                        var itemsBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.OneWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.Items))
                        };

                        BindingOperations.SetBinding(ls, ItemsControl.ItemsSourceProperty, itemsBinding);



                        if (!(ls is Selector))
                        {
                            return;
                        }



                        var selectedBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedItem))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedItemProperty, selectedBinding);


                        var selectedindexBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedIndex))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedIndexProperty, selectedindexBinding);



                        var selectedValuePathBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedValuePath))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedValuePathProperty, selectedValuePathBinding);

                        var selectedValueBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedValue))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedValueProperty, selectedValueBinding);
#if SILVERLIGHT_5 || WINDOWS_PHONE_8
                        if (!(ls is ListBox))
#else
                        if (!(ls is ListBox) && (!(ls is ListView)))
#endif

                        {
                            return;
                        }

                        var selectionModeBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectionMode))
                        };

                        BindingOperations.SetBinding(ls, ListBox.SelectionModeProperty, selectionModeBinding);


                    }));






        }

        /// <summary>
        /// The abstraction of Items AndS electionGroup
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TCollection"></typeparam>
        /// <typeparam name="TList"></typeparam>
        public interface IObservableItemsAndSelectionGroup<out TValue, out TCollection, out TList>
            where TList : IList
            where TCollection : ICollection
        {
            FrameworkElement BindedTo { get; set; }
            string SelectedValuePath { get; set; }
            SelectionMode SelectionMode { get; set; }
            Object SelectedValue { get; set; }
            TCollection Items { get; }
            int SelectedIndex { get; }
            TValue SelectedItem
            {
                get;
            }
            TList SelectedItems
            {
                get;
            }
        }
        public class ObservableItemsAndSelectionGroup<T> : BindableBase<ObservableItemsAndSelectionGroup<T>>, IObservableItemsAndSelectionGroup<T, ObservableCollection<T>, IList>
        {

            public ObservableItemsAndSelectionGroup()
            {
                base.AddDisposeAction(() => BindedTo = null);

            }

            public FrameworkElement BindedTo { get; set; }

            public SelectionMode SelectionMode
            {
                get { return _SelectionModeLocator(this).Value; }
                set { _SelectionModeLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property SelectionMode SelectionMode Setup
            protected Property<SelectionMode> _SelectionMode = new Property<SelectionMode> { LocatorFunc = _SelectionModeLocator };
            static Func<BindableBase, ValueContainer<SelectionMode>> _SelectionModeLocator = RegisterContainerLocator<SelectionMode>("SelectionMode", model => model.Initialize("SelectionMode", ref model._SelectionMode, ref _SelectionModeLocator, _SelectionModeDefaultValueFactory));
            static Func<SelectionMode> _SelectionModeDefaultValueFactory = null;
            #endregion



            public string SelectedValuePath
            {
                get { return _SelectedValuePathLocator(this).Value; }
                set { _SelectedValuePathLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property string SelectedValuePath Setup
            protected Property<string> _SelectedValuePath = new Property<string> { LocatorFunc = _SelectedValuePathLocator };
            static Func<BindableBase, ValueContainer<string>> _SelectedValuePathLocator = RegisterContainerLocator<string>("SelectedValuePath", model => model.Initialize("SelectedValuePath", ref model._SelectedValuePath, ref _SelectedValuePathLocator, _SelectedValuePathDefaultValueFactory));
            static Func<string> _SelectedValuePathDefaultValueFactory = null;
            #endregion


            public object SelectedValue
            {
                get { return _SelectedValueLocator(this).Value; }
                set { _SelectedValueLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property object SelectedValue Setup
            protected Property<object> _SelectedValue = new Property<object> { LocatorFunc = _SelectedValueLocator };
            static Func<BindableBase, ValueContainer<object>> _SelectedValueLocator = RegisterContainerLocator<object>("SelectedValue", model => model.Initialize("SelectedValue", ref model._SelectedValue, ref _SelectedValueLocator, _SelectedValueDefaultValueFactory));
            static Func<object> _SelectedValueDefaultValueFactory = null;
            #endregion



            public ObservableCollection<T> Items
            {
                get { return _ItemsLocator(this).Value; }
                set { _ItemsLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property ObservableCollection<T>  Items Setup
            protected Property<ObservableCollection<T>> _Items = new Property<ObservableCollection<T>> { LocatorFunc = _ItemsLocator };
            static Func<BindableBase, ValueContainer<ObservableCollection<T>>> _ItemsLocator = RegisterContainerLocator<ObservableCollection<T>>("Items", model => model.Initialize("Items", ref model._Items, ref _ItemsLocator, _ItemsDefaultValueFactory));
            static Func<ObservableCollection<T>> _ItemsDefaultValueFactory = () => new ObservableCollection<T>();
            #endregion



            public int SelectedIndex
            {
                get { return _SelectedIndexLocator(this).Value; }
                set { _SelectedIndexLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property int SelectedIndex Setup
            protected Property<int> _SelectedIndex = new Property<int> { LocatorFunc = _SelectedIndexLocator };
            static Func<BindableBase, ValueContainer<int>> _SelectedIndexLocator = RegisterContainerLocator<int>("SelectedIndex", model => model.Initialize("SelectedIndex", ref model._SelectedIndex, ref _SelectedIndexLocator, _SelectedIndexDefaultValueFactory));
            static Func<int> _SelectedIndexDefaultValueFactory = null;
            #endregion



            public T SelectedItem
            {
                get { return _SelectedItemLocator(this).Value; }
                set { _SelectedItemLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property T SelectedItem Setup
            protected Property<T> _SelectedItem = new Property<T> { LocatorFunc = _SelectedItemLocator };
            static Func<BindableBase, ValueContainer<T>> _SelectedItemLocator = RegisterContainerLocator<T>("SelectedItem", model => model.Initialize("SelectedItem", ref model._SelectedItem, ref _SelectedItemLocator, _SelectedItemDefaultValueFactory));
            static Func<T> _SelectedItemDefaultValueFactory = null;
            #endregion


            public IList SelectedItems
            {
                get
                {
                    if (BindedTo != null)
                    {
                        dynamic x = BindedTo;
                        return x.SelectedItems as IList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }





        }

        /// <summary>
        /// This pattern is craated for
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TThis"></typeparam>
        /// <typeparam name="TChildren"></typeparam>
        /// <typeparam name="TState"></typeparam>
        public class ParentChildrenView<TParent, TThis, TChildren, TState> : BindableBase<ParentChildrenView<TParent, TThis, TChildren, TState>>
        {

            public TState State
            {
                get { return _StateLocator(this).Value; }
                set { _StateLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property TState State Setup
            protected Property<TState> _State = new Property<TState> { LocatorFunc = _StateLocator };
            static Func<BindableBase, ValueContainer<TState>> _StateLocator = RegisterContainerLocator<TState>("State", model => model.Initialize("State", ref model._State, ref _StateLocator, _StateDefaultValueFactory));
            static Func<TState> _StateDefaultValueFactory = null;
            #endregion


            public TParent Parent
            {
                get { return _ParentLocator(this).Value; }
                set { _ParentLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property TParent  Parent Setup
            protected Property<TParent> _Parent = new Property<TParent> { LocatorFunc = _ParentLocator };
            static Func<BindableBase, ValueContainer<TParent>> _ParentLocator = RegisterContainerLocator<TParent>("Parent", model => model.Initialize("Parent", ref model._Parent, ref _ParentLocator, _ParentDefaultValueFactory));
            static Func<TParent> _ParentDefaultValueFactory = null;
            #endregion


            public TThis This
            {
                get { return _ThisLocator(this).Value; }
                set { _ThisLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property TThis This Setup
            protected Property<TThis> _This = new Property<TThis> { LocatorFunc = _ThisLocator };
            static Func<BindableBase, ValueContainer<TThis>> _ThisLocator = RegisterContainerLocator<TThis>("This", model => model.Initialize("This", ref model._This, ref _ThisLocator, _ThisDefaultValueFactory));
            static Func<TThis> _ThisDefaultValueFactory = null;
            #endregion



            public ObservableCollection<TChildren> Children
            {
                get { return _ChildrenLocator(this).Value; }
                set { _ChildrenLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property ObservableCollection<TChildren> Children Setup
            protected Property<ObservableCollection<TChildren>> _Children = new Property<ObservableCollection<TChildren>> { LocatorFunc = _ChildrenLocator };
            static Func<BindableBase, ValueContainer<ObservableCollection<TChildren>>> _ChildrenLocator = RegisterContainerLocator<ObservableCollection<TChildren>>("Children", model => model.Initialize("Children", ref model._Children, ref _ChildrenLocator, _ChildrenDefaultValueFactory));
            static Func<ObservableCollection<TChildren>> _ChildrenDefaultValueFactory = null;
            #endregion


        }




    }


}
