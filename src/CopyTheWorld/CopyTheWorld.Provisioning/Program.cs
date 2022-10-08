using CopyTheWorld.Provisioning.IotHub.Populate;
using CopyTheWorld.Provisioning.IotHub.Simulate;
using CopyTheWorld.Provisioning.IotHub.SimulationConfig;
using CopyTheWorld.Provisioning.Mapping.Create;
using CopyTheWorld.Provisioning.Populate;
using System.CommandLine;

var returnCode = 0;


var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The xlsx file to read the twin data from.");
var twinInstanceOption = new Option<string>("--adtEndpoint", "The URL of the Azure Digital Twins instance");
var populateCommand =
    new Command("populate", "Upload twin and relationships to Azure Digital Twins") { fileOption, twinInstanceOption };

populateCommand.SetHandler(async context =>
{
    var populateHandler = new PopulateHandler();
    var cancellationToken = context.GetCancellationToken();
    var file = context.ParseResult.GetValueForOption(fileOption);
    var twinInstance = context.ParseResult.GetValueForOption(twinInstanceOption);
    returnCode = await populateHandler.Handle(file, twinInstance, cancellationToken);
});

var twinCommand = new Command("twins", "Provisioning commands for Azure Digital Twins.") { populateCommand };

var iotHubOption = new Option<string>("--iotHub", "The hostname of the IoT Hub");
var populateDevicesCommand = new Command("populate", "Add devices to IoT Hub") { fileOption, iotHubOption };
populateDevicesCommand.SetHandler(async context =>
{
    var handler = new AddDevicesToIoTHubHandler();
    var cancellationToken = context.GetCancellationToken();
    var file = context.ParseResult.GetValueForOption(fileOption);
    var iotHub = context.ParseResult.GetValueForOption(iotHubOption);
    returnCode = await handler.Handle(file, iotHub, cancellationToken);
});

var configureSimulationCommand =
    new Command("generate-simulation-config", "Creates configuration needed for simulation") { iotHubOption };
configureSimulationCommand.SetHandler(async context =>
{
    var handler = new GenerateSimulationConfigHandler();
    var cancellationToken = context.GetCancellationToken();
    var iotHub = context.ParseResult.GetValueForOption(iotHubOption);
    returnCode = await handler.Handle(iotHub, cancellationToken);
});

var simulation = new Command("simulate", "Simulates devices") { iotHubOption };
simulation.SetHandler(async context =>
{
    var handler = new SimulationHandler();
    var cancellationToken = context.GetCancellationToken();
    var iotHub = context.ParseResult.GetValueForOption(iotHubOption);
    returnCode = await handler.Handle(iotHub, cancellationToken);
});

var devicesCommand =
    new Command("iothub", "Provisioning commands for IoT Hub")
    {
        populateDevicesCommand, configureSimulationCommand, simulation
    };

var storageOption =
    new Option<string>("--storageEndpoint", "The endpoint to the table endpoint of the storage account");
var createMappingCommand =
    new Command("create", "Creates a new mapping in the mapping table") { fileOption, storageOption };
createMappingCommand.SetHandler(async context =>
{
    var handler = new CreateMappingHandler();
    var cancellationToken = context.GetCancellationToken();
    var file = context.ParseResult.GetValueForOption(fileOption);
    var storageEndpoint = context.ParseResult.GetValueForOption(storageOption);
    returnCode = await handler.Handle(file, storageEndpoint, cancellationToken);
});

var mappingCommand = new Command("mapping", "Provisioning commands for the mapping table") { createMappingCommand };

var rootCommand = new RootCommand("Provisioning app for Azure Digital Twins") { devicesCommand, twinCommand, mappingCommand };
await rootCommand.InvokeAsync(args);

return returnCode;