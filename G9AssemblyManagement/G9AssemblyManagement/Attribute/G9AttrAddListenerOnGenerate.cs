using System;
using System.Runtime.CompilerServices;

namespace G9AssemblyManagement.Attribute
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class G9AttrAddListenerOnGenerateAttribute : System.Attribute
    {
        #region ### Fields And Properties ###

        #endregion

        #region ### Methods ###

        /// <summary>
        /// </summary>
        /// <param name="customNickname"></param>
        public G9AttrAddListenerOnGenerateAttribute(/*[CallerMemberName]*/ string customNickname = null)
        {
        }

        public void DoSomethingWithDeclaringType(Type t)
        {
        }

        #endregion
    }
}