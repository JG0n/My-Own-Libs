/// <summary>
/// A registered service with a lifetime
/// </summary>
public class Service
{
    public string Class { get; init; }
    public Lifetime Lifetime { get; init; }
    public object? Instance { get; set; }


    /// <summary>
    /// Initialize an instance of the class
    /// </summary>
    /// <param name="objectType">Type of registered service</param>
    /// <param name="lifetime">The life time of the service.
    /// <see cref="Lifetime.Scope">, <see cref="Lifetime.Transient"> and <see cref="Lifetime.Singleton">
    /// </param>
    public Service(Type objectType, Lifetime lifetime)
    {
        Class = objectType.FullName ?? throw new ArgumentNullException();
        Lifetime = lifetime;
    }
}
