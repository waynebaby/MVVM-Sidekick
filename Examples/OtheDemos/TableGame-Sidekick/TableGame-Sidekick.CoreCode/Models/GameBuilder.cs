using MVVMSidekick.Commands;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace TableGame_Sidekick.Models
{

    public static class GameBuilder
    {
        public static GameBuilder<TContext> Create<TContext>()
        {
            return new GameBuilder<TContext>();
        }
    }

    public abstract class BuilderBase<TProduct, TContext, TBuilder>
        where TProduct : IGameModel<TContext>, new()
        where TBuilder : BuilderBase<TProduct, TContext, TBuilder>
    {

        public BuilderBase()
        {
            _product = new Lazy<TProduct>
            (() =>
            {
                var tp = new TProduct();
                foreach (var item in BuildingActions)
                {
                    try
                    {
                        item(tp);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ERROR: when call {0}==>CreateProduct()", typeof(TProduct).Name);
                        Debug.WriteLine(ex.ToString());

                    }
                }
                return tp;

            }, true);
        }

        Lazy<TProduct> _product;
        public virtual TProduct CurrentProduct
        {
            get
            {
                return _product.Value;
            }
        }

        protected List<Action<TProduct>> BuildingActions = new List<Action<TProduct>>();




        public TBuilder HasNameAndDescription(string name, string description = null)
        {
            BuildingActions.Add(x => x.Name = name);
            BuildingActions.Add(x => x.Description = description);
            return this as TBuilder;
        }

    }


    public class GameBuilder<TContext> : BuilderBase<Game<TContext>, TContext, GameBuilder<TContext>>
    {

        public GameBuilder()
        {

        }

        public GameBuilder<TContext> WithContext(TContext context)
        {
            BuildingActions.Add(x => x.GameExecutingContext = context);
            return this as GameBuilder<TContext>;
        }
        public GameStateBuilder<TContext> HasStartState(string name, string description = null)
        {
            var bd = new GameStateBuilder<TContext>(this)
                .HasNameAndDescription(name, description);

            BuildingActions.Add(x =>
            {
                var state = bd.CurrentProduct;
                x.StartState = state;
                x.AllStates[state.Name] = state;
            });
            return bd;
        }

        public GameStateBuilder<TContext> HasEndState(string name, string description = null)
        {
            var bd = new GameStateBuilder<TContext>(this)
                .HasNameAndDescription(name, description);

            BuildingActions.Add(x =>
            {
                var state = bd.CurrentProduct;
                x.EndStates[state.Name] = state;
                x.AllStates[state.Name] = state;
            });
            return bd;
        }

        public GameStateBuilder<TContext> HasState(string name, string description = null)
        {
            var bd = new GameStateBuilder<TContext>(this)
                .HasNameAndDescription(name, description);

            BuildingActions.Add(x =>
            {
                var state = bd.CurrentProduct;
                x.AllStates[state.Name] = state;
            });
            return bd;
        }

        public GameBuilder<TContext> HasCommand(string name,
           Func<Game<TContext>, ReactiveCommand>  factory)
        {
            var cmd = new ReactiveCommand();
            BuildingActions.Add(x =>
            {
                x.Commands.AddOrUpdateByKey(new KeyValuePair<string, ReactiveCommand>(name, factory(x)));
            });
            return this;
        }



    }




    public class GameStateBuilder<TContext> : BuilderBase<GameState<TContext>, TContext, GameStateBuilder<TContext>>
    {
        public GameStateBuilder(GameBuilder<TContext> parent)
        {
            _Parent = parent;

        }

        private GameBuilder<TContext> _Parent;


        public GameBuilder<TContext> EndHasState()
        {
            return _Parent;
        }


        public StateChangeCheckerBuilder<TContext> HasCheckerForStateChange(Func<TContext, bool> checkLogic)
        {
            var bd = new StateChangeCheckerBuilder<TContext>(this);
            BuildingActions.Add(x =>
            {
                var ck = bd.CurrentProduct;
                ck.CheckContextFunction = checkLogic;
            });

            return bd;
        }

        public ContextDataChangeCheckerBuilder<TContext> HasCheckerForContextDataChange(Func<TContext, bool> checkLogic)
        {
            var bd = new ContextDataChangeCheckerBuilder<TContext>(this);
            BuildingActions.Add(x =>
            {
                var ck = bd.CurrentProduct;
                ck.CheckContextFunction = checkLogic;
            });

            return bd;
        }


    }
    public class StateChangeCheckerBuilder<TContext> : BuilderBase<StateChangeChecker<TContext>, TContext, StateChangeCheckerBuilder<TContext>>
    {
        public StateChangeCheckerBuilder(GameStateBuilder<TContext> parent)
        {
            _Parent = parent;
        }
        private GameStateBuilder<TContext> _Parent;


        public GameStateBuilder<TContext> ThenGoToState(string stateName)
        {

            BuildingActions.Add(x =>
            {
                var ck = CurrentProduct;
                ck.TargetStateName = stateName;
            });

            return _Parent;
        }

    }

    public class ContextDataChangeCheckerBuilder<TContext> : BuilderBase<ContextDataChangeChecker<TContext>, TContext, ContextDataChangeCheckerBuilder<TContext>>
    {
        public ContextDataChangeCheckerBuilder(GameStateBuilder<TContext> parent)
        {
            _Parent = parent;
        }
        private GameStateBuilder<TContext> _Parent;

        public GameStateBuilder<TContext> ThenDo(Action<TContext> changingAction)
        {
            BuildingActions.Add(x =>
            {
                var ck = CurrentProduct;
                ck.ChangeActions.Add(new ContextDataChangeAction<TContext>() { ChangingAction = changingAction });
            });
            return _Parent;
        }
    }



}
