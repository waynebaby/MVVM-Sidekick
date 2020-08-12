namespace MVVMSidekick.ViewModels
{
    public class Bindable<TBindable> : BindableBase<TBindable> where TBindable : Bindable<TBindable>

    { }
}
