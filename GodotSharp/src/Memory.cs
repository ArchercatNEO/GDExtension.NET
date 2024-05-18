using GodotSharp;

namespace Godot;

public unsafe struct Memory
{
    [DynamicImport(EntryPoint = "mem_alloc")]
    public static unsafe partial void* Alloc(ulong bytes);

    [DynamicImport(EntryPoint = "mem_realloc")]
    public static unsafe partial void* Realloc(void* prev, ulong bytes);

    [DynamicImport(EntryPoint = "mem_free")]
    public static unsafe partial void Free(void* ptr);
}