using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TableGame_Sidekick.Models
{

    public abstract class BuilderBase<TProduct, TContext, TBuilder>
        where TProduct : IGameModel<TContext>, new()
        where TBuilder : BuilderBase<TProduct, TContext, TBuilder>
    {
        public virtual TProduct CreateProduct()
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
        }

        protected List<Action<TProduct>> BuildingActions = new List<Action<TProduct>>();

        public TBuilder WithContext(TContext context)
        {
            BuildingActions.Add(x => x.GameExecutingContext = context);
            return this as TBuilder;
        }


        public TBuilder WithName(string name)
        {
            BuildingActions.Add(x => x.Name = name);
            return this as TBuilder;
        }


        public TBuilder WithDescription(string description)
        {
            BuildingActions.Add(x => x.Description = description);
            return this as TBuilder;
        }



    }

    public class GameBuilder<TContext> : BuilderBase<Game<TContext>, TContext,GameBuilder<TContext>>
    {



        public GameBuilder<TContext> HasStartState(Action<GameStateBuilder<TContext>> builderAction)
        {
            var bd = new GameStateBuilder<TContext>();
            builderAction(bd);

            BuildingActions.Add(x =>
            {
                var state = bd.CreateProduct();
                x.StartState = state;
                x.AllStates[state.Name] = state;
            });
            return this;
        }

        public GameBuilder<TContext> HasEndState(Action<GameStateBuilder<TContext>> builderAction)
        {
            var bd = new GameStateBuilder<TContext>();
            builderAction(bd);

            BuildingActions.Add(x =>
            {
                var state = bd.CreateProduct();
                x.EndStates[state.Name] = state;
                x.AllStates[state.Name] = state;
            });
            return this;
        }


    }


    public class GameStateBuilder<TContext> : BuilderBase<GameState<TContext>, TContext, GameStateBuilder<TContext>>
    {
        //public GameStateBuilder<TContext> HasStateChangeChecker(Func<TContext ,bool> checker )
        //{
        //    BuildingActions.Add(x =>
        //    {
        //        var cd=new 

        //    });
        //}

    }
    public class StateChangeCheckerBuilder<TContext> : BuilderBase<StateChangeChecker<TContext>, TContext, StateChangeCheckerBuilder<TContext>>
    {

    }

    public class ContextDataChangeCheckerBuilder<TContext> : BuilderBase<ContextDataChangeChecker<TContext>, TContext, ContextDataChangeCheckerBuilder<TContext>>
    {

    }



}
