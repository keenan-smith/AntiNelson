using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute() // The fuck am I suppose to do here???
        {
        }
    }
}
