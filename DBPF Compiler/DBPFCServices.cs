namespace DBPF_Compiler;

public static class DBPFCServices
{
    private static readonly Dictionary<string, object> _services = [];

    public static T AddService<T>() where T : notnull, new()
    {
        T service = new();
        AddService(service);

        return service;
    }
    public static void AddService<T>(T service) where T : notnull =>
        _services.Add(typeof(T).Name, service);

    public static T? GetService<T>()
    {
        object? service = GetService(typeof(T).Name);
        return service == null ? default : (T)service;
    }
    public static object? GetService(string serviceName) =>
        _services.TryGetValue(serviceName, out object? service) ? service : null;
}
