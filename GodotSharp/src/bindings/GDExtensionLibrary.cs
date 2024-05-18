using System.Runtime.InteropServices;
using GDExtension.NativeInterop;

namespace GDExtension;

//TODO source gen this class
/// <summary>
/// Binding for the functions described in gdextension_interface.h <br/>
/// Largely source generated from the API JSON file
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GDExtensionLibrary
{
    public static IntPtr Addr;

    void* Library;
    char* LibraryPath;
    bool Reloadable;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RegisterExtensionClassDelegate(IntPtr library, StringName className, StringName parentClass, ClassCreationInfo* info);
    public static RegisterExtensionClassDelegate RegisterExtensionClassInternal;
    public static void RegisterExtensionClass(string className, string parentClass, ClassCreationInfo info)
    {
        RegisterExtensionClassInternal.Invoke(Addr, className, parentClass, &info);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RegisterExtensionClassMethodDelegate(IntPtr library, StringName className, MethodInfo* methodBind);
    public static RegisterExtensionClassMethodDelegate RegisterExtensionClassMethodInternal;
    public static void RegisterExtensionClassMethod(string className, MethodInfo methodBind)
    {
        RegisterExtensionClassMethodInternal.Invoke(Addr, className, &methodBind);
    }
}