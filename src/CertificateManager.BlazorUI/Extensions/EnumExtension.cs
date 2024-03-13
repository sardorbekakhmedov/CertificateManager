using System.ComponentModel.DataAnnotations;

namespace CertificateManager.BlazorUI.Extensions;

public static class EnumExtension
{
    public static string GetDisplayName(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes == null || descriptionAttributes.Length == 0)
        {
            return value.ToString();
        }

        return descriptionAttributes[0].Name;
    }

}