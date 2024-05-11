//! This is where we define other classes and types defined in gdextension.h
global using unsafe GDExtensionClassLibraryPtr = void*;
using System.Runtime.InteropServices;

namespace GDExtension.NativeInterop;

public enum GDExtensionInitializationLevel {
	Core,
	Servers,
	Scene,
	Editor,
	Max,
}

public enum ModuleInitializationLevel {
	Core = 0,
	Servers = 1,
	Scene = 2,
	Editor = 3,
	Max = 4
};

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr GetProcAddress(string functionName);

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
