// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
using System.Windows.Threading;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#endif






namespace MVVMSidekick
{

	namespace ViewModels
	{
		using EventRouting;
		using System.Reactive.Disposables;
		using Utilities;
		using Views;
		using MVVMSidekick.Common;


		/// <summary>
		/// <para>A ViewModel by default, with basic implement of name-value container.</para>
		/// <para>缺省的 ViewModel。可以用作最简单的字典绑定</para>
		/// </summary>
		public class ViewModel : ViewModelBase<ViewModel>
		{

		}

		/// <summary>
		/// <para>Base type of bindable model.</para>
		/// <para>ViewModel 基类</para>
		/// </summary>
		[DataContract]
		public abstract class BindableBase
			: DisposeGroupBase, INotifyPropertyChanged, IBindable
		{
			/// <summary>
			/// Releases unmanaged and - optionally - managed resources.
			/// </summary>
			/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}


			/// <summary>
			/// Occurs when [_ errors changed].
			/// </summary>
			protected event EventHandler<DataErrorsChangedEventArgs> _ErrorsChanged;
			/// <summary>
			/// Raises the errors changed.
			/// </summary>
			/// <param name="propertName">Name of the propert.</param>
			protected internal void RaiseErrorsChanged(string propertName)
			{
				if (_ErrorsChanged != null)
				{
					_ErrorsChanged(this, new DataErrorsChangedEventArgs(propertName));
				}
			}

			/// <summary>
			/// Gets the bindable instance identifier.
			/// </summary>
			/// <value>The bindable instance identifier.</value>
			abstract public String BindableInstanceId { get; }





			/// <summary>
			/// The _ is validation activated
			/// </summary>
			private bool _IsValidationActivated = false;
			/// <summary>
			/// <para>Gets ot sets if the validation is activatied. This is a flag only， internal logic is not depend on this.</para>
			/// <para>读取/设置 此模型是否激活验证。这只是一个标记，内部逻辑并没有参考这个值</para>
			/// </summary>
			/// <value><c>true</c> if this instance is validation activated; otherwise, <c>false</c>.</value>
			public bool IsValidationActivated
			{
				get { return _IsValidationActivated; }
				set { _IsValidationActivated = value; }
			}

			/// <summary>
			/// The _ is notification activated
			/// </summary>
			private bool _IsNotificationActivated = true;
			/// <summary>
			/// <para>Gets ot sets if the property change notification is activatied. </para>
			/// <para>读取/设置 此模型是否激活变化通知</para>
			/// </summary>
			/// <value><c>true</c> if this instance is notification activated; otherwise, <c>false</c>.</value>
			public bool IsNotificationActivated
			{
				get { return (!IsInDesignMode) ? _IsNotificationActivated : false; }
				set { _IsNotificationActivated = value; }
			}

			/// <summary>
			/// Gets a value indicating whether this instance is in design mode.
			/// </summary>
			/// <value><c>true</c> if this instance is in design mode; otherwise, <c>false</c>.</value>
			public static bool IsInDesignMode { get { return Utilities.Runtime.IsInDesignMode; } }






			///// <summary>
			/////  <para>0 for not disposed, 1 for disposed</para>
			/////  <para>0 表示没有被Dispose 1 反之</para>
			///// </summary>
			//private int disposedFlag = 0;

			#region  Index and property names/索引与字段名
			/// <summary>
			/// <para>Get all property names that were defined in subtype, or added objectly in runtime</para>
			/// <para>取得本VM实例已经定义的所有字段名。其中包括静态声明的和动态添加的。</para>
			/// </summary>
			/// <returns>String[]  Property names/字段名数组</returns>
			public abstract string[] GetFieldNames();

			///// <summary>
			///// <para>Gets or sets  poperty values by property name index.</para>
			///// <para>使用索引方式取得/设置字段值</para>
			///// </summary>
			///// <param name="name">Property name/字段名</param>
			///// <returns>Property value/字段值</returns>
			/// <summary>
			/// Gets or sets the <see cref="System.Object"/> with the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <returns>System.Object.</returns>
			public abstract object this[string name] { get; set; }


			#endregion



			#region Propery Changed Logic/ Propery Changed事件相关逻辑


			/// <summary>
			/// Raises the property changed.
			/// </summary>
			/// <param name="lazyEAFactory">The lazy ea factory.</param>
			protected internal void RaisePropertyChanged(Func<PropertyChangedEventArgs> lazyEAFactory)
			{


				if (this.PropertyChanged != null)
				{
					var ea = lazyEAFactory();
					this.PropertyChanged(this, ea);
				}


			}

			/// <summary>
			/// <para>Event that raised when properties were changed and Notification was activited</para>
			/// <para> VM属性任何绑定用值被修改后,在启用通知情况下触发此事件</para>
			/// </summary>
			public event PropertyChangedEventHandler PropertyChanged;


			#endregion

			#region 验证与错误相关逻辑






			/// <summary>
			/// Checks the error.
			/// </summary>
			/// <param name="test">The test.</param>
			/// <param name="errorMessage">The error message.</param>
			/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
			protected bool CheckError(Func<Boolean> test, string errorMessage)
			{

				var rval = test();
				if (rval)
				{
					SetErrorAndTryNotify(errorMessage);
				}
				return rval;

			}


			///// <summary>
			///// 验证错误内容
			///// </summary>
			//string IDataErrorInfo.Error
			//{
			//    get
			//    {
			//        return GetError();
			//    }


			//}
			/// <summary>
			/// <para>Gets the validate error of this model </para>
			/// <para>取得错误内容</para>
			/// </summary>
			/// <value>The error.</value>
			public abstract string Error { get; }
			/// <summary>
			/// <para>Sets the validate error of this model </para>
			/// <para>设置错误内容</para>
			/// </summary>
			/// <param name="value">The value.</param>
			/// <returns>Error string/错误内容字符串</returns>
			protected abstract void SetError(string value);

			/// <summary>
			/// <para>Sets the validate error of this model and notify </para>
			/// <para>设置错误内容并且尝试用事件通知</para>
			/// </summary>
			/// <param name="value">The value.</param>
			/// <returns>Error string/错误内容字符串</returns>
			protected abstract void SetErrorAndTryNotify(string value);



			/// <summary>
			/// <para>Gets validate error string of this field</para>
			/// <para>取得对于每个字段，验证失败所产生的错误信息</para>
			/// </summary>
			/// <param name="propertyName">Property Name of error /要检查错误的属性名</param>
			/// <returns>Rrror string /错误字符串</returns>
			protected abstract string GetColumnError(string propertyName);



			#endregion


			//   public abstract bool IsUIBusy { get; set; }









			/// <summary>
			/// Gets or sets the event router.
			/// </summary>
			/// <value>The event router.</value>
			public abstract EventRouter LocalEventRouter { get; set; }

		}

		/// <summary>
		/// <para>Extension methods of models</para>
		/// <para>为Model增加的一些快捷方法</para>
		/// </summary>
		public static class BindableBaseExtensions
		{



			/// <summary>
			/// <para>Config Value Container with delegate</para>
			/// <para>使用连续的API设置ValueContainer的一些参数</para>
			/// </summary>
			/// <typeparam name="TProperty">ValueContainer内容的类型</typeparam>
			/// <param name="target">ValueContainer的配置目标实例</param>
			/// <param name="action">配置内容</param>
			/// <returns>ValueContainer的配置目标实例</returns>
			public static ValueContainer<TProperty> Config<TProperty>(this ValueContainer<TProperty> target, Action<ValueContainer<TProperty>> action)
			{
				action(target);
				return target;
			}

			/// <summary>
			/// <para>Add Idisposeable to model's despose action list</para>
			/// <para>将IDisposable 对象注册到VM中的销毁对象列表。</para>
			/// </summary>
			/// <typeparam name="T">Type of Model /Model的类型</typeparam>
			/// <param name="item">IDisposable Inastance/IDisposable实例</param>
			/// <param name="targetGroup">The tg.</param>
			/// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
			/// <param name="comment">The comment.</param>
			/// <param name="caller">The caller.</param>
			/// <param name="file">The file.</param>
			/// <param name="line">The line.</param>
			/// <returns>T.</returns>
			public static T DisposeWith<T>(this T item, IDisposeGroup targetGroup, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
			{

				targetGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
				return item;


			}


			/// <summary>
			/// <para>注册在目标VM绑定的视图Unload的时候Dispose </para>
			/// <para>Register a dispose object that would dispose when target viewmodel's view unload</para>
			/// </summary>
			/// <typeparam name="T"><para>任意IDisposable对象类型</para><para>some IDisposable Type</para></typeparam>
			/// <param name="item"><para>注册的Disposeable对象</para><para>Disposable instance that would be registered</para></param>
			/// <param name="viewModel">注册到的View Model</param>
			/// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
			/// <param name="comment">The comment.</param>
			/// <param name="caller">The caller.</param>
			/// <param name="file">The file.</param>
			/// <param name="line">The line.</param>
			/// <returns>T.</returns>
			public static T DisposeWhenUnload<T>(this T item, IViewModelLifetime viewModel, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
			{

				viewModel.UnloadDisposeGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
				return item;


			}
			/// <summary>
			/// <para>注册在目标VM绑定的视图与VM解除绑定的的时候Dispose </para>
			/// <para>Register a dispose object that would dispose when target viewmodel's view unbind with the viewmodel</para>
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="item"></param>
			/// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
			/// <param name="comment">The comment.</param>
			/// <param name="caller">The caller.</param>
			/// <param name="file">The file.</param>
			/// <param name="line">The line.</param>
			/// <returns>T.</returns>
			public static T DisposeWhenUnbind<T>(this T item, IViewModelLifetime viewModel, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
			{

				viewModel.UnbindDisposeGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
				return item;


			}

			/// <summary>
			/// Initializes the specified property name.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="model">The model.</param>
			/// <param name="propertyName">Name of the property.</param>
			/// <param name="reference">The reference.</param>
			/// <param name="locator">The locator.</param>
			/// <param name="defaultValueFactory">The default value factory.</param>
			/// <returns>ValueContainer&lt;T&gt;.</returns>
			public static ValueContainer<T> Initialize<T>(this BindableBase model, string propertyName, ref Property<T> reference, ref Func<BindableBase, ValueContainer<T>> locator, Func<T> defaultValueFactory = null)
			{
				if (reference == null)
					reference = new Property<T> { LocatorFunc = locator };
				if (reference.Container == null)
				{
					reference.Container = new ValueContainer<T>(propertyName, model);
					if (defaultValueFactory != null)
					{
						reference.Container.Value = defaultValueFactory();
					}
				}
				return reference.Container;
			}

			/// <summary>
			/// Initializes the specified property name.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="model">The model.</param>
			/// <param name="propertyName">Name of the property.</param>
			/// <param name="reference">The reference.</param>
			/// <param name="locator">The locator.</param>
			/// <param name="defaultValueFactory">The default value factory.</param>
			/// <returns>ValueContainer&lt;T&gt;.</returns>
			public static ValueContainer<T> Initialize<T>(this BindableBase model, string propertyName, ref Property<T> reference, ref Func<BindableBase, ValueContainer<T>> locator, Func<BindableBase, T> defaultValueFactory = null)
			{
				return Initialize(model, propertyName, ref reference, ref locator, () => (defaultValueFactory != null) ? defaultValueFactory(model) : default(T));
			}
		}


		/// <summary>
		/// <para>A slot to place the value container field and value container locator.</para>
		/// <para>属性定义。一个属性定义包括一个创建/定位属性“值容器”的静态方法引用，和一个缓存该方法执行结果“值容器”的槽位</para>
		/// </summary>
		/// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
		public class Property<TProperty>
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Property{TProperty}"/> class.
			/// </summary>
			public Property()
			{

			}

			/// <summary>
			/// <para>Locate or create the value container of this model intances</para>
			/// <para>通过定位方法定位本Model实例中的值容器</para>
			/// </summary>
			/// <param name="model">Model intances/model 实例</param>
			/// <returns>Value Container of this property/值容器</returns>
			public ValueContainer<TProperty> LocateValueContainer(BindableBase model)
			{

				return LocatorFunc(model);
			}


			/// <summary>
			/// <para>Gets sets the factory to locate/create value container of this model instance</para>
			/// <para>读取/设置定位值容器用的方法。</para>
			/// </summary>
			/// <value>The locator function.</value>
			public Func<BindableBase, ValueContainer<TProperty>> LocatorFunc
			{
				internal get;
				set;
			}

			/// <summary>
			/// <para>Gets or sets Value Container, it can be recently create and cached here，by LocatorFunc </para>
			/// <para>读取/设置值容器,这事值容器LocatorFunc创建值容器并且缓存的位置 </para>
			/// </summary>
			/// <value>The container.</value>
			public ValueContainer<TProperty> Container
			{
				get;
				set;
			}


		}

		/// <summary>
		/// <para>Value Container, holds the value of certain field, with notifition /and compare support</para>
		/// <para>值容器</para>
		/// </summary>
		/// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
		public class ValueContainer<TProperty> : IErrorInfo, IValueCanSet<TProperty>, IValueCanGet<TProperty>, IValueContainer, INotifyChanges<TProperty>
		{


			#region Constructors /构造器
			/// <summary>
			/// <para>Create a new Value Container</para>
			/// <para>创建属性值容器</para>
			/// </summary>
			/// <param name="info">Property name/属性名</param>
			/// <param name="model"><para>The model that Value Container will be held with.</para>
			/// <para>所属的model实例</para></param>
			/// <param name="initValue">The first value of this container/初始值</param>
			public ValueContainer(string info, BindableBase model, TProperty initValue = default(TProperty))
				: this(info, model, (v1, v2) =>
					{
						if (v1 == null)
						{
							if (v2 == null)
							{
								return true;
							}
							else
							{
								return false;
							}
						}
						else if (v2 == null)
						{
							return false;
						}
						else
						{
							return v1.Equals(v2);
						}

					}, initValue)
			{
			}





			/// <summary>
			/// <para>Create a new Value Container</para>
			/// <para>创建属性值容器</para>
			/// </summary>
			/// <param name="info">Property name/属性名</param>
			/// <param name="model"><para>The model that Value Container will be held with.</para>
			/// <para>所属的model实例</para></param>
			/// <param name="equalityComparer"><para>Comparer of new/old value, for notifition.</para>
			/// <para>判断两个值是否相等的比较器,用于判断是否通知变更</para></param>
			/// <param name="initValue">The first value of this container/初始值</param>
			public ValueContainer(string info, BindableBase model, Func<TProperty, TProperty, bool> equalityComparer, TProperty initValue = default(TProperty))
			{
				EqualityComparer = equalityComparer;
				PropertyName = info;
				PropertyType = typeof(TProperty);
				Model = model;
				Value = initValue;
				_Errors = new ObservableCollection<ErrorEntity>();
				_Errors.GetEventObservable(model)
					.Subscribe
					(
						e =>
						{
							model.RaiseErrorsChanged(PropertyName);
						}
					)
					.DisposeWith(model);

			}

			#endregion

			/// <summary>
			/// <para>Event that raised when value was changed</para>
			/// <para>值变更时触发的事件</para>
			/// </summary>
			public event EventHandler<ValueChangedEventArgs<TProperty>> ValueChanged;

			/// <summary>
			/// <para>Gets comparer instance of new/old value, for notifition.</para>
			/// <para>读取判断两个值是否相等的比较器,用于判断是否通知变更</para>
			/// </summary>
			/// <value>The equality comparer.</value>
			public Func<TProperty, TProperty, bool> EqualityComparer { get; private set; }

			/// <summary>
			/// Property name /属性名
			/// </summary>
			/// <value>The name of the property.</value>
			public string PropertyName { get; private set; }

			/// <summary>
			/// The _value
			/// </summary>
			TProperty _value;

			/// <summary>
			/// Value/值
			/// </summary>
			/// <value>The value.</value>
			public TProperty Value
			{
				get { return _value; }
				set { SetValueAndTryNotify(value); }
			}

			/// <summary>
			/// <para>Save the value and try raise the value changed event</para>
			/// <para>保存值并且尝试触发更改事件</para>
			/// </summary>
			/// <param name="value">New value/属性值</param>
			/// <returns>ValueContainer&lt;TProperty&gt;.</returns>
			public ValueContainer<TProperty> SetValueAndTryNotify(TProperty value)
			{
				InternalPropertyChange(this.Model, value, ref _value, PropertyName);
				return this;
			}

			/// <summary>
			/// <para>Save the value and do not try raise the value changed event</para>
			/// <para>仅保存值 不尝试触发更改事件</para>
			/// </summary>
			/// <param name="value">New value/属性值</param>
			/// <returns>ValueContainer&lt;TProperty&gt;.</returns>
			public ValueContainer<TProperty> SetValue(TProperty value)
			{
				_value = value;
				return this;
			}


			/// <summary>
			/// Internals the property change.
			/// </summary>
			/// <param name="objectInstance">The object instance.</param>
			/// <param name="newValue">The new value.</param>
			/// <param name="currentValue">The current value.</param>
			/// <param name="message">The message.</param>
			private void InternalPropertyChange(BindableBase objectInstance, TProperty newValue, ref TProperty currentValue, string message)
			{
				var changing = (this.EqualityComparer != null) ?
					!this.EqualityComparer(newValue, currentValue) :
					!Object.Equals(newValue, currentValue);


				if (changing)
				{
					var oldvalue = currentValue;
					currentValue = newValue;

					ValueChangedEventArgs<TProperty> arg = null;

					Func<PropertyChangedEventArgs> lzf =
						() =>
						{

							arg = arg ?? new ValueChangedEventArgs<TProperty>(message, oldvalue, newValue);
							return arg;
						};


					objectInstance.RaisePropertyChanged(lzf);

					if (ValueChanged != null) ValueChanged(this, lzf() as ValueChangedEventArgs<TProperty>);
					if (ValueChangedWithNameOnly != null) ValueChangedWithNameOnly(this, new PropertyChangedEventArgs(message));
					if (ValueChangedWithNothing != null) ValueChangedWithNothing(this, EventArgs.Empty);
				}
			}


			/// <summary>
			/// <para>The model instance that Value Container was held.</para>
			/// <para>此值容器所在的Model</para>
			/// </summary>
			/// <value>The model.</value>
			public BindableBase Model { get; internal set; }





			/// <summary>
			/// Gets or sets the value.
			/// </summary>
			/// <value>The value.</value>
			object IValueContainer.Value
			{
				get
				{
					return Value;
				}
				set
				{
					SetValueAndTryNotify((TProperty)value);
				}
			}


			/// <summary>
			/// Gets the type of property/读取值类型
			/// </summary>
			/// <value>The type of the property.</value>
			public Type PropertyType
			{
				get;
				private set;
			}


			/// <summary>
			/// The _ errors
			/// </summary>
			ObservableCollection<ErrorEntity> _Errors;

			/// <summary>
			/// Gets the errors.
			/// </summary>
			/// <value>The errors.</value>
			public ObservableCollection<ErrorEntity> Errors
			{
				get { return _Errors; }

			}



#if NETFX_CORE
			bool _IsCopyToAllowed = !typeof(ICommand).GetTypeInfo().IsAssignableFrom(typeof(TProperty).GetTypeInfo());
#else
            /// <summary>
            /// The _ is copy to allowed
            /// </summary>
            bool _IsCopyToAllowed = !typeof(ICommand).IsAssignableFrom(typeof(TProperty));
#endif
			/// <summary>
			/// <para>Can be copied by CopyTo method</para>
			/// <para>是否可以被 `Copyto` 复制到另外一个属性</para>
			/// </summary>
			/// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
			public bool IsCopyToAllowed
			{
				get { return _IsCopyToAllowed; }
				set { _IsCopyToAllowed = value; }
			}



			/// <summary>
			/// Occurs when [value changed with name only].
			/// </summary>
			public event PropertyChangedEventHandler ValueChangedWithNameOnly;

			/// <summary>
			/// Occurs when [value changed with nothing].
			/// </summary>
			public event EventHandler ValueChangedWithNothing;
		}


		/// <summary>
		/// <para>Event args that fired when property changed, with old value and new value field.</para>
		/// <para>值变化事件参数</para>
		/// </summary>
		/// <typeparam name="TProperty">Type of propery/变化属性的类型</typeparam>
		public class ValueChangedEventArgs<TProperty> : PropertyChangedEventArgs
		{
			/// <summary>
			/// Constructor of ValueChangedEventArgs
			/// </summary>
			/// <param name="propertyName">Name of the property.</param>
			/// <param name="oldValue">The old value.</param>
			/// <param name="newValue">The new value.</param>
			public ValueChangedEventArgs(string propertyName, TProperty oldValue, TProperty newValue)
				: base(propertyName)
			{
				NewValue = newValue;
				OldValue = oldValue;
			}

			/// <summary>
			/// New Value
			/// </summary>
			/// <value>The new value.</value>
			public TProperty NewValue { get; private set; }
			/// <summary>
			/// Old Value
			/// </summary>
			/// <value>The old value.</value>
			public TProperty OldValue { get; private set; }
		}


		///// <summary>
		///// <para>A Bindebale Tuple</para>
		///// <para>一个可绑定的Tuple实现</para>
		///// </summary>
		///// <typeparam name="TItem1">Type of first item/第一个元素的类型</typeparam>
		///// <typeparam name="TItem2">Type of second item/第二个元素的类型</typeparam>
		//[DataContract]
		//public class BindableTuple<TItem1, TItem2> : BindableBase<BindableTuple<TItem1, TItem2>>
		//{
		//	/// <summary>
		//	/// Initializes a new instance of the <see cref="BindableTuple{TItem1, TItem2}"/> class.
		//	/// </summary>
		//	/// <param name="item1">The item1.</param>
		//	/// <param name="item2">The item2.</param>
		//	public BindableTuple(TItem1 item1, TItem2 item2)
		//	{
		//		this.IsNotificationActivated = false;
		//		Item1 = item1;
		//		Item2 = item2;
		//		this.IsNotificationActivated = true;
		//	}
		//	/// <summary>
		//	/// 第一个元素
		//	/// </summary>
		//	/// <value>The item1.</value>

		//	public TItem1 Item1
		//	{
		//		get { return _Item1Locator(this).Value; }
		//		set { _Item1Locator(this).SetValueAndTryNotify(value); }
		//	}
		//	#region Property TItem1 Item1 Setup
		//	/// <summary>
		//	/// The _ item1
		//	/// </summary>
		//	protected Property<TItem1> _Item1 = new Property<TItem1> { LocatorFunc = _Item1Locator };
		//	/// <summary>
		//	/// The _ item1 locator
		//	/// </summary>
		//	static Func<BindableBase, ValueContainer<TItem1>> _Item1Locator = RegisterContainerLocator<TItem1>("Item1", model => model.Initialize("Item1", ref model._Item1, ref _Item1Locator, _Item1DefaultValueFactory));
		//	/// <summary>
		//	/// The _ item1 default value factory
		//	/// </summary>
		//	static Func<BindableBase, TItem1> _Item1DefaultValueFactory = null;
		//	#endregion

		//	/// <summary>
		//	/// 第二个元素
		//	/// </summary>
		//	/// <value>The item2.</value>

		//	public TItem2 Item2
		//	{
		//		get { return _Item2Locator(this).Value; }
		//		set { _Item2Locator(this).SetValueAndTryNotify(value); }
		//	}
		//	#region Property TItem2 Item2 Setup
		//	/// <summary>
		//	/// The _ item2
		//	/// </summary>
		//	protected Property<TItem2> _Item2 = new Property<TItem2> { LocatorFunc = _Item2Locator };
		//	/// <summary>
		//	/// The _ item2 locator
		//	/// </summary>
		//	static Func<BindableBase, ValueContainer<TItem2>> _Item2Locator = RegisterContainerLocator<TItem2>("Item2", model => model.Initialize("Item2", ref model._Item2, ref _Item2Locator, _Item2DefaultValueFactory));
		//	/// <summary>
		//	/// The _ item2 default value factory
		//	/// </summary>
		//	static Func<BindableBase, TItem2> _Item2DefaultValueFactory = null;
		//	#endregion


		//}
		///// <summary>
		///// <para>Fast create Bindable Tuple </para>
		///// <para>帮助快速创建BindableTuple的帮助类</para>
		///// </summary>
		//public static class BindableTuple
		//{
		//	/// <summary>
		//	/// Create a Tuple
		//	/// </summary>
		//	/// <typeparam name="TItem1">The type of the item1.</typeparam>
		//	/// <typeparam name="TItem2">The type of the item2.</typeparam>
		//	/// <param name="item1">The item1.</param>
		//	/// <param name="item2">The item2.</param>
		//	/// <returns>
		//	/// BindableTuple&lt;TItem1, TItem2&gt;.
		//	/// </returns>

		//	public static BindableTuple<TItem1, TItem2> Create<TItem1, TItem2>(TItem1 item1, TItem2 item2)
		//	{
		//		return new BindableTuple<TItem1, TItem2>(item1, item2);
		//	}

		//}


		/// <summary>
		/// <para>Model type with detail subtype type parameter.</para>
		/// <para>具有子类详细类型定义的model </para>
		/// <example>
		/// public class Class1:BindableBase&lt;Class1&gt;  {}
		/// </example>
		/// </summary>
		/// <typeparam name="TSubClassType">Sub Type / 子类类型</typeparam>
		[DataContract]
		public abstract class BindableBase<TSubClassType> : BindableBase, INotifyDataErrorInfo where TSubClassType : BindableBase<TSubClassType>
		{
			protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
				_plainPropertyContainerGetters =
				  new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);

			static BindableBase()
			{

			}

			/// <summary>
			/// The _plain property container getters
			/// </summary>




			/// <summary>
			/// Initializes a new instance of the <see cref="BindableBase{TSubClassType}"/> class.
			/// </summary>
			public BindableBase()
			{

				//_BindableInstanceIdLocator(this).SetValueAndTryNotify( string.Format("{0}:{1}", this.GetType().Name, base._instanceIdOfThisType));
			}


			/// <summary>
			/// Gets the bindable instance identifier.
			/// </summary>
			/// <value>The bindable instance identifier.</value>

			public override string BindableInstanceId
			{
				get { return _instanceIdOfThisType.ToString(); }

			}






			/// <summary>
			/// 清除值
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="property">The property.</param>
			public void ResetPropertyValue<T>(Property<T> property)
			{
				if (property != null)
				{
					var oldContainer = property.Container;
					if (oldContainer != null)
					{


						property.Container = null;
						property.LocatorFunc(oldContainer.Model);
						oldContainer.SetValueAndTryNotify(property.Container.Value);
						property.Container = oldContainer;
					}
				}


			}








			/// <summary>
			/// <para>Cast a model instance to current model subtype</para>
			/// <para>将一个 model 引用特化为本子类型的引用</para>
			/// </summary>
			/// <param name="model">some bindable model/某种可绑定model</param>
			/// <returns>Current sub type instance/本类型引用</returns>
			public static TSubClassType CastToCurrentType(BindableBase model)
			{
				return (TSubClassType)model;

			}
			///// <summary>
			///// <para>Type cache of container getter</para>
			///// <para>每个属性类型独占的一个专门的类型缓存。</para>
			///// </summary>
			///// <typeparam name="TProperty"></typeparam>
			//protected static class TypeDic<TProperty>
			//{
			//	public static Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>> _propertyContainerGetters = new Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>>();

			//}

			/// <summary>
			/// 根据索引获取属性值
			/// </summary>
			/// <param name="colName">Name of the col.</param>
			/// <returns>属性值</returns>
			public override object this[string colName]
			{
				get
				{
					var lc = GetOrCreatePlainLocator(colName, this);
					return lc((TSubClassType)this).Value;
				}
				set
				{

					var lc = GetOrCreatePlainLocator(colName, this);
					lc((TSubClassType)this).Value = value;
				}
			}

			/// <summary>
			/// Gets the or create plain locator.
			/// </summary>
			/// <param name="colName">Name of the col.</param>
			/// <param name="viewModel">The view model.</param>
			/// <returns>Func&lt;TSubClassType, IValueContainer&gt;.</returns>
			private static Func<TSubClassType, IValueContainer> GetOrCreatePlainLocator(string colName, BindableBase viewModel)
			{
				Func<TSubClassType, IValueContainer> pf;
				if (!_plainPropertyContainerGetters.TryGetValue(colName, out pf))
				{
					var p = new ValueContainer<object>(colName, viewModel);

					Func<TSubClassType, ValueContainer<object>> tpf = _ => p;
					pf = tpf;
					_plainPropertyContainerGetters[colName] = pf;
					//TypeDic<object>._propertyContainerGetters[colName] = tpf;
				}
				return pf;
			}




			//#if SILVERLIGHT_5 || WINDOWS_PHONE_8 || WINDOWS_PHONE_7
			//			/// <summary>
			//			/// The _plain property container getters
			//			/// </summary>
			//			protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
			//			 _plainPropertyContainerGetters =
			//			 new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);
			//#else
			//			/// <summary>
			//			/// The _plain property container getters
			//			/// </summary>
			//			protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
			//				_plainPropertyContainerGetters =
			//				new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);
			//#endif



			/// <summary>
			/// Gets the error.
			/// </summary>
			/// <value>The error.</value>
			public override string Error
			{
				get { return _ErrorLocator(this).Value; }
			}

			/// <summary>
			/// Sets the error.
			/// </summary>
			/// <param name="value">The value.</param>
			protected override void SetError(string value)
			{
				_ErrorLocator(this).SetValue(value);
			}

			/// <summary>
			/// Sets the error and try notify.
			/// </summary>
			/// <param name="value">The value.</param>
			protected override void SetErrorAndTryNotify(string value)
			{
				_ErrorLocator(this).SetValueAndTryNotify(value);
			}


			#region Property string Error Setup

			/// <summary>
			/// The _ error
			/// </summary>
			protected Property<string> _Error =
			  new Property<string> { LocatorFunc = _ErrorLocator };
			/// <summary>
			/// The _ error locator
			/// </summary>
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			static Func<BindableBase, ValueContainer<string>> _ErrorLocator =
				RegisterContainerLocator<string>(
				"Error",
				model =>
				{
					model._Error =
						model._Error
						??
						new Property<string> { LocatorFunc = _ErrorLocator };
					return model._Error.Container =
						model._Error.Container
						??
						new ValueContainer<string>("Error", model);
				});

			#endregion






			/// <summary>
			/// 注册一个属性容器的定位器。
			/// </summary>
			/// <typeparam name="TProperty">The type of the property.</typeparam>
			/// <param name="propertyName">属性名</param>
			/// <param name="getOrCreateLocatorMethod">属性定位/创建方法 也就是定位器</param>
			/// <returns>
			/// 注册后的定位器
			/// </returns>
			protected static Func<BindableBase, ValueContainer<TProperty>> RegisterContainerLocator<TProperty>(string propertyName, Func<TSubClassType, ValueContainer<TProperty>> getOrCreateLocatorMethod)
			{

				//TypeDic<TProperty>._propertyContainerGetters[propertyName] = getOrCreateLocatorMethod;
				_plainPropertyContainerGetters[propertyName] = getOrCreateLocatorMethod;
				return o => getOrCreateLocatorMethod((TSubClassType)o);


			}


			/// <summary>
			/// 根据属性名取得一个值容器
			/// </summary>
			/// <typeparam name="TProperty">The type of the property.</typeparam>
			/// <param name="propertyName">属性名</param>
			/// <returns>值容器</returns>
			/// <exception cref="System.Exception">
			/// Property Not Exists!
			/// or
			/// Property ' + propertyName + ' is found but it does not match the property type ' + type of(TProperty).Name + '!
			/// </exception>
			public ValueContainer<TProperty> GetValueContainer<TProperty>(string propertyName)
			{
				Func<TSubClassType, ValueContainer<TProperty>> containerGetterCreater;
				Func<TSubClassType, IValueContainer> contPlanGetter;
				if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contPlanGetter))
				{
					throw new Exception("Property Not Exists!");

				}

				containerGetterCreater = contPlanGetter as Func<TSubClassType, ValueContainer<TProperty>>;
				if (containerGetterCreater == null)
				{
					throw new Exception("Property '" + propertyName + "' is found but it does not match the property type '" + typeof(TProperty).Name + "'!");
				}

				return containerGetterCreater((TSubClassType)(Object)this);

			}

			/// <summary>
			/// 根据表达式树取得一个值容器
			/// </summary>
			/// <typeparam name="TProperty">The type of the property.</typeparam>
			/// <param name="expression">表达式树</param>
			/// <returns>
			/// 值容器
			/// </returns>
			public ValueContainer<TProperty> GetValueContainer<TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
			{
				var propName = MVVMSidekick.Utilities.ExpressionHelper.GetPropertyName<TSubClassType, TProperty>(expression);
				return GetValueContainer<TProperty>(propName);

			}




			/// <summary>
			/// 根据属性名取得一个值容器
			/// </summary>
			/// <param name="propertyName">属性名</param>
			/// <returns>值容器</returns>
			/// <exception cref="System.NotImplementedException"></exception>
			public IValueContainer GetValueContainer(string propertyName)
			{
				Func<TSubClassType, IValueContainer> contianerGetterCreater;
				if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
				{
					this[propertyName] = null;
					if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
					{
						throw new NotImplementedException(string.Format("Current property \"{0}\" is not implemented", propertyName));
					}
				}
				return contianerGetterCreater((TSubClassType)(Object)this);

			}


			/// <summary>
			/// 根据属性名取得多个值容器
			/// </summary>
			/// <param name="propertyNames">The property names.</param>
			/// <returns>值容器</returns>
			public IValueContainer[] GetValueContainers(params string[] propertyNames)
			{
				return propertyNames.Select(pn => GetValueContainer(pn)).ToArray();

			}

			/// <summary>
			/// 根据表达式树取得多个值容器
			/// </summary>
			/// <param name="expressions">The expressions.</param>
			/// <returns>值容器</returns>
			public IValueContainer[] GetValueContainers(params Expression<Func<TSubClassType, object>>[] expressions)
			{

				var names = expressions.Select(expression =>
					  MVVMSidekick
						.Utilities
						.ExpressionHelper
						.GetPropertyName<TSubClassType>(expression)
					).ToArray();

				return GetValueContainers(names);
			}




			/// <summary>
			/// 获取某一属性的验证错误信息
			/// </summary>
			/// <param name="propertyName">属性名</param>
			/// <returns>错误信息字符串</returns>
			protected override string GetColumnError(string propertyName)
			{
				if (_plainPropertyContainerGetters[propertyName]((TSubClassType)this).Errors.Count > 0)
				{


					var error = string.Join(",", _plainPropertyContainerGetters[propertyName]((TSubClassType)this).Errors.Select(x => x.Message));
					var propertyContainer = this.GetValueContainer(propertyName);
#if NETFX_CORE
					if (propertyContainer != null && typeof(INotifyDataErrorInfo).GetTypeInfo().IsAssignableFrom(propertyContainer.PropertyType.GetTypeInfo()))
#else

                    if (propertyContainer != null && typeof(INotifyDataErrorInfo).IsAssignableFrom(propertyContainer.PropertyType))
#endif
					{
						INotifyDataErrorInfo di = this[propertyName] as INotifyDataErrorInfo;
						if (di != null)
						{
							error = error + "\r\n-----Inner " + propertyName + " as INotifyDataErrorInfo -------\r\n\t" + di.HasErrors.ToString();
						}
					}

					return error;
				}
				return null;
			}



			/// <summary>
			/// 获取所有属性名，包括静态声明和动态添加的
			/// </summary>
			/// <returns>System.String[].</returns>
			public override string[] GetFieldNames()
			{
				return _plainPropertyContainerGetters.Keys.ToArray();
			}


			/// <summary>
			/// 创建一个VM副本
			/// </summary>
			/// <returns>新引用</returns>
			public TSubClassType Clone()
			{
				var x = (TSubClassType)Activator.CreateInstance(typeof(TSubClassType));
				CopyTo(x);
				return x;
			}

			/// <summary>
			/// Copyrefs the specified source.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="source">The source.</param>
			/// <param name="target">The target.</param>
			static void Copyref<T>(T source, ref T target)
			{


				if (source == null)
				{
					target = source;
					return;
				}

				var sourcetype = source.GetType().GetTypeOrTypeInfo();
				if (sourcetype.IsValueType || source is string)
				{
					target = source;
				}
#if !(SILVERLIGHT_5 || WINDOWS_PHONE_8 || WINDOWS_PHONE_7 || NETFX_CORE)

				else if (typeof(ICloneable).IsAssignableFrom(sourcetype))
				{
					target = (T)((ICloneable)source).Clone();
				}
#endif
				else if (typeof(System.Collections.IList).GetTypeOrTypeInfo().IsAssignableFrom(sourcetype))
				{
					var tarcol = target as System.Collections.IList;
					var scol = source as System.Collections.IList;
					if (tarcol == null)
					{

						var newcol = sourcetype.IsArray ?
							Array.CreateInstance(sourcetype.GetElementType(), scol.Count) :
							System.Activator.CreateInstance(source.GetType(), new object[0]) as System.Collections.IList;


						tarcol = (System.Collections.IList)newcol;
					}
					else
					{
						tarcol.Clear();
					}
					if (tarcol != null)
					{


						foreach (var item in scol)
						{
							object newv = null;
							Copyref(item, ref newv);
							tarcol.Add(newv);
						}
						target = (T)tarcol;
					}
					else
					{
						target = default(T);
					}
				}
			}

			/// <summary>
			/// Copies to.
			/// </summary>
			/// <param name="target">The target.</param>
			public void CopyTo(TSubClassType target)
			{
				foreach (var item in GetFieldNames())
				{
					var ctThis = GetValueContainer(item);
					var ctTarget = target.GetValueContainer(item);
					if (ctThis.IsCopyToAllowed)
					{
						object temp = null;
						Copyref(this[item], ref temp);
						target[item] = temp;
					}


				}
			}


			/// <summary>
			/// Occurs when [errors changed].
			/// </summary>
			public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
			{
				add { _ErrorsChanged += value; }
				remove { _ErrorsChanged -= value; }
			}



			/// <summary>
			/// Gets the errors.
			/// </summary>
			/// <param name="propertyName">Name of the property.</param>
			/// <returns>System.Collections.IEnumerable.</returns>
			public System.Collections.IEnumerable GetErrors(string propertyName)
			{
				if (this.GetFieldNames().Contains(propertyName))
				{
					return this.GetValueContainer(propertyName).Errors;
				}
				else
				{
					return null;
				}

			}


			/// <summary>
			/// Gets a value indicating whether this instance has errors.
			/// </summary>
			/// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
			public bool HasErrors
			{
				get
				{
					//  return false;
					RefreshErrors();
					return !string.IsNullOrEmpty(this.Error);

				}
			}

			/// <summary>
			/// Refreshes the errors.
			/// </summary>
			private void RefreshErrors()
			{
				var sb = new StringBuilder();
				var rt = GetAllErrors().Select(x =>
				{
					return sb.Append(x.Message).Append(":").AppendLine(x.Exception == null ? " " : x.Exception.ToString());
				}
					)
					.ToArray();
				this.SetErrorAndTryNotify(sb.ToString());


			}

			/// <summary>
			/// Gets all errors.
			/// </summary>
			/// <returns>ErrorEntity[].</returns>
			public ErrorEntity[] GetAllErrors()
			{
				var errors = GetFieldNames()
					 .SelectMany(name => this.GetValueContainer(name).Errors)
					 .Where(x => x != null)
					 .ToArray();
				return errors;
			}

			//public override IDictionary<string,object >  Values
			//{
			//    get { return new BindableAccesser<TSubClassType>(this); }
			//}
			/// <summary>
			/// 给这个模型分配的消息路由引用（延迟加载）
			/// </summary>
			/// <value>The event router.</value>


			public override EventRouter LocalEventRouter
			{
				get { return _LocalEventRouterLocator(this).Value; }
				set { _LocalEventRouterLocator(this).SetValueAndTryNotify(value); }
			}
			#region Property EventRouter LocalEventRouter Setup
			protected Property<EventRouter> _LocalEventRouter = new Property<EventRouter> { LocatorFunc = _LocalEventRouterLocator };
			static Func<BindableBase, ValueContainer<EventRouter>> _LocalEventRouterLocator = RegisterContainerLocator<EventRouter>("LocalEventRouter", model => model.Initialize("LocalEventRouter", ref model._LocalEventRouter, ref _LocalEventRouterLocator, _LocalEventRouterDefaultValueFactory));
			static Func<EventRouter> _LocalEventRouterDefaultValueFactory = () => { return new EventRouter(); };
			#endregion


		}







		/// <summary>
		/// Class DisposeGroupBase.
		/// </summary>
		[DataContract]
		public abstract class DisposeGroupBase : InstanceCounableBase, IDisposeGroup
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DisposeGroupBase"/> class.
			/// </summary>
			public DisposeGroupBase()
			{
				CreateDisposeList();

			}

			/// <summary>
			/// Creates the dispose list.
			/// </summary>
			private void CreateDisposeList()
			{
				_disposeInfoList = new Lazy<List<DisposeEntry>>(() => new List<DisposeEntry>(), true);

			}

			/// <summary>
			/// Called when [deserializing].
			/// </summary>
			/// <param name="context">The context.</param>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2238:ImplementSerializationMethodsCorrectly"), OnDeserializing]
			public void OnDeserializing(System.Runtime.Serialization.StreamingContext context)
			{
				OnDeserializingActions();
			}

			/// <summary>
			/// Called when [deserializing actions].
			/// </summary>
			protected virtual void OnDeserializingActions()
			{

				CreateDisposeList();
			}


			#region Disposing Logic/Disposing相关逻辑
			/// <summary>
			/// Finalizes an instance of the <see cref="DisposeGroupBase"/> class.
			/// </summary>
			~DisposeGroupBase()
			{
				Dispose(false);
			}



			/// <summary>
			/// <para>Logic actions need to be executed when the instance is disposing</para>
			/// <para>销毁对象时 需要执行的操作</para>
			/// </summary>
			private Lazy<List<DisposeEntry>> _disposeInfoList;

			/// <summary>
			/// Gets the dispose information list.
			/// </summary>
			/// <value>The dispose information list.</value>
			public IList<DisposeEntry> DisposeInfoList { get { return _disposeInfoList.Value; } }

			//protected static Func<DisposeGroupBase, List<DisposeInfo>> _locateDisposeInfos =
			//    m =>
			//    {
			//        if (m._disposeInfoList == null)
			//        {
			//            Interlocked.CompareExchange(ref m._disposeInfoList, new List<DisposeInfo>(), null);

			//        }
			//        return m._disposeInfoList;

			//    };

			/// <summary>
			/// <para>Register logic actions need to be executed when the instance is disposing</para>
			/// <para>注册一个销毁对象时需要执行的操作</para>
			/// </summary>
			/// <param name="newAction">Disposing action/销毁操作</param>
			/// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
			/// <param name="comment">The comment.</param>
			/// <param name="caller">The caller.</param>
			/// <param name="file">The file.</param>
			/// <param name="line">The line.</param>
			public void AddDisposeAction(Action newAction, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
			{

				var di = new DisposeEntry
				{
					CallingCodeContext = CallingCodeContext.Create(comment, caller, file, line),
					Action = newAction,
					IsNeedCheckOnFinalizer = needCheckInFinalizer

				};
				_disposeInfoList.Value.Add(di);

			}


			/// <summary>
			/// <para>Register an object that need to be disposed when the instance is disposing</para>
			/// <para>销毁对象时 需要一起销毁的对象</para>
			/// </summary>
			/// <param name="item">disposable object/需要一起销毁的对象</param>
			/// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
			/// <param name="comment">The comment.</param>
			/// <param name="caller">The caller.</param>
			/// <param name="file">The file.</param>
			/// <param name="line">The line.</param>
			public void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
			{
				AddDisposeAction(() => item.Dispose(), needCheckInFinalizer, comment, caller, file, line);
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
			/// <para>Do all the dispose </para>
			/// <para>销毁，尝试运行所有注册的销毁操作</para>
			/// </summary>
			/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
			protected virtual void Dispose(bool disposing)
			{
				var disposeList = Interlocked.Exchange(ref _disposeInfoList, new Lazy<List<DisposeEntry>>(() => new List<DisposeEntry>(), true));
				if (disposeList != null && disposeList.IsValueCreated)
				{
					var l = disposeList.Value
						.Select
						(
							info =>
							{
								var ea = DisposeEventArgs.Create(info);
								//Exception gotex = null;
								try
								{
									if (DisposingEntry != null)
									{
										DisposingEntry(this, ea);
									}
									if (disposing || info.IsNeedCheckOnFinalizer)
									{
										info.Action();
									}


								}
								catch (Exception ex)
								{
									info.Exception = ex;

								}
								finally
								{
									if (DisposedEntry != null)
									{
										DisposedEntry(this, ea);
									}
								}

								return info;
							}

						)
						.Where(x => x.Exception != null)
						.ToArray();
					if (l.Length > 0)
					{
						OnDisposeExceptions(l);
					}
				}

				

			}




			/// <summary>
			/// <para>If dispose actions got exceptions, will handled here. </para>
			/// <para>处理Dispose 时产生的Exception</para>
			/// </summary>
			/// <param name="disposeInfoWithExceptions"><para>The exception and dispose infomation</para>
			/// <para>需要处理的异常信息</para></param>

			protected virtual void OnDisposeExceptions(IList<DisposeEntry> disposeInfoWithExceptions)
			{

			}


			#endregion


			/// <summary>
			/// Occurs when [disposing entry].
			/// </summary>
			public event EventHandler<DisposeEventArgs> DisposingEntry;

			/// <summary>
			/// Occurs when [disposed entry].
			/// </summary>
			public event EventHandler<DisposeEventArgs> DisposedEntry;
		}


		/// <summary>
		/// 默认的实现
		/// </summary>
		public class DisposeGroup : DisposeGroupBase
		{

		}

		/// <summary>
		/// Interface IBindable
		/// </summary>
		public interface IBindable : INotifyPropertyChanged, IDisposable, IDisposeGroup
		{

			/// <summary>
			/// Gets or sets the event router.
			/// </summary>
			/// <value>The event router.</value>
			EventRouter LocalEventRouter { get; set; }
			/// <summary>
			/// Gets the bindable instance identifier.
			/// </summary>
			/// <value>The bindable instance identifier.</value>
			string BindableInstanceId { get; }

			/// <summary>
			/// Gets the error.
			/// </summary>
			/// <value>The error.</value>
			string Error { get; }

			//IDictionary<string,object >  Values { get; }
			/// <summary>
			/// Gets the field names.
			/// </summary>
			/// <returns>System.String[].</returns>
			string[] GetFieldNames();
			/// <summary>
			/// Gets or sets the <see cref="System.Object"/> with the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <returns>System.Object.</returns>
			object this[string name] { get; set; }
		}


		//#if !NETFX_CORE

		//        public class StringToViewModelInstanceConverter : TypeConverter
		//        {
		//            public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
		//            {

		//                //if (sourceType == typeof(string))
		//                    return true;
		//                //return base.CanConvertFrom(context, sourceType);
		//            }
		//            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		//            {
		//                return true;
		//            }

		//            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		//            {

		//                var str = value.ToString();
		//                var t = Type.GetType(str);
		//                var v = Activator.CreateInstance(t);
		//                return v;
		//                ////  return base.ConvertFrom(context, culture, value);
		//            }
		//            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		//            {
		//                return value.ToString();
		//            }
		//        }

		//        [TypeConverter(typeof(StringToViewModelInstanceConverter))]
		//#endif

		/// <summary>
		/// Interface IViewModelLifetime
		/// </summary>
		public interface IViewModelLifetime : IDisposeGroup
		{
			/// <summary>
			/// Called when [binded to view].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <param name="oldValue">The old value.</param>
			/// <returns>Task.</returns>
			Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue);
			/// <summary>
			/// Called when [unbinded from view].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <param name="newValue">The new value.</param>
			/// <returns>Task.</returns>
			Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue);
			/// <summary>
			/// Called when [binded view load].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <returns>Task.</returns>
			Task OnBindedViewLoad(MVVMSidekick.Views.IView view);
			/// <summary>
			/// Called when [binded view unload].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <returns>Task.</returns>
			Task OnBindedViewUnload(MVVMSidekick.Views.IView view);


			/// <summary>
			/// the group that would dispose when Unbind
			/// </summary>
			IDisposeGroup UnbindDisposeGroup { get; }
			/// <summary>
			///  the group that would dispose when Unload
			/// </summary>
			IDisposeGroup UnloadDisposeGroup { get; }

		}
		/// <summary>
		/// Interface IViewModel
		/// </summary>
		public partial interface IViewModel : IBindable, INotifyPropertyChanged, IViewModelLifetime
		{
#if NETFX_CORE
			/// <summary>
			/// Gets the dispatcher of view.
			/// </summary>
			Windows.UI.Core.CoreDispatcher Dispatcher { get; }
#else
            /// <summary>
            /// Gets the dispatcher of view.
            /// </summary>
            /// <value>The dispatcher.</value>
            Dispatcher Dispatcher { get; }

#endif
			/// <summary>
			/// Waits for close.
			/// </summary>
			/// <param name="closingCallback">The closing callback.</param>
			/// <returns>Task.</returns>
			Task WaitForClose(Action closingCallback = null);
			/// <summary>
			/// Gets a value indicating whether this instance is UI busy.
			/// </summary>
			/// <value><c>true</c> if this instance is UI busy; otherwise, <c>false</c>.</value>
			bool IsUIBusy { get; }
			/// <summary>
			/// Gets a value indicating whether [have return value].
			/// </summary>
			/// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
			bool HaveReturnValue { get; }
			/// <summary>
			/// Closes the view and dispose.
			/// </summary>
			void CloseViewAndDispose();
			/// <summary>
			/// Gets or sets the stage manager.
			/// </summary>
			/// <value>The stage manager.</value>
			MVVMSidekick.Views.IStageManager StageManager { get; set; }


			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the in.</typeparam>
			/// <typeparam name="Tout">The type of the out.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="cancellationToken">The cancellation token.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns></returns>
			Task<Tout> ExecuteTask<Tin, Tout>(Func<Tin, CancellationToken, Task<Tout>> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true);

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the in.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="cancellationToken">The cancellation token.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>in value</returns>
			Task ExecuteTask<Tin>(Func<Tin, CancellationToken, Task> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true);


			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the in.</typeparam>
			/// <typeparam name="Tout">The type of the out.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>out value</returns>
			Task<Tout> ExecuteTask<Tin, Tout>(Func<Tin, Task<Tout>> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true);

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the in.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns></returns>
			Task ExecuteTask<Tin>(Func<Tin, Task> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true);

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tout">The type of the out.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns></returns>
			Task<Tout> ExecuteTask<Tout>(Func<Task<Tout>> taskBody, bool UIBusyWhenExecuting = true);

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <param name="taskBody">The task body.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>Task.</returns>
			Task ExecuteTask(Func<Task> taskBody, bool UIBusyWhenExecuting = true);

			//IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence,IViewModel , Func<Tin, Task<Tout>> taskBody);
			//IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence, Func<Tin,Task<Tout>> taskBody, TaskScheduler scheduler);


			/// <summary>
			/// Set: Will VM be Disposed when unbind from View.
			/// </summary>
			/// <value><c>true</c> if this instance is disposing when unbind required; otherwise, <c>false</c>.</value>
			bool IsDisposingWhenUnbindRequired { get; }

			/// <summary>
			/// Set: Will VM be Disposed when unload from View.
			/// </summary>
			/// <value><c>true</c> if this instance is disposing when unload required; otherwise, <c>false</c>.</value>
			bool IsDisposingWhenUnloadRequired { get; }

#if NETFX_CORE

			/// <summary>
			/// Load state of this view
			/// </summary>
			/// <param name="navigationParameter"></param>
			/// <param name="pageState"></param>
			void LoadState(Object navigationParameter, Dictionary<String, Object> pageState);

			/// <summary>
			/// Save state of this view
			/// </summary>
			/// <param name="pageState"></param>
			void SaveState(Dictionary<String, Object> pageState);
#endif
		}

		/// <summary>
		/// Interface IViewModel
		/// </summary>
		/// <typeparam name="TResult">The type of the t result.</typeparam>
		public partial interface IViewModel<TResult> : IViewModel
		{
			/// <summary>
			/// Waits for close with result.
			/// </summary>
			/// <param name="closingCallback">The closing callback.</param>
			/// <returns>Task&lt;TResult&gt;.</returns>
			Task<TResult> WaitForCloseWithResult(Action closingCallback = null);
			/// <summary>
			/// Gets or sets the result.
			/// </summary>
			/// <value>The result.</value>
			TResult Result { get; set; }
		}


		/// <summary>
		/// Struct NoResult
		/// </summary>
		[DataContract]
		public struct NoResult
		{

		}

		/// <summary>
		/// Struct ShowAwaitableResult
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		public struct ShowAwaitableResult<TViewModel>
		{
			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			public TViewModel ViewModel { get; set; }
			/// <summary>
			/// Gets or sets the closing.
			/// </summary>
			/// <value>The closing.</value>
			public Task Closing { get; set; }

		}
		/// <summary>
		/// Class ViewModelBase.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <typeparam name="TResult">The type of the t result.</typeparam>
		public partial class ViewModelBase<TViewModel, TResult> : ViewModelBase<TViewModel>, IViewModel<TResult>
			where TViewModel : ViewModelBase<TViewModel, TResult>, IViewModel<TResult>
		{

			/// <summary>
			/// Releases unmanaged and - optionally - managed resources.
			/// </summary>
			/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}

			/// <summary>
			/// Gets a value indicating whether [have return value].
			/// </summary>
			/// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
			public override bool HaveReturnValue { get { return true; } }

			/// <summary>
			/// Waits for close with result.
			/// </summary>
			/// <param name="closingCallback">The closing callback.</param>
			/// <returns>Task&lt;TResult&gt;.</returns>
			public async Task<TResult> WaitForCloseWithResult(Action closingCallback = null)
			{
				var t = new TaskCompletionSource<TResult>();

				this.AddDisposeAction(
					() =>
					{
						if (closingCallback != null)
						{
							closingCallback();
						}
						t.SetResult(Result);
					}
					);


				await t.Task;
				return Result;
			}

			/// <summary>
			/// Gets or sets the result.
			/// </summary>
			/// <value>The result.</value>
			public TResult Result
			{
				get { return _ResultLocator(this).Value; }
				set { _ResultLocator(this).SetValueAndTryNotify(value); }
			}

			#region Property TResult Result Setup
			/// <summary>
			/// The _ result
			/// </summary>
			protected Property<TResult> _Result =
			  new Property<TResult> { LocatorFunc = _ResultLocator };
			/// <summary>
			/// The _ result locator
			/// </summary>
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			static Func<BindableBase, ValueContainer<TResult>> _ResultLocator =
				RegisterContainerLocator<TResult>(
					"Result",
					model =>
					{
						model._Result =
							model._Result
							??
							new Property<TResult> { LocatorFunc = _ResultLocator };
						return model._Result.Container =
							model._Result.Container
							??
							new ValueContainer<TResult>("Result", model);
					});
			#endregion


		}


		/// <summary>
		/// 一个VM,带有若干界面特性
		/// </summary>
		/// <typeparam name="TViewModel">本身的类型</typeparam>
		[DataContract]
		public abstract partial class ViewModelBase<TViewModel> : BindableBase<TViewModel>, IViewModel where TViewModel : ViewModelBase<TViewModel>
		{
			IDisposeGroup _UnbindDisposeGroup = new DisposeGroup();

			IDisposeGroup _UnloadDisposeGroup = new DisposeGroup();
			/// <summary>
			/// Resource Group that need dispose when Unbind from UI;
			/// </summary>

			public IDisposeGroup UnbindDisposeGroup
			{
				get { return _UnbindDisposeGroup; }
				//set { _UnbindDisposeGroup = value; }
			}

			/// <summary>
			/// Resource Group that need dispose when Unload from UI;
			/// </summary>											   

			public IDisposeGroup UnloadDisposeGroup
			{
				get { return _UnloadDisposeGroup; }
				//set { _UnloadDisposeGroup = value; }
			}


			/// <summary>
			/// Releases unmanaged and - optionally - managed resources.
			/// </summary>
			/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="ViewModelBase{TViewModel}" /> class.
			/// </summary>
			public ViewModelBase()
			{
#if WPF
				this.IsDisposingWhenUnloadRequired = true;
#endif

				GetValueContainer(x => x.UIBusyTaskCount)
					.GetNewValueObservable()
					.Select(e =>
						e.EventArgs != 0)
					.DistinctUntilChanged()
					.Subscribe(isBusy =>
						IsUIBusy = isBusy)
					.DisposeWith(this);
			}





			/// <summary>
			/// Called when [binded to view].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <param name="oldValue">The old value.</param>
			/// <returns>Task.</returns>
			Task IViewModelLifetime.OnBindedToView(IView view, IViewModel oldValue)
			{

				return OnBindedToView(view, oldValue);
			}

			/// <summary>
			/// Called when [unbinded from view].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <param name="newValue">The new value.</param>
			/// <returns>Task.</returns>
			Task IViewModelLifetime.OnUnbindedFromView(IView view, IViewModel newValue)
			{
				return OnUnbindedFromView(view, newValue);
			}

			/// <summary>
			/// Called when [binded view load].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <returns>Task.</returns>
			Task IViewModelLifetime.OnBindedViewLoad(IView view)
			{
				foreach (var item in GetFieldNames())
				{
					RaisePropertyChanged(() => new PropertyChangedEventArgs(item));
				}
				return OnBindedViewLoad(view);
			}
			/// <summary>
			/// Called when [binded view unload].
			/// </summary>
			/// <param name="view">The view.</param>
			/// <returns>Task.</returns>
			Task IViewModelLifetime.OnBindedViewUnload(IView view)
			{
				return OnBindedViewUnload(view);
			}


			/// <summary>
			/// This will be invoked by view when this viewmodel is set to view's ViewModel property.
			/// </summary>
			/// <param name="view">Set target view</param>
			/// <param name="oldValue">Value before set.</param>
			/// <returns>Task awaiter</returns>
			protected virtual async Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
			{
				//#if SILVERLIGHT_5
				//                await T.askEx.Yield();
				//#else
				//                await T.ask.Yield();
				//#endif
				if (view != null)
				{
					StageManager = new StageManager(this) { CurrentBindingView = view };
					StageManager.InitParent(() => view.Parent);
				}
				//StageManager.DisposeWith(this);
				await TaskExHelper.Yield();
			}


			/// <summary>
			/// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.
			/// </summary>
			/// <param name="view">Overwrite target view.</param>
			/// <param name="newValue">The value replacing</param>
			/// <returns>Task awaiter</returns>
			protected virtual async Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
			{
				UnbindDisposeGroup.Dispose();

				if (IsDisposingWhenUnbindRequired)
				{
					Dispose();
				}
				await TaskExHelper.Yield();
			}

			/// <summary>
			/// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
			/// </summary>
			/// <param name="view">View that firing Load event</param>
			/// <returns>Task awaiter</returns>
			protected virtual async Task OnBindedViewLoad(IView view)
			{
				if (view != null)
				{
					StageManager = new StageManager(this) { CurrentBindingView = view };
					StageManager.InitParent(() => view.Parent);
				}

				await TaskExHelper.Yield();
			}

			/// <summary>
			/// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
			/// </summary>
			/// <param name="view">View that firing Unload event</param>
			/// <returns>Task awaiter</returns>
			protected virtual async Task OnBindedViewUnload(IView view)
			{
				UnloadDisposeGroup.Dispose();
				if (IsDisposingWhenUnloadRequired)
				{
					this.Dispose();
				}
				await TaskExHelper.Yield();
			}

			/// <summary>
			/// Set: Will VM be Disposed when unbind from View.
			/// </summary>
			/// <value><c>true</c> if this instance is disposing when unbind required; otherwise, <c>false</c>.</value>
			public bool IsDisposingWhenUnbindRequired { get; protected set; }

			/// <summary>
			/// Set: Will VM be Disposed when unload from View.
			/// </summary>
			/// <value><c>true</c> if this instance is disposing when unload required; otherwise, <c>false</c>.</value>
			public bool IsDisposingWhenUnloadRequired { get; protected set; }


#if NETFX_CORE
			/// <summary>
			/// Populates the page with content passed during navigation.  Any saved state is also
			/// provided when recreating a page from a prior session.
			/// </summary>
			/// <param name="navigationParameter">The parameter value passed to
			/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
			/// </param>
			/// <param name="pageState">A dictionary of state preserved by this page during an earlier
			/// session.  This will be null the first time a page is visited.</param>
			public virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
			{

			}

			/// <summary>
			/// Preserves state associated with this page in case the application is suspended or the
			/// page is discarded from the navigation cache. 
			/// </summary>
			/// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
			public virtual void SaveState(Dictionary<String, Object> pageState)
			{

			}
#endif

			/// <summary>
			/// The _ stage manager
			/// </summary>
			MVVMSidekick.Views.IStageManager _StageManager = new TestingStageManager();

			/// <summary>
			/// Gets or sets the stage manager.												 I
			/// </summary>
			/// <value>The stage manager.</value>
			//[Microsoft.Practices.Unity.Dependency(Testing.Constants.DependencyKeyForTesting)]
			public MVVMSidekick.Views.IStageManager StageManager
			{
				get { return _StageManager; }
				set { _StageManager = value; }
			}

			/// <summary>
			/// 是否有返回值
			/// </summary>
			/// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
			public virtual bool HaveReturnValue { get { return false; } }
			/// <summary>
			/// 本UI是否处于忙状态
			/// </summary>
			/// <value><c>true</c> if this instance is UI busy; otherwise, <c>false</c>.</value>

			public bool IsUIBusy
			{
				get { return _IsUIBusyLocator(this).Value; }
				set { _IsUIBusyLocator(this).SetValueAndTryNotify(value); }
			}
			#region Property bool IsUIBusy Setup
			/// <summary>
			/// The _ is UI busy
			/// </summary>
			protected Property<bool> _IsUIBusy = new Property<bool> { LocatorFunc = _IsUIBusyLocator };
			/// <summary>
			/// The _ is UI busy locator
			/// </summary>
			static Func<BindableBase, ValueContainer<bool>> _IsUIBusyLocator = RegisterContainerLocator<bool>("IsUIBusy", model => model.Initialize("IsUIBusy", ref model._IsUIBusy, ref _IsUIBusyLocator, _IsUIBusyDefaultValueFactory));
			/// <summary>
			/// The _ is UI busy default value factory
			/// </summary>
			static Func<bool> _IsUIBusyDefaultValueFactory = null;
			#endregion



			/// <summary>
			/// Gets or sets the UI busy task count.
			/// </summary>
			/// <value>The UI busy task count.</value>
			private int UIBusyTaskCount
			{
				get { return _UIBusyTaskCountLocator(this).Value; }
				set { _UIBusyTaskCountLocator(this).SetValueAndTryNotify(value); }
			}
			#region Property int UIBusyTaskCount Setup
			private Property<int> _UIBusyTaskCount = new Property<int> { LocatorFunc = _UIBusyTaskCountLocator };
			private static Func<BindableBase, ValueContainer<int>> _UIBusyTaskCountLocator = RegisterContainerLocator<int>("UIBusyTaskCount", model => model.Initialize("UIBusyTaskCount", ref model._UIBusyTaskCount, ref _UIBusyTaskCountLocator, _UIBusyTaskCountDefaultValueFactory));
			private static Func<int> _UIBusyTaskCountDefaultValueFactory = null;
			#endregion


			/// <summary>
			/// Waits for close.
			/// </summary>
			/// <param name="closingCallback">The closing callback.</param>
			/// <returns>Task.</returns>
			public async Task WaitForClose(Action closingCallback = null)
			{
				var t = new TaskCompletionSource<object>();

				this.AddDisposeAction(
					() =>
					{
						if (closingCallback != null)
						{
							closingCallback();
						}
						t.SetResult(null);
					}
					);


				await t.Task;
			}
			/// <summary>
			/// Closes the view and dispose.
			/// </summary>
			public void CloseViewAndDispose()
			{
				this.StageManager?.CurrentBindingView?.SelfClose();
				Dispose();
			}



			/// <summary>
			/// Runs the on dispatcher.
			/// </summary>
			/// <param name="action">The action.</param>
			private async void RunOnDispatcher(Action action)
			{


#if NETFX_CORE
				await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(action));

#elif NET45

				await Dispatcher.BeginInvoke(action).Task;
#else
                await TaskExHelper.Yield();
                Dispatcher.BeginInvoke(action);

#endif

			}

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the tin.</typeparam>
			/// <typeparam name="Tout">The type of the tout.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="cancellationToken">The cancellation token.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>Task&lt;Tout&gt;.</returns>
			public virtual async Task<Tout> ExecuteTask<Tin, Tout>(Func<Tin, CancellationToken, Task<Tout>> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true)
			{
				//TaskCompletionSource<Tout> taskComplet = new TaskCompletionSource<Tout>();
				//Tout result = default(Tout);
				//Action action =
				//  async () =>
				//  {
				//      taskComplet.SetResult();
				//  };


				if (UIBusyWhenExecuting)
				{
					using (
						Disposable.Create(
							() =>
								UIBusyTaskCount--))
					{
						UIBusyTaskCount++;

						return await taskBody(inputContext, cancellationToken);

					}
				}
				else
				{
					return await taskBody(inputContext, cancellationToken);
				}



			}


			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the in.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="cancellationToken">The cancellation token.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>
			/// Task.
			/// </returns>
			public virtual async Task ExecuteTask<Tin>(Func<Tin, CancellationToken, Task> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true)
			{

				await ExecuteTask<Tin, object>(async (i, c) => { await taskBody(i, c); return null; }, inputContext, cancellationToken, UIBusyWhenExecuting);
			}


			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the tin.</typeparam>
			/// <typeparam name="Tout">The type of the tout.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>Task&lt;Tout&gt;.</returns>
			public virtual async Task<Tout> ExecuteTask<Tin, Tout>(Func<Tin, Task<Tout>> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true)
			{
				return await ExecuteTask<Tin, Tout>(async (i, c) => await taskBody(i), inputContext, CancellationToken.None, UIBusyWhenExecuting);

			}

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tin">The type of the in.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="inputContext">The input context.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>
			/// Task.
			/// </returns>
			public virtual async Task ExecuteTask<Tin>(Func<Tin, Task> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true)
			{
				await ExecuteTask<Tin, object>(async (i, c) => { await taskBody(i); return null; }, inputContext, CancellationToken.None, UIBusyWhenExecuting);

			}

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <typeparam name="Tout">The type of the out.</typeparam>
			/// <param name="taskBody">The task body.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>
			/// Task&lt;Tout&gt;.
			/// </returns>
			public virtual async Task<Tout> ExecuteTask<Tout>(Func<Task<Tout>> taskBody, bool UIBusyWhenExecuting = true)
			{
				return await ExecuteTask<object, Tout>(
					async (i, c) =>
					{
						return await taskBody();
					},
					null,
					CancellationToken.None,
					UIBusyWhenExecuting);

			}

			/// <summary>
			/// Executes the task.
			/// </summary>
			/// <param name="taskBody">The task body.</param>
			/// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
			/// <returns>Task.</returns>
			public virtual async Task ExecuteTask(Func<Task> taskBody, bool UIBusyWhenExecuting = true)
			{
				await ExecuteTask<object, object>(
					async (i, c) =>
					{
						await taskBody();
						return null;
					},
					null,
					CancellationToken.None, UIBusyWhenExecuting);

			}

#if NETFX_CORE
			private Windows.UI.Core.CoreDispatcher GetCurrentViewDispatcher()
			{
				Windows.UI.Xaml.DependencyObject dp = null;
				if (this.StageManager == null)
				{
					return null;
				}
				else if ((dp = (this.StageManager.CurrentBindingView as Windows.UI.Xaml.DependencyObject)) == null)
				{
					return null;
				}
				return dp.Dispatcher;

			}
#else

            /// <summary>
            /// Gets the current view dispatcher.
            /// </summary>
            /// <returns>Dispatcher.</returns>
            private Dispatcher GetCurrentViewDispatcher()
            {
                DependencyObject dp = null;
                if (this.StageManager == null)
                {
                    return null;
                }
                else if ((dp = (this.StageManager.CurrentBindingView as DependencyObject)) == null)
                {
                    return null;
                }
                return dp.Dispatcher;

            }
#endif
#if NETFX_CORE
			/// <summary>
			/// Gets the current view dispatcher.	
			/// </summary>
			public Windows.UI.Core.CoreDispatcher Dispatcher
			{
				get
				{

					var current = GetCurrentViewDispatcher();
					if (current != null)
					{
						return current;
					}
					if (Windows.UI.Xaml.Window.Current == null)
					{
						return null;
					}
					return Windows.UI.Xaml.Window.Current.Dispatcher;
				}


			}



#elif WPF
			/// <summary>
			/// Explode the dispatcher of current view.
			/// </summary>
			public Dispatcher Dispatcher
			{
				get
				{
					var current = GetCurrentViewDispatcher();
					if (current != null)
					{
						return current;
					}
					if (Application.Current == null)
					{
						return null;
					}

					return Application.Current.Dispatcher;
				}

							
			}
#elif SILVERLIGHT_5 || WINDOWS_PHONE_8
            /// <summary>
            /// Gets the dispatcher.
            /// </summary>
            /// <value>The dispatcher.</value>
            public Dispatcher Dispatcher
            {
                get
                {
                    var current = GetCurrentViewDispatcher();
                    if (current != null)
                    {
                        return current;
                    }
                    if (Application.Current == null)
                    {
                        return null;
                    }
                    else if (Application.Current.RootVisual == null)
                    {
                        return null;
                    }
                    else return Application.Current.RootVisual.Dispatcher;
                }


            }
#endif
		}


		/// <summary>
		/// Class ErrorEntity.
		/// </summary>
		public class ErrorEntity
		{
			/// <summary>
			/// Gets or sets the message.
			/// </summary>
			/// <value>The message.</value>
			public string Message { get; set; }
			/// <summary>
			/// Gets or sets the exception.
			/// </summary>
			/// <value>The exception.</value>
			public Exception Exception { get; set; }
			/// <summary>
			/// Gets or sets the inner error information source.
			/// </summary>
			/// <value>The inner error information source.</value>
			public IErrorInfo InnerErrorInfoSource { get; set; }
			/// <summary>
			/// Returns a <see cref="System.String" /> that represents this instance.
			/// </summary>
			/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
			public override string ToString()
			{

				return null;// string.Format("{0}，{1}，{2}", Message, Exception, InnerErrorInfoSource);
			}
		}
		/// <summary>
		/// Interface IErrorInfo
		/// </summary>
		public interface IErrorInfo
		{
			/// <summary>
			/// Gets the errors.
			/// </summary>
			/// <value>The errors.</value>
			ObservableCollection<ErrorEntity> Errors { get; }
		}

		/// <summary>
		/// Interface IValueCanSet
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public interface IValueCanSet<in T>
		{
			/// <summary>
			/// Sets the value.
			/// </summary>
			/// <value>The value.</value>
			T Value { set; }
		}

		/// <summary>
		/// Interface IValueCanGet
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public interface IValueCanGet<out T>
		{
			/// <summary>
			/// Gets the value.
			/// </summary>
			/// <value>The value.</value>
			T Value { get; }
		}



		/// <summary>
		/// Interface INotifyChanges
		/// </summary>
		public interface INotifyChanges
		{
			/// <summary>
			/// Occurs when [value changed with name only].
			/// </summary>
			event PropertyChangedEventHandler ValueChangedWithNameOnly;
			/// <summary>
			/// Occurs when [value changed with nothing].
			/// </summary>
			event EventHandler ValueChangedWithNothing;

		}
		/// <summary>
		/// Interface INotifyChanges
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public interface INotifyChanges<T> : INotifyChanges
		{
			/// <summary>
			/// Occurs when [value changed].
			/// </summary>
			event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

		}
		/// <summary>
		/// Interface IValueContainer
		/// </summary>
		public interface IValueContainer : IErrorInfo, INotifyChanges
		{
			/// <summary>
			/// Gets the type of the property.
			/// </summary>
			/// <value>The type of the property.</value>
			Type PropertyType { get; }
			/// <summary>
			/// Gets or sets the value.
			/// </summary>
			/// <value>The value.</value>
			Object Value { get; set; }
			/// <summary>
			/// Gets or sets a value indicating whether this instance is copy to allowed.
			/// </summary>
			/// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
			bool IsCopyToAllowed { get; set; }
		}

		/// <summary>
		/// Interface ICommandModel
		/// </summary>
		/// <typeparam name="TCommand">The type of the t command.</typeparam>
		/// <typeparam name="TResource">The type of the t resource.</typeparam>
		public interface ICommandModel<TCommand, TResource> : ICommand
		{
			/// <summary>
			/// Gets the command core.
			/// </summary>
			/// <value>The command core.</value>
			TCommand CommandCore { get; }
			/// <summary>
			/// Gets or sets a value indicating whether [last can execute value].
			/// </summary>
			/// <value><c>true</c> if [last can execute value]; otherwise, <c>false</c>.</value>
			bool LastCanExecuteValue { get; set; }
			/// <summary>
			/// Gets or sets the resource.
			/// </summary>
			/// <value>The resource.</value>
			TResource Resource { get; set; }
		}

		/// <summary>
		/// Class StringResourceReactiveCommandModel.
		/// </summary>
		public class StringResourceReactiveCommandModel : CommandModel<ReactiveCommand, string>
		{

		}

		/// <summary>
		/// 用于封装ICommand的ViewModel。一般包括一个Command实例和对应此实例的一组资源
		/// </summary>
		/// <typeparam name="TCommand">ICommand 详细类型</typeparam>
		/// <typeparam name="TResource">配合Command 的资源类型</typeparam>
		public class CommandModel<TCommand, TResource> : BindableBase<CommandModel<TCommand, TResource>>, ICommandModel<TCommand, TResource>, ICommandWithViewModel
			where TCommand : ICommand
		{
			/// <summary>
			/// Returns a <see cref="System.String" /> that represents this instance.
			/// </summary>
			/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
			public override string ToString()
			{
				return Resource.ToString();
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="CommandModel{TCommand, TResource}"/> class.
			/// </summary>
			public CommandModel()
			{ }
			/// <summary>
			/// 构造器
			/// </summary>
			/// <param name="commandCore">ICommand核心</param>
			/// <param name="resource">初始资源</param>
			public CommandModel(TCommand commandCore, TResource resource)
			{
				CommandCore = commandCore;
				commandCore.CanExecuteChanged += commandCore_CanExecuteChanged;
				Resource = resource;
			}

			/// <summary>
			/// Handles the CanExecuteChanged event of the commandCore control.
			/// </summary>
			/// <param name="sender">The source of the event.</param>
			/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
			void commandCore_CanExecuteChanged(object sender, EventArgs e)
			{
				if (CanExecuteChanged != null)
				{
					this.CanExecuteChanged(this, e);
				}

			}


			/// <summary>
			/// ICommand核心
			/// </summary>
			/// <value>The command core.</value>
			public TCommand CommandCore
			{
				get;
				private set;

			}

			//public CommandModel<TCommand, TResource> ConfigCommandCore(Action<TCommand> commandConfigAction)
			//{
			//    commandConfigAction(CommandCore);
			//    return this;
			//}


			/// <summary>
			/// 上一次是否能够运行的值
			/// </summary>
			/// <value><c>true</c> if [last can execute value]; otherwise, <c>false</c>.</value>
			public bool LastCanExecuteValue
			{
				get { return _LastCanExecuteValueLocator(this).Value; }
				set { _LastCanExecuteValueLocator(this).SetValueAndTryNotify(value); }
			}


			#region Property bool LastCanExecuteValue Setup

			/// <summary>
			/// The _ last can execute value
			/// </summary>
			protected Property<bool> _LastCanExecuteValue =
			  new Property<bool> { LocatorFunc = _LastCanExecuteValueLocator };
			/// <summary>
			/// The _ last can execute value locator
			/// </summary>
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			static Func<BindableBase, ValueContainer<bool>> _LastCanExecuteValueLocator =
				RegisterContainerLocator<bool>(
				"LastCanExecuteValue",
				model =>
				{
					model._LastCanExecuteValue =
						model._LastCanExecuteValue
						??
						new Property<bool> { LocatorFunc = _LastCanExecuteValueLocator };
					return model._LastCanExecuteValue.Container =
						model._LastCanExecuteValue.Container
						??
						new ValueContainer<bool>("LastCanExecuteValue", model);
				});

			#endregion



			/// <summary>
			/// 资源
			/// </summary>
			/// <value>The resource.</value>
			public TResource Resource
			{
				get { return _ResourceLocator(this).Value; }
				set { _ResourceLocator(this).SetValueAndTryNotify(value); }
			}


			#region Property TResource Resource Setup

			/// <summary>
			/// The _ resource
			/// </summary>
			protected Property<TResource> _Resource =
			  new Property<TResource> { LocatorFunc = _ResourceLocator };
			/// <summary>
			/// The _ resource locator
			/// </summary>
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			static Func<BindableBase, ValueContainer<TResource>> _ResourceLocator =
				RegisterContainerLocator<TResource>(
				"Resource",
				model =>
				{
					model._Resource =
						model._Resource
						??
						new Property<TResource> { LocatorFunc = _ResourceLocator };
					return model._Resource.Container =
						model._Resource.Container
						??
						new ValueContainer<TResource>("Resource", model);
				});

			#endregion











			/// <summary>
			/// 判断是否可执行
			/// </summary>
			/// <param name="parameter">指定参数</param>
			/// <returns><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.</returns>
			public bool CanExecute(object parameter)
			{
				var s = CommandCore.CanExecute(parameter);
				LastCanExecuteValue = s;
				return s;
			}

			/// <summary>
			/// Occurs when [can execute changed].
			/// </summary>
			public event EventHandler CanExecuteChanged;

			/// <summary>
			/// 执行
			/// </summary>
			/// <param name="parameter">指定参数</param>
			public void Execute(object parameter)
			{
				var ec = CommandCore as EventCommandBase;
				var eargs = parameter as EventCommandEventArgs;
				if (ec != null && eargs != null)
				{
					ec.OnCommandExecute(eargs);
				}
				else
					CommandCore.Execute(parameter);
			}

			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			public BindableBase ViewModel
			{
				get
				{
					var c = CommandCore as ICommandWithViewModel;
					if (c != null)
					{
						return c.ViewModel;
					}
					return null;
				}
				set
				{
					var c = CommandCore as ICommandWithViewModel;
					if (c != null)
					{
						c.ViewModel = value;
					}

				}
			}
		}

		/// <summary>
		/// 可绑定的CommandVM 扩展方法集
		/// </summary>
		public static class CommandModelExtensions
		{
			/// <summary>
			/// 根据ICommand实例创建CommandModel
			/// </summary>
			/// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
			/// <typeparam name="TResource">附加资源类型</typeparam>
			/// <param name="command">ICommand实例</param>
			/// <param name="resource">资源实例</param>
			/// <returns>CommandModel实例</returns>
			public static CommandModel<TCommand, TResource> CreateCommandModel<TCommand, TResource>(this TCommand command, TResource resource)
				where TCommand : ICommand
			{
				return new CommandModel<TCommand, TResource>(command, resource);
			}

			/// <summary>
			/// 据ICommand实例创建不具备/弱类型资源的CommandModel
			/// </summary>
			/// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
			/// <param name="command">ICommand实例</param>
			/// <param name="resource">资源实例</param>
			/// <returns>CommandModel实例</returns>
			public static CommandModel<TCommand, object> CreateCommandModel<TCommand>(this TCommand command, object resource = null)
			where TCommand : ICommand
			{
				return new CommandModel<TCommand, object>(command, null);
			}

			/// <summary>
			/// 为CommandModel指定ViewModel
			/// </summary>
			/// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
			/// <typeparam name="TResource">附加资源类型</typeparam>
			/// <param name="cmdModel">CommandModel具体实例</param>
			/// <param name="viewModel">ViewModel具体实例</param>
			/// <returns>CommandModel&lt;TCommand, TResource&gt;.</returns>
			public static CommandModel<TCommand, TResource> WithViewModel<TCommand, TResource>(this CommandModel<TCommand, TResource> cmdModel, BindableBase viewModel)
				where TCommand : ICommand
			{
				//cmdModel.
				var cmd2 = cmdModel.CommandCore as ICommandWithViewModel;
				if (cmd2 != null)
				{
					cmd2.ViewModel = viewModel;
				}
				return cmdModel;
			}
		}

	}

}
