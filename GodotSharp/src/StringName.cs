using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using GDExtension.NativeInterop;

namespace GDExtension;

//This probably has some memory layout rules but beats void*
[StructLayout(LayoutKind.Explicit)]
public sealed class StringName
{
	/// <summary>
	/// Pointer to the shared string
	/// </summary>
	[FieldOffset(0)] internal IntPtr _data = 314;

	public unsafe StringName(NativeCalls calls, string name)
	{
		Console.WriteLine($"[{_data}], {name}");
		
		GCHandle _selfPin = GCHandle.Alloc(this, GCHandleType.Pinned);
		IntPtr ptr = _selfPin.AddrOfPinnedObject();
		
		byte[] utf16 = Encoding.Default.GetBytes(name);
		byte[] utf8 = Encoding.Convert(Encoding.Default, Encoding.Latin1, utf16);
		byte[] ascii = [65, 76, 67, 65, 0];
		void* start = Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(ascii));
		calls.NewStringNameFromUtf8CharsAndLengthInternal.Invoke(ptr, null, 0);
		
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