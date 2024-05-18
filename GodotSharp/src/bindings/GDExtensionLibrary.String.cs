using System.Runtime.InteropServices;

namespace GDExtension;

public unsafe partial struct GDExtensionLibrary
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NewStringNameFromUtf8CharsAndLengthDelegate(out StringName outString, string source);
    public static NewStringNameFromUtf8CharsAndLengthDelegate NewStringNameFromUtf8CharsAndLengthInternal;
}