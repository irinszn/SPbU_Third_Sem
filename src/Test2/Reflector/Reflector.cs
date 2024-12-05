namespace MyReflector;

using System.Reflection;

/// <summary>
/// Class that implements the output of all types of a class to a file.
/// </summary>
public class MyReflector
{
    /// <summary>
    /// Prints class structure in file.
    /// </summary>
    /// <param name="someClass">Name of the class.</param>
    public void PrintStructure(Type someClass)
    {
        var className = someClass.Name;

        var fileName = $"{className}.cs";

        using var writer = new StreamWriter(fileName);

        writer.WriteLine($"Class {className}");

        foreach (var field in someClass.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
        {
            writer.WriteLine($"{GetFieldModifiers(field)} {field.FieldType.Name} {field.Name}");
        }

        foreach (var method in someClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
        {
            if (method.IsSpecialName)
            {
                continue;
            }

            writer.WriteLine($"{GetMethodModifiers(method)} {method.ReturnType.Name} {method.Name} ({GetMethodParameters(method)}) {{}}");
        }

        foreach (var nestedType in someClass.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
        {
            writer.WriteLine($"Nested class: {nestedType.Name}");
        }
    }

    /// <summary>
    /// Compares the presence of types in classes.
    /// </summary>
    /// <param name="a">First class.</param>
    /// <param name="b">Second class.</param>
    public void DiffClasses(Type a, Type b)
    {
        var aMembers = a.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        var bMembers = b.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        foreach (var aMember in aMembers)
        {
            var bMember = bMembers.FirstOrDefault(m => m.Name == aMember.Name && m.MemberType == aMember.MemberType);

            if (bMember == null)
            {
                Console.WriteLine($" {aMember.MemberType} {aMember.Name} is only in {a.Name}");
            }
        }
    }

    private string GetFieldModifiers(FieldInfo field)
    {
        var modificator = "";
 
        if (field.IsPublic)
            modificator += "public ";
        else if (field.IsPrivate)
            modificator += "private ";
        else if (field.IsAssembly)
            modificator += "internal ";
        else if (field.IsFamily)
            modificator += "protected ";
        else if (field.IsFamilyAndAssembly)
            modificator += "private protected ";
        else if (field.IsFamilyOrAssembly)
            modificator += "protected internal ";

        if (field.IsStatic) modificator += "static ";

        return modificator;
    }

    private string GetMethodModifiers(MethodInfo method)
    {
        var modificator = "";
 
        if (method.IsPublic)
            modificator += "public ";
        else if (method.IsPrivate)
            modificator += "private ";
        else if (method.IsAssembly)
            modificator += "internal ";
        else if (method.IsFamily)
            modificator += "protected ";
        else if (method.IsFamilyAndAssembly)
            modificator += "private protected ";
        else if (method.IsFamilyOrAssembly)
            modificator += "protected internal ";

        if (method.IsStatic) modificator += "static ";

        return modificator;
    }

    private string GetMethodParameters(MethodInfo method)
    {
        var params = method.GetParameters();
        var result = "";
        var count = 0;

        foreach (var param in params)
        {
            var modificator = "";

            if (param.IsIn) 
                modificator = "in";
            else 
                if (param.IsOut) 
                    modificator = "out";

            result += $"{param.ParameterType.Name} {modificator} {param.Name}";

            if (count < params.Length - 1)
                result += ", ";

            count++;
        }

        return result;
    }
}