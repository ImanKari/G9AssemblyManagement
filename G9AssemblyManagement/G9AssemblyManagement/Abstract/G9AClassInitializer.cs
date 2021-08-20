using System;
using G9AssemblyManagement.Core;

namespace G9AssemblyManagement.Abstract
{
    /// <summary>
    ///     Abstract helper class to assign an instance of the class
    /// </summary>
    public abstract class G9AClassInitializer : IDisposable
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        protected G9AClassInitializer()
        {
            G9CAssemblyHandler.AssignInstanceOfType(this);
        }

        /// <summary>
        ///     Deconstructor
        /// </summary>
        ~G9AClassInitializer()
        {
            G9CAssemblyHandler.UnassignInstanceOfType(this);
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            G9CAssemblyHandler.UnassignInstanceOfType(this);
        }
    }
}