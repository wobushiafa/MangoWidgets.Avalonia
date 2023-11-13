using Avalonia.Controls;
#if WINDOWS
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Microsoft.Win32.SafeHandles;
using static Windows.Win32.PInvoke;
#endif


namespace MangoWidgets.Avalonia.Extensions;

public static class WindowExtensions
{
#if WINDOWS
    public static void SetHook(this Window window)
    {
        var hwnd = window.TryGetPlatformHandle()!.Handle;
        var result = SetWindowsHookEx(WINDOWS_HOOK_ID.WH_CALLWNDPROC, HookCallback, new SafeProcessHandle(IntPtr.Zero, true), (uint)hwnd);
    }

    private static LRESULT HookCallback(int code, WPARAM wParam, LPARAM lParam)
    {
        if (code >= 0 && wParam == new WPARAM(0x0213))
        {
            var rect = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
            Console.WriteLine($"{{rect.left}},{rect.right}");
        }
        return CallNextHookEx(new SafeProcessHandle(IntPtr.Zero, true), code, wParam, lParam);
    }
#endif


    
    /// <summary>
    /// 设置窗体能否被截图
    /// </summary>
    /// <param name="window"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static Window SetPreventScreenshot(this Window window,bool flag)
    {
#if WINDOWS
        var hwnd = window.TryGetPlatformHandle()!.Handle;
        SetWindowDisplayAffinity(new Windows.Win32.Foundation.HWND(hwnd),
            flag ? Windows.Win32.UI.WindowsAndMessaging.WINDOW_DISPLAY_AFFINITY.WDA_EXCLUDEFROMCAPTURE :
                Windows.Win32.UI.WindowsAndMessaging.WINDOW_DISPLAY_AFFINITY.WDA_NONE);
#endif
        return window;
    }

}