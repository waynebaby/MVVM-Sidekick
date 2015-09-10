using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TableGame_Sidekick.Models
{

    //[DataContract(IsReference=true) ] //if you want
    public class GameState<TContext> : BindableBase<GameState<TContext>>
    {


        public ObservableCollection<ContextDataChangeChecker<TContext>> SelfChangeCheckers
        {
            get { return _SelfChangeCheckersLocator(this).Value; }
            set { _SelfChangeCheckersLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<ContextDataChangeChecker<TContext>>  SelfChangeCheckers Setup        
        protected Property<ObservableCollection<ContextDataChangeChecker<TContext>>> _SelfChangeCheckers = new Property<ObservableCollection<ContextDataChangeChecker<TContext>>> { LocatorFunc = _SelfChangeCheckersLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<ContextDataChangeChecker<TContext>>>> _SelfChangeCheckersLocator = RegisterContainerLocator<ObservableCollection<ContextDataChangeChecker<TContext>>>("SelfChangeCheckers", model => model.Initialize("SelfChangeCheckers", ref model._SelfChangeCheckers, ref _SelfChangeCheckersLocator, _SelfChangeCheckersDefaultValueFactory));
        static Func<ObservableCollection<ContextDataChangeChecker<TContext>>> _SelfChangeCheckersDefaultValueFactory = () => default(ObservableCollection<ContextDataChangeChecker<TContext>>);
        #endregion

        //Use propvm + tab +tab  to create a new property of bindable here:


        public ObservableCollection<StateChangeChecker<TContext>> MoveToNextStageCheckers
        {
            get { return _MoveToNextStageCheckersLocator(this).Value; }
            set { _MoveToNextStageCheckersLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<StateChangeChecker<TContext>> MoveToNextStageCheckers Setup        
        protected Property<ObservableCollection<StateChangeChecker<TContext>>> _MoveToNextStageCheckers = new Property<ObservableCollection<StateChangeChecker<TContext>>> { LocatorFunc = _MoveToNextStageCheckersLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<StateChangeChecker<TContext>>>> _MoveToNextStageCheckersLocator = RegisterContainerLocator<ObservableCollection<StateChangeChecker<TContext>>>("MoveToNextStageCheckers", model => model.Initialize("MoveToNextStageCheckers", ref model._MoveToNextStageCheckers, ref _MoveToNextStageCheckersLocator, _MoveToNextStageCheckersDefaultValueFactory));
        static Func<ObservableCollection<StateChangeChecker<TContext>>> _MoveToNextStageCheckersDefaultValueFactory = () => default(ObservableCollection<StateChangeChecker<TContext>>);
        #endregion


        //ObservableCollection<StateChangeChecker<TContext>> MoveToNextStageCheckers { get; }
        //    = new ObservableCollection<StateChangeChecker<TContext>>();


    }






}

