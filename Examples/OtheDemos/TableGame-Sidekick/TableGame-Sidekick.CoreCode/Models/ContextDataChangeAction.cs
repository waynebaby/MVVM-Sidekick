using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Models
{

    //[DataContract(IsReference=true) ] //if you want
    public class ContextDataChangeAction<TContext> : BindableBase<ContextDataChangeAction<TContext>>, IGameModel<TContext>
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
        /// 游戏名 Game Name
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
		/// 游戏说明 Game Description
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


		public Action<TContext> ChangingAction
        {
            get { return _ChangingActionLocator(this).Value; }
            set { _ChangingActionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Action<TContext> ChangingAction Setup        
        protected Property<Action<TContext>> _ChangingAction = new Property<Action<TContext>> { LocatorFunc = _ChangingActionLocator };
        static Func<BindableBase, ValueContainer<Action<TContext>>> _ChangingActionLocator = RegisterContainerLocator<Action<TContext>>("ChangingAction", model => model.Initialize("ChangingAction", ref model._ChangingAction, ref _ChangingActionLocator, _ChangingActionDefaultValueFactory));
        static Func<Action<TContext>> _ChangingActionDefaultValueFactory = () => default(Action<TContext>);
        #endregion


    }





}
