using System;
using System.Runtime.InteropServices;
using GDExtension;
using GDExtension.NativeInterop;

public class Summator3D
{
    public static unsafe ClassCreationInfo GenerateBind()
    {
        return new()
        {
            IsVirtual = 0,
            IsAbstract = 0,
            IsExposed = 1,
            IsRuntime = 1,
            create_instance_func = Marshal.GetFunctionPointerForDelegate<CreateInstanceDelegate>(CreateInstance),
            recreate_instance_func = 0,
            free_instance_func = Marshal.GetFunctionPointerForDelegate<FreeDelegate>(Free),
            to_string_func = 0,
            get_func = Marshal.GetFunctionPointerForDelegate<GetBindDelegate>(GetBind),
            set_func = Marshal.GetFunctionPointerForDelegate<SetBindDelegate>(SetBind),
            get_property_list_func = 0,
            free_property_list_func = 0,
            property_can_revert_func = 0,
            property_get_revert_func = 0,
            validate_property_func = 0,
            notification_func = 0,
            reference_func = 0,
            unreference_func = 0,
            get_rid_func = 0,
            get_virtual_call_data_func = 0,
            get_virtual_func = 0,
            call_virtual_with_data_func = 0,
            class_userdata = null,
        };
    }

    ~Summator3D()
    {
        Console.WriteLine("Summator is somehow getting collected");
    }

    private delegate IntPtr CreateInstanceDelegate(IntPtr userData);
    static Summator3D global;
    private static IntPtr CreateInstance(IntPtr userData)
    {
        return 0;
        global = new Summator3D();
        Console.WriteLine(global);
        GCHandle handle = GCHandle.Alloc(global, GCHandleType.Pinned);
        IntPtr addr = handle.AddrOfPinnedObject();
        Console.WriteLine(addr);
        return addr;
    }

    private delegate void FreeDelegate();
    private static void Free()
    {
        Console.WriteLine("Attempting to free object");
    }

    private delegate byte GetBindDelegate(IntPtr instance, string fieldName, IntPtr ret);
    private static byte GetBind(IntPtr instance, string fieldName, IntPtr ret)
    {
        Console.WriteLine("Attempting to GetBind object");
        return 0;
    }

    private delegate byte SetBindDelegate(IntPtr instance, string fieldName, IntPtr value);
    private static byte SetBind(IntPtr instance, string fieldName, IntPtr value)
    {
        Console.WriteLine("Attempting to SetBind object");
        return 0;
    }
    
    private int count = 0;

    public void Add()
    {
        count += 3;
    }

    public static unsafe void AddCall(IntPtr userData, IntPtr selfPtr, Variant* args, long argCount, Variant retVal, IntPtr err)
    {
        Console.WriteLine(selfPtr);
        Summator3D self = *(Summator3D*)selfPtr;
        self.Add();
    }

    public int GetTotal()
    {
        return count;
    }
}