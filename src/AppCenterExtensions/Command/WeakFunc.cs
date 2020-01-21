using System;
using System.Diagnostics;
using System.Reflection;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Command
{
    internal sealed class WeakFunc<TResult>
    {
        private Func<TResult> _staticFunc;

        private MethodInfo Method { get; set; }

        public bool IsStatic => _staticFunc != null;

        public string MethodName
            => _staticFunc != null ? _staticFunc.Method.Name : Method.Name;

        private WeakReference FuncReference { get; set; }

        private object LiveReference { get; set; }

        private WeakReference Reference { get; set; }

        public WeakFunc(Func<TResult> func, bool keepTargetAlive = false)
            : this(func == null ? null : func.Target, func, keepTargetAlive)
        {
        }

        private WeakFunc(object target, Func<TResult> func, bool keepTargetAlive = false)
        {
            if (func.Method.IsStatic)
            {
                _staticFunc = func;

                if (target != null)
                {
                    Reference = new WeakReference(target);
                }

                return;
            }

            Method = func.Method;
            FuncReference = new WeakReference(func.Target);

            LiveReference = keepTargetAlive ? func.Target : null;
            Reference = new WeakReference(target);

#if DEBUG
            if (FuncReference != null
                && FuncReference.Target != null
                && !keepTargetAlive)
            {
                var type = FuncReference.Target.GetType();

                if (type.Name.StartsWith("<>")
                    && type.Name.Contains("DisplayClass"))
                {
                    Debug.WriteLine(
                        "You are attempting to register a lambda with a closure without using keepTargetAlive. Are you sure? Check http://galasoft.ch/s/mvvmweakaction for more info.");
                }
            }
#endif
        }

        public bool IsAlive
        {
            get
            {
                if (_staticFunc == null
                    && Reference == null
                    && LiveReference == null)
                {
                    return false;
                }

                if (_staticFunc != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }

                    return true;
                }

                // Non static action

                if (LiveReference != null)
                {
                    return true;
                }

                if (Reference != null)
                {
                    return Reference.IsAlive;
                }

                return false;
            }
        }

        public object Target
        {
            get
            {
                if (Reference == null)
                {
                    return null;
                }

                return Reference.Target;
            }
        }

        private object FuncTarget
        {
            get
            {
                if (LiveReference != null)
                {
                    return LiveReference;
                }

                return FuncReference == null ? null : FuncReference.Target;
            }
        }

        public TResult Execute()
        {
            if (_staticFunc != null)
            {
                return _staticFunc();
            }

            var funcTarget = FuncTarget;

            if (IsAlive)
            {
                if (Method != null
                    && (LiveReference != null
                        || FuncReference != null)
                    && funcTarget != null)
                {
                    return (TResult) Method.Invoke(funcTarget, null);
                }
            }

            return default(TResult);
        }

        public void MarkForDeletion()
        {
            Reference = null;
            FuncReference = null;
            LiveReference = null;
            Method = null;
            _staticFunc = null;
        }
    }
}