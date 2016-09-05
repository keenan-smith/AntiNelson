using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.PB_GameObjects
{
    public class AutoSave : MonoBehaviour
    {
        private DateTime lastSave;

        public void Start()
        {
            lastSave = DateTime.Now;
        }

        public void Update()
        {
            
        }
    }
}
