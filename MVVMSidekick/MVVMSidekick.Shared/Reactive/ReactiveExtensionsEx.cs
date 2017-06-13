using MVVMSidekick.Utilities;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;

namespace MVVMSidekick.Reactive
{

    public class ObservableWithModel<TModel> : ObservableWithModel<TModel, ValueChangedEventArgs>
    {
        public ObservableWithModel(TModel model, IObservable<ValueChangedEventArgs> core) : base(model, core)
        {
            //        _core = core;
            //        Model = model;
        }

    }

    public class ObservableWithModel<TModel, TValue> : IObservable<TValue>
    {
        public ObservableWithModel(TModel model, IObservable<TValue> core)
        {
            _core = core;
            Model = model;
        }
        IObservable<TValue> _core;
        public TModel Model { get; private set; }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            return _core.Subscribe(observer);
        }
    }
    /// <summary>
    /// Addistional Extension Methods
    /// </summary>
    public static class ReactiveExtensionsEx
    {


        /// <summary>
        /// Listens to the changed properties and merge the event to a sequence.
        /// </summary>
        /// <typeparam name="TModel">The type of the source model.</typeparam>
        /// <typeparam name="TValue">The type of the source value.</typeparam>
        /// <param name="source">The source model.</param>
        /// <param name="property">The properties expressions.</param>
        /// <returns>Event sequence</returns>
        public static ObservableWithModel<TModel> Listen<TModel, TValue>(this TModel source,
                 Expression<Func<TModel, TValue>> property
            ) where TModel : BindableBase<TModel>
        {


            var value = source.GetValueContainer(property).Value;

            var rval = new ObservableWithModel<TModel>(
                source,
                source
                    .GetValueContainer(property)
                    .GetEventObservable()
                    .Select(y =>
                        y.EventArgs)
                    .StartWith(
                        new ValueChangedEventArgs<TValue>(property.Name, default(TValue), value)));


            return rval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sourceOb"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static ObservableWithModel<TModel, IDictionary<string, ValueChangedEventArgs>> Also<TModel, TValue>(this ObservableWithModel<TModel> sourceOb,
                Expression<Func<TModel, TValue>> property
           ) where TModel : BindableBase<TModel>
        {

            var source = sourceOb.Model;
            var value = source.GetValueContainer(property).Value;
            var seq1 = sourceOb;
            var seq2 = source
                    .GetValueContainer(property)
                    .GetEventObservable()
                    .Select(y =>
                        y.EventArgs)
                    .StartWith(
                        new ValueChangedEventArgs<TValue>(property.Name, default(TValue), value));

            var newseq = seq1.Zip(
                seq2,
                (i1, i2) => new Dictionary<string, ValueChangedEventArgs>()
                {
                    { i1.PropertyName, i1 },
                    { i2.PropertyName, i2 }
                });

            return new ObservableWithModel<TModel, IDictionary<string, ValueChangedEventArgs>>(
                    source,
                    newseq
                );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sourceOb"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static ObservableWithModel<TModel, IDictionary<string, ValueChangedEventArgs>> Also<TModel, TValue>(this ObservableWithModel<TModel, IDictionary<string, ValueChangedEventArgs>> sourceOb,
         Expression<Func<TModel, TValue>> property
            ) where TModel : BindableBase<TModel>
        {

            var source = sourceOb.Model;
            var value = source.GetValueContainer(property).Value;
            var seq1 = sourceOb;
            var seq2 = source
                    .GetValueContainer(property)
                    .GetEventObservable()
                    .Select(y =>
                        y.EventArgs)
                    .StartWith(
                        new ValueChangedEventArgs<TValue>(property.Name, default(TValue), value));

            var newseq = seq1.Zip(
                seq2,
                (i1, i2) =>
                {
                    i1[i2.PropertyName] = i2;
                    return i1;
                });

            return new ObservableWithModel<TModel, IDictionary<string, ValueChangedEventArgs>>(
                    source,
                    newseq
                );
        }

    }


}
