namespace CopyTheWorld.Shared;

using Azure.DigitalTwins.Core;
using System.Reflection;

public static class BasicDigitalTwinExtensions
{
    public static T SetModelOnTwin<T>(this T twin) where T : BasicDigitalTwin
    {
        var attribute = typeof(T).GetCustomAttribute<DtmiAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException($"No {nameof(DtmiAttribute)} was found on type {typeof(T).Name}");
        }

        twin.Metadata.ModelId = attribute.Dtmi;

        return twin;
    }
}