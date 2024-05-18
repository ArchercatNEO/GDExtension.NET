using GDExtension;
using GDExtension.NativeInterop;
using GodotSharp;

namespace Godot;

public unsafe partial class ClassDB
{
    /// <summary>
    /// Register a C# class into ClassDB
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="parentName"></param>
    /// <param name="info"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class3")]
    public static partial void RegisterExtensionClass(IntPtr libPtr, StringName className, StringName parentName, ClassCreationInfo* info);

    /// <summary>
    /// Register a method as part of a registered class into ClassDB
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="method"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_method")]
    public static partial void RegisterExtensionClassMethod(IntPtr libPtr, StringName className, MethodInfo* method);

    /// <summary>
    /// Register a virtual method of a registered class into ClassDB
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="method"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_virtual_method")]
    public static partial void RegisterExtensionClassVirtualMethod(IntPtr libPtr, StringName className, VirtualMethodInfo* method);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="parentName"></param>
    /// <param name="info"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_integer_constant")]
    public static partial void RegisterExtensionClassIntergerConstant(IntPtr libPtr, StringName className, StringName enumName, StringName constant, long value, byte isBitfield);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="parentName"></param>
    /// <param name="info"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_property")]
    public static partial void RegisterExtensionClassProperty(IntPtr libPtr, StringName className, PropertyInfo* propertyInfo, StringName get, StringName set);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="parentName"></param>
    /// <param name="info"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_property_indexed")]
    public static partial void RegisterExtensionClassPropertyIndexer(IntPtr libPtr, StringName className, PropertyInfo* propertyInfo, StringName get, StringName set, long index);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="parentName"></param>
    /// <param name="info"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_property_group")]
    public static partial void RegisterExtensionClassProprtyGroup(IntPtr libPtr, StringName className, string group, string groupPrefix);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="libPtr"></param>
    /// <param name="className"></param>
    /// <param name="parentName"></param>
    /// <param name="info"></param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_property_subgroup")]
    public static partial void RegisterExtensionClassProprtySubgroup(IntPtr libPtr, StringName className, string subgroup, string subgroupPrefix);

    /// <summary>
    /// Add a signal to a previously registered class
    /// </summary>
    /// <param name="libPtr">The library pointer</param>
    /// <param name="className">The class that will contain the signal</param>
    /// <param name="signalName">The name of the signal to be added</param>
    /// <param name="argInfos">A pointer to an array of the argument information</param>
    /// <param name="argCount">The length of the arg info array</param>
    [DynamicImport(EntryPoint = "classdb_register_extension_class_signal")]
    public static partial void RegisterExtensionClassSignal(IntPtr libPtr, StringName className, StringName signalName, PropertyInfo* argInfos, long argCount);

    /// <summary>
    /// Unregisters a previously registered class for cleanup
    /// </summary>
    /// <param name="libPtr">The library pointer</param>
    /// <param name="className">The name of the class to unregister</param>
    [DynamicImport(EntryPoint = "classdb_unregister_extension_class")]
    public static partial void UnregisterExtensionClass(IntPtr libPtr, StringName className);

    /// <summary>
    /// Marks an already registered extension class (that much inherit from <cref EditorPlugin/>) as a plugin
    /// </summary>
    /// <param name="className">The name of the class to be registered</param>
    [DynamicImport(EntryPoint = "editor_add_plugin")]
    public static partial void AddEditorPlugin(StringName className);

    /// <summary>
    /// Remove an active editor plugin
    /// </summary>
    /// <param name="className">The name of the class to be removed</param>
    [DynamicImport(EntryPoint = "editor_remove_plugin")]
    public static partial void RemoveEditorPlugin(StringName className);

    /// <summary>
    /// Loads XML documentation into the editor
    /// </summary>
    /// <param name="xmldoc">The XML to be loaded</param>
    [DynamicImport(EntryPoint = "editor_help_load_xml_from_utf8_chars")]
    public static partial void LoadEditorXmlDoc(string xmldoc);
}