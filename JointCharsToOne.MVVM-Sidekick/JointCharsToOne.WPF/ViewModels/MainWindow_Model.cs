using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows;

namespace JointCharsToOne.WPF.ViewModels
{

    public class MainWindow_Model : ViewModelBase<MainWindow_Model>
    {



        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<String> _TitleDefaultValueFactory = () => "Title is Here";
        #endregion

        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性


        protected override async Task OnBindedToView(IView view, IViewModel oldValue)
        {
            await base.OnBindedToView(view, oldValue);
            // This method will be called when this VM is set to a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.

            if (!IsInDesignMode)
            {
                //不输入就不能进行
                this.GetValueContainer(x => x.CharsToInput)
                    .GetNewValueObservable()
                    .Select(e => !string.IsNullOrEmpty(e.EventArgs))
                    .Subscribe(this.CommandCreateWorkspace.CommandCore.CanExecuteObserver)
                    .DisposeWith(this);
            }
        }

        protected override async Task OnUnbindedFromView(IView view, IViewModel newValue)
        {
            await base.OnUnbindedFromView(view, newValue);
            // This method will be called when this VM is removed from a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }



        public string CharsToInput
        {
            get { return _CharsToInputLocator(this).Value; }
            set { _CharsToInputLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string CharsToInput Setup
        protected Property<string> _CharsToInput = new Property<string> { LocatorFunc = _CharsToInputLocator };
        static Func<BindableBase, ValueContainer<string>> _CharsToInputLocator = RegisterContainerLocator<string>("CharsToInput", model => model.Initialize("CharsToInput", ref model._CharsToInput, ref _CharsToInputLocator, _CharsToInputDefaultValueFactory));
        static Func<string> _CharsToInputDefaultValueFactory = () => "雅哈";
        #endregion



        public string Message
        {
            get { return _MessageLocator(this).Value; }
            set { _MessageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Message Setup
        protected Property<string> _Message = new Property<string> { LocatorFunc = _MessageLocator };
        static Func<BindableBase, ValueContainer<string>> _MessageLocator = RegisterContainerLocator<string>("Message", model => model.Initialize("Message", ref model._Message, ref _MessageLocator, _MessageDefaultValueFactory));
        static Func<string> _MessageDefaultValueFactory = () => "请输入要合并的字";
        #endregion



        public CommandModel<ReactiveCommand, String> CommandCreateWorkspace
        {
            get { return _CommandCreateWorkspaceLocator(this).Value; }
            set { _CommandCreateWorkspaceLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandCreateWorkspace Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandCreateWorkspace = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandCreateWorkspaceLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandCreateWorkspaceLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandCreateWorkspace", model => model.Initialize("CommandCreateWorkspace", ref model._CommandCreateWorkspace, ref _CommandCreateWorkspaceLocator, _CommandCreateWorkspaceDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandCreateWorkspaceDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                cmd.Subscribe(
                    async _ =>
                    {
                        var vm = CastToCurrentType(model);
                        var wt = await vm.StageManager.DefaultStage.ShowAndGetViewModel<Workspace_Model>();
                        using (var newvm = wt.ViewModel)
                        {
                            var chars = vm.CharsToInput;
                            double width = (double)1 / (double)(chars.Length);
                            double left = 0;


                            var s = ServiceLocator.Instance.Resolve<ITextToPathService>();
                            foreach (var c in chars)
                            {
                                var ci = new CharItem(newvm, s)
                                        {
                                            CharToDisplay = c,
                                            DisplayLocationX = 0,
                                            DisplayLocationY = 0,
                                            DisplayZoomX = 1,
                                            DisplayZoomY = 1

                                        };
                                newvm.CharsToDisplay
                                    .Add(ci);

                            }
                            await wt.Closing;
                        }

                    }).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("开始调整");
            };
        #endregion



        public CommandModel<ReactiveCommand, String> CommandLoadXml
        {
            get { return _CommandLoadXmlLocator(this).Value; }
            set { _CommandLoadXmlLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandLoadXml Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandLoadXml = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandLoadXmlLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandLoadXmlLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandLoadXml", model => model.Initialize("CommandLoadXml", ref model._CommandLoadXml, ref _CommandLoadXmlLocator, _CommandLoadXmlDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandLoadXmlDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                cmd.Subscribe(
                    async _ =>
                    {
                        var vm = CastToCurrentType(model);
                        var dg = new Microsoft.Win32.OpenFileDialog();
                        dg.Filter = "xml|*.xml"; 
                        var rs = dg.ShowDialog();
                        if (rs==null ||(!rs.Value ))
                        {
                            return;
                        }
                        var wt = await vm.StageManager.DefaultStage.ShowAndGetViewModel<Workspace_Model>();
                        using (var newvm = wt.ViewModel)
                        {

                            if (rs.HasValue && rs.Value)
                            {
                                var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(ObservableCollection<CharItem>));
                                using (var fs = dg.OpenFile())
                                {
                                    newvm.CharsToDisplay = ser.ReadObject(fs) as ObservableCollection<CharItem>;

                                }

                                MessageBox.Show("Load OK");
                            }
                            var service = ServiceLocator.Instance.Resolve<ITextToPathService>();
                            foreach (var item in newvm.CharsToDisplay)
                            {
                                item.WireEvents(newvm, service);

                            }



                            await newvm.WaitForClose();

                        }

                    }).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("从文件读取");
            };
        #endregion



    }

}

