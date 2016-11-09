using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.Enumerables;

namespace ManPAD.ManPAD_API.Types
{
    public class ESPDraw
    {
        #region Variables
        public string text;
        public GameObject game_object;
        public EESPItem type;
        public Vector3 screenPoint;
        public Rect box;
        #endregion

        public ESPDraw(string text, GameObject game_object, EESPItem type, Vector3 screenPoint, Rect box)
        {
            this.text = text;
            this.game_object = game_object;
            this.type = type;
            this.screenPoint = screenPoint;
            this.box = box;
        }
    }
}
