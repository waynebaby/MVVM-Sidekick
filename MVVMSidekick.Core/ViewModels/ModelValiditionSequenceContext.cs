using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;

namespace MVVMSidekick.ViewModels
{
    public struct ModelValiditionSequenceContext<TModel>
    {
        public TModel Model { get; set; }
        public IObservable<(TModel Model, IValueContainer ValueContainer, ValueChangedEventArgs EventArgs)> ListenChangedSequence { get; set; }
        public Lazy<IDictionary<string, IValueContainer>> FieldsListenedTo { get; set; }
    }
}
