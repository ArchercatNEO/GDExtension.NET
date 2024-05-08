// See https://aka.ms/new-console-template for more information
using GodotCompatibility;

public unsafe class Bootloader
{
    public static void BootloaderInit(GDExtensionInterfaceGetProcAddress p_get_proc_address, GDExtensionClassLibraryPtr p_library, GDExtensionInitialization *r_initialization)
    {

    }

    public static void Initialize(ModuleInitializationLevel p_level)
    {
        delegate*<ModuleInitializationLevel, void> init = &Uninitialize;
        ClassCreationInfo newClass = new()
        {
            IsVirtual = 0,
            IsAbstract = 0,
            IsExposed = 1,
            IsRuntime = 1,
            set_func = null,
            get_func = null,
            get_property_list_func = null,
            free_instance_func = null,
            free_property_list_func = null,
            property_can_revert_func = null,
            property_get_revert_func = null,
            validate_property_func = null,
            notification_func = null,
            to_string_func = null,
            recreate_instance_func = null,
            reference_func = null,
            unreference_func = null,
            create_instance_func = null,
            get_rid_func = null,
            get_virtual_call_data_func = null,
            get_virtual_func = null,
            call_virtual_with_data_func = null,
            class_userdata = null,
        };

        Internals.RegisterExtensionClass(null, null, null, newClass);
    }

    public static void Uninitialize(ModuleInitializationLevel p_level)
    {

    }
}

