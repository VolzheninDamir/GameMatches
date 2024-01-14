using MatchGame.Commands.Base;
using MatchGame.Models;
using MatchGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MatchGame.Commands
{
    public class FieldClickCommand : Command
    {
        public override void Execute(object parameters)
        {
            object[] param = parameters as object[];
            GameViewModel gvm = param[1] as GameViewModel;
            FieldModel fm = param[0] as FieldModel;

            gvm.MatchClick(fm);
        }
    }
}
