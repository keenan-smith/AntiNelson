using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;

namespace PointBlank.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReplaceCodeAttribute : Attribute
    {
        #region Variables
        private Type _methodClass;
        private string _methodName;
        private bool _useIL;
        private BindingFlags _flags;
        #endregion

        #region Properties
        public Type methodClass
        {
            get
            {
                return _methodClass;
            }
        }

        public string methodName
        {
            get
            {
                return _methodName;
            }
        }

        public BindingFlags flags
        {
            get
            {
                return _flags;
            }
        }

        public MethodInfo method
        {
            get
            {
                try
                {
                    return methodClass.GetMethod(methodName, flags);
                }
                catch (Exception ex)
                {
                    PBLogging.logError("ERROR: Exception while getting method!", ex);
                    return null;
                }
            }
        }

        public bool useIL
        {
            get
            {
                return _useIL;
            }
        }
        #endregion

        public ReplaceCodeAttribute(Type methodClass, string methodName, BindingFlags flags, bool useIL = false)
        {
            _methodClass = methodClass;
            _methodName = methodName;
            _useIL = useIL;
            _flags = flags;
        }
    }
}
