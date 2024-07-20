public static class EnumHelper
{
    public static string ToStringList(Type enumType) => string.Join(",", Enum.GetNames(enumType));
}