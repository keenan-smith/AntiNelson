using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_API.GUI.Extensions
{
    public abstract class MPGUIElement
    {
        #region Abstract Functions
        public abstract void draw();
        #endregion

        #region Virtual Functions
        public virtual void OnHover() {}

        public virtual void OnClick() {}
        #endregion
    }
}
