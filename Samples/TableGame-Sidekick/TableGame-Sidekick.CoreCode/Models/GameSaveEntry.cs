using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TableGame_Sidekick.Models
{

	[DataContract]
	public class GameSaveEntry : IGameSaveEntry
	{
		[DataMember]
		public object ContextObject
		{
			get; set;
		}

		[DataMember]  
		public string CurrentState
		{
			get; set;
		}

		[DataMember]
		public string ContextTypeFullName
		{
			get; set;
		}
	}
}
