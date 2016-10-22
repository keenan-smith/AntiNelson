using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Server;

namespace PointBlank.Anticheat.Extensions
{
    internal class VeAC_PositionSave
    {
        public Vector3 position;
        public Quaternion rotation;

        public VeAC_PositionSave(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
