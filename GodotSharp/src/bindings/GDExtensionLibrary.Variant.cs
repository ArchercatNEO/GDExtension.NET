namespace GDExtension;

public unsafe partial struct GDExtensionLibrary
{
    public unsafe delegate IntPtr FromTypeToVariantDelegate(long type);
    public static FromTypeToVariantDelegate FromTypeToVariantInternal;
    public static IntPtr FromTypeToVariant(long type)
    {
        return FromTypeToVariantInternal.Invoke(type);
    }
}
    