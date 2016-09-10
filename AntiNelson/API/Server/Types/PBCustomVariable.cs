using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server.Enumerables;

namespace PointBlank.API.Server.Types
{

    [Serializable]
    public class CustomVariable
    {

        public bool @bool;
        public byte @byte;
        public short @short;
        public int @int;
        public long @long;
        public double @double;
        public float @float;
        public string @string;

        private ECustomVariableType type;

        public CustomVariable(bool b)
        {

            this.@bool = b;
            this.type = ECustomVariableType.BOOLEAN;

        }

        public CustomVariable(byte by)
        {

            this.@byte = by;
            this.type = ECustomVariableType.BYTE;

        }

        public CustomVariable(short s)
        {

            this.@short = s;
            this.type = ECustomVariableType.SHORT;

        }

        public CustomVariable(int i)
        {

            this.@int = i;
            this.type = ECustomVariableType.INT;

        }

        public CustomVariable(long l)
        {

            this.@long = l;
            this.type = ECustomVariableType.LONG;

        }

        public CustomVariable(double d)
        {

            this.@double = d;
            this.type = ECustomVariableType.DOUBLE;

        }

        public CustomVariable(float f)
        {

            this.@float = f;
            this.type = ECustomVariableType.FLOAT;

        }

        public CustomVariable(string s)
        {

            this.@string = s;
            this.type = ECustomVariableType.STRING;

        }

        public Object getValue()
        {

            switch (type)
            {

                default:
                    return null;
                case ECustomVariableType.BOOLEAN:
                    return @bool;
                case ECustomVariableType.BYTE:
                    return @byte;
                case ECustomVariableType.SHORT:
                    return @short;
                case ECustomVariableType.INT:
                    return @int;
                case ECustomVariableType.LONG:
                    return @long;
                case ECustomVariableType.DOUBLE:
                    return @double;
                case ECustomVariableType.FLOAT:
                    return @float;
                case ECustomVariableType.STRING:
                    return @string;

            }

        }

    }

}
