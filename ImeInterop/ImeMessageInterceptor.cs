using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImeInterop
{
    public class ImeMessageInterceptor : NativeWindow
    {
        public event Action<string>? OnImeComposition;
        public event Action? OnImeStartComposition;
        public event Action? OnImeEndComposition;

        const int WM_IME_STARTCOMPOSITION = 0x010D;
        const int WM_IME_COMPOSITION = 0x010F;
        const int WM_IME_ENDCOMPOSITION = 0x010E;
        private const int GCS_COMPSTR = 0x0008;

        public void Attach(Control target)
        {
            if (!target.IsHandleCreated)
                target.HandleCreated += (s, e) => AssignHandle(target.Handle);
            else
                AssignHandle(target.Handle);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_IME_STARTCOMPOSITION)
            {
                OnImeStartComposition?.Invoke();
            }
            else if (m.Msg == WM_IME_COMPOSITION)
            {
                string compositionText = GetImeString(m.HWnd, GCS_COMPSTR);
                OnImeComposition?.Invoke(compositionText);
            }
            else if (m.Msg == WM_IME_ENDCOMPOSITION)
            {
                OnImeEndComposition?.Invoke();
            }
        }

        private string GetImeString(IntPtr hWnd, int type)
        {
            IntPtr hIMC = ImmGetContext(hWnd);
            if (hIMC == IntPtr.Zero) return string.Empty;

            try
            {
                int size = ImmGetCompositionString(hIMC, type, null, 0);
                if (size <= 0) return string.Empty;

                byte[] buffer = new byte[size];
                ImmGetCompositionString(hIMC, type, buffer, size);

                string encodingName = GetInputLanguageEncoding();
                try
                {
                    return Encoding.GetEncoding(encodingName).GetString(buffer).TrimEnd('\0');
                }
                catch
                {
                    return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                }
            }
            finally
            {
                ImmReleaseContext(hWnd, hIMC);
            }
        }

        private string GetInputLanguageEncoding()
        {
            IntPtr hKL = GetKeyboardLayout(GetCurrentThreadId());
            int langId = (ushort)(hKL.ToInt64() & 0xFFFF);

            return langId switch
            {
                0x0412 => "ks_c_5601-1987", // Korean
                0x0411 => "shift_jis",      // Japanese
                0x0804 => "gb2312",         // Chinese (Simplified)
                0x0404 => "big5",           // Chinese (Traditional)
                _ => "utf-8",
            };
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll")]
        private static extern int ImmGetCompositionString(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);
    }
}
