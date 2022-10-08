namespace CopyTheWorld.Provisioning.IotHub.Simulate;

using Microsoft.Azure.Devices.Client;
using Shared.DeviceMessages;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

internal sealed class SimulationHandler
{
    private static readonly Random random = new Random();

    public async Task<int> Handle(string? iotHubEndpoint, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(iotHubEndpoint))
        {
            await Console.Error.WriteLineAsync("The iotHubEndpoint was not supplied.");
            return 1;
        }

        var timers = new List<Timer>();

        try
        {
            var simulationConfigs =
                await JsonSerializer.DeserializeAsync<IEnumerable<SimulationConfiguration>>(File.OpenRead("./simulation.json"),
                    cancellationToken: cancellationToken);
            foreach (var simulationConfig in simulationConfigs)
            {
                var deviceClient =
                    DeviceClient.CreateFromConnectionString(simulationConfig.ConnectionString, TransportType.Http1);
                var timer = new Timer(this.Callback, new DeviceSimulation(simulationConfig, deviceClient, cancellationToken), random.Next(0, 500), 10000);
                timers.Add(timer);
            }
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(exception.Message);
            return 1;
        }

        await Task.Delay(7200000, cancellationToken);

        return 0;
    }

    private async void Callback(object? state)
    {
        try
        {
            var deviceSimulation = state as DeviceSimulation;
            object message;
            switch (deviceSimulation.SimulationConfig.Type)
            {
                case "roomMotionSensor":
                    {
                        message = new RoomMotionSensorMessage
                        {
                            MotionDetected = DateTime.UtcNow.Second % 2 == 0,
                            BatteryLevel = 1 + 99 * random.NextDouble()
                        };
                        break;
                    }
                case "deskMotionSensor":
                    {
                        message = new DeskMotionSensorMessage
                        {
                            Motion = DateTime.UtcNow.Second % 2 == 0,
                            MetaData = new DeskMotionSensorMetaData
                            {
                                BatteryLevel = 1 + random.Next(0, 100)
                            }
                        };
                        break;
                    }
                case "temperatureSensor":
                    {
                        message = new TemperatureSensorMessage { Temperature = 18 + 4 * random.NextDouble() };
                        break;
                    }
                case "co2Sensor":
                    {
                        message = new Co2MotionSensorMessage { Co2 = 500 + random.Next(0, 501) };
                        break;
                    }
                default:
                    throw new InvalidOperationException("The device type is not supported for simulation.");
            }

            var messageAsJson = JsonSerializer.Serialize(message);
            var iotHubMessage = new Message(Encoding.UTF8.GetBytes(messageAsJson));

            await deviceSimulation.DeviceClient.SendEventAsync(iotHubMessage, deviceSimulation.CancellationToken);
            await Console.Out.WriteLineAsync(messageAsJson);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}