// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Patterns.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.Windows;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;



#elif WPF

using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;



#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
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

		namespace ItemsAndSelection
		{


			/// <summary>
			/// Class ItemsAndSelectionGroup.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			public class ItemsAndSelectionGroup<T> : ItemsAndSelectionGroup<T, ObservableCollection<T>, SelectionMode>
			{
				/// <summary>
				/// Initializes static members of the <see cref="ItemsAndSelectionGroup{T}"/> class.
				/// </summary>
				static ItemsAndSelectionGroup()
				{
					_ItemsDefaultValueFactory = () => new ObservableCollection<T>();

				}
			}


			/// <summary>
			/// Class ItemsAndSelectionGroupBinder.
			/// </summary>
			public class ItemsAndSelectionGroupBinder : ElementBinderBase<ItemsAndSelectionGroupBinder>
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="ItemsAndSelectionGroupBinder"/> class.
				/// </summary>
				/// <param name="bindingAction">The binding action.</param>
				/// <param name="disposeAction">The dispose action.</param>
				public ItemsAndSelectionGroupBinder(Action<ItemsAndSelectionGroupBinder> bindingAction, Action<ItemsAndSelectionGroupBinder> disposeAction)
					: base(bindingAction, disposeAction)
				{ }


				/// <summary>
				/// Gets the binder.
				/// </summary>
				/// <param name="obj">The object.</param>
				/// <returns>ItemsAndSelectionGroupBinder.</returns>
				public static ItemsAndSelectionGroupBinder GetBinder(DependencyObject obj)
				{
					return (ItemsAndSelectionGroupBinder)obj.GetValue(BinderProperty);
				}

				/// <summary>
				/// Sets the binder.
				/// </summary>
				/// <param name="obj">The object.</param>
				/// <param name="value">The value.</param>
				public static void SetBinder(DependencyObject obj, ItemsAndSelectionGroupBinder value)
				{
					obj.SetValue(BinderProperty, value);
				}

				// Using a DependencyProperty as the backing store for ElementBinder.  This enables animation, styling, binding, etc...
				/// <summary>
				/// The binder property
				/// </summary>
				public static readonly DependencyProperty BinderProperty =
					DependencyProperty.RegisterAttached("Binder", typeof(ItemsAndSelectionGroupBinder), typeof(ItemsAndSelectionGroupBinder), new PropertyMetadata(
						null,
						BinderPropertyChangedCallback));


			}

			/// <summary>
			/// Interface IItemsAndSelectionGroupBinding
			/// </summary>
			public interface IItemsAndSelectionGroupBinding
			{
				/// <summary>
				/// Gets or sets the binder.
				/// </summary>
				/// <value>The binder.</value>
				MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroupBinder Binder { get; set; }
			}

			/// <summary>
			/// Class ItemsAndSelectionGroup.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <typeparam name="TCollection">The type of the t collection.</typeparam>
			/// <typeparam name="TSelectionMode">The type of the t selection mode.</typeparam>
			public class ItemsAndSelectionGroup<T, TCollection, TSelectionMode> : BindableBase<ItemsAndSelectionGroup<T, TCollection, TSelectionMode>>, IItemsAndSelectionGroupBinding
				where TCollection : ICollection<T>, INotifyCollectionChanged
			{

				/// <summary>
				/// Initializes a new instance of the <see cref="ItemsAndSelectionGroup{T, TCollection, TSelectionMode}"/> class.
				/// </summary>
				public ItemsAndSelectionGroup()
				{

					base.AddDisposeAction(() => Binder = null);
					this.GetValueContainer(x => x.Items).GetNullObservable()
						.Subscribe(_ => ResetSelection())
						.DisposeWith(this);

				}


				/// <summary>
				/// Resets the selection.
				/// </summary>
				private void ResetSelection()
				{
					ResetPropertyValue(_SelectedValue);
					ResetPropertyValue(_SelectedIndex);
					ResetPropertyValue(_SelectedItem);

				}


				/// <summary>
				/// Gets or sets the binder.
				/// </summary>
				/// <value>The binder.</value>
				public ItemsAndSelectionGroupBinder Binder
				{
					get { return _BinderLocator(this).Value; }
					set { _BinderLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property ElementBinder Binder Setup
				/// <summary>
				/// The _ binder
				/// </summary>
				protected Property<ItemsAndSelectionGroupBinder> _Binder = new Property<ItemsAndSelectionGroupBinder> { LocatorFunc = _BinderLocator };
				/// <summary>
				/// The _ binder locator
				/// </summary>
				static Func<BindableBase, ValueContainer<ItemsAndSelectionGroupBinder>> _BinderLocator = RegisterContainerLocator<ItemsAndSelectionGroupBinder>("Binder", model => model.Initialize("Binder", ref model._Binder, ref _BinderLocator, _BinderDefaultValueFactory));
				/// <summary>
				/// The _ binder default value factory
				/// </summary>
				static Func<BindableBase, ItemsAndSelectionGroupBinder> _BinderDefaultValueFactory = model =>
					{
						var vm = CastToCurrentType(model);

						Action<ItemsAndSelectionGroupBinder> bindingAc =
							 eb =>
							 {


								 var ls = eb.Element as ItemsControl;
								 if (ls == null)
								 {
									 return;
								 }


								 if (vm == null)
								 {
									 return;
								 }


								 var itemsBinding = new Binding()
								 {
									 Source = vm,
									 Mode = BindingMode.OneWay,
									 Path = new PropertyPath("Items")
								 };

								 BindingOperations.SetBinding(ls, ItemsControl.ItemsSourceProperty, itemsBinding);



								 if (!(ls is Selector))
								 {
									 return;
								 }



								 var selectedBinding = new Binding()
								 {
									 Source = vm,
									 Mode = BindingMode.TwoWay,
									 Path = new PropertyPath("SelectedItem")
								 };

								 BindingOperations.SetBinding(ls, Selector.SelectedItemProperty, selectedBinding);


								 var selectedindexBinding = new Binding()
								 {
									 Source = vm,
									 Mode = BindingMode.TwoWay,
									 Path = new PropertyPath("SelectedIndex")
								 };

								 BindingOperations.SetBinding(ls, Selector.SelectedIndexProperty, selectedindexBinding);



								 var selectedValuePathBinding = new Binding()
								 {
									 Source = vm,
									 Mode = BindingMode.TwoWay,
									 Path = new PropertyPath("SelectedValuePath")
								 };

								 BindingOperations.SetBinding(ls, Selector.SelectedValuePathProperty, selectedValuePathBinding);

								 var selectedValueBinding = new Binding()
								 {
									 Source = vm,
									 Mode = BindingMode.TwoWay,
									 Path = new PropertyPath("SelectedValue")
								 };

								 BindingOperations.SetBinding(ls, Selector.SelectedValueProperty, selectedValueBinding);
#if SILVERLIGHT_5 || WINDOWS_PHONE_8||WINDOWS_PHONE_7
                                 if (!(ls is ListBox))
#else
								 if (!(ls is ListBox) && (!(ls is ListView)))
#endif

								 {
									 return;
								 }

								 var selectionModeBinding = new Binding()
								 {
									 Source = vm,
									 Mode = BindingMode.TwoWay,
									 Path = new PropertyPath("SelectionMode")
								 };

								 BindingOperations.SetBinding(ls, ListBox.SelectionModeProperty, selectionModeBinding);

							 };


						return new ItemsAndSelectionGroupBinder(bindingAc, (e) => e.Element = null).DisposeWith(vm);

					};

				#endregion



				/// <summary>
				/// Gets the binded to.
				/// </summary>
				/// <value>The binded to.</value>
				public FrameworkElement BindedTo
				{
					get { return Binder.Element; }

				}



				/// <summary>
				/// Gets or sets the selection mode.
				/// </summary>
				/// <value>The selection mode.</value>
				public TSelectionMode SelectionMode
				{
					get { return _SelectionModeLocator(this).Value; }
					set { _SelectionModeLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property TSelectionMode SelectionMode Setup
				/// <summary>
				/// The _ selection mode
				/// </summary>
				protected Property<TSelectionMode> _SelectionMode = new Property<TSelectionMode> { LocatorFunc = _SelectionModeLocator };
				/// <summary>
				/// The _ selection mode locator
				/// </summary>
				static Func<BindableBase, ValueContainer<TSelectionMode>> _SelectionModeLocator = RegisterContainerLocator<TSelectionMode>("SelectionMode", model => model.Initialize("SelectionMode", ref model._SelectionMode, ref _SelectionModeLocator, _SelectionModeDefaultValueFactory));
				/// <summary>
				/// The _ selection mode default value factory
				/// </summary>
				static Func<TSelectionMode> _SelectionModeDefaultValueFactory = null;
				#endregion




				/// <summary>
				/// Gets or sets the selected value path.
				/// </summary>
				/// <value>The selected value path.</value>
				public string SelectedValuePath
				{
					get { return _SelectedValuePathLocator(this).Value; }
					set { _SelectedValuePathLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property string SelectedValuePath Setup
				/// <summary>
				/// The _ selected value path
				/// </summary>
				protected Property<string> _SelectedValuePath = new Property<string> { LocatorFunc = _SelectedValuePathLocator };
				/// <summary>
				/// The _ selected value path locator
				/// </summary>
				static Func<BindableBase, ValueContainer<string>> _SelectedValuePathLocator = RegisterContainerLocator<string>("SelectedValuePath", model => model.Initialize("SelectedValuePath", ref model._SelectedValuePath, ref _SelectedValuePathLocator, _SelectedValuePathDefaultValueFactory));
				/// <summary>
				/// The _ selected value path default value factory
				/// </summary>
				static Func<string> _SelectedValuePathDefaultValueFactory = null;
				#endregion


				/// <summary>
				/// Gets or sets the selected value.
				/// </summary>
				/// <value>The selected value.</value>
				public object SelectedValue
				{
					get { return _SelectedValueLocator(this).Value; }
					set { _SelectedValueLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property object SelectedValue Setup
				/// <summary>
				/// The _ selected value
				/// </summary>
				protected Property<object> _SelectedValue = new Property<object> { LocatorFunc = _SelectedValueLocator };
				/// <summary>
				/// The _ selected value locator
				/// </summary>
				static Func<BindableBase, ValueContainer<object>> _SelectedValueLocator = RegisterContainerLocator<object>("SelectedValue", model => model.Initialize("SelectedValue", ref model._SelectedValue, ref _SelectedValueLocator, _SelectedValueDefaultValueFactory));
				/// <summary>
				/// The _ selected value default value factory
				/// </summary>
				static Func<object> _SelectedValueDefaultValueFactory = null;
				#endregion




				/// <summary>
				/// Gets or sets the items.
				/// </summary>
				/// <value>The items.</value>
				public TCollection Items
				{
					get { return _ItemsLocator(this).Value; }
					set { _ItemsLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property TCollection Items Setup
				/// <summary>
				/// The _ items
				/// </summary>
				protected Property<TCollection> _Items = new Property<TCollection> { LocatorFunc = _ItemsLocator };
				/// <summary>
				/// The _ items locator
				/// </summary>
				protected static Func<BindableBase, ValueContainer<TCollection>> _ItemsLocator = RegisterContainerLocator<TCollection>("Items", model => model.Initialize("Items", ref model._Items, ref _ItemsLocator, _ItemsDefaultValueFactory));
				/// <summary>
				/// The _ items default value factory
				/// </summary>
				protected static Func<TCollection> _ItemsDefaultValueFactory = null;
				#endregion




				/// <summary>
				/// Gets or sets the index of the selected.
				/// </summary>
				/// <value>The index of the selected.</value>
				public int SelectedIndex
				{
					get { return _SelectedIndexLocator(this).Value; }
					set { _SelectedIndexLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property int SelectedIndex Setup
				/// <summary>
				/// The _ selected index
				/// </summary>
				protected Property<int> _SelectedIndex = new Property<int> { LocatorFunc = _SelectedIndexLocator };
				/// <summary>
				/// The _ selected index locator
				/// </summary>
				static Func<BindableBase, ValueContainer<int>> _SelectedIndexLocator = RegisterContainerLocator<int>("SelectedIndex", model => model.Initialize("SelectedIndex", ref model._SelectedIndex, ref _SelectedIndexLocator, _SelectedIndexDefaultValueFactory));
				/// <summary>
				/// The _ selected index default value factory
				/// </summary>
				static Func<int> _SelectedIndexDefaultValueFactory = () => -1;
				#endregion



				/// <summary>
				/// Gets or sets the selected item.
				/// </summary>
				/// <value>The selected item.</value>
				public T SelectedItem
				{
					get { return _SelectedItemLocator(this).Value; }
					set
					{
						_SelectedItemLocator(this).SetValueAndTryNotify(value);
						base.RaisePropertyChanged(() => new PropertyChangedEventArgs("SelectedItems"));
					}
				}
				#region Property T SelectedItem Setup
				/// <summary>
				/// The _ selected item
				/// </summary>
				protected Property<T> _SelectedItem = new Property<T> { LocatorFunc = _SelectedItemLocator };
				/// <summary>
				/// The _ selected item locator
				/// </summary>
				static Func<BindableBase, ValueContainer<T>> _SelectedItemLocator = RegisterContainerLocator<T>("SelectedItem", model => model.Initialize("SelectedItem", ref model._SelectedItem, ref _SelectedItemLocator, _SelectedItemDefaultValueFactory));
				/// <summary>
				/// The _ selected item default value factory
				/// </summary>
				static Func<T> _SelectedItemDefaultValueFactory = null;
				#endregion


				/// <summary>
				/// Gets the selected items.
				/// </summary>
				/// <value>The selected items.</value>
				public IEnumerable SelectedItems
				{
					get
					{
						if (BindedTo != null)
						{

							try
							{
#if WINDOWS_PHONE_7
                                var m = BindedTo.GetType().GetProperty("SelectedItems");
                                if (m!=null)
                                {
                                    return m.GetValue(BindedTo,new object [0]) as IEnumerable;
                                }
                               
#else
								dynamic x = BindedTo;
								return x.SelectedItems;
#endif

							}
							catch (Exception)
							{


							}

						}
						return null;
					}
				}










			}
		}



		namespace Tree
		{
			/// <summary>
			/// Interface ITreeItem
			/// </summary>
			/// <typeparam name="TNodeValue">The type of the t node value.</typeparam>
			/// <typeparam name="TState">The type of the t state.</typeparam>
			public interface ITreeItem<out TNodeValue, TState>
			{
				/// <summary>
				/// Gets the value.
				/// </summary>
				/// <value>The value.</value>
				TNodeValue Value { get; }
				/// <summary>
				/// Gets or sets the state.
				/// </summary>
				/// <value>The state.</value>
				TState State { get; set; }
				/// <summary>
				/// Gets the parent.
				/// </summary>
				/// <value>The parent.</value>
				ITreeItem<Object, TState> Parent { get; }
				/// <summary>
				/// Gets the children.
				/// </summary>
				/// <value>The children.</value>
				ICollection<ITreeItem<object, TState>> Children { get; }
				/// <summary>
				/// Gets the type of the node value.
				/// </summary>
				/// <value>The type of the node value.</value>
				Type NodeValueType { get; }
			}



			//[DataContract(IsReference=true) ] //if you want
			/// <summary>
			/// Class TreeItemModelBase.
			/// </summary>
			/// <typeparam name="TNodeValue">The type of the t node value.</typeparam>
			/// <typeparam name="TState">The type of the t state.</typeparam>
			/// <typeparam name="TSubType">The type of the t sub type.</typeparam>
			public abstract class TreeItemModelBase<TNodeValue, TState, TSubType> :
				BindableBase<TSubType>,
				ITreeItem<TNodeValue, TState>
				where TSubType : TreeItemModelBase<TNodeValue, TState, TSubType>
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="TreeItemModelBase{TNodeValue, TState, TSubType}"/> class.
				/// </summary>
				public TreeItemModelBase()
				{

				}






				/// <summary>
				/// Gets the type of the node value.
				/// </summary>
				/// <value>The type of the node value.</value>
				public Type NodeValueType
				{
					get
					{
						return typeof(TNodeValue);
					}
				}

				/// <summary>
				/// Gets or sets the value.
				/// </summary>
				/// <value>The value.</value>
				public TNodeValue Value
				{
					get { return _ValueLocator(this).Value; }
					set { _ValueLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property TNodeValue Value Setup
				/// <summary>
				/// The _ value
				/// </summary>
				protected Property<TNodeValue> _Value = new Property<TNodeValue> { LocatorFunc = _ValueLocator };
				/// <summary>
				/// The _ value locator
				/// </summary>
				static Func<BindableBase, ValueContainer<TNodeValue>> _ValueLocator = RegisterContainerLocator<TNodeValue>("Value", model => model.Initialize("Value", ref model._Value, ref _ValueLocator, _ValueDefaultValueFactory));
				/// <summary>
				/// The _ value default value factory
				/// </summary>
				static Func<TNodeValue> _ValueDefaultValueFactory = null;
				#endregion



				/// <summary>
				/// Gets or sets the state.
				/// </summary>
				/// <value>The state.</value>
				public TState State
				{
					get { return _StateLocator(this).Value; }
					set { _StateLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property TState State Setup
				/// <summary>
				/// The _ state
				/// </summary>
				protected Property<TState> _State = new Property<TState> { LocatorFunc = _StateLocator };
				/// <summary>
				/// The _ state locator
				/// </summary>
				static Func<BindableBase, ValueContainer<TState>> _StateLocator = RegisterContainerLocator<TState>("State", model => model.Initialize("State", ref model._State, ref _StateLocator, _StateDefaultValueFactory));
				/// <summary>
				/// The _ state default value factory
				/// </summary>
				static Func<TState> _StateDefaultValueFactory = null;
				#endregion



				/// <summary>
				/// Gets or sets the parent.
				/// </summary>
				/// <value>The parent.</value>
				public ITreeItem<object, TState> Parent
				{
					get { return _ParentLocator(this).Value; }
					set { _ParentLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property INode<object, TState> Parent Setup
				/// <summary>
				/// The _ parent
				/// </summary>
				protected Property<ITreeItem<object, TState>> _Parent = new Property<ITreeItem<object, TState>> { LocatorFunc = _ParentLocator };
				/// <summary>
				/// The _ parent locator
				/// </summary>
				static Func<BindableBase, ValueContainer<ITreeItem<object, TState>>> _ParentLocator = RegisterContainerLocator<ITreeItem<object, TState>>("Parent", model => model.Initialize("Parent", ref model._Parent, ref _ParentLocator, _ParentDefaultValueFactory));
				/// <summary>
				/// The _ parent default value factory
				/// </summary>
				static Func<ITreeItem<object, TState>> _ParentDefaultValueFactory = null;
				#endregion




				//public ObservableCollection<ITreeItem<object, TState>> Children
				//{
				//    get { return _ChildrenLocator(this).Value; }
				//    set { _ChildrenLocator(this).SetValueAndTryNotify(value); }
				//}
				//#region Property ObservableCollection<ITreeItem<object,TState >> Children Setup
				//protected Property<ObservableCollection<ITreeItem<object, TState>>> _Children = new Property<ObservableCollection<ITreeItem<object, TState>>> { LocatorFunc = _ChildrenLocator };
				//static Func<BindableBase, ValueContainer<ObservableCollection<ITreeItem<object, TState>>>> _ChildrenLocator = RegisterContainerLocator<ObservableCollection<ITreeItem<object, TState>>>("Children", model => model.Initialize("Children", ref model._Children, ref _ChildrenLocator, _ChildrenDefaultValueFactory));
				//static Func<ObservableCollection<ITreeItem<object, TState>>> _ChildrenDefaultValueFactory = () => new ObservableCollection<ITreeItem<object, TState>>();
				//#endregion



				/// <summary>
				/// Gets or sets the children.
				/// </summary>
				/// <value>The children.</value>
				public Collection<ITreeItem<object, TState>> Children
				{
					get { return _ChildrenLocator(this).Value; }
					set { _ChildrenLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property Collection <ITreeItem<object, TState>> Children Setup
				/// <summary>
				/// The _ children
				/// </summary>
				protected Property<Collection<ITreeItem<object, TState>>> _Children = new Property<Collection<ITreeItem<object, TState>>> { LocatorFunc = _ChildrenLocator };
				/// <summary>
				/// The _ children locator
				/// </summary>
				static Func<BindableBase, ValueContainer<Collection<ITreeItem<object, TState>>>> _ChildrenLocator = RegisterContainerLocator<Collection<ITreeItem<object, TState>>>("Children", model => model.Initialize("Children", ref model._Children, ref _ChildrenLocator, _ChildrenDefaultValueFactory));
				/// <summary>
				/// The _ children default value factory
				/// </summary>
				static Func<Collection<ITreeItem<object, TState>>> _ChildrenDefaultValueFactory = () => new ObservableCollection<ITreeItem<object, TState>>();
				#endregion


				/// <summary>
				/// Gets the children.
				/// </summary>
				/// <value>The children.</value>
				ICollection<ITreeItem<object, TState>> ITreeItem<TNodeValue, TState>.Children
				{
					get { return Children; }
				}
			}




			/// <summary>
			/// Class TreeViewItemModel.
			/// </summary>
			/// <typeparam name="TValue">The type of the t value.</typeparam>
			public class TreeViewItemModel<TValue> : TreeItemModelBase<TValue, TreeViewItemState, TreeViewItemModel<TValue>>
			{

			}


			/// <summary>
			/// Class TreeItemModel.
			/// </summary>
			/// <typeparam name="TValue">The type of the t value.</typeparam>
			/// <typeparam name="TState">The type of the t state.</typeparam>
			public class TreeItemModel<TValue, TState> : TreeItemModelBase<TValue, TState, TreeItemModel<TValue, TState>>
			{

			}







			//[DataContract(IsReference=true) ] //if you want
			/// <summary>
			/// Class TreeViewItemState.
			/// </summary>
			public class TreeViewItemState : BindableBase<TreeViewItemState>
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="TreeViewItemState"/> class.
				/// </summary>
				public TreeViewItemState()
				{


				}

				/// <summary>
				/// Gets or sets a value indicating whether this instance is selected.
				/// </summary>
				/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
				public bool IsSelected
				{
					get { return _IsSelectedLocator(this).Value; }
					set { _IsSelectedLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property bool IsSelected Setup
				/// <summary>
				/// The _ is selected
				/// </summary>
				protected Property<bool> _IsSelected = new Property<bool> { LocatorFunc = _IsSelectedLocator };
				/// <summary>
				/// The _ is selected locator
				/// </summary>
				static Func<BindableBase, ValueContainer<bool>> _IsSelectedLocator = RegisterContainerLocator<bool>("IsSelected", model => model.Initialize("IsSelected", ref model._IsSelected, ref _IsSelectedLocator, _IsSelectedDefaultValueFactory));
				/// <summary>
				/// The _ is selected default value factory
				/// </summary>
				static Func<bool> _IsSelectedDefaultValueFactory = null;
				#endregion


				/// <summary>
				/// Gets or sets a value indicating whether this instance is checked.
				/// </summary>
				/// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
				public bool IsChecked
				{
					get { return _IsCheckedLocator(this).Value; }
					set { _IsCheckedLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property bool IsChecked Setup
				/// <summary>
				/// The _ is checked
				/// </summary>
				protected Property<bool> _IsChecked = new Property<bool> { LocatorFunc = _IsCheckedLocator };
				/// <summary>
				/// The _ is checked locator
				/// </summary>
				static Func<BindableBase, ValueContainer<bool>> _IsCheckedLocator = RegisterContainerLocator<bool>("IsChecked", model => model.Initialize("IsChecked", ref model._IsChecked, ref _IsCheckedLocator, _IsCheckedDefaultValueFactory));
				/// <summary>
				/// The _ is checked default value factory
				/// </summary>
				static Func<bool> _IsCheckedDefaultValueFactory = null;
				#endregion


				/// <summary>
				/// Gets or sets a value indicating whether this instance can be selected.
				/// </summary>
				/// <value><c>true</c> if this instance can be selected; otherwise, <c>false</c>.</value>
				public bool CanBeSelected
				{
					get { return _CanBeSelectedLocator(this).Value; }
					set { _CanBeSelectedLocator(this).SetValueAndTryNotify(value); }
				}
				#region Property bool CanBeSelected Setup
				/// <summary>
				/// The _ can be selected
				/// </summary>
				protected Property<bool> _CanBeSelected = new Property<bool> { LocatorFunc = _CanBeSelectedLocator };
				/// <summary>
				/// The _ can be selected locator
				/// </summary>
				static Func<BindableBase, ValueContainer<bool>> _CanBeSelectedLocator = RegisterContainerLocator<bool>("CanBeSelected", model => model.Initialize("CanBeSelected", ref model._CanBeSelected, ref _CanBeSelectedLocator, _CanBeSelectedDefaultValueFactory));
				/// <summary>
				/// The _ can be selected default value factory
				/// </summary>
				static Func<bool> _CanBeSelectedDefaultValueFactory = null;
				#endregion




			}

		}



		/// <summary>
		/// Class ElementBinderBase.
		/// </summary>
		/// <typeparam name="TSubType">The type of the t sub type.</typeparam>
		public class ElementBinderBase<TSubType> : DependencyObject, IDisposable where
		TSubType : ElementBinderBase<TSubType>
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ElementBinderBase{TSubType}"/> class.
			/// </summary>
			public ElementBinderBase() { }
			/// <summary>
			/// Initializes a new instance of the <see cref="ElementBinderBase{TSubType}"/> class.
			/// </summary>
			/// <param name="bindingAction">The binding action.</param>
			/// <param name="disposeAction">The dispose action.</param>
			public ElementBinderBase(Action<TSubType> bindingAction, Action<TSubType> disposeAction)
			{
				_bindingAction = bindingAction;
				_disposeAction = ts =>
				{
					disposeAction(ts);

				};
			}
			/// <summary>
			/// The _binding action
			/// </summary>
			protected Action<TSubType> _bindingAction;
			/// <summary>
			/// The _dispose action
			/// </summary>
			protected Action<TSubType> _disposeAction;




			/// <summary>
			/// The binder property changed callback
			/// </summary>
			protected static PropertyChangedCallback BinderPropertyChangedCallback = (o, e) =>
			{
				if (e.NewValue is TSubType)
				{
					var eb = e.NewValue as TSubType;
					eb.Element = o as FrameworkElement;

					if (eb._bindingAction != null)
					{
						try
						{
							eb._bindingAction(eb);
						}
						catch (Exception ex)
						{

							Debug.WriteLine(ex);
						}
						finally
						{
							eb._bindingAction = null;
						}


					}
				}

			};







			/// <summary>
			/// Gets or sets the element.
			/// </summary>
			/// <value>The element.</value>
			public FrameworkElement Element
			{
				get { return (FrameworkElement)GetValue(ElementProperty); }
				set { SetValue(ElementProperty, value); }
			}

			// Using a DependencyProperty as the backing store for Element.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The element property
			/// </summary>
			public static readonly DependencyProperty ElementProperty =
				DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(TSubType), new PropertyMetadata(null));








			#region IDisposable Members

			/// <summary>
			/// The _ disposed
			/// </summary>
			int _Disposed = 0;
			/// <summary>
			/// Finalizes an instance of the <see cref="ElementBinderBase{TSubType}"/> class.
			/// </summary>
			~ElementBinderBase()
			{
				Dispose(false);
			}
			/// <summary>
			/// Disposes this instance.
			/// </summary>
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			/// <summary>
			/// Releases unmanaged and - optionally - managed resources.
			/// </summary>
			/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
			virtual protected void Dispose(bool disposing)
			{
				if (disposing)
				{
					var v = Interlocked.Exchange(ref _Disposed, 1);
					if (v == 0)
					{
						try
						{
							if (_disposeAction != null)
							{
								_disposeAction(this as TSubType);

							}
						}
						catch (Exception ex)
						{
							Debug.WriteLine(ex);
						}
						finally
						{
							_disposeAction = null;

						}
						_disposeAction = null;
						_bindingAction = null;
					}
				}
			}



			#endregion


		}




	}


}
