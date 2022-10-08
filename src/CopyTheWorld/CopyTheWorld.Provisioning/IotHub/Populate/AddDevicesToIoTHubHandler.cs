namespace CopyTheWorld.Provisioning.IotHub.Populate;

using Azure.Identity;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class AddDevicesToIoTHubHandler
{
    public async Task<int> Handle(FileInfo? file, string? iotHubEndpoint, CancellationToken cancellationToken)
    {
        if (file is { Exists: false })
        {
            await Console.Error.WriteLineAsync($"{file.FullName} does not exist.");
            return 1;
        }

        if (string.IsNullOrEmpty(iotHubEndpoint))
        {
            await Console.Error.WriteLineAsync("The adtEndpoint was not supplied.");
            return 1;
        }

        try
        {
            var dataSet = new DataSourceReader().GetDataSet(file!.FullName);
            var registryManager = RegistryManager.Create(iotHubEndpoint, new DefaultAzureCredential());

            await AddDevicesForTable(dataSet, "RoomMotionSensors", "roomMotionSensor", registryManager,
                cancellationToken);
            await AddDevicesForTable(dataSet, "DeskMotionSensors", "deskMotionSensor", registryManager,
                cancellationToken);
            await AddDevicesForTable(dataSet, "TemperatureSensors", "temperatureSensor", registryManager,
                cancellationToken);
            await AddDevicesForTable(dataSet, "Co2Sensors", "co2Sensor", registryManager,
                cancellationToken);
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(exception.Message);
            return 1;
        }

        return 0;
    }

    private static async Task AddDevicesForTable(DataSet dataSet, string tableName, string deviceType,
        RegistryManager registryManager, CancellationToken cancellationToken)
    {
        var table = dataSet.Tables[tableName];
        var deviceIds = table.Rows.Cast<DataRow>().Select(row => row.GetStringValue("ID"));
        foreach (var deviceId in deviceIds)
        {
            var twin = new Twin(deviceId) { Tags = { ["type"] = deviceType } };
            var device = await registryManager.GetDeviceAsync(deviceId, cancellationToken);
            if (device != null)
            {
                await Console.Out.WriteLineAsync($"Device {deviceId} from table {tableName} exists, skipping...");
                continue;
            }

            var result =
                await registryManager.AddDeviceWithTwinAsync(new Device(deviceId), twin, cancellationToken);
            if (result.IsSuccessful)
            {
                await Console.Out.WriteLineAsync($"Added {deviceId} from table {tableName}");
                continue;
            }

            await Console.Error.WriteLineAsync($"Could not add device {deviceId} from table {tableName}");
            throw new InvalidOperationException();
        }
    }
}