//! This class is the interface between the godot runtime and this dll
//! We need to use the gdextension header + json to source gen this class

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension.NativeInterop;

public unsafe class NativeCalls(GetProcAddress procAdressLoader, void* library)
{
    public T? LoadProcAdress<T>(string name) where T : Delegate
    {
        var pointer = procAdressLoader(name);
        var callback = Marshal.GetDelegateForFunctionPointer<T>(pointer);
        return callback;
    }


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NewStringNameFromUtf8CharsAndLengthDelegate(IntPtr outString, [MarshalAs(UnmanagedType.LPStr)] string source);
    public NewStringNameFromUtf8CharsAndLengthDelegate NewStringNameFromUtf8CharsAndLengthInternal;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RegisterExtensionClassDelegate(void* library, StringName className, StringName parentClass, ClassCreationInfo* info);
    public RegisterExtensionClassDelegate RegisterExtensionClassInternal;
    public void RegisterExtensionClass(string className, string parentClass, ClassCreationInfo info)
    {
        StringName name = new(this, className);
        StringName parentClassName = new(this, parentClass);
        RegisterExtensionClassInternal.Invoke(library, name, parentClassName, &info);
    }

    public static extern void PrintErrorBind(string message, string functionName, string filePath, int lineNumber, GDExtensionBool notifyEditor); 
    public static void PrintError(string message, [CallerFilePath] string fileName = "", [CallerMemberName] string functionName = "", [CallerLineNumber] int lineNumber = 0)
    {
        PrintErrorBind(message, functionName, fileName, lineNumber, 0);
    }
}