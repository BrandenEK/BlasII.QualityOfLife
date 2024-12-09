
namespace BlasII.QualityOfLife;

internal abstract class BaseModule
{
    public abstract string Name { get; }
    public abstract int Order { get; }

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }
}
