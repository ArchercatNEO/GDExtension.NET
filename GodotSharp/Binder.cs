using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GDExtension.NativeInterop;

namespace GDExtension;

public unsafe class GDExtensionBinder
{
    private static NativeCalls? nativeCalls;
    public static int[] LevelInitialized = new int[4] { 0, 0, 0, 0 };
    
    protected static NativeCalls? InitializeAssembly(IntPtr p_get_proc_address, GDExtensionClassLibraryPtr p_library, GDExtensionInitialization* r_initialization, GDExtensionBinder userBinder)
    {
        var initializeLevel = Marshal.GetFunctionPointerForDelegate<InitializeLevelChangedDelegate>(userBinder.InitializeLevel);
        var deinitializeLevel = Marshal.GetFunctionPointerForDelegate<InitializeLevelChangedDelegate>(userBinder.DeinitializeLevel);
        
        if (nativeCalls != null)
        {
            r_initialization->initialize = initializeLevel;
            r_initialization->deinitialize = deinitializeLevel;
            r_initialization->MinimumLevel = userBinder.MinimumInitializationLevel;
		    return nativeCalls;
        }

        uint* rawInterface = (uint*)(void*)p_get_proc_address;
        if (rawInterface[0] == 4 && rawInterface[0] == 1)
        {
            //TODO log
            return null;
        }

        var procAddressLoader = Marshal.GetDelegateForFunctionPointer<GetProcAddress>(p_get_proc_address);

        NativeCalls callBinder = new(procAddressLoader, p_library);

        //TODO Load error

        //TODO Check compatibility

        //TODO Load everything (with source gen probably)
        callBinder.NewStringNameFromUtf8CharsAndLengthInternal = callBinder.LoadProcAdress<NativeCalls.NewStringNameFromUtf8CharsAndLengthDelegate>("string_name_new_with_latin1_chars");
        StringName name = new(callBinder, "MyAwesomeNode");
        callBinder.RegisterExtensionClassInternal = callBinder.LoadProcAdress<NativeCalls.RegisterExtensionClassDelegate>("classdb_register_extension_class3");

        r_initialization->initialize = initializeLevel;
        r_initialization->deinitialize = deinitializeLevel;
        r_initialization->MinimumLevel = userBinder.MinimumInitializationLevel;

        //TODO variant stuff
        //TODO register engine classes
        nativeCalls = callBinder;
        return nativeCalls;
    }

    [UnmanagedCallConv(CallConvs = [ typeof(CallConvCdecl) ])]
    protected virtual void InitializeLevel(IntPtr _, GDExtensionInitializationLevel level)
    {
        //! ClassDB._currentLevel = p_level;

        if (LevelInitialized[(int)level] == 0)
        {
            //! ClassDB.Initialize(p_level);
        }
        LevelInitialized[(int)level]++;
    }

    [UnmanagedCallConv(CallConvs = [ typeof(CallConvCdecl) ])]
    protected virtual void DeinitializeLevel(IntPtr _, GDExtensionInitializationLevel level)
    {
        //! ClassDB._currentLevel = p_level;

        LevelInitialized[(int)level]--;
        if (LevelInitialized[(int)level] == 0)
        {
            //! EditorPlugins.Deinitialize(p_level)
            //! ClassDB.Deinitialize(p_level);
        }
    }

    protected virtual GDExtensionInitializationLevel MinimumInitializationLevel => GDExtensionInitializationLevel.Max;
}
