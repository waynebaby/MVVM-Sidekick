#if BLAZOR

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components
{
    /// <summary>
    /// <para>MVVM-Sidekick Blazor验证器组件</para>
    /// <para>MVVM-Sidekick Blazor validator component</para>
    /// </summary>
    /// <remarks>
    /// <para>为Blazor EditForm提供MVVM-Sidekick框架的数据验证支持</para>
    /// <para>Provides data validation support for Blazor EditForm using MVVM-Sidekick framework</para>
    /// </remarks>
    public class MVVMSidekickValidator : ComponentBase
    {
        /// <summary>
        /// <para>当前编辑上下文</para>
        /// <para>Current edit context</para>
        /// </summary>
        /// <value>
        /// <para>级联传递的EditContext参数</para>
        /// <para>Cascading EditContext parameter</para>
        /// </value>
        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }


        /// <summary>
        /// <para>组件初始化时执行的方法</para>
        /// <para>Method executed when component is initialized</para>
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <para>当EditContext为空或模型不实现所需接口时抛出</para>
        /// <para>Thrown when EditContext is null or model doesn't implement required interfaces</para>
        /// </exception>
        /// <remarks>
        /// <para>设置验证消息存储并绑定错误变更事件处理器</para>
        /// <para>Sets up validation message store and binds error change event handlers</para>
        /// </remarks>
        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException("MVVMSidekickValidator requires a cascading parameter of type EditContext. For example, you can use MVVMSidekickValidator inside an EditForm.");
            }
            var model = CurrentEditContext.Model as BindableBase;
            if (model == null)
            {
                throw new InvalidOperationException("MVVMSidekickValidator requires model is a BindableBase.");
            }
            var errorModel = CurrentEditContext.Model as INotifyDataErrorInfo;
            if (errorModel == null)
            {
                throw new InvalidOperationException("MVVMSidekickValidator requires model of the form implements INotifyDataErrorInfo.");
            }
            ValidationMessageStore messages = new ValidationMessageStore(CurrentEditContext);
            errorModel.ErrorsChanged += (o, e) =>
            {

                messages.Clear();
                foreach (var errorEntry in model.GetAllErrors())
                {
                    FieldIdentifier fieldIdentifier = CurrentEditContext.Field(errorEntry.PropertyName);
                    messages.Add(in fieldIdentifier, errorEntry.ToString());
                }

            };

            CurrentEditContext.OnValidationRequested += (o, e) =>
              {
                  messages.Clear();
                  foreach (var errorEntry in model.GetAllErrors())
                  {
                      FieldIdentifier fieldIdentifier = CurrentEditContext.Field(errorEntry.PropertyName);
                      messages.Add(in fieldIdentifier, errorEntry.ToString());
                  }
              };

        }


    }
}
#endif