using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GDExtension;
using GDExtension.NativeInterop;

namespace Summator;

public class SummatorBinder : GDExtensionBinder
{
    private static NativeCalls calls;
    
    [UnmanagedCallersOnly(EntryPoint = "init_assembly")]
    public static unsafe void BindAssembly(IntPtr p_get_proc_address, void* p_library, GDExtensionInitialization* r_initialization)
    {
        SummatorBinder binder = new();
        calls = InitializeAssembly(p_get_proc_address, p_library, r_initialization, binder);
    }

    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    protected override void InitializeLevel(nint _, GDExtensionInitializationLevel p_level)
    {
        base.InitializeLevel(_, p_level);
        Console.WriteLine($"SummatorBinder initializing module {p_level}");

        ClassCreationInfo newClass = Summator3D.GenerateBind();
        
        calls.RegisterExtensionClass("Summator3D", "Node", ref newClass);
    }

    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    protected override void DeinitializeLevel(nint _, GDExtensionInitializationLevel p_level)
    {
        base.DeinitializeLevel(_, p_level);
        Console.WriteLine($"SummatorBinder deinitializing module {p_level}");
    }
}