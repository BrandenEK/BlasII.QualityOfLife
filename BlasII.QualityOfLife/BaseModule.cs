
namespace BlasII.QualityOfLife;

internal abstract class BaseModule
{
    public string Name { get; }
    public int Order { get; }

    public BaseModule()
    {
        string type = GetType().Name;
        int sep = type.IndexOf('_');

        Name = type[..sep];
        Order = int.Parse(type[(sep + 1)..]);
    }

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }
}
