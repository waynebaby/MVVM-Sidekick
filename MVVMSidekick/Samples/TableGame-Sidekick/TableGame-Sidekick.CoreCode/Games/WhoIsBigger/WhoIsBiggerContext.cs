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
        public ObservableCollection<string> Messages
        {
            get { return _MessagesLocator(this).Value; }
            set { _MessagesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<string> Messages Setup        
        protected Property<ObservableCollection<string>> _Messages = new Property<ObservableCollection<string>> { LocatorFunc = _MessagesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<string>>> _MessagesLocator = RegisterContainerLocator<ObservableCollection<string>>(nameof(Messages), model => model.Initialize(nameof(Messages), ref model._Messages, ref _MessagesLocator, _MessagesDefaultValueFactory));
        static Func<ObservableCollection<string>> _MessagesDefaultValueFactory = () => new ObservableCollection<string>();
        #endregion


        public ObservableCollection<string> LastMessages
        {
            get { return _LastMessagesLocator(this).Value; }
            set { _LastMessagesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<string> LastMessages Setup        
        protected Property<ObservableCollection<string>> _LastMessages = new Property<ObservableCollection<string>> { LocatorFunc = _LastMessagesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<string>>> _LastMessagesLocator = RegisterContainerLocator<ObservableCollection<string>>(nameof(LastMessages), model => model.Initialize(nameof(LastMessages), ref model._LastMessages, ref _LastMessagesLocator, _LastMessagesDefaultValueFactory));
        static Func<ObservableCollection<string>> _LastMessagesDefaultValueFactory = () => null;
        #endregion



        public int MaxLastMessageCount
        {
            get { return _MaxLastMessageCountLocator(this).Value; }
            set { _MaxLastMessageCountLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int MaxLastMessageCount Setup        
        protected Property<int> _MaxLastMessageCount = new Property<int> { LocatorFunc = _MaxLastMessageCountLocator };
        static Func<BindableBase, ValueContainer<int>> _MaxLastMessageCountLocator = RegisterContainerLocator<int>(nameof(MaxLastMessageCount), model => model.Initialize(nameof(MaxLastMessageCount), ref model._MaxLastMessageCount, ref _MaxLastMessageCountLocator, _MaxLastMessageCountDefaultValueFactory));
        static Func<int> _MaxLastMessageCountDefaultValueFactory = () => 10;
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


        [DataMember]
        public ObservableCollection<decimal> UserInputs
        {
            get { return _UserInputsLocator(this).Value; }
            set { _UserInputsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<decimal> UserInputs Setup        
        protected Property<ObservableCollection<decimal>> _UserInputs = new Property<ObservableCollection<decimal>> { LocatorFunc = _UserInputsLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<decimal>>> _UserInputsLocator = RegisterContainerLocator<ObservableCollection<decimal>>(nameof(UserInputs), model => model.Initialize(nameof(UserInputs), ref model._UserInputs, ref _UserInputsLocator, _UserInputsDefaultValueFactory));
        static Func<ObservableCollection<decimal>> _UserInputsDefaultValueFactory = () => new ObservableCollection<decimal>();
        #endregion

        [DataMember]
        public int CurrentUserIndex
        {
            get { return _CurrentUserIndexLocator(this).Value; }
            set { _CurrentUserIndexLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int CurrentUserIndex Setup        
        protected Property<int> _CurrentUserIndex = new Property<int> { LocatorFunc = _CurrentUserIndexLocator };
        static Func<BindableBase, ValueContainer<int>> _CurrentUserIndexLocator = RegisterContainerLocator<int>("CurrentUserIndex", model => model.Initialize("CurrentUserIndex", ref model._CurrentUserIndex, ref _CurrentUserIndexLocator, _CurrentUserIndexDefaultValueFactory));
        static Func<int> _CurrentUserIndexDefaultValueFactory = () => 0;
        #endregion




    }
}
