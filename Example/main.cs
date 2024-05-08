global using unsafe GDExtensionInterfaceGetProcAddress = delegate* unmanaged[Cdecl]<string, void*>;
global using unsafe GDExtensionClassLibraryPtr = void*;
global using unsafe Callback = delegate* unmanaged[Cdecl]<GodotCompatibility.ModuleInitializationLevel, void>;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GodotCompatibility;

public static unsafe class Bootloader
{
    [UnmanagedCallersOnly(EntryPoint = "init_assembly")]
    public static void Bootload(GDExtensionInterfaceGetProcAddress p_get_proc_address, void* p_library, GDExtensionInitialization* r_initialization)
    {
        ExampleBinder callback = new();
        GDExtensionBinding.Init(p_get_proc_address, p_library, r_initialization, callback);
    }
}

public class ExampleBinder : GDExtensionCallback
{
    [UnmanagedCallConv(CallConvs = [ typeof(CallConvCdecl) ])]
    public override void GDExtensionLevelInitialized(GDExtensionInitializationLevel level)
    {
        Console.WriteLine($"GDExtension initializing module {level}");
    }

    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public override void GDExtensionLevelDeinitialized(GDExtensionInitializationLevel level)
    {
        Console.WriteLine($"GDExtension deinitializing module {level}");
    }
}