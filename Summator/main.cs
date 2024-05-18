using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GDExtension;
using GDExtension.NativeInterop;
using Godot;

namespace Summator;

public class SummatorBinder : GDExtensionBinder
{
    static IntPtr libPtr;
    
    [UnmanagedCallersOnly(EntryPoint = "init_assembly")]
    public static unsafe void BindAssembly(IntPtr p_get_proc_address, IntPtr p_library, GDExtensionInitialization* r_initialization)
    {
        libPtr = p_library;
        SummatorBinder binder = new();
        InitializeAssembly(p_get_proc_address, p_library, r_initialization, binder);
    }

    protected unsafe override void InitializeLevel(nint _, GDExtensionInitializationLevel p_level)
    {
        base.InitializeLevel(_, p_level);
        Console.WriteLine($"SummatorBinder initializing module {p_level}");
        
        if (p_level == GDExtensionInitializationLevel.Scene)
        {
            ClassCreationInfo newClass = Summator3D.GenerateBind();
            ClassDB.RegisterExtensionClass(libPtr, "Summator3D", "Node", &newClass);
            
            MethodInfo info = new()
            {
                Name = new("Add"),
                UserData = null,
                MethodFlags = MethodFlags.Normal,
                CallFunc = Marshal.GetFunctionPointerForDelegate<ClassMethodCall>(Summator3D.AddCall),
                HasReturnValue = 0,
                ArgumentCount = 0,
                ArgumentInfoMetadata = null,
                DefaultArgumentCount = 0,
                DefaultArguments = null
            };

            ClassDB.RegisterExtensionClassMethod(libPtr, "Summator3D", &info);
        }
    }

    protected override void DeinitializeLevel(nint _, GDExtensionInitializationLevel p_level)
    {
        base.DeinitializeLevel(_, p_level);
        Console.WriteLine($"SummatorBinder deinitializing module {p_level}");
    }
}