namespace CopyTheWorld.Provisioning.IotHub;

internal sealed class SimulationConfiguration
{
    public string DeviceId { get; }

    public string ConnectionString { get; }

    public string Type { get; }

    public SimulationConfiguration(string deviceId, string connectionString, string type)
    {
        this.DeviceId = deviceId;
        this.ConnectionString = connectionString;
        this.Type = type;
    }
}