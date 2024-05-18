using System.Runtime.InteropServices;
using GDExtension.NativeInterop;

namespace GDExtension;

public unsafe class GDExtensionBinder
{
    private static bool apiInitialized = false;
    public static int[] LevelInitialized = new int[4] { 0, 0, 0, 0 };
    
    protected static void InitializeAssembly(IntPtr p_get_proc_address, IntPtr p_library, GDExtensionInitialization* r_initialization, GDExtensionBinder userBinder)
    {
        var initializeLevel = Marshal.GetFunctionPointerForDelegate<InitializeLevelChangedDelegate>(userBinder.InitializeLevel);
        var deinitializeLevel = Marshal.GetFunctionPointerForDelegate<InitializeLevelChangedDelegate>(userBinder.DeinitializeLevel);
        
        if (apiInitialized)
        {
            r_initialization->initialize = initializeLevel;
            r_initialization->deinitialize = deinitializeLevel;
            r_initialization->MinimumLevel = userBinder.MinimumInitializationLevel;
        }

        uint* rawInterface = (uint*)(void*)p_get_proc_address;
        if (rawInterface[0] == 4 && rawInterface[0] == 1)
        {
            //TODO log
            return;
        }

        var procAddressLoader = Marshal.GetDelegateForFunctionPointer<GetProcAddress>(p_get_proc_address);

        //TODO Load error

        //TODO Check compatibility

        //TODO Load everything (with source gen probably)
        GDExtensionLibrary.NewStringNameFromUtf8CharsAndLengthInternal = procAddressLoader.LoadProcAddr<GDExtensionLibrary.NewStringNameFromUtf8CharsAndLengthDelegate>("string_name_new_with_utf8_chars");
        GDExtensionLibrary.RegisterExtensionClassInternal = procAddressLoader.LoadProcAddr<GDExtensionLibrary.RegisterExtensionClassDelegate>("classdb_register_extension_class3");
        GDExtensionLibrary.RegisterExtensionClassMethodInternal = procAddressLoader.LoadProcAddr<GDExtensionLibrary.RegisterExtensionClassMethodDelegate>("classdb_register_extension_class_method");

        r_initialization->initialize = initializeLevel;
        r_initialization->deinitialize = deinitializeLevel;
        r_initialization->MinimumLevel = userBinder.MinimumInitializationLevel;

        Variant.InitializeBindings();
        //TODO register engine classes

        apiInitialized = true;
    }

    protected virtual void InitializeLevel(IntPtr _, GDExtensionInitializationLevel level)
    {
        //! ClassDB._currentLevel = p_level;

        if (LevelInitialized[(int)level] == 0)
        {
            //! ClassDB.Initialize(p_level);
        }
        LevelInitialized[(int)level]++;
    }

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

    protected virtual GDExtensionInitializationLevel MinimumInitializationLevel => GDExtensionInitializationLevel.Core;
}
