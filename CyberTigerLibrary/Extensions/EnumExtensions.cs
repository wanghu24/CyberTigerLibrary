using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CyberTigerLibrary.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();

            return enumType
                    .GetMember(enumValue.ToString())
                    .Where(x => x.MemberType == MemberTypes.Field && ((FieldInfo)x).FieldType == enumType)
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
        }
        public static string GetDisplayName(this int e, Type enumType)
        {
            var name = Enum.GetName(enumType, e);

            return enumType
                    .GetMember(name)
                    .Where(x => x.MemberType == MemberTypes.Field && ((FieldInfo)x).FieldType == enumType)
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? name;
        }
        public static List<KeyValuePair<string, int>> GetEnumList(this Type enumType)
        {
            var list = new List<KeyValuePair<string, int>>();

            foreach (int e in Enum.GetValues(enumType))
            {
                list.Add(new KeyValuePair<string, int>(e.GetDisplayName(enumType), e));
            }

            return list;
        }
        public static T ToEnum<T>(this int val) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), val))
            {
                return (T)Enum.ToObject(typeof(T), val);
            }

            return default(T);
        }
        public static Type GetEnumType(this string typeName)
        {
            try
            {
                Type enumType = Type.GetType(typeName);
                return enumType;
            }
            catch (Exception)
            {

            }

            return null;
        }
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }
        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }
        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
    }
}

