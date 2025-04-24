using System.Runtime.InteropServices;
using System.Text;

namespace ImeInterop
{
    public class ImeMessageInterceptor : NativeWindow
    {
        public event EventHandler<ImeCompositionEventArgs>? OnImeComposition;
        public event EventHandler? OnImeStartComposition;
        public event EventHandler? OnImeEndComposition;

        private const int WM_IME_STARTCOMPOSITION = 0x010D;
        private const int WM_IME_COMPOSITION = 0x010F;
        private const int WM_IME_ENDCOMPOSITION = 0x010E;
        private const int GCS_COMPSTR = 0x0008;

        public void Attach(Control target)
        {
            if (!target.IsHandleCreated)
                target.HandleCreated += (s, e) => AssignHandle(target.Handle);
            else
                AssignHandle(target.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_IME_STARTCOMPOSITION)
            {
                OnImeStartComposition?.Invoke(this, EventArgs.Empty);
            }
            else if (m.Msg == WM_IME_COMPOSITION)
            {
                string compositionText = GetImeStringUnicode(m.HWnd, GCS_COMPSTR);
                OnImeComposition?.Invoke(this, new ImeCompositionEventArgs(compositionText));

                // 조합 문자열이 사라졌다면 "조합 종료"로 간주
                if (string.IsNullOrEmpty(compositionText))
                {
                    OnImeEndComposition?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (m.Msg == WM_IME_ENDCOMPOSITION)
            {
                OnImeEndComposition?.Invoke(this, EventArgs.Empty);
            }

            base.WndProc(ref m);
        }

        public static string GetImeStringUnicode(IntPtr hWnd, int type)
        {
            IntPtr hIMC = ImmGetContext(hWnd);
            if (hIMC == IntPtr.Zero) return string.Empty;

            try
            {
                int size = ImmGetCompositionStringW(hIMC, type, IntPtr.Zero, 0);
                if (size <= 0) return string.Empty;

                IntPtr buffer = Marshal.AllocHGlobal(size);
                try
                {
                    ImmGetCompositionStringW(hIMC, type, buffer, size);

                    // size is in bytes, UTF-16 is 2 bytes per char
                    return Marshal.PtrToStringUni(buffer, size / 2) ?? string.Empty;

                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            finally
            {
                ImmReleaseContext(hWnd, hIMC);
            }
        }

        [DllImport("imm32.dll")] private static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("imm32.dll")] private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
        [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
        private static extern int ImmGetCompositionStringW(IntPtr hIMC, int dwIndex, IntPtr lpBuf, int dwBufLen);
    }
}
