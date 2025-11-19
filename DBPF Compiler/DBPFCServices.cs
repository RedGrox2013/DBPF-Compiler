namespace DBPF_Compiler;

public static class DBPFCServices
{
    private static readonly List<object> _services = [];

    public static T AddService<T>() where T : notnull, new()
    {
        T service = new();
        AddService(service);

        return service;
    }
    public static void AddService<T>(T service) where T : notnull =>
        _services.Add(service);

    public static T? GetService<T>()
    {
        foreach (var service in _services)
            if (service is T res)
                return res;

        return default;
    }
}
