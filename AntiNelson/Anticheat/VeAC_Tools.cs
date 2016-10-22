using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.Anticheat
{
    internal class VeAC_Tools
    {
        public static float getDistance(Vector3 startPos, Vector3 endPos)
        {
            return Vector3.Distance(startPos, endPos);
        }
    }
}
