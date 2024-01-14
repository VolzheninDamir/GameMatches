using MatchGame.Data;
using MatchGame.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchGame.Models
{
    internal class FieldModel : ViewModel
    {
        #region I
        private int i;
        public int I { get => i; set => Set(ref i, value); }
        #endregion

        #region Image
        private string image = ImagesPaths.None;
        public string Image { get => image; set => Set(ref image, value); }
        #endregion
    }
}
