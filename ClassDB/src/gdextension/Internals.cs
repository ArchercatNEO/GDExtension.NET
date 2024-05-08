//! This class is the interface between the godot runtime and this dll
//! We need to use the gdextension header + json to source gen this class

namespace GodotCompatibility;

public unsafe class Internals
{
    public static bool LoadProcAdress()
    {
        //Magic load
        return false;
    }

    public static delegate*<void*, StringName*, StringName*, ClassCreationInfo, void> RegisterExtensionClass; 
    public static extern void PrintError(string message, string functionName, string filePath, int lineNumber, GDExtensionBool notifyEditor); 
}