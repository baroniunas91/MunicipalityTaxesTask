using MunicipalityTaxesAPI.Exceptions;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;

namespace MunicipalityTaxesAPI.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     Returns <see cref="EnumMemberAttribute"/> value of <see cref="Enum"/>
        /// </summary>
        /// <param name="value"><see cref="Enum"/> value</param>
        /// <returns><see cref="EnumMemberAttribute"/> value</returns>
        public static string GetEnumMemberValue(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            // Get fieldInfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringValue attributes
            var stringValueAttribute = (EnumMemberAttribute)fieldInfo?
                .GetCustomAttribute(typeof(EnumMemberAttribute), false);

            // Return the first if there was a match.
            return stringValueAttribute?.Value;
        }

        /// <summary>
        ///     Returns <see cref="Enum"/> value by <see cref="EnumMemberAttribute"/> value
        /// </summary>
        /// <param name="description"><see cref="EnumMemberAttribute"/> value</param>
        /// <returns>Value if successfully parsed, null otherwise</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when value cannot be parsed</exception>
        public static T ParseByAttribute<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                {
                    if (attribute.Value == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new HttpStatusException(HttpStatusCode.BadRequest, $"Parameter {description} out of range");
        }
    }
}
