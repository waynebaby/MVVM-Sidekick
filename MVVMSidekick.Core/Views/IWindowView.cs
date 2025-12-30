namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>窗口视图接口，表示窗口类型的视图</para>
    /// <para>Interface for window view that represents a window type view</para>
    /// </summary>
    public interface IWindowView:IView
    {
        /// <summary>
        /// <para>获取或设置是否需要自动设置窗口所有者</para>
        /// <para>Gets or sets whether automatic owner setting is needed</para>
        /// </summary>
        bool IsAutoOwnerSetNeeded { get; set; }
    }
}