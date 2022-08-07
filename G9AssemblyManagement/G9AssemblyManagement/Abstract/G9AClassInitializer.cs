using System;
using G9AssemblyManagement.Core;

namespace G9AssemblyManagement.Abstract
{
    /// <summary>
    ///     Abstract helper class to assign an instance of the class automatically.
    ///     This helper class can help to add automatically all instances of a type that is inherited by that.
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

        /// <inheritdoc />
        public virtual void Dispose()
        {
            G9CAssemblyHandler.UnassignInstanceOfType(this);
        }

        /// <summary>
        ///     Destructor
        /// </summary>
        ~G9AClassInitializer()
        {
            G9CAssemblyHandler.UnassignInstanceOfType(this);
        }
    }
}