#if !BLAZOR


namespace MVVMSidekick.Views
{
    public static class ViewDisguiseManager
    {


        public static IViewDisguise GetViewDisguise(this DependencyObject dobj)
        {
            return (IViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
        }

        public static void SetViewDisguise(this DependencyObject dobj, IViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }

        public static IViewDisguise GetOrCreateViewDisguise(this DependencyObject dobj)
        {
            var dis = (IViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                switch (dobj)
                {
                    case Page p:
                        dis = p.GetOrCreateViewDisguise();
                        break;

                    case UserControl c:
                        dis = c.GetOrCreateViewDisguise();
                        break;

#if WPF
                    case Window w:
                        dis = w.GetOrCreateViewDisguise();
                        break;
#endif
                    default:
                        throw new InvalidOperationException($"This kind of view ({ dobj?.GetType()?.Name ?? "null"}) is not supported");
                }
                dobj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }



        public static PageViewDisguise GetOrCreateViewDisguise(this Page dobj)
        {
            var dis = (PageViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new PageViewDisguise(dobj);
                dobj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        public static void SetViewDisguise(this Page dobj, PageViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }


        public static ControlViewDisguise GetOrCreateViewDisguise(this UserControl dobj)
        {
            var dis = (ControlViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new ControlViewDisguise(dobj);
                dobj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        public static void SetViewDisguise(this UserControl dobj, PageViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }

#if WPF

        public static WindowViewDisguise GetOrCreateViewDisguise(this Window dobj)
        {
            var dis = (WindowViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new WindowViewDisguise(dobj);
                dobj?.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        public static void SetViewDisguise(this Window dobj, WindowViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }
#endif

        // Using a DependencyProperty as the backing store for ViewDisguise.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewDisguiseProperty =
            DependencyProperty.RegisterAttached(nameof(GetViewDisguise).Remove(0, 3), typeof(IViewDisguise), typeof(ViewDisguiseManager), new PropertyMetadata(null));



    }
}
#endif