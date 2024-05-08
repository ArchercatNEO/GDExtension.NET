using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GodotCompatibility;

public unsafe class GDExtensionBinding
{
    public static GDExtensionBool ApiInitialized = 0;
    public static int[] LevelInitialized = new int[4] { 0, 0, 0, 0 };
    private static List<InitData> InitData = [];

    private static InitData cache;

    public static void Error(string message, [CallerFilePath] string fileName = "", [CallerMemberName] string functionName = "", [CallerLineNumber] int lineNumber = 0)
    {
        Internals.PrintError(message, functionName, fileName, lineNumber, 0);
    }
    
    public static byte Init(GDExtensionInterfaceGetProcAddress p_get_proc_address, GDExtensionClassLibraryPtr p_library, GDExtensionInitialization* r_initialization, GDExtensionCallback initData)
    {
        InitData data = initData.IntoStructure();
        cache = data;
        
        if (ApiInitialized == 1)
        {
            r_initialization->initialize = &InitializeLevel;
            r_initialization->deinitialize = &DeinitializeLevel;
            r_initialization->userdata = &data;
            r_initialization->MinimumLevel = initData.GetMinimumInitializationLevel();
		    return 1;
        }

        uint* rawInterface = (uint*)(void*)p_get_proc_address;
        if (rawInterface[0] == 4 && rawInterface[0] == 1)
        {
            //TODO log
            return 0;
        }

        //TODO Load error

        //TODO Check compatibility

        //TODO Load everything (with source gen probably)

        r_initialization->initialize = &InitializeLevel;
        r_initialization->deinitialize = &DeinitializeLevel;
        r_initialization->userdata = &data;
        r_initialization->MinimumLevel = initData.GetMinimumInitializationLevel();

        //TODO variant stuff
        //TODO register engine classes

        ApiInitialized = 1;

        Console.WriteLine("GDExtensionBinder succesfully ran");
        return 1;
    }

    [UnmanagedCallersOnly(CallConvs = [ typeof(CallConvCdecl) ])]
    public static void InitializeLevel(void *p_userdata, GDExtensionInitializationLevel p_level)
    {
        //! ClassDB._currentLevel = p_level;
        Console.WriteLine($"GDExtensionBinder initializing level {p_level}");

        InitData* data = (InitData*)p_userdata;
        InitData structu = *data;
        Console.WriteLine(cache.Equals(structu));
        if (data != null)
        {
            Console.WriteLine("Calling user data");
            var callback = Marshal.GetDelegateForFunctionPointer<InitializeCallback>(cache.InitCallback);
            callback.Invoke(p_level);
            Console.WriteLine("Done calling user data");
        }

        if (LevelInitialized[(int)p_level] == 0)
        {
            //! ClassDB.Initialize(p_level);
        }
        LevelInitialized[(int)p_level]++;
    }

    [UnmanagedCallersOnly(CallConvs = [ typeof(CallConvCdecl) ])]
    public static void DeinitializeLevel(void *p_userdata, GDExtensionInitializationLevel p_level)
    {
        Console.WriteLine($"GDExtensionBinder deinitializing level {p_level}");
        //! ClassDB._currentLevel = p_level;

        InitData* data = (InitData*)p_userdata;
        if (data != null)
        {
            var callback = Marshal.GetDelegateForFunctionPointer<InitializeCallback>(cache.TerminateCallback);
            callback.Invoke(p_level);
        }

        LevelInitialized[(int)p_level]--;
        if (LevelInitialized[(int)p_level] == 0)
        {
            //! EditorPlugins.Deinitialize(p_level)
            //! ClassDB.Deinitialize(p_level);
        }
    }
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void InitializeCallback(GDExtensionInitializationLevel level);

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct InitData
{
    public required int MinimumInitializationLevel;
    public required IntPtr InitCallback;
    public required IntPtr TerminateCallback;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not InitData data)
        {
            return false;
        }

        if (this.MinimumInitializationLevel != data.MinimumInitializationLevel)
        {
            Console.WriteLine($"Minimum level is different orig: {MinimumInitializationLevel}, godot: {data.MinimumInitializationLevel}");
            return false;
        }

        if (this.InitCallback != data.InitCallback)
        {
            Console.WriteLine($"Init callback is different orig: {InitCallback}, godot: {data.InitCallback}");
            return false;
        }

        if (this.TerminateCallback != data.TerminateCallback)
        {
            Console.WriteLine($"TerminateCallback is different orig: {TerminateCallback}, godot: {data.TerminateCallback}");
            return false;
        }

        return true;
    }
}

public class GDExtensionCallback
{
    public virtual GDExtensionInitializationLevel GetMinimumInitializationLevel() => GDExtensionInitializationLevel.Max;
    
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public virtual void GDExtensionLevelInitialized(GDExtensionInitializationLevel level)
    {
        Console.WriteLine($"Default initializing module {level}");
    }

    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public virtual void GDExtensionLevelDeinitialized(GDExtensionInitializationLevel level)
    {
        Console.WriteLine($"Default deinitializing module {level}");
    }

    internal unsafe InitData IntoStructure()
    {
        return new()
        {
            MinimumInitializationLevel = (int)GetMinimumInitializationLevel(),
            InitCallback = Marshal.GetFunctionPointerForDelegate<InitializeCallback>(GDExtensionLevelInitialized),
            TerminateCallback = Marshal.GetFunctionPointerForDelegate<InitializeCallback>(GDExtensionLevelDeinitialized)
        };
    }
}
