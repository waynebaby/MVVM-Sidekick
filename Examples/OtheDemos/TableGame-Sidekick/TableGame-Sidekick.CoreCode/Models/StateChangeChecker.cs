using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Models
{

    //[DataContract(IsReference=true) ] //if you want
    public class StateChangeChecker<TContext> : BindableBase<StateChangeChecker<TContext>>,IGameModel<TContext>
    {


        public TContext GameExecutingContext
        {
            get { return _GameExecutingContextLocator(this).Value; }
            set { _GameExecutingContextLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property TContext GameExecutingContext Setup        
        protected Property<TContext> _GameExecutingContext = new Property<TContext> { LocatorFunc = _GameExecutingContextLocator };
        static Func<BindableBase, ValueContainer<TContext>> _GameExecutingContextLocator = RegisterContainerLocator<TContext>("GameExecutingContext", model => model.Initialize("GameExecutingContext", ref model._GameExecutingContext, ref _GameExecutingContextLocator, _GameExecutingContextDefaultValueFactory));
        static Func<TContext> _GameExecutingContextDefaultValueFactory = () => default(TContext);
        #endregion


        /// <summary>
        /// 名  Name
        /// </summary>
        public string Name
        {
            get { return _NameLocator(this).Value; }
            set { _NameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Name Setup        
        protected Property<string> _Name = new Property<string> { LocatorFunc = _NameLocator };
        static Func<BindableBase, ValueContainer<string>> _NameLocator = RegisterContainerLocator<string>("Name", model => model.Initialize("Name", ref model._Name, ref _NameLocator, _NameDefaultValueFactory));
        static Func<string> _NameDefaultValueFactory = () => default(string);
        #endregion

        /// <summary>
        /// 说明  Description
        /// </summary>
		public string Description
		{
			get { return _DescriptionLocator(this).Value; }
			set { _DescriptionLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property string Description Setup        
		protected Property<string> _Description = new Property<string> { LocatorFunc = _DescriptionLocator };
		static Func<BindableBase, ValueContainer<string>> _DescriptionLocator = RegisterContainerLocator<string>("Description", model => model.Initialize("Description", ref model._Description, ref _DescriptionLocator, _DescriptionDefaultValueFactory));
		static Func<string> _DescriptionDefaultValueFactory = () => default(string);
		#endregion


        /// <summary>
        /// 检测逻辑 Checker Logic
        /// </summary>
        public Func<TContext, bool> CheckContextFunction
        {
            get { return _CheckContextFunctionLocator(this).Value; }
            set { _CheckContextFunctionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Func<TContext,bool> CheckContextFunction Setup        
        protected Property<Func<TContext, bool>> _CheckContextFunction = new Property<Func<TContext, bool>> { LocatorFunc = _CheckContextFunctionLocator };
        static Func<BindableBase, ValueContainer<Func<TContext, bool>>> _CheckContextFunctionLocator = RegisterContainerLocator<Func<TContext, bool>>("CheckContextFunction", model => model.Initialize("CheckContextFunction", ref model._CheckContextFunction, ref _CheckContextFunctionLocator, _CheckContextFunctionDefaultValueFactory));
        static Func<Func<TContext, bool>> _CheckContextFunctionDefaultValueFactory = () => default(Func<TContext, bool>);
        #endregion



        public string TargetStateName
        {
            get { return _TargetStateNameLocator(this).Value; }
            set { _TargetStateNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string TargetStateName Setup        
        protected Property<string> _TargetStateName = new Property<string> { LocatorFunc = _TargetStateNameLocator };
        static Func<BindableBase, ValueContainer<string>> _TargetStateNameLocator = RegisterContainerLocator<string>("TargetStateName", model => model.Initialize("TargetStateName", ref model._TargetStateName, ref _TargetStateNameLocator, _TargetStateNameDefaultValueFactory));
        static Func<string> _TargetStateNameDefaultValueFactory = () => default(string);
        #endregion

    }





}
