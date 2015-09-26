using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TableGame_Sidekick.Models;

namespace TableGame_Sidekick.Games.WhoIsBigger
{
	public class WhoIsBiggerGameFactory : IGameFactory<Game<WhoIsBiggerContext>>
	{
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
				.HasStartState("GameStart", "Game Start")
					.EndHasState()
				.HasState("PlayerInput")
					 .EndHasState()
				.HasEndState("GameSet")
					.EndHasState()
				.CurrentProduct;  

			return game;
        }
	}
}
