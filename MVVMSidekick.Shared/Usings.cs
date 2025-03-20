
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using MVVMSidekick.ViewModels;
global using System.Reactive.Linq;
global using System.Reactive.Subjects;
global using System.Windows;
global using System.IO;
global using MVVMSidekick.Services;
global using MVVMSidekick.EventRouting;
global using System.Threading;
 
global using System.Reflection;

global using Microsoft.Extensions.DependencyInjection;




#if WINDOWS_UWP
global using Windows.UI.Xaml;
global using Windows.UI.Xaml.Controls;
global using Windows.UI.Xaml.Navigation;
global using Windows.UI.Xaml.Media; 
global using Microsoft.Xaml.Interactivity;
global using Windows.UI.Xaml.Data;
global using Windows.Foundation;
#endif







#if WPF
global using System.Windows.Controls;
global using System.Windows.Media;

global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Data;
global using Microsoft.Xaml.Behaviors;
global using System.Collections.Concurrent;
global using System.Windows.Navigation;

global using MVVMSidekick.Views;
global using System.Windows.Controls.Primitives;
global using MVVMSidekick.Utilities;
#endif

#if WinUI3
global using Microsoft.UI.Xaml;
global using Microsoft.UI.Xaml.Controls;
global using Microsoft.UI.Xaml.Navigation;
global using Microsoft.UI.Xaml.Media;
global using Microsoft.UI.Xaml.Data;
global using Microsoft.Xaml.Interactivity;
global using DependencyObject = Microsoft.UI.Xaml.DependencyObject;
global using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;
global using FrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
global using PropertyMetadata = Microsoft.UI.Xaml.PropertyMetadata;
global using RoutedEventHandler = Microsoft.UI.Xaml.RoutedEventHandler;
global using UIElement = Microsoft.UI.Xaml.UIElement;
global using PropertyPath = Microsoft.UI.Xaml.PropertyPath;
global using PropertyChangedCallback = Microsoft.UI.Xaml.PropertyChangedCallback;

global using Panel = Microsoft.UI.Xaml.Controls.Panel;
#endif