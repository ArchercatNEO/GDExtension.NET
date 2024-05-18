using System.Runtime.InteropServices;

namespace GDExtension.NativeInterop;

#region GDExtension loading API

public enum GDExtensionInitializationLevel {
	Core,
	Servers,
	Scene,
	Editor,
	Max,
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr GetProcAddress(string functionName);

public static class GetProcAddrExtensions
{
	public static T? LoadProcAddr<T>(this GetProcAddress self, string functionName) where T : Delegate
	{
		var procAddr = self.Invoke(functionName);
		if (procAddr == IntPtr.Zero)
		{
			Console.WriteLine($"Failed to load gdextension function {functionName}");
			return null;
		}

		T delegatePtr = Marshal.GetDelegateForFunctionPointer<T>(procAddr);
		return delegatePtr;
	}
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void InitializeLevelChangedDelegate(IntPtr _, GDExtensionInitializationLevel level);

[StructLayout(LayoutKind.Sequential)]
public unsafe struct GDExtensionInitialization
{
    public GDExtensionInitializationLevel MinimumLevel;
    public void* userdata;
	
    public IntPtr initialize; 
	public IntPtr deinitialize; 
}

#endregion GDExtension loading API

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PropertyInfo
{
	public Variant.Type Type;
	
	public StringName Name;

	public StringName ClassName;
	public uint HintFlags;
	public StringName HintString;
	public uint UsageFlags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct MethodInfo
{
	/// <summary>
	/// The name of the method
	/// </summary>
	public StringName Name;

	/// <summary>
	/// Pointer to captured variables in lambdas
	/// </summary>
	public void* UserData;

	/// <summary>
	/// 
	/// </summary>
	public IntPtr CallFunc;
	public IntPtr CallFuncPtr;
	public MethodFlags MethodFlags;
	
	public byte HasReturnValue;
	public PropertyInfo* ReturnValue;
	public MethodArgumentMetadata ReturnMetadata;
	
	public int ArgumentCount;
	public PropertyInfo* ArgumentInfo;
	public MethodArgumentMetadata* ArgumentInfoMetadata;

	public int DefaultArgumentCount;
	public Variant* DefaultArguments;
}

public unsafe struct VirtualMethodInfo
{
	
}

/// <summary>
/// All the information needed to call a method
/// </summary>
/// <param name="userData">Captured variables</param>
/// <param name="self">The "this" pointer</param>
/// <param name="args">Pointer to an Variant array of size argCount</param>
/// <param name="argCount">The length of the args array</param>
/// <param name="returnVal">Pointer to the return value that will be read</param>
/// <param name="err">Pointer to error register</param>
public unsafe delegate void ClassMethodCall(IntPtr userData, IntPtr self, Variant* args, long argCount, Variant returnVal, IntPtr err);

/// <summary>
/// 
/// </summary>
/// <param name="userData"></param>
/// <param name="self"></param>
/// <param name="args"></param>
/// <param name="argCount"></param>
/// <param name="returnVal"></param>
/// <param name="err"></param>
public unsafe delegate void ClassMethodPtrCall(void* userData, void* self, Variant* args, long argCount, Variant returnVal, IntPtr err);

[Flags]
public enum MethodFlags
{
	Normal = 1 << 0,
	Editor = 1 << 1,
	Const = 1 << 2,
	Virtual = 1 << 3,
	Varargs = 1 << 4,
	Static = 1 << 5,
}

/// <summary>
/// Specifies the bit size of ints or floats
/// </summary>
public enum MethodArgumentMetadata
{
	None,
	IntIsInt8,
	IntIsInt16,
	IntIsInt32,
	IntIsInt64,
	IntIsUInt8,
	IntIsUInt16,
	IntIsUInt32,
	IntIsUInt64,
	RealIsFloat32,
	RealIsFloat64,
}

//This also has memory things
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ClassCreationInfo
{
	public required GDExtensionBool IsVirtual;
	public required GDExtensionBool IsAbstract;
	public required GDExtensionBool IsExposed;
	public required GDExtensionBool IsRuntime;
	public required IntPtr set_func;
	public required IntPtr get_func;
	public required IntPtr get_property_list_func;
	public required IntPtr free_property_list_func;
	public required IntPtr property_can_revert_func;
	public required IntPtr property_get_revert_func;
	public required IntPtr validate_property_func;
	public required IntPtr notification_func;
	public required IntPtr to_string_func;
	public required IntPtr reference_func;
	public required IntPtr unreference_func;
	
	/// <summary>
	/// Default constructor. Mandatory even in virtual/abstract classes
	/// </summary>
	public required IntPtr create_instance_func;
	
	/// <summary>
	/// Destructor. Mandatory
	/// </summary>
	public required IntPtr free_instance_func;

	/// <summary>
	/// Used for hot-reloading a class instance
	/// </summary>
	public required IntPtr recreate_instance_func;
	
	/// <summary>
	/// Queries a virtual function by name and returns a callback to invoke the requested virtual function.
	/// </summary>
	public required IntPtr get_virtual_func;
	
	/// <summary>
	/// Paired with `call_virtual_with_data_func`, this is an alternative to `get_virtual_func` for extensions that
	/// need or benefit from extra data when calling virtual functions.
	/// Returns user data that will be passed to `call_virtual_with_data_func`.
	/// Returning `NULL` from this function signals to Godot that the virtual function is not overridden.
	/// Data returned from this function should be managed by the extension and must be valid until the extension is deinitialized.
	/// You should supply either `get_virtual_func`, or `get_virtual_call_data_func` with `call_virtual_with_data_func`.
	/// </summary>
	public required IntPtr get_virtual_call_data_func;
	
	/// <summary>
	/// Used to call virtual functions when `get_virtual_call_data_func` is not null.
	/// </summary>
	public required IntPtr call_virtual_with_data_func;
	
	public required IntPtr get_rid_func;
	
	/// <summary>
	/// Per-class user data, later accessible in instance bindings.
	/// </summary>
	public required void *class_userdata;
}
