//! This is where we define other classes and types defined in gdextension.h

global using unsafe GDExtensionInterfaceGetProcAddress = delegate* unmanaged[Cdecl]<string, void*>;
global using unsafe GDExtensionClassLibraryPtr = void*;
global using unsafe Callback = delegate* unmanaged[Cdecl]<GodotCompatibility.ModuleInitializationLevel, void>;

namespace GodotCompatibility;


//TODO use actual methods instead of random memory
using unsafe GDExtensionClassSet = void*;
using unsafe GDExtensionClassGet = void*;
using unsafe GDExtensionClassGetPropertyList = void*;
using unsafe GDExtensionClassFreePropertyList = void*;
using unsafe GDExtensionClassPropertyCanRevert = void*;
using unsafe GDExtensionClassPropertyGetRevert = void*;
using unsafe GDExtensionClassValidateProperty = void*;
using unsafe GDExtensionClassNotification2 = void*;
using unsafe GDExtensionClassToString = void*;
using unsafe GDExtensionClassReference = void*;
using unsafe GDExtensionClassUnreference = void*;
using unsafe GDExtensionClassCreateInstance = void*;
using unsafe GDExtensionClassFreeInstance = void*;
using unsafe GDExtensionClassRecreateInstance = void*;
using unsafe GDExtensionClassGetVirtual = void*;
using unsafe GDExtensionClassGetVirtualCallData = void*;
using unsafe GDExtensionClassCallVirtualWithData = void*;
using unsafe GDExtensionClassGetRID = void*;


//This also has memory things
public unsafe struct ClassCreationInfo
{
	public required GDExtensionBool IsVirtual;
	public required GDExtensionBool IsAbstract;
	public required GDExtensionBool IsExposed;
	public required GDExtensionBool IsRuntime;
	public required GDExtensionClassSet set_func;
	public required GDExtensionClassGet get_func;
	public required GDExtensionClassGetPropertyList get_property_list_func;
	public required GDExtensionClassFreePropertyList free_property_list_func;
	public required GDExtensionClassPropertyCanRevert property_can_revert_func;
	public required GDExtensionClassPropertyGetRevert property_get_revert_func;
	public required GDExtensionClassValidateProperty validate_property_func;
	public required GDExtensionClassNotification2 notification_func;
	public required GDExtensionClassToString to_string_func;
	public required GDExtensionClassReference reference_func;
	public required GDExtensionClassUnreference unreference_func;
	
	/// <summary>
	/// Default constructor. Mandatory even in virtual/abstract classes
	/// </summary>
	public required GDExtensionClassCreateInstance create_instance_func;
	
	/// <summary>
	/// Destructor. Mandatory
	/// </summary>
	public required GDExtensionClassFreeInstance free_instance_func;
	public required GDExtensionClassRecreateInstance recreate_instance_func;
	
	/// <summary>
	/// Queries a virtual function by name and returns a callback to invoke the requested virtual function.
	/// </summary>
	public required GDExtensionClassGetVirtual get_virtual_func;
	
	/// <summary>
	/// Paired with `call_virtual_with_data_func`, this is an alternative to `get_virtual_func` for extensions that
	/// need or benefit from extra data when calling virtual functions.
	/// Returns user data that will be passed to `call_virtual_with_data_func`.
	/// Returning `NULL` from this function signals to Godot that the virtual function is not overridden.
	/// Data returned from this function should be managed by the extension and must be valid until the extension is deinitialized.
	/// You should supply either `get_virtual_func`, or `get_virtual_call_data_func` with `call_virtual_with_data_func`.
	/// </summary>
	public required GDExtensionClassGetVirtualCallData get_virtual_call_data_func;
	
	/// <summary>
	/// Used to call virtual functions when `get_virtual_call_data_func` is not null.
	/// </summary>
	public required GDExtensionClassCallVirtualWithData call_virtual_with_data_func;
	
	public required GDExtensionClassGetRID get_rid_func;
	
	/// <summary>
	/// Per-class user data, later accessible in instance bindings.
	/// </summary>
	public required void *class_userdata;
}

//This probably has some memory layout rules but beats void*
public struct StringName
{

}

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

//May need to be unsafe

public unsafe struct GDExtensionInitialization
{
    public GDExtensionInitializationLevel MinimumLevel;
    public void* userdata;
	
    public delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void> initialize; 
	public delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void> deinitialize; 
}