namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>可绑定类，继承自BindableBase提供基础绑定功能</para>
    /// <para>Bindable class, inherits from BindableBase providing basic binding functionality</para>
    /// </summary>
    /// <typeparam name="TBindable">可绑定类型 / Bindable type</typeparam>
    public class Bindable<TBindable> : BindableBase<TBindable> where TBindable : Bindable<TBindable>
    { }
}
