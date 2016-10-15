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
        /// <summary>
        /// The class that contains the original method.
        /// </summary>
        public Type methodClass
        {
            get
            {
                return _methodClass;
            }
        }

        /// <summary>
        /// The original method name.
        /// </summary>
        public string methodName
        {
            get
            {
                return _methodName;
            }
        }

        /// <summary>
        /// The flags of the original method.
        /// </summary>
        public BindingFlags flags
        {
            get
            {
                return _flags;
            }
        }

        /// <summary>
        /// The original method.
        /// </summary>
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

        /// <summary>
        /// Use the IL to redirect.
        /// </summary>
        public bool useIL
        {
            get
            {
                return _useIL;
            }
        }
        #endregion

        /// <summary>
        /// A method that has this attribute redirects the original method to the one contianing this attribute.
        /// </summary>
        /// <param name="methodClass">The class that contains the method you want to redirect.</param>
        /// <param name="methodName">The method name you want to redirect.</param>
        /// <param name="flags">The flags of the original method.</param>
        /// <param name="useIL">Use IL to redirect the code</param>
        public ReplaceCodeAttribute(Type methodClass, string methodName, BindingFlags flags, bool useIL = false)
        {
            _methodClass = methodClass;
            _methodName = methodName;
            _useIL = useIL;
            _flags = flags;
        }
    }
}
