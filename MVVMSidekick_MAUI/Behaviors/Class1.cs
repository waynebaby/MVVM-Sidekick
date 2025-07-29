/// <summary>
/// <para>MAUI行为类文件，提供Button点击事件消息行为实现</para>
/// <para>MAUI behaviors class file that provides Button click event message behavior implementation</para>
/// </summary>
/// <remarks>
/// <para>这个文件包含ButtonClickedEventMessageBehavior类，用于将Button的点击事件转换为事件路由消息</para>
/// <para>This file contains the ButtonClickedEventMessageBehavior class for converting Button click events to event routing messages</para>
/// <para>基于MAUI平台的Behavior模式，支持事件路由和消息传递</para>
/// <para>Based on MAUI platform's Behavior pattern, supports event routing and message passing</para>
/// </remarks>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick_MAUI.Behaviors
{
    /// <summary>
    /// <para>Button点击事件消息行为类，将Button的点击事件转换为事件路由消息</para>
    /// <para>Button clicked event message behavior class that converts Button click events to event routing messages</para>
    /// </summary>
    /// <remarks>
    /// <para>继承自Behavior&lt;Button&gt;，用于为Button控件添加点击事件消息路由功能</para>
    /// <para>Inherits from Behavior&lt;Button&gt; to add click event message routing functionality to Button controls</para>
    /// <para>支持自定义事件消息和事件路由器配置</para>
    /// <para>Supports custom event message and event router configuration</para>
    /// </remarks>
    public class ButtonClickedEventMessageBehavior : Behavior<Button>
    {
        /// <summary>
        /// <para>初始化ButtonClickedEventMessageBehavior实例</para>
        /// <para>Initializes a new instance of ButtonClickedEventMessageBehavior</para>
        /// </summary>
        public ButtonClickedEventMessageBehavior()
        {

        }

        /// <summary>
        /// <para>事件消息绑定属性，用于指定点击事件时发送的消息内容</para>
        /// <para>Event message bindable property used to specify the message content sent when click event occurs</para>
        /// </summary>
        public static readonly BindableProperty EventMessageProperty =
            BindableProperty.Create(
                nameof(EventMessage),
                typeof(string),
                typeof(ButtonClickedEventMessageBehavior));

        /// <summary>
        /// <para>获取或设置事件消息内容</para>
        /// <para>Gets or sets the event message content</para>
        /// </summary>
        /// <value>
        /// <para>要发送的事件消息字符串</para>
        /// <para>The event message string to send</para>
        /// </value>
        public string EventMessage
        {
            get => (string)GetValue(EventMessageProperty);
            set => SetValue(EventMessageProperty, value);
        }

        /// <summary>
        /// <para>事件路由器绑定属性，用于指定处理事件的路由器实例</para>
        /// <para>Event router bindable property used to specify the router instance for handling events</para>
        /// </summary>
        public static readonly BindableProperty EventRouterProperty =
        BindableProperty.Create(
            nameof(EventRouter),
            typeof(MVVMSidekick.EventRouting.EventRouter),
            typeof(ButtonClickedEventMessageBehavior), MVVMSidekick.EventRouting.EventRouter.Instance);
            
        /// <summary>
        /// <para>获取或设置事件路由器实例</para>
        /// <para>Gets or sets the event router instance</para>
        /// </summary>
        /// <value>
        /// <para>用于处理事件路由的EventRouter实例</para>
        /// <para>The EventRouter instance used for handling event routing</para>
        /// </value>
        public MVVMSidekick.EventRouting.EventRouter EventRouter
        {
            get => (MVVMSidekick.EventRouting.EventRouter)GetValue(EventRouterProperty);
            set => SetValue(EventRouterProperty, value);
        }

        /// <summary>
        /// <para>获取附加的Button对象</para>
        /// <para>Gets the attached Button object</para>
        /// </summary>
        /// <value>
        /// <para>当前附加的Button实例，如果未附加则为null</para>
        /// <para>The currently attached Button instance, or null if not attached</para>
        /// </value>
        public Button? AttachedObject
        {
            get; private set;
        }
        
        /// <summary>
        /// <para>当行为附加到Button时调用，设置点击事件处理</para>
        /// <para>Called when the behavior is attached to a Button, sets up click event handling</para>
        /// </summary>
        /// <param name="bindable">
        /// <para>要附加到的Button控件</para>
        /// <para>The Button control to attach to</para>
        /// </param>
        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Clicked += Bindable_Clicked;
            AttachedObject = bindable;
        }
        
        /// <summary>
        /// <para>当行为从Button分离时调用，清理事件处理和引用</para>
        /// <para>Called when the behavior is detached from a Button, cleans up event handling and references</para>
        /// </summary>
        /// <param name="bindable">
        /// <para>要分离的Button控件</para>
        /// <para>The Button control to detach from</para>
        /// </param>
        protected override void OnDetachingFrom(Button bindable)
        {

            bindable.Clicked -= Bindable_Clicked;
            AttachedObject = null;
            base.OnDetachingFrom(bindable);
        }

        /// <summary>
        /// <para>Button点击事件处理程序，将点击事件转换为事件路由消息</para>
        /// <para>Button click event handler that converts click events to event routing messages</para>
        /// </summary>
        /// <param name="sender">
        /// <para>事件发送者</para>
        /// <para>The event sender</para>
        /// </param>
        /// <param name="e">
        /// <para>事件参数</para>
        /// <para>The event arguments</para>
        /// </param>
        private void Bindable_Clicked(object? sender, EventArgs e)
        {
            (EventRouter?? MVVMSidekick.EventRouting.EventRouter.Instance).RaiseEvent(sender, e, typeof(EventArgs), EventMessage, true, false);
        }
    }
}
