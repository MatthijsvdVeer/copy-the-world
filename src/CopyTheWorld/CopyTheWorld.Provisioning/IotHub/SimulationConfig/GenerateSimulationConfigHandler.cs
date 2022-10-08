namespace CopyTheWorld.Provisioning.IotHub.SimulationConfig;

using Azure.Identity;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

internal sealed class GenerateSimulationConfigHandler
{

    public async Task<int> Handle(string? iotHubEndpoint, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(iotHubEndpoint))
        {
            await Console.Error.WriteLineAsync("The iotHubEndpoint was not supplied.");
            return 1;
        }

        try
        {
            var configs = new List<SimulationConfiguration>();
            var registryManager = RegistryManager.Create(iotHubEndpoint, new DefaultAzureCredential());

            var query = registryManager.CreateQuery("SELECT * FROM devices");
            while (query.HasMoreResults)
            {
                var twins = await query.GetNextAsTwinAsync();
                foreach (var twin in twins)
                {
                    var device = await registryManager.GetDeviceAsync(twin.DeviceId, cancellationToken);
                    var connectionString = $"HostName={iotHubEndpoint};DeviceId={twin.DeviceId};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
                    var twinTag = twin.Tags["type"];
                    var simulationConfiguration = new SimulationConfiguration(twin.DeviceId, connectionString, twinTag.ToString());
                    configs.Add(simulationConfiguration);
                    await Console.Out.WriteLineAsync($"Found config for {twin.DeviceId}");
                }

            }

            var configJson = JsonSerializer.Serialize(configs);
            await File.WriteAllTextAsync("./simulation.json", configJson, Encoding.UTF8, cancellationToken);
            await Console.Out.WriteLineAsync($"Wrote config to ./simulation.json");
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(exception.Message);
            return 1;
        }

        return 0;
    }
}