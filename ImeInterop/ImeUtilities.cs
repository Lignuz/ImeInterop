using System.Runtime.InteropServices;

namespace ImeInterop
{
    public static class ImeUtilities
    {
        public static string GetInputLanguageEncoding()
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
    }
}
