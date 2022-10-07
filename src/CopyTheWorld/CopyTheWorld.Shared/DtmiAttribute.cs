namespace CopyTheWorld.Shared;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class DtmiAttribute : Attribute
{
    public string Dtmi { get; }

    public DtmiAttribute(string dtmi)
    {
        Dtmi = dtmi;
    }
}