using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Models
{
    
    public class Game<TContext> : BindableBase<Game<TContext>>
    {




        public GameState<TContext> StartState
        {
            get { return _StartStateLocator(this).Value; }
            set { _StartStateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property GameState<TContext> StartState Setup        
        protected Property<GameState<TContext>> _StartState = new Property<GameState<TContext>> { LocatorFunc = _StartStateLocator };
        static Func<BindableBase, ValueContainer<GameState<TContext>>> _StartStateLocator = RegisterContainerLocator<GameState<TContext>>("StartState", model => model.Initialize("StartState", ref model._StartState, ref _StartStateLocator, _StartStateDefaultValueFactory));
        static Func<GameState<TContext>> _StartStateDefaultValueFactory = () => default(GameState<TContext>);
        #endregion



        public GameState<TContext> CurrentState
        {
            get { return _CurrentStateLocator(this).Value; }
            set { _CurrentStateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property GameState<TContext> CurrentState Setup        
        protected Property<GameState<TContext>> _CurrentState = new Property<GameState<TContext>> { LocatorFunc = _CurrentStateLocator };
        static Func<BindableBase, ValueContainer<GameState<TContext>>> _CurrentStateLocator = RegisterContainerLocator<GameState<TContext>>("CurrentState", model => model.Initialize("CurrentState", ref model._CurrentState, ref _CurrentStateLocator, _CurrentStateDefaultValueFactory));
        static Func<GameState<TContext>> _CurrentStateDefaultValueFactory = () => default(GameState<TContext>);
        #endregion



        public IDictionary<string, GameState<TContext>> EndStates
        {
            get { return _EndStatesLocator(this).Value; }
            set { _EndStatesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IDictionary<string, GameState<TContext>>  EndStates Setup        
        protected Property<IDictionary<string, GameState<TContext>>> _EndStates = new Property<IDictionary<string, GameState<TContext>>> { LocatorFunc = _EndStatesLocator };
        static Func<BindableBase, ValueContainer<IDictionary<string, GameState<TContext>>>> _EndStatesLocator = RegisterContainerLocator<IDictionary<string, GameState<TContext>>>("EndStates", model => model.Initialize("EndStates", ref model._EndStates, ref _EndStatesLocator, _EndStatesDefaultValueFactory));
        static Func<IDictionary<string, GameState<TContext>>> _EndStatesDefaultValueFactory = () => default(IDictionary<string, GameState<TContext>>);
        #endregion




        public IDictionary<string, GameState<TContext>> AllStates
        {
            get { return _AllStatesLocator(this).Value; }
            set { _AllStatesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IDictionary<string, GameState<TContext>> AllStates Setup        
        protected Property<IDictionary<string, GameState<TContext>>> _AllStates = new Property<IDictionary<string, GameState<TContext>>> { LocatorFunc = _AllStatesLocator };
        static Func<BindableBase, ValueContainer<IDictionary<string, GameState<TContext>>>> _AllStatesLocator = RegisterContainerLocator<IDictionary<string, GameState<TContext>>>("AllStates", model => model.Initialize("AllStates", ref model._AllStates, ref _AllStatesLocator, _AllStatesDefaultValueFactory));
        static Func<IDictionary<string, GameState<TContext>>> _AllStatesDefaultValueFactory = () => default(IDictionary<string, GameState<TContext>>);
        #endregion


    }
}
