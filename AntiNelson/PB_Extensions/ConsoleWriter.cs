using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.PB_Extensions
{
    public class ConsoleWriter : TextWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }

        public override void Write(string value)
        {
            if (WriteEvent != null) WriteEvent(value);
        }

        public override void WriteLine(string value)
        {
            if (WriteLineEvent != null) WriteLineEvent(value);
        }

        public delegate void consoleWLOut(string text);
        public delegate void consoleWOut(string text);

        public event consoleWOut WriteEvent;
        public event consoleWLOut WriteLineEvent;
    }
}
