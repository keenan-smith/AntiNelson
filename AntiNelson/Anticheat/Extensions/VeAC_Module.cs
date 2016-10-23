using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.Anticheat.Extensions
{
    internal abstract class VeAC_Module : MonoBehaviour
    {
        #region Abstract Functions
        public abstract bool check();
        #endregion
    }
}
