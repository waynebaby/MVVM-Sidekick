using MVVMSidekick.Common;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using MVVMSidekick.Utilities;
namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// 验证扩展方法类，提供模型验证功能
    /// Validation extension methods class that provides model validation functionality
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// 必填字段错误消息常量
        /// Required field error message constant
        /// </summary>
        private const string REQUIRED_MESSAGE = "Field should not be empty.";
        
        /// <summary>
        /// 选择范围错误消息常量
        /// In choices error message constant
        /// </summary>
        private const string INCHOICES_MESSAGE = "Value is out of Choices";
        
        /// <summary>
        /// 字符串长度错误消息常量
        /// String length error message constant
        /// </summary>
        private const string STRINGLENGTH_MESSAGE = "The length of string is out of limitation.";
        
        /// <summary>
        /// 正则表达式匹配错误消息常量
        /// Regex pattern mismatch error message constant
        /// </summary>
        private const string REGEX_MESSAGE = "Pattern mismatch.";
        
        /// <summary>
        /// 列表长度错误消息常量
        /// List length error message constant
        /// </summary>
        private const string LISTLIENGTH_MESSAGE = "The item count of list is out of limitation.";

        /// <summary>
        /// 当指定属性值发生变化时进行验证
        /// Validates when specified property values change
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="model">模型实例 / The model instance</param>
        /// <param name="properties">要监听变化的属性表达式数组 / Array of property expressions to listen for changes</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> ValidateOnChange<TModel>(
            this TModel model,
            params Expression<Func<TModel, object>>[] properties)
            where TModel : BindableBase<TModel>
        {
            var fields = model.GetValueContainers(properties);
            return new ModelValiditionSequenceContext<TModel>
            {
                Model = model,
                ListenChangedSequence = model.ListenValueChangedEvents(properties),
                FieldsListenedTo = new Lazy<IDictionary<string, IValueContainer>>(() => fields.ToDictionary(x => x.PropertyName))
            };
        }

        /// <summary>
        /// 当指定单个属性值发生变化时进行验证
        /// Validates when a specified single property value changes
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="model">模型实例 / The model instance</param>
        /// <param name="property">要监听变化的属性表达式 / Property expression to listen for changes</param>
        /// <returns>属性验证序列上下文 / Property validation sequence context</returns>
        public static PropertyValiditionSequenceContext<TModel, TValue> ValidateOnChange<TModel, TValue>(
          this TModel model,
           Expression<Func<TModel, TValue>> property)
          where TModel : BindableBase<TModel>
        {
            var field = model.GetValueContainer(property);
            return new PropertyValiditionSequenceContext<TModel, TValue>
            {
                ModelContext = new ModelValiditionSequenceContext<TModel>
                {
                    Model = model,
                    ListenChangedSequence = field.GetValueChangedEventObservable().Select(x => (model, field as IValueContainer, x.EventArgs as ValueChangedEventArgs)),
                    FieldsListenedTo = new Lazy<IDictionary<string, IValueContainer>>(() => new Dictionary<string, IValueContainer>() { { field.PropertyName, field } })
                },
                PropertyExpression = property,
                PropertyName = field.PropertyName
            };

        }

        /// <summary>
        /// 专注于特定属性的验证配置
        /// Focus on validation configuration for a specific property
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标属性表达式 / The target property expression</param>
        /// <param name="validationConfigurations">验证配置动作 / The validation configurations action</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> FocusOnProperty<TModel, TValue>(
            this ModelValiditionSequenceContext<TModel> context,
            Expression<Func<TModel, TValue>> property, Action<PropertyValiditionSequenceContext<TModel, TValue>> validationConfigurations)
        {
            validationConfigurations?.Invoke(new PropertyValiditionSequenceContext<TModel, TValue>
            {
                ModelContext = context,
                PropertyExpression = property,
                PropertyName = property.GetPropertyName()
            });
            return context;
        }

        /// <summary>
        /// 匹配规则后执行相应动作
        /// Execute corresponding action after matching rule
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="ruleName">规则名称 / The rule name</param>
        /// <param name="ruleMatcher">规则匹配器 / The rule matcher</param>
        /// <param name="matchAction">匹配时执行的动作 / The action to execute when matched</param>
        /// <param name="mismatchAction">不匹配时执行的动作 / The action to execute when mismatched</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> MatchThenAction<TModel>(
            this ModelValiditionSequenceContext<TModel> context, string ruleName,
            Func<TModel, bool> ruleMatcher,
            Action<(String RuleName, ModelValiditionSequenceContext<TModel> Context)> matchAction = null,
            Action<(String RuleName, ModelValiditionSequenceContext<TModel> Context)> mismatchAction = null)
            where TModel : BindableBase<TModel>
        {
            if (matchAction != null || mismatchAction != null)
            {
                context.ListenChangedSequence
       
                    .Subscribe(e => {

                        try
                        {
                            var isMatched = ruleMatcher(e.Model);
                            if (isMatched)
                            {
                                matchAction?.Invoke((ruleName, context));
                            }
                            else
                            {
                                mismatchAction?.Invoke((ruleName, context));
                            }
                        }
                        catch (Exception ex)
                        {
                            EventRouter.RaiseErrorEvent(e.Model, ex);
                        }
                    })
                    .DisposeWith(context.Model);

            }

            return context;
        }
        /// <summary>
        /// 当规则不匹配时显示错误消息
        /// Show error message when rule mismatches
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="ruleMatcher">规则匹配器 / The rule matcher</param>
        /// <param name="errorMessageFactory">错误消息工厂 / The error message factory</param>
        /// <param name="ruleName">规则名称 / The rule name</param>
        /// <param name="exFactory">异常工厂 / The exception factory</param>
        /// <param name="throwException">是否抛出异常 / Whether to throw exception</param>
        /// <param name="toProperties">目标属性表达式数组 / Array of target property expressions</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> MismatchThenMessage<TModel>(
            this ModelValiditionSequenceContext<TModel> context, Func<TModel, bool> ruleMatcher,
            Func<string> errorMessageFactory,
            string ruleName = null, Func<Exception> exFactory = null,
            bool throwException = false,
            params Expression<Func<TModel, object>>[] toProperties)
            where TModel : BindableBase<TModel>
        {
            var propertyNames = toProperties?.Select(x => x.GetPropertyName()).ToArray();
            if (propertyNames == null || propertyNames.Length==0)
            {
                propertyNames = context.FieldsListenedTo.Value.Keys.ToArray();
            }

            ruleName = ruleName ?? context.Model.GenerateEasyValidationRuleName("[..]");
            return context.MatchThenAction(
                ruleName,
                ruleMatcher,
                c => 
                    c.RemoveError(),
                c => 
                    c.AddError(errorMessageFactory?.Invoke(), exFactory?.Invoke(), throwException, propertyNames));
        }

        /// <summary>
        /// 当指定属性的规则不匹配时显示错误消息
        /// Show error message when rule for specified property mismatches
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标属性表达式 / The target property expression</param>
        /// <param name="ruleMatcher">规则匹配器 / The rule matcher</param>
        /// <summary>
        /// 当指定属性的规则不匹配时显示错误消息
        /// Show error message when rule for specified property mismatches
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标属性表达式 / The target property expression</param>
        /// <param name="ruleMatcher">规则匹配器 / The rule matcher</param>
        /// <param name="errorMessageFactory">错误消息工厂 / The error message factory</param>
        /// <param name="ruleName">规则名称 / The rule name</param>
        /// <param name="exFactory">异常工厂 / The exception factory</param>
        /// <param name="throwException">是否抛出异常 / Whether to throw exception</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> MismatchThenMessage<TModel,TValue>(
            this ModelValiditionSequenceContext<TModel> context, Expression<Func<TModel, TValue>> property,Func<TModel, bool> ruleMatcher,
            Func<string> errorMessageFactory,
            string ruleName = null, Func<Exception> exFactory = null,
            bool throwException = false)
            where TModel : BindableBase<TModel>
        {
            ruleName = ruleName ?? context.Model.GenerateEasyValidationRuleName("[..]");
            return context.MatchThenAction(
                ruleName,
                ruleMatcher,
                c =>
                    c.RemoveError(),
                c =>
                    c.AddError(errorMessageFactory?.Invoke(), exFactory?.Invoke(), throwException, property.GetPropertyName()));
        }

        /// <summary>
        /// 当属性验证规则不匹配时显示错误消息
        /// Show error message when property validation rule mismatches
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">属性验证序列上下文 / The property validation sequence context</param>
        /// <param name="ruleMatcher">规则匹配器 / The rule matcher</param>
        /// <param name="errorMessageFactory">错误消息工厂 / The error message factory</param>
        /// <param name="ruleName">规则名称 / The rule name</param>
        /// <param name="exFactory">异常工厂 / The exception factory</param>
        /// <param name="throwException">是否抛出异常 / Whether to throw exception</param>
        /// <returns>属性验证序列上下文 / Property validation sequence context</returns>
        public static PropertyValiditionSequenceContext<TModel, TValue> MismatchThenMessage<TModel, TValue>(
           this PropertyValiditionSequenceContext<TModel, TValue> context, Func<TModel, bool> ruleMatcher,
               Func<string> errorMessageFactory,
            string ruleName = null, Func<Exception> exFactory = null,
            bool throwException = false)
           where TModel : BindableBase<TModel>
        {
            var propertyName = context.PropertyName;
            ruleName = ruleName ?? context.ModelContext.Model.GenerateEasyValidationRuleName(context.PropertyName);
            context.ModelContext.MatchThenAction(
                ruleName,
                ruleMatcher,
                c =>
                    c.RemoveError(),
                c => 
                    c.AddError(errorMessageFactory?.Invoke(), exFactory?.Invoke(), throwException, propertyName));

            return context;
        }

        /// <summary>
        /// 生成简单的验证规则名称
        /// Generate simple validation rule name
        /// </summary>
        /// <param name="model">绑定基类实例 / The bindable base instance</param>
        /// <param name="propertyName">属性名称 / The property name</param>
        /// <param name="ruleType">规则类型 / The rule type</param>
        /// <returns>生成的规则名称 / The generated rule name</returns>
        private static string GenerateEasyValidationRuleName(this BindableBase model, string propertyName, [CallerMemberName] string ruleType = null)
        {
            return $"{model.BindableInstanceId}-{propertyName}-{Guid.NewGuid()}";
        }

        /// <summary>
        /// 必填字段验证（模型级别）
        /// Required field validation (model level)
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标属性表达式 / The target property expression</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> Required<TModel, TValue>(
          this ModelValiditionSequenceContext<TModel> context,
              Expression<Func<TModel, TValue>> property,
              Func<string> messageFactory =null
        )
            where TModel : BindableBase<TModel>
        {
            messageFactory= messageFactory??new Func<string>( ()=> REQUIRED_MESSAGE);
            var propertyName = property.GetPropertyName();
            var easyRuleName = context.Model.GenerateEasyValidationRuleName(propertyName);
                return context.MismatchThenMessage(property,
                m => 
                {
                    var value = (TValue)m.GetValueContainer(propertyName).Value;
                    if (typeof(TValue) == typeof(String))
                    {
                        return !string.IsNullOrEmpty(value as string);
                    }
                    return !EqualityComparer<TValue>.Default.Equals(value, default);
                },
                messageFactory, easyRuleName);
        }


        /// <summary>
        /// 必填字段验证（属性级别）
        /// Required field validation (property level)
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">属性验证序列上下文 / The property validation sequence context</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>属性验证序列上下文 / Property validation sequence context</returns>
        public static PropertyValiditionSequenceContext<TModel, TValue> Required<TModel, TValue>(
            this PropertyValiditionSequenceContext<TModel, TValue> context,
            Func<string> messageFactory=null
        )
            where TModel : BindableBase<TModel>
        {
            context.ModelContext.Required(context.PropertyExpression, messageFactory);
            return context;
        }

        /// <summary>
        /// 值选择范围验证
        /// Value in choices validation
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标属性表达式 / The target property expression</param>
        /// <param name="choices">允许的值集合 / The allowed value choices</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> InChoices<TModel, TValue>(
          this ModelValiditionSequenceContext<TModel> context,
              Expression<Func<TModel, TValue>> property,
              ISet<TValue> choices,
              Func<string> messageFactory =null
        )
            where TModel : BindableBase<TModel>
        {
            messageFactory = messageFactory ?? new Func<string>(() => INCHOICES_MESSAGE);

            var propertyName = property.GetPropertyName();
            var easyRuleName = context.Model.GenerateEasyValidationRuleName(propertyName);


            return context.MismatchThenMessage(property,
                m =>
                {
                    var value = (TValue)m.GetValueContainer(propertyName).Value;
                    return choices.Contains(value);
                },
                messageFactory, easyRuleName,null,false );
        }


        /// <summary>
        /// 值选择范围验证（属性级别）
        /// Value in choices validation (property level)
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">属性值类型 / The property value type</typeparam>
        /// <param name="context">属性验证序列上下文 / The property validation sequence context</param>
        /// <param name="choices">允许的值集合 / The allowed value choices</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>属性验证序列上下文 / Property validation sequence context</returns>
        public static PropertyValiditionSequenceContext<TModel, TValue> InChoices<TModel, TValue>(
            this PropertyValiditionSequenceContext<TModel, TValue> context,
            ISet<TValue> choices,
            Func<string> messageFactory= null
        )
            where TModel : BindableBase<TModel>
        {
            context.ModelContext.InChoices(context.PropertyExpression, choices, messageFactory);
            return context;
        }

        /// <summary>
        /// 字符串长度验证
        /// String length validation
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标字符串属性表达式 / The target string property expression</param>
        /// <param name="min">最小长度 / The minimum length</param>
        /// <param name="max">最大长度 / The maximum length</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> StringLength<TModel>(
          this ModelValiditionSequenceContext<TModel> context,
              Expression<Func<TModel, string>> property,
              int? min = null,
              int? max = null,
              Func<string> messageFactory=null )
        where TModel : BindableBase<TModel>
        {
            messageFactory = messageFactory ?? new Func<string>(() => STRINGLENGTH_MESSAGE);

            var propertyName = property.GetPropertyName();
            var easyRuleName = context.Model.GenerateEasyValidationRuleName(propertyName);

            return context.MismatchThenMessage(property,
                m =>
                  {
                      var value = m.GetValueContainer(propertyName).Value as string;
                      var l = value?.Length ?? 0;
                      return (min.HasValue ? l >= min : true) && (max.HasValue ? l <= max : true);
                  },
                messageFactory,
                easyRuleName);
        }

        /// <summary>
        /// 字符串长度验证（属性级别）
        /// String length validation (property level)
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="context">属性验证序列上下文 / The property validation sequence context</param>
        /// <param name="min">最小长度 / The minimum length</param>
        /// <param name="max">最大长度 / The maximum length</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>属性验证序列上下文 / Property validation sequence context</returns>
        public static PropertyValiditionSequenceContext<TModel, string> StringLength<TModel>(
            this PropertyValiditionSequenceContext<TModel, string> context,
                int? min = null,
                int? max = null,
                Func<string> messageFactory=null)
            where TModel : BindableBase<TModel>
        {
            context.ModelContext.StringLength(context.PropertyExpression, min, max, messageFactory);
            return context;
        }


        /// <summary>
        /// 正则表达式验证
        /// Regular expression validation
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="context">模型验证序列上下文 / The model validation sequence context</param>
        /// <param name="property">目标字符串属性表达式 / The target string property expression</param>
        /// <param name="pattern">正则表达式模式 / The regular expression pattern</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>模型验证序列上下文 / Model validation sequence context</returns>
        public static ModelValiditionSequenceContext<TModel> Regex<TModel>(
            this ModelValiditionSequenceContext<TModel> context,
            Expression<Func<TModel, string>> property,
            string pattern,
            Func<string> messageFactory=null
        )
              where TModel : BindableBase<TModel>
        {
            messageFactory = messageFactory ?? new Func<string>(() => REGEX_MESSAGE);

            var propertyName = property.GetPropertyName();
            var easyRuleName = context.Model.GenerateEasyValidationRuleName(propertyName);
            if (string.IsNullOrEmpty(pattern))
            {
                throw new InvalidOperationException("Pattern of Regex cannot be null or empty!");
            }
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            return context.MismatchThenMessage(property,
                m =>
                {
                    var value = m.GetValueContainer(propertyName).Value as string;
                    Match match = regex.Match(value);
                    return (match.Success && match.Index == 0 && match.Length == value.Length); ;
                },
                messageFactory,
                easyRuleName);
        }

        /// <summary>
        /// 正则表达式验证（属性级别）
        /// Regular expression validation (property level)
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="context">属性验证序列上下文 / The property validation sequence context</param>
        /// <param name="pattern">正则表达式模式 / The regular expression pattern</param>
        /// <param name="messageFactory">错误消息工厂 / The error message factory</param>
        /// <returns>属性验证序列上下文 / Property validation sequence context</returns>
        public static PropertyValiditionSequenceContext<TModel, string> Regex<TModel>(
            this PropertyValiditionSequenceContext<TModel, string> context,
            string pattern,
            Func<string> messageFactory=null
        )
              where TModel : BindableBase<TModel>
        {
            context.ModelContext.Regex(context.PropertyExpression, pattern, messageFactory);
            return context;

        }

        public static ModelValiditionSequenceContext<TModel> ListCount<TModel, TValue>(
            this ModelValiditionSequenceContext<TModel> context,
              Expression<Func<TModel, IList<TValue>>> property,
              int? min = null,
              int? max = null,
              Func<string> messageFactory=null
        )
        where TModel : BindableBase<TModel>
        {
            messageFactory = messageFactory ?? new Func<string>(() => LISTLIENGTH_MESSAGE);

            var propertyName = property.GetPropertyName();
            var easyRuleName = context.Model.GenerateEasyValidationRuleName(propertyName);


            return context.MismatchThenMessage(property,
                m =>
                {
                    var value = m.GetValueContainer(propertyName).Value as IList<TValue>;
                    var l = value?.Count ?? 0;
                    return (min.HasValue ? l >= min : true) && (max.HasValue ? l <= max : true);
                },
                messageFactory,
                easyRuleName);
        }


        public static PropertyValiditionSequenceContext<TModel, IList<TValue>> ListCount<TModel, TValue>(
            this PropertyValiditionSequenceContext<TModel, IList<TValue>> context,
             int? min = null,
              int? max = null,
              Func<string> messageFactory = null
        )
        where TModel : BindableBase<TModel>
        {

            context.ModelContext.ListCount(context.PropertyExpression, min, max, messageFactory);
            return context;

        }


        public static void AddError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext, string message, Exception ex = null, bool throwException = false, params Expression<Func<TModel, object>>[] toProperties)
        where TModel : BindableBase<TModel>
        {
            var containers = namedContext.Context.Model.GetValueContainers(toProperties);
            InternalAddError(namedContext, message, ex, containers, throwException);

        }

        /// <summary>
        /// 添加错误信息到指定属性
        /// Add error message to specified properties
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="namedContext">命名上下文 / The named context</param>
        /// <param name="message">错误消息 / The error message</param>
        /// <param name="ex">异常信息 / The exception</param>
        /// <param name="throwException">是否抛出异常 / Whether to throw exception</param>
        /// <param name="toProperties">目标属性数组 / The target properties array</param>
        public static void AddError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext, string message, Exception ex = null, bool throwException = false, params string[] toProperties)
            where TModel : BindableBase<TModel>
        {
            var containers = namedContext.Context.Model.GetValueContainers(toProperties);
            InternalAddError(namedContext, message, ex, containers, throwException);
        }

        /// <summary>
        /// 添加错误信息到所有监听的字段
        /// Add error message to all listened fields
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="namedContext">命名上下文 / The named context</param>
        /// <param name="message">错误消息 / The error message</param>
        /// <param name="ex">异常信息 / The exception</param>
        /// <param name="throwException">是否抛出异常 / Whether to throw exception</param>
        public static void AddError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext, string message, Exception ex = null, bool throwException = false)
          where TModel : BindableBase<TModel>
        {
            var containers = namedContext.Context.FieldsListenedTo.Value.Values;
            InternalAddError(namedContext, message, ex, containers, throwException);
        }

        /// <summary>
        /// 内部错误添加方法
        /// Internal error addition method
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="namedContext">命名上下文 / The named context</param>
        /// <param name="message">错误消息 / The error message</param>
        /// <param name="ex">异常信息 / The exception</param>
        /// <param name="containers">值容器集合 / The value containers collection</param>
        /// <param name="throwException">是否抛出异常 / Whether to throw exception</param>
        private static void InternalAddError<TModel>((string RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext, string message, Exception ex, IEnumerable<IValueContainer> containers, bool throwException) where TModel : BindableBase<TModel>
        {
            foreach (var container in containers)
            {
                container.AddErrorEntry(namedContext.RuleName, message, ex);
            }

            if (throwException && ex != null)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除指定规则的错误信息
        /// Remove error message for specified rule
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="namedContext">命名上下文 / The named context</param>
        public static void RemoveError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext)
          where TModel : BindableBase<TModel>
        {
            var model = namedContext.Context.Model;
            var containers = model.GetValueContainers(model.GetFieldNames());
            foreach (var container in containers)
            {
                container.RemoveErrorEntry(namedContext.RuleName);
            }
        }

        /// <summary>
        /// 清除所有错误信息
        /// Clear all error messages
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <param name="namedContext">命名上下文 / The named context</param>
        public static void ClearError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext)
          where TModel : BindableBase<TModel>
        {
            var model = namedContext.Context.Model;
            var containers = model.GetValueContainers(model.GetFieldNames());
            foreach (var container in containers)
            {
                container.ClearErrorEntries();
            }
        }

       


    }
}
