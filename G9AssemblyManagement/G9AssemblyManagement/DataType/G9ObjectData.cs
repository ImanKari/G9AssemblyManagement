#if !NET35 && !NET40
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     On Develop
    /// </summary>
    public class G9ObjectData : DynamicObject
    {
        /// <summary>
        /// </summary>
        private readonly Dictionary<string, dynamic> _properties =
            new Dictionary<string, dynamic>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out dynamic result)
        {
            result = _properties.ContainsKey(binder.Name) ? _properties[binder.Name] : null;

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, dynamic value)
        {
            if (value == null)
            {
                if (_properties.ContainsKey(binder.Name))
                    _properties.Remove(binder.Name);
            }
            else
            {
                _properties[binder.Name] = value;
            }

            return true;
        }
    }
}
#endif