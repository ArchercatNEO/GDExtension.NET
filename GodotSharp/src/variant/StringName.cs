using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GDExtension.NativeInterop;
using GodotSharp;

namespace GDExtension;

[StructLayout(LayoutKind.Sequential)]
public sealed partial class StringName
{
	/// <summary>
	/// Pointer to the shared string
	/// </summary>
	internal IntPtr _data;

	public static implicit operator StringName(string name)
	{
		Console.WriteLine($"[0], {name}");
		GDExtensionLibrary.NewStringNameFromUtf8CharsAndLengthInternal(out var stringName, name);
		Console.WriteLine($"[{stringName._data}], {stringName}");
		return stringName;
	}

    [DynamicImport]
	static partial void StringNameNewFromUtf8CharsAndLength();

	public unsafe StringName(string name)
	{
		//GCHandle self = GCHandle.Alloc(this, GCHandleType.Pinned);
		//IntPtr addr = self.AddrOfPinnedObject();
		
		//self.Free();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[DynamicImport]
	public bool IsEmpty()
	{
		return _data == IntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[DynamicImport]
	public bool IsAllocated()
	{
		return _data != IntPtr.Zero;
	}

	[DynamicImport]
    public override unsafe string ToString()
    {
		string repr = Marshal.PtrToStringUni(_data);
        return repr;
    }
}