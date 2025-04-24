namespace ImeInterop
{
    public class ImeManager
    {
        private readonly Dictionary<IntPtr, ImeMessageInterceptor> _interceptors = new();

        public void Attach(Control control,
            EventHandler<ImeCompositionEventArgs>? onComposition = null,
            EventHandler? onStart = null,
            EventHandler? onEnd = null)
        {
            var ime = new ImeMessageInterceptor();
            if (onComposition != null)
                ime.OnImeComposition += onComposition;
            if (onStart != null)
                ime.OnImeStartComposition += onStart;
            if (onEnd != null)
                ime.OnImeEndComposition += onEnd;

            ime.Attach(control);
            _interceptors[control.Handle] = ime;
        }

        public void Detach(Control control)
        {
            if (_interceptors.TryGetValue(control.Handle, out var ime))
            {
                ime.ReleaseHandle();
                _interceptors.Remove(control.Handle);
            }
        }
    }
}
