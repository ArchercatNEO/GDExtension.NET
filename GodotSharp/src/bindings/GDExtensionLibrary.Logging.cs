using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension;

public unsafe partial struct GDExtensionLibrary
{
    public delegate void PrintErrorDelegate(string message, string functionName, string filePath, int lineNumber, GDExtensionBool notifyEditor); 
    public static PrintErrorDelegate? PrintErrorInternal;
    public static void PrintError(string message, [CallerFilePath] string fileName = "", [CallerMemberName] string functionName = "", [CallerLineNumber] int lineNumber = 0)
    {
        PrintErrorInternal.Invoke(message, functionName, fileName, lineNumber, 0);
    }

    [LibraryImport("godot")]
    public static partial void Hello(int value);
}