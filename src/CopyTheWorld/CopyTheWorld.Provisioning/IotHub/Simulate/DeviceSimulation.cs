namespace CopyTheWorld.Provisioning.IotHub.Simulate;

using Microsoft.Azure.Devices.Client;

internal class DeviceSimulation
{
    public SimulationConfiguration SimulationConfig { get; }

    public DeviceClient DeviceClient { get; }

    public CancellationToken CancellationToken { get; }

    public DeviceSimulation(SimulationConfiguration simulationConfig, DeviceClient deviceClient, CancellationToken cancellationToken)
    {
        this.SimulationConfig = simulationConfig;
        this.DeviceClient = deviceClient;
        this.CancellationToken = cancellationToken;
    }
}