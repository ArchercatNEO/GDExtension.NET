using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GDExtension.NativeInterop;

namespace GDExtension;

//This probably has some memory layout rules but beats void*
[StructLayout(LayoutKind.Sequential)]
public sealed class StringName
{
	/// <summary>
	/// Pointer to the shared string
	/// </summary>
	internal IntPtr _data;

	public unsafe StringName(NativeCalls calls, string name)
	{
		Console.WriteLine($"[{_data}], {name}");
		GCHandle self = GCHandle.Alloc(this, GCHandleType.Pinned);
		IntPtr addr = self.AddrOfPinnedObject();
		
		calls.NewStringNameFromUtf8CharsAndLengthInternal.Invoke(addr, name);
		
		self.Free();
		Console.WriteLine($"[{_data}], {this}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsEmpty()
	{
		return _data == IntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsAllocated()
	{
		return _data != IntPtr.Zero;
	}

    public override unsafe string ToString()
    {
		string repr = Marshal.PtrToStringUni(_data);
        return repr;
    }
}