using System;
using System.Reflection;
using LinqTools;
using System.ComponentModel;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class TypedFieldAttribute : Attribute
{
    public readonly Type CorrespondingType;

    public TypedFieldAttribute (Type type)
    {
        CorrespondingType = type;
    }
}

public static class EnumUtils
{
    public static T GetAttribute<T>(this Enum enumValue) where T: Attribute
    {
        MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString())
            .FirstOrDefault();

        if (memberInfo != null)
        {
            return memberInfo.GetCustomAttributes(typeof (T), false).FirstOrDefault() as T;
        }
        return null;
    }

    public static Type GetCorrespondingType(this Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                TypedFieldAttribute attr = Attribute.GetCustomAttribute(field, typeof(TypedFieldAttribute)) as TypedFieldAttribute;
                if (attr != null)
                {
                    return attr.CorrespondingType;
                }
            }
        }
        return null;
    }

    public static string GetDescription(this Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
        }
        return null;
    }

    public static List<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}
