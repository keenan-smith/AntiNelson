using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;

namespace ManPAD.ManPAD_API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CodeReplaceAttribute : Attribute
    {
        #region Variables
        private string _funcName;
        private Type _funcClass;
        private BindingFlags _funcFlags;
        private RedirectCallsState _funcBackup;
        private bool _stateChanged = false;
        #endregion

        #region Properties
        public MethodInfo method
        {
            get
            {
                return _funcClass.GetMethod(_funcName, _funcFlags);
            }
        }

        public RedirectCallsState callState
        {
            get
            {
                return _funcBackup;
            }
            set
            {
                if (!_stateChanged)
                {
                    _funcBackup = value;
                    _stateChanged = true;
                }
            }
        }
        #endregion

        public CodeReplaceAttribute(string funcName, Type funcClass, BindingFlags funcFlags)
        {
            _funcName = funcName;
            _funcClass = funcClass;
            _funcFlags = funcFlags;
        }
    }
}
