using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TableGame_Sidekick.Models;
using System.Reactive.Linq;


namespace TableGame_Sidekick.Games.WhoIsBigger
{
    public class WhoIsBiggerGameFactory : IGameFactory<Game<WhoIsBiggerContext>>
    {
        public static class GameStateNames
        {
            public static readonly string GameStart = nameof(GameStart);
            public static readonly string UserInput = nameof(UserInput);
            public static readonly string GameSet = nameof(GameSet);


        }
        public Game<WhoIsBiggerContext> Create()
        {
            var game = GameBuilder.Create<WhoIsBiggerContext>()
                //.HasOneOfCommands(
                //	"UserInput",g=>
                //	{

                //	},
                //	g=>
                //	{

                //	}
                //)
                .HasCommand("StartGame", g =>
                {
                    var cmd = new ReactiveCommand();
                    cmd.ListenCanExecuteObservable(g
                            .GetValueContainer(x => x.CurrentState)
                            .GetNullObservable()
                            .Select(_ => g.CurrentState.Name == GameStateNames.GameStart));

                    cmd.Subscribe(e => g.CurrentState = g.AllStates[GameStateNames.UserInput]);
                    return cmd;
                })
                .HasCommand("PlayerInput", g =>
                {
                    var cmd = new ReactiveCommand();

                    cmd.ListenCanExecuteObservable(g
                            .GetValueContainer(x => x.CurrentState)
                            .GetNullObservable()
                            .Select(_ =>
                                g.CurrentState.Name == GameStateNames.UserInput
                                    ||
                                g.CurrentState.Name == GameStateNames.GameSet
                            ));

                    cmd.Subscribe(e =>
                            {
                                var input = (decimal)e.EventArgs.EventArgs;
                                g.GameExecutingContext.UserInputs.Add(input);
                                g.GameExecutingContext.CurrentUserIndex++;
                            }
                                    );
                    return cmd;
                })
                .HasStartState("GameStart", "Game Start")
                    .EndHasState()

                .HasState(GameStateNames.UserInput)
                        .HasCheckerForContextDataChange(c => c.CurrentUserIndex != c.UserInputs.Count)
                            .ThenDo(c => c.CurrentUserIndex = c.UserInputs.Count)

                        .HasCheckerForContextDataChange(c => true)
                            .ThenDo(c =>
                            {
                                var msg = string.Format("{0}'s  input is {1}", c.Users[c.CurrentUserIndex], c.UserInputs[c.CurrentUserIndex]);
                                c.Messages.Add(msg);
                                c.LastMessages.Add(msg);
                            })

                        .HasCheckerForContextDataChange(c => c.LastMessages.Count > c.MaxLastMessageCount)
                            .ThenDo(c =>
                            {
                                while (c.LastMessages.Count > c.MaxLastMessageCount) { c.LastMessages.RemoveAt(0); }
                            })

                        .HasCheckerForStateChange(c => c.UserInputs.Count == c.Users.Count)
                            .ThenGoToState(GameStateNames.GameSet)
                     .EndHasState()
                .HasEndState(GameStateNames.UserInput)
                    .EndHasState()

                .WithContext(new WhoIsBiggerContext
                {
                    Users = new System.Collections.ObjectModel.ObservableCollection<string>()
                    {
                        "U1",
                        "U2"
                    }
                })
                .CurrentProduct;

            return game;
        }
    }
}
