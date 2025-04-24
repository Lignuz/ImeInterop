namespace ImeInterop
{
    public class ImeCompositionEventArgs : EventArgs
    {
        public string CompositionText { get; }

        public ImeCompositionEventArgs(string text)
        {
            CompositionText = text;
        }
    }
}
