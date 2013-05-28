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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Markup;

namespace JointCharsToOne.WPF.ViewModels
{


    [DataContract] //if you want

    public class CharItem : BindableBase<CharItem>
    {
        public CharItem()
        {
        }

        public CharItem(Workspace_Model ws, ITextToPathService service)
        {
            // Use propery to init value here:
            if (IsInDesignMode)
            {
                //Add design time demo data init here. These will not execute in runtime.
            }
            else
            {

                WireEvents(ws, service);

            }


        }

        public void WireEvents(Workspace_Model ws, ITextToPathService service)
        {


            ws.GetValueContainer(x => x.FontSize)
               .GetNullObservable()
                .Merge(
                    Observable.Range(0, 1).Select(i => (object)null)

                    )
               .Subscribe
               (
                   e => FontSize = ws.FontSize
               )
               .DisposeWith(ws);



            ws.GetValueContainer(x => x.SelectedFont)
               .GetNullObservable()
                .Merge(
                    Observable.Range(0, 1).Select(i => (object)null)

                    )
               .Subscribe
               (
                   e => TextfaceString = ws.SelectedFont
               )
               .DisposeWith(ws);

            GetValueContainer(x => x.FontSize)
                .GetNullObservable()
                .Merge(
                     GetValueContainer(x => x.DisplayLocationX)
                        .GetNullObservable()
                    )
                .Merge(
                     GetValueContainer(x => x.DisplayLocationY)
                        .GetNullObservable()
                    )
                .Merge(
                    Observable.Range(0, 1).Select(i => (object)null)

                    )
                    .Subscribe
                    (
                        _ =>
                        {
                            LocationXPixel = FontSize * DisplayLocationX;
                            LocationYPixel = FontSize * DisplayLocationY;
                        }
                    ).DisposeWith(ws);





            GetValueContainer(x => x.FontSize)
                .GetNullObservable()
                .Merge(
                     GetValueContainer(x => x.DisplayZoomX)
                        .GetNullObservable()
                    )
                .Merge(
                     GetValueContainer(x => x.DisplayZoomY)
                        .GetNullObservable()
                    )
                .Merge(
                     GetValueContainer(x => x.DisplayLocationX)
                        .GetNullObservable()
                    )
                .Merge(
                     GetValueContainer(x => x.DisplayLocationY)
                        .GetNullObservable()
                    )
                        .Merge(
                     GetValueContainer(x => x.MaskRetanglePercentLeft)
                        .GetNullObservable()
                    )
                            .Merge(
                     GetValueContainer(x => x.MaskRetanglePercentRight)
                        .GetNullObservable()
                    )
                            .Merge(
                     GetValueContainer(x => x.MaskRetanglePercentTop)
                        .GetNullObservable()
                    )
                     .Merge(
                     GetValueContainer(x => x.MaskRetanglePercentBottom)
                        .GetNullObservable()
                    )
                            .Merge(
                     GetValueContainer(x => x.TextfaceString)
                        .GetNullObservable()
                    )
                            .Merge(
                     GetValueContainer(x => x.CharToDisplay)
                        .GetNullObservable()
                    )
                            .Merge(
                     GetValueContainer(x => x.DisplayLocationY)
                        .GetNullObservable()
                    )
                .Subscribe
                (
                     e =>
                     {
                         var masks = new Thickness
                             (
                                FontSize * MaskRetanglePercentLeft,
                                FontSize * MaskRetanglePercentTop,
                                FontSize * MaskRetanglePercentRight,
                                FontSize * MaskRetanglePercentBottom

                             );

                         PathData = service.Text2Path(this.CharToDisplay.ToString(), "", true, TextfaceString, (int)FontSize, masks);

                     }


                ).DisposeWith(ws);
        }

        [DataMember]
        public double MaskRetanglePercentLeft
        {
            get { return _MaskRetanglePercentLeftLocator(this).Value; }
            set { _MaskRetanglePercentLeftLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double MaskRetanglePercentLeft Setup
        protected Property<double> _MaskRetanglePercentLeft = new Property<double> { LocatorFunc = _MaskRetanglePercentLeftLocator };
        static Func<BindableBase, ValueContainer<double>> _MaskRetanglePercentLeftLocator = RegisterContainerLocator<double>("MaskRetanglePercentLeft", model => model.Initialize("MaskRetanglePercentLeft", ref model._MaskRetanglePercentLeft, ref _MaskRetanglePercentLeftLocator, _MaskRetanglePercentLeftDefaultValueFactory));
        static Func<double> _MaskRetanglePercentLeftDefaultValueFactory = () => 0;
        #endregion

        [DataMember]
        public double MaskRetanglePercentRight
        {
            get { return _MaskRetanglePercentRightLocator(this).Value; }
            set { _MaskRetanglePercentRightLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double MaskRetanglePercentRight Setup
        protected Property<double> _MaskRetanglePercentRight = new Property<double> { LocatorFunc = _MaskRetanglePercentRightLocator };
        static Func<BindableBase, ValueContainer<double>> _MaskRetanglePercentRightLocator = RegisterContainerLocator<double>("MaskRetanglePercentRight", model => model.Initialize("MaskRetanglePercentRight", ref model._MaskRetanglePercentRight, ref _MaskRetanglePercentRightLocator, _MaskRetanglePercentRightDefaultValueFactory));
        static Func<double> _MaskRetanglePercentRightDefaultValueFactory = () => 0;
        #endregion


        [DataMember]
        public double MaskRetanglePercentTop
        {
            get { return _MaskRetanglePercentTopLocator(this).Value; }
            set { _MaskRetanglePercentTopLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double MaskRetanglePercentTop Setup
        protected Property<double> _MaskRetanglePercentTop = new Property<double> { LocatorFunc = _MaskRetanglePercentTopLocator };
        static Func<BindableBase, ValueContainer<double>> _MaskRetanglePercentTopLocator = RegisterContainerLocator<double>("MaskRetanglePercentTop", model => model.Initialize("MaskRetanglePercentTop", ref model._MaskRetanglePercentTop, ref _MaskRetanglePercentTopLocator, _MaskRetanglePercentTopDefaultValueFactory));
        static Func<double> _MaskRetanglePercentTopDefaultValueFactory = () => 0;
        #endregion

        [DataMember]
        public double MaskRetanglePercentBottom
        {
            get { return _MaskRetanglePercentBottomLocator(this).Value; }
            set { _MaskRetanglePercentBottomLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double MaskRetanglePercentBottom Setup
        protected Property<double> _MaskRetanglePercentBottom = new Property<double> { LocatorFunc = _MaskRetanglePercentBottomLocator };
        static Func<BindableBase, ValueContainer<double>> _MaskRetanglePercentBottomLocator = RegisterContainerLocator<double>("MaskRetanglePercentBottom", model => model.Initialize("MaskRetanglePercentBottom", ref model._MaskRetanglePercentBottom, ref _MaskRetanglePercentBottomLocator, _MaskRetanglePercentBottomDefaultValueFactory));
        static Func<double> _MaskRetanglePercentBottomDefaultValueFactory = () => 0;
        #endregion


        [DataMember]
        public String TextfaceString
        {
            get { return _TextfaceStringLocator(this).Value; }
            set { _TextfaceStringLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String TextfaceString Setup
        protected Property<String> _TextfaceString = new Property<String> { LocatorFunc = _TextfaceStringLocator };
        static Func<BindableBase, ValueContainer<String>> _TextfaceStringLocator = RegisterContainerLocator<String>("TextfaceString", model => model.Initialize("TextfaceString", ref model._TextfaceString, ref _TextfaceStringLocator, _TextfaceStringDefaultValueFactory));
        static Func<String> _TextfaceStringDefaultValueFactory = null;
        #endregion


        [DataMember]
        public string PathData
        {
            get { return _PathDataLocator(this).Value; }
            set { _PathDataLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string PathData Setup
        protected Property<string> _PathData = new Property<string> { LocatorFunc = _PathDataLocator };
        static Func<BindableBase, ValueContainer<string>> _PathDataLocator = RegisterContainerLocator<string>("PathData", model => model.Initialize("PathData", ref model._PathData, ref _PathDataLocator, _PathDataDefaultValueFactory));
        static Func<string> _PathDataDefaultValueFactory = () => "";
        #endregion

        [DataMember]
        public Double FontSize
        {
            get { return _FontSizeLocator(this).Value; }
            set { _FontSizeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Double FontSize Setup
        protected Property<Double> _FontSize = new Property<Double> { LocatorFunc = _FontSizeLocator };
        static Func<BindableBase, ValueContainer<Double>> _FontSizeLocator = RegisterContainerLocator<Double>("FontSize", model => model.Initialize("FontSize", ref model._FontSize, ref _FontSizeLocator, _FontSizeDefaultValueFactory));
        static Func<Double> _FontSizeDefaultValueFactory = null;
        #endregion

        [DataMember]
        public double LocationXPixel
        {
            get { return _LocationXPixelLocator(this).Value; }
            set { _LocationXPixelLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double  LocationXPixel Setup
        protected Property<double> _LocationXPixel = new Property<double> { LocatorFunc = _LocationXPixelLocator };
        static Func<BindableBase, ValueContainer<double>> _LocationXPixelLocator = RegisterContainerLocator<double>("LocationXPixel", model => model.Initialize("LocationXPixel", ref model._LocationXPixel, ref _LocationXPixelLocator, _LocationXPixelDefaultValueFactory));
        static Func<double> _LocationXPixelDefaultValueFactory = null;
        #endregion


        [DataMember]
        public double LocationYPixel
        {
            get { return _LocationYPixelLocator(this).Value; }
            set { _LocationYPixelLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double LocationYPixel Setup
        protected Property<double> _LocationYPixel = new Property<double> { LocatorFunc = _LocationYPixelLocator };
        static Func<BindableBase, ValueContainer<double>> _LocationYPixelLocator = RegisterContainerLocator<double>("LocationYPixel", model => model.Initialize("LocationYPixel", ref model._LocationYPixel, ref _LocationYPixelLocator, _LocationYPixelDefaultValueFactory));
        static Func<double> _LocationYPixelDefaultValueFactory = null;
        #endregion

        [DataMember]
        public Char CharToDisplay
        {
            get { return _CharToDisplayLocator(this).Value; }
            set { _CharToDisplayLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Char CharToDisplay Setup
        protected Property<Char> _CharToDisplay = new Property<Char> { LocatorFunc = _CharToDisplayLocator };
        static Func<BindableBase, ValueContainer<Char>> _CharToDisplayLocator = RegisterContainerLocator<Char>("CharToDisplay", model => model.Initialize("CharToDisplay", ref model._CharToDisplay, ref _CharToDisplayLocator, _CharToDisplayDefaultValueFactory));
        static Func<Char> _CharToDisplayDefaultValueFactory = () => '哈';
        #endregion

        [DataMember]
        public double DisplayZoomX
        {
            get { return _DisplayZoomXLocator(this).Value; }
            set { _DisplayZoomXLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double DisplayZoomX Setup
        protected Property<double> _DisplayZoomX = new Property<double> { LocatorFunc = _DisplayZoomXLocator };
        static Func<BindableBase, ValueContainer<double>> _DisplayZoomXLocator = RegisterContainerLocator<double>("DisplayZoomX", model => model.Initialize("DisplayZoomX", ref model._DisplayZoomX, ref _DisplayZoomXLocator, _DisplayZoomXDefaultValueFactory));
        static Func<double> _DisplayZoomXDefaultValueFactory = () => 1;
        #endregion


        [DataMember]
        public double DisplayZoomY
        {
            get { return _DisplayZoomYLocator(this).Value; }
            set { _DisplayZoomYLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double DisplayZoomY Setup
        protected Property<double> _DisplayZoomY = new Property<double> { LocatorFunc = _DisplayZoomYLocator };
        static Func<BindableBase, ValueContainer<double>> _DisplayZoomYLocator = RegisterContainerLocator<double>("DisplayZoomY", model => model.Initialize("DisplayZoomY", ref model._DisplayZoomY, ref _DisplayZoomYLocator, _DisplayZoomYDefaultValueFactory));
        static Func<double> _DisplayZoomYDefaultValueFactory = () => 1;
        #endregion


        [DataMember]
        public double DisplayLocationX
        {
            get { return _DisplayLocationXLocator(this).Value; }
            set { _DisplayLocationXLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double DisplayLocationX Setup
        protected Property<double> _DisplayLocationX = new Property<double> { LocatorFunc = _DisplayLocationXLocator };
        static Func<BindableBase, ValueContainer<double>> _DisplayLocationXLocator = RegisterContainerLocator<double>("DisplayLocationX", model => model.Initialize("DisplayLocationX", ref model._DisplayLocationX, ref _DisplayLocationXLocator, _DisplayLocationXDefaultValueFactory));
        static Func<double> _DisplayLocationXDefaultValueFactory = () => 0;
        #endregion


        [DataMember]
        public double DisplayLocationY
        {
            get { return _DisplayLocationYLocator(this).Value; }
            set { _DisplayLocationYLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double DisplayLocationY Setup
        protected Property<double> _DisplayLocationY = new Property<double> { LocatorFunc = _DisplayLocationYLocator };
        static Func<BindableBase, ValueContainer<double>> _DisplayLocationYLocator = RegisterContainerLocator<double>("DisplayLocationY", model => model.Initialize("DisplayLocationY", ref model._DisplayLocationY, ref _DisplayLocationYLocator, _DisplayLocationYDefaultValueFactory));
        static Func<double> _DisplayLocationYDefaultValueFactory = () => 0;
        #endregion

        //Use propvm + tab +tab  to create a new property of bindable here:


    }





    //[DataContract]
    public class Workspace_Model : ViewModelBase<Workspace_Model>
    {
        // If you have install the code sniplets, use"propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性


        protected override async Task OnBindedToView(IView view, IViewModel oldValue)
        {
            await base.OnBindedToView(view, oldValue);
            // This method will be called when this VM is set to a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }

        protected override async Task OnUnbindedFromView(IView view, IViewModel newValue)
        {
            await base.OnUnbindedFromView(view, newValue);
            // This method will be called when this VM is removed from a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }



        public String SelectedFont
        {
            get { return _SelectedFontLocator(this).Value; }
            set { _SelectedFontLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String SelectedFont Setup
        protected Property<String> _SelectedFont = new Property<String> { LocatorFunc = _SelectedFontLocator };
        static Func<BindableBase, ValueContainer<String>> _SelectedFontLocator = RegisterContainerLocator<String>("SelectedFont", model => model.Initialize("SelectedFont", ref model._SelectedFont, ref _SelectedFontLocator, _SelectedFontDefaultValueFactory));
        static Func<String> _SelectedFontDefaultValueFactory = null;
        #endregion



        public string ForeColor
        {
            get { return _ForeColorLocator(this).Value; }
            set { _ForeColorLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string ForeColor Setup
        protected Property<string> _ForeColor = new Property<string> { LocatorFunc = _ForeColorLocator };
        static Func<BindableBase, ValueContainer<string>> _ForeColorLocator = RegisterContainerLocator<string>("ForeColor", model => model.Initialize("ForeColor", ref model._ForeColor, ref _ForeColorLocator, _ForeColorDefaultValueFactory));
        static Func<string> _ForeColorDefaultValueFactory = () => "Black";
        #endregion




        public string BackColor
        {
            get { return _BackColorLocator(this).Value; }
            set { _BackColorLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string BackColor Setup
        protected Property<string> _BackColor = new Property<string> { LocatorFunc = _BackColorLocator };
        static Func<BindableBase, ValueContainer<string>> _BackColorLocator = RegisterContainerLocator<string>("BackColor", model => model.Initialize("BackColor", ref model._BackColor, ref _BackColorLocator, _BackColorDefaultValueFactory));
        static Func<string> _BackColorDefaultValueFactory = () => "White";
        #endregion



        public ObservableCollection<String> FontsInSystem
        {
            get { return _FontsInSystemLocator(this).Value; }
            set { _FontsInSystemLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<String> FontsInSystem Setup
        protected Property<ObservableCollection<String>> _FontsInSystem = new Property<ObservableCollection<String>> { LocatorFunc = _FontsInSystemLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<String>>> _FontsInSystemLocator = RegisterContainerLocator<ObservableCollection<String>>("FontsInSystem", model => model.Initialize("FontsInSystem", ref model._FontsInSystem, ref _FontsInSystemLocator, _FontsInSystemDefaultValueFactory));
        static Func<ObservableCollection<String>> _FontsInSystemDefaultValueFactory =
            () => 
                new ObservableCollection<String>( 
            
                Fonts.SystemFontFamilies.Select(x=>x.FamilyNames[XmlLanguage.GetLanguage("en-us") ])

                .ToList ()
                )
                //"BatangChe",
                //"DFKai-SB",
                //"Dotum",
                //"DutumChe",
                //"FangSong",
                //"GulimChe",
                //"Gungsuh",
                //"GungsuhChe",
                //"KaiTi",
                //"Malgun Gothic",
                //"Microsoft JhengHei",
                //"Microsoft YaHei",
                //"MingLiU",
                //"MingLiu_HKSCS",
                //"MingLiu_HKSCS-ExtB",
                //"MingLiu-ExtB",
                //"MS UI Gothic",
                //"NSimSun",
                //"NSimSun-18030",
                //"PMingLiu-ExtB",
                //"SimHei",
                //"SimSun-18030",
                //"SimSun-ExtB"            
            ;
        #endregion



        public String PathForAll
        {
            get { return _PathForAllLocator(this).Value; }
            set { _PathForAllLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String PathForAll Setup
        protected Property<String> _PathForAll = new Property<String> { LocatorFunc = _PathForAllLocator };
        static Func<BindableBase, ValueContainer<String>> _PathForAllLocator = RegisterContainerLocator<String>("PathForAll", model => model.Initialize("PathForAll", ref model._PathForAll, ref _PathForAllLocator, _PathForAllDefaultValueFactory));
        static Func<String> _PathForAllDefaultValueFactory = null;
        #endregion

        public ObservableCollection<CharItem> CharsToDisplay
        {
            get { return _CharsToDisplayLocator(this).Value; }
            set { _CharsToDisplayLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<CharItem>  CharsToDisplay Setup
        protected Property<ObservableCollection<CharItem>> _CharsToDisplay = new Property<ObservableCollection<CharItem>> { LocatorFunc = _CharsToDisplayLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<CharItem>>> _CharsToDisplayLocator = RegisterContainerLocator<ObservableCollection<CharItem>>("CharsToDisplay", model => model.Initialize("CharsToDisplay", ref model._CharsToDisplay, ref _CharsToDisplayLocator, _CharsToDisplayDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<CharItem>> _CharsToDisplayDefaultValueFactory = model =>
           {
               if (IsInDesignMode)
               {
                   return new ObservableCollection<CharItem>()
                   {
                 
                        new CharItem(CastToCurrentType (model),null){ CharToDisplay ='合', DisplayZoomX=.4, DisplayLocationX=.6 }, 
                        new CharItem(CastToCurrentType (model),null){ CharToDisplay ='牙', DisplayZoomX=.6}, 
                   };
               }
               else
               {
                   return new ObservableCollection<CharItem>();
               };
           };
        #endregion



        public double FontSize
        {
            get { return _FontSizeLocator(this).Value; }
            set { _FontSizeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double FontSize Setup
        protected Property<double> _FontSize = new Property<double> { LocatorFunc = _FontSizeLocator };
        static Func<BindableBase, ValueContainer<double>> _FontSizeLocator = RegisterContainerLocator<double>("FontSize", model => model.Initialize("FontSize", ref model._FontSize, ref _FontSizeLocator, _FontSizeDefaultValueFactory));
        static Func<double> _FontSizeDefaultValueFactory = () => 30;
        #endregion


        public CommandModel<ReactiveCommand, String> CommandSaveToFile
        {
            get { return _CommandSaveToFileLocator(this).Value; }
            set { _CommandSaveToFileLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandSaveToFile Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandSaveToFile = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSaveToFileLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSaveToFileLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandSaveToFile", model => model.Initialize("CommandSaveToFile", ref model._CommandSaveToFile, ref _CommandSaveToFileLocator, _CommandSaveToFileDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSaveToFileDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                cmd.Subscribe(
                    async _ =>
                    {
                        var vm = CastToCurrentType(model);
                        var dg = new Microsoft.Win32.SaveFileDialog();
                        dg.Filter = "xml|*.xml";
                        var rs = dg.ShowDialog();
                        if (rs.HasValue && rs.Value)
                        {
                            var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(ObservableCollection<CharItem>));
                            using (var fs = dg.OpenFile())
                            {
                                ser.WriteObject(fs, vm.CharsToDisplay);

                            }

                            MessageBox.Show("Save OK");
                        }




                    }).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("保存到文件");
            };
        #endregion



        public CommandModel<ReactiveCommand, String> CommandCopyToClipBoard
        {
            get { return _CommandCopyToClipBoardLocator(this).Value; }
            set { _CommandCopyToClipBoardLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandCopyToClipBoard Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandCopyToClipBoard = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandCopyToClipBoardLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandCopyToClipBoardLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandCopyToClipBoard", model => model.Initialize("CommandCopyToClipBoard", ref model._CommandCopyToClipBoard, ref _CommandCopyToClipBoardLocator, _CommandCopyToClipBoardDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandCopyToClipBoardDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                cmd.Subscribe(e =>
                {
                    var surface = e.EventArgs.Parameter as FrameworkElement;
                    Transform transform = surface.LayoutTransform;


                    Size size = new Size(surface.ActualWidth, surface.ActualHeight);
                    surface.Measure(size);
                    surface.Arrange(new Rect(size));

                    RenderTargetBitmap renderBitmap =
                      new RenderTargetBitmap(
                        (int)size.Width,
                        (int)size.Height,
                        96d,
                        96d,
                        PixelFormats.Default);
                    renderBitmap.Render(surface);

                    MemoryStream outStream = new MemoryStream();

                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(outStream);

                    outStream.Seek(0, SeekOrigin.Begin);

                    var s = new BitmapImage();
                    s.BeginInit();
                    s.StreamSource = outStream;
                    s.EndInit();
                    Clipboard.SetImage(s);

                    MessageBox.Show("Copy OK");

                }).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("拷贝到剪贴板");
            };
        #endregion



    }
}

