using System;

public static class TypeUtil
{
    public static T GetAttribute<T>(this Type type)
            where T:Attribute
    {
        object[] attributes = type.GetCustomAttributes(true);

        foreach (object attribute in attributes)
        {
            if (attribute is T)
            {
                return attribute as T;
            }
        }

        return null;
    }
}