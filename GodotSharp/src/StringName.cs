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
	internal IntPtr _data = 314;

	public unsafe StringName(NativeCalls calls, string name)
	{
		Console.WriteLine($"[{_data}], {name}");
		
		GCHandle _selfPin = GCHandle.Alloc(this, GCHandleType.Pinned);
		IntPtr ptr = _selfPin.AddrOfPinnedObject();
		
		byte[] utf8 = Encoding.UTF8.GetBytes(name);
		calls.NewStringNameFromUtf8CharsAndLengthInternal.Invoke(ptr, name, utf8.Length);
		
		_selfPin.Free();
		
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
        byte* strPtr = (byte*)_data;
		string repr = Marshal.PtrToStringUTF8(_data);
        return repr;
    }
}