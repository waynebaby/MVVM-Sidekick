using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableGame_Sidekick.Models;

namespace TableGame_Sidekick.WPF.Test
{
    [TestClass]
    public class GameBuilderTest
    {
        [TestMethod]
        public void SimpleBuilderPlayground()
        {

            var gb = GameBuilder.Create<String>()
                    .WithContext("a")
                    .HasNameAndDescription("name", "asdsadsa")
                    .HasEndState("name2", "asdsadsa")
                        .HasCheckerForStateChange(s => s == "").ThenGoToState("")
                        .HasCheckerForStateChange(s => s != "").ThenGoToState("")
                        .HasCheckerForContextDataChange(s => s == "").ThenDo(t => t.ToCharArray())
                        .EndHasState()
                    .HasStartState("", "")
                        .EndHasState();

        }
    }
}
