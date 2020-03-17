using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TableGame_Sidekick.Models
{

    //[DataContract(IsReference=true) ] //if you want
    public class GameState<TContext> : BindableBase<GameState<TContext>>,IGameModel<TContext>
    {


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


        public ObservableCollection<ContextDataChangeChecker<TContext>> SelfChangeCheckers
        {
            get { return _SelfChangeCheckersLocator(this).Value; }
            set { _SelfChangeCheckersLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<ContextDataChangeChecker<TContext>>  SelfChangeCheckers Setup        
        protected Property<ObservableCollection<ContextDataChangeChecker<TContext>>> _SelfChangeCheckers = new Property<ObservableCollection<ContextDataChangeChecker<TContext>>> { LocatorFunc = _SelfChangeCheckersLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<ContextDataChangeChecker<TContext>>>> _SelfChangeCheckersLocator = RegisterContainerLocator<ObservableCollection<ContextDataChangeChecker<TContext>>>("SelfChangeCheckers", model => model.Initialize("SelfChangeCheckers", ref model._SelfChangeCheckers, ref _SelfChangeCheckersLocator, _SelfChangeCheckersDefaultValueFactory));
        static Func<ObservableCollection<ContextDataChangeChecker<TContext>>> _SelfChangeCheckersDefaultValueFactory = () => new ObservableCollection<ContextDataChangeChecker<TContext>>();
        #endregion
        


        public ObservableCollection<StateChangeChecker<TContext>> MoveToNextStageCheckers
        {
            get { return _MoveToNextStageCheckersLocator(this).Value; }
            set { _MoveToNextStageCheckersLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<StateChangeChecker<TContext>> MoveToNextStageCheckers Setup        
        protected Property<ObservableCollection<StateChangeChecker<TContext>>> _MoveToNextStageCheckers = new Property<ObservableCollection<StateChangeChecker<TContext>>> { LocatorFunc = _MoveToNextStageCheckersLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<StateChangeChecker<TContext>>>> _MoveToNextStageCheckersLocator = RegisterContainerLocator<ObservableCollection<StateChangeChecker<TContext>>>("MoveToNextStageCheckers", model => model.Initialize("MoveToNextStageCheckers", ref model._MoveToNextStageCheckers, ref _MoveToNextStageCheckersLocator, _MoveToNextStageCheckersDefaultValueFactory));
        static Func<ObservableCollection<StateChangeChecker<TContext>>> _MoveToNextStageCheckersDefaultValueFactory = () => new ObservableCollection<StateChangeChecker<TContext>>();
        #endregion


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

        


    }






}

