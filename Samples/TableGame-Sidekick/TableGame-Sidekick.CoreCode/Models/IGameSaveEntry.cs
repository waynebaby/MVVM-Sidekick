using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Models
{
	public interface IGameSaveEntry
	{
		string CurrentState { get; set; }
		string ContextTypeFullName { get; set; }
		object ContextObject { get; set; }
	}
}
