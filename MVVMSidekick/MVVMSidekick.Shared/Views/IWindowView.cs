namespace MVVMSidekick.Views
{
    public interface IWindowView:IView
    {
        bool IsAutoOwnerSetNeeded { get; set; }
    }
}