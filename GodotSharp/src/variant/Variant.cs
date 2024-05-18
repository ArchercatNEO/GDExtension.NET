using System.Runtime.InteropServices;
using GodotSharp;

namespace GDExtension;

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Variant
{
    private fixed byte opaque[64]; 
    
    public enum Type
    {

    }


    public static void InitializeBindings()
    {
        //GDExtensionLibrary.FromTypeToVariantInternal = nativeCalls.LoadProcAdress<NativeCalls.FromTypeToVariantDelegate>("get_variant_from_type_constructor");
        VariantToInt64Internal = Marshal.GetDelegateForFunctionPointer<VariantToInt64Cast>(GDExtensionLibrary.FromTypeToVariant(2));
    }

    private delegate long VariantToInt64Cast(long* retVal, Variant self);
    private static VariantToInt64Cast VariantToInt64Internal;
    
    public static unsafe explicit operator long(Variant self)
    {
        long retVal;
        VariantToInt64Internal(&retVal, self);
        return retVal;
    }

    static partial void FromTypeToVariant();
}
