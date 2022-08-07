using System;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for instance listener
    /// </summary>
    public class G9DtInstanceListener<TType> : IDisposable
    {
        #region ### Fields And Properties ###

        /// <summary>
        ///     Action to handle dispose
        /// </summary>
        private Action<int, Guid> _disposeAction;

        /// <summary>
        ///     Action to handle stop and resume
        /// </summary>
        private Action<bool> _stopAndResumeAction;

        /// <summary>
        ///     Specifies data type is disposed or no
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        ///     Specifies listener identity
        /// </summary>
        public readonly Guid Identity;

        /// <summary>
        ///     Specifies listener is active or no
        /// </summary>
        public bool IsActive { private set; get; }

        /// <summary>
        ///     Specifies executable action, call on assign new instance
        /// </summary>
        public Action<TType> OnAssignInstanceCallback { private set; get; }

        /// <summary>
        ///     Specifies callback action, the callback will be executed automatically on
        ///     unassign an instance.
        /// </summary>
        public Action<TType> OnUnassignInstanceCallback { private set; get; }

        /// <summary>
        ///     Specifies callback action, the callback will be executed automatically on receive
        ///     exception. if don't set it, core ignoring the exception.
        /// </summary>
        public Action<Exception> OnExceptionCallback { private set; get; }

        /// <summary>
        ///     Specifies type full name hash code
        /// </summary>
        public readonly int TypeHashCode;

        #endregion

        #region ### Methods ###

        /// <summary>
        ///     Constructor
        /// </summary>
        public G9DtInstanceListener(Guid identity, int typeHashCode, Action<TType> onAssignInstanceCallback,
            Action<int, Guid> dispose, Action<TType> onUnassignInstanceCallback,
            Action<Exception> onExceptionCallback)
        {
            Identity = identity;
            IsActive = true;
            OnAssignInstanceCallback = onAssignInstanceCallback;
            _disposeAction = dispose;
            TypeHashCode = typeHashCode;
            OnUnassignInstanceCallback = onUnassignInstanceCallback;
            OnExceptionCallback = onExceptionCallback;
        }

        /// <summary>
        ///     Deconstructor
        /// </summary>
        ~G9DtInstanceListener()
        {
            Dispose();
        }

        /// <summary>
        ///     Method for stop listener
        /// </summary>
        public void StopListener()
        {
            if (_isDisposed) throw new ObjectDisposedException(GetType().Name);
            IsActive = false;
            _stopAndResumeAction?.Invoke(IsActive);
        }

        /// <summary>
        ///     Method for resume stopped listener
        /// </summary>
        public void ResumeListener()
        {
            if (_isDisposed) throw new ObjectDisposedException(GetType().Name);
            IsActive = true;
            _stopAndResumeAction?.Invoke(IsActive);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_isDisposed) return;
            _disposeAction?.Invoke(TypeHashCode, Identity);
            _disposeAction = null;
            _stopAndResumeAction = null;
            OnAssignInstanceCallback = OnUnassignInstanceCallback = null;
            OnExceptionCallback = null;
            _isDisposed = true;
        }

        /// <summary>
        ///     Convert TType to object
        /// </summary>
        /// <param name="v">Other type of struct</param>
        public static explicit operator G9DtInstanceListener<object>(G9DtInstanceListener<TType> v)
        {
            var clone = new G9DtInstanceListener<object>(v.Identity, v.TypeHashCode,
                o => v.OnAssignInstanceCallback?.Invoke((TType)o), v._disposeAction,
                o => v.OnUnassignInstanceCallback?.Invoke((TType)o), v.OnExceptionCallback);
            v._stopAndResumeAction = isActive =>
            {
                if (isActive)
                    clone.ResumeListener();
                else
                    clone.StopListener();
            };
            return clone;
        }

        #endregion
    }
}