using System;
using System.Runtime.InteropServices;

class Program
{
    const int INPUT_KEYBOARD = 1;
    const uint KEYEVENTF_KEYUP = 0x0002;

    const ushort VK_MENU = 0x12;   // Alt
    const ushort VK_SHIFT = 0x10;  // Shift

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        [FieldOffset(0)]
        public KEYBDINPUT ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    static void KeyDown(ushort vk)
    {
        INPUT[] input = new INPUT[1];
        input[0].type = INPUT_KEYBOARD;
        input[0].U.ki.wVk = vk;

        SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
    }

    static void KeyUp(ushort vk)
    {
        INPUT[] input = new INPUT[1];
        input[0].type = INPUT_KEYBOARD;
        input[0].U.ki.wVk = vk;
        input[0].U.ki.dwFlags = KEYEVENTF_KEYUP;

        SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
    }

    static void Main()
    {
        // Alt down + Shift down
        KeyDown(VK_MENU);
        KeyDown(VK_SHIFT);

        // release Shift then Alt
        KeyUp(VK_SHIFT);
        KeyUp(VK_MENU);
    }
}
