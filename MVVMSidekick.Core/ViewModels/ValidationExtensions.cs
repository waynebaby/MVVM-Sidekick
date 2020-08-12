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
    public static class ValidationExtensions
    {
        private const string REQUIRED_MESSAGE = "Field should not be empty.";
        private const string INCHOICES_MESSAGE = "Value is out of Choices";
        private const string STRINGLENGTH_MESSAGE = "The length of string is out of limitation.";
        private const string REGEX_MESSAGE = "Pattern mismatch.";
        private const string LISTLIENGTH_MESSAGE = "The item count of list is out of limitation.";

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

        private static string GenerateEasyValidationRuleName(this BindableBase model, string propertyName, [CallerMemberName] string ruleType = null)
        {
            return $"{model.BindableInstanceId}-{propertyName}-{Guid.NewGuid()}";
        }


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


        public static PropertyValiditionSequenceContext<TModel, TValue> Required<TModel, TValue>(
            this PropertyValiditionSequenceContext<TModel, TValue> context,
            Func<string> messageFactory=null
        )
            where TModel : BindableBase<TModel>
        {
            context.ModelContext.Required(context.PropertyExpression, messageFactory);
            return context;
        }



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
            var regex = new Regex(pattern);

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

        public static void AddError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext, string message, Exception ex = null, bool throwException = false, params string[] toProperties)
            where TModel : BindableBase<TModel>
        {
            var containers = namedContext.Context.Model.GetValueContainers(toProperties);
            InternalAddError(namedContext, message, ex, containers, throwException);
        }

        public static void AddError<TModel>(this (String RuleName, ModelValiditionSequenceContext<TModel> Context) namedContext, string message, Exception ex = null, bool throwException = false)
          where TModel : BindableBase<TModel>
        {
            var containers = namedContext.Context.FieldsListenedTo.Value.Values;

            InternalAddError(namedContext, message, ex, containers, throwException);

        }

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
