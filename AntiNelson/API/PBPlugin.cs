using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API
{
    public abstract class PBPlugin : MonoBehaviour
    {

        public PBPlugin() { }

        public abstract void load();

    }
}
