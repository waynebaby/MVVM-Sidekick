using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

namespace TableGame_Sidekick.Games.WhoIsBigger
{


	[DataContract]
	public class WhoIsBiggerContext : BindableBase<WhoIsBiggerContext>
	{


		[DataMember]
		public string OutputMessage
		{
			get { return _OutputMessageLocator(this).Value; }
			set { _OutputMessageLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property string OutputMessage Setup        
		protected Property<string> _OutputMessage = new Property<string> { LocatorFunc = _OutputMessageLocator };
		static Func<BindableBase, ValueContainer<string>> _OutputMessageLocator = RegisterContainerLocator<string>("OutputMessage", model => model.Initialize("OutputMessage", ref model._OutputMessage, ref _OutputMessageLocator, _OutputMessageDefaultValueFactory));
		static Func<string> _OutputMessageDefaultValueFactory = () => default(string);
		#endregion


		[DataMember]
		public ObservableCollection<string> Users
		{
			get { return _UsersLocator(this).Value; }
			set { _UsersLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property ObservableCollection<string> Users Setup        
		protected Property<ObservableCollection<string>> _Users = new Property<ObservableCollection<string>> { LocatorFunc = _UsersLocator };
		static Func<BindableBase, ValueContainer<ObservableCollection<string>>> _UsersLocator = RegisterContainerLocator<ObservableCollection<string>>("Users", model => model.Initialize("Users", ref model._Users, ref _UsersLocator, _UsersDefaultValueFactory));
		static Func<ObservableCollection<string>> _UsersDefaultValueFactory = () => default(ObservableCollection<string>);
		#endregion


		public int CurrentUserIndex
		{
			get { return _CurrentUserIndexLocator(this).Value; }
			set { _CurrentUserIndexLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property int CurrentUserIndex Setup        
		protected Property<int> _CurrentUserIndex = new Property<int> { LocatorFunc = _CurrentUserIndexLocator };
		static Func<BindableBase, ValueContainer<int>> _CurrentUserIndexLocator = RegisterContainerLocator<int>("CurrentUserIndex", model => model.Initialize("CurrentUserIndex", ref model._CurrentUserIndex, ref _CurrentUserIndexLocator, _CurrentUserIndexDefaultValueFactory));
		static Func<int> _CurrentUserIndexDefaultValueFactory = () => default(int);
		#endregion




	}
}
