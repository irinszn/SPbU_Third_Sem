namespace MyNUnit;

using System.Reflection;
using System.Threading.Tasks;

public class TestRunner
{
    public void RunTest(string path)
    {
        var assemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom);
    
        var tasks = new List<Task>();

        foreach (var assembly in assemblies)
        {
            tasks.Add(Task.Run(() => ExecuteTests(assembly)));
        }

        Task.WaitAll(tasks.ToArray());
    }

    private void ExecuteTests(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            ExecuteBeforeClass(type);
            var testMethods = type.GetMethods().Where(m => m.GetCustomAttribute<TestAttribute>() != null);

            foreach (var method in testMethods)
            {
                ExecuteBefore(type);
                ExecuteTest(method);
                ExecuteAfter(type);
            }

            ExecuteAfterClass(type);
        }
    }

    private void ExecuteBeforeClass(Type type)
    {
        var methodBeforeClass = type.GetMethod("BeforeClass", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        methodBeforeClass?.Invoke(null, null);
    }

    private void ExecuteAfterClass(Type type)
    {
        var methodAfterClass = type.GetMethod("AfterClass", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        methodAfterClass?.Invoke(null, null);
    }

    private void ExecuteBefore(Type type)
    {
        var methodBefore = type.GetMethod("Before", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        methodBefore?.Invoke(Activator.CreateInstance(type), null);
    }

    private void ExecuteAfter(Type type)
    {
        var methodAfter = type.GetMethod("After", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        methodAfter?.Invoke(Activator.CreateInstance(type), null);
    }

    private void ExecuteTest(MethodInfo method)
    {
        var instance = Activator.CreateInstance(method.DeclaringType); //создает экземпляр типа метода
        var testAttribute = method.GetCustomAttribute<TestAttribute>(); //экземпляр атрибута

        if (testAttribute?.Ignore != null)
        {
            Console.WriteLine($"Ignored: {method.Name} - Reason: {testAttribute.Ignore}");
            return;
        }

        try
        {
            method.Invoke(instance, null);
            Console.WriteLine($"Passed: {method.Name}");
        }
        catch (Exception ex)
        {
            if (testAttribute?.Expected != null && ex.InnerException?.GetType() == testAttribute.Expected)
            {
                Console.WriteLine($"Expected Exception for {method.Name}: {ex.InnerException.Message}");
            }
            else
            {
                Console.WriteLine($"Failed: {method.Name} - Exception: {ex.InnerException?.Message}");
            }
        }
    }
}