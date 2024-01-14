using MatchGame.Commands.Base;
using MatchGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MatchGame.Commands
{
    internal class AcceptCommand : Command
    {
        public override void Execute(object parametr)
        {
            GameViewModel gvm = parametr as GameViewModel;
            gvm.Accept();
        }

    }
    internal class ResetCommand : Command
    {
        public override void Execute(object parametr)
        {
            GameViewModel gvm = parametr as GameViewModel;
            gvm.StartGame();
        }

    }
    internal class RulesCommand : Command
    {
        public override void Execute(object parametr)
        {
            GameViewModel gvm = parametr as GameViewModel;
            gvm.Rules();
        }

    }
    internal class SaveCommand : Command
    {
        public override void Execute(object parametr)
        {
            GameViewModel gvm = parametr as GameViewModel;
            gvm.SaveGame();
        }

    }
    internal class DownloadCommand : Command
    {
        public override void Execute(object parametr)
        {
            GameViewModel gvm = parametr as GameViewModel;
            gvm.LoadGame();
        }

    }
}
