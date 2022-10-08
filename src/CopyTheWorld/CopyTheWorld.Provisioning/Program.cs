using CopyTheWorld.Provisioning.IotHub.Populate;
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

var iotHubHostName = new Option<string>("--iotHub", "The hostname of the IoT Hub");
var populateDevicesCommand = new Command("populate", "Add devices to IoT Hub"){ fileOption, iotHubHostName};
populateDevicesCommand.SetHandler(async context =>
{
    var handler = new AddDevicesToIoTHubHandler();
    var cancellationToken = context.GetCancellationToken();
    var file = context.ParseResult.GetValueForOption(fileOption);
    var iotHub = context.ParseResult.GetValueForOption(iotHubHostName);
    returnCode = await handler.Handle(file, iotHub, cancellationToken);

});

var devicesCommand = new Command("iothub", "Provisioning commands for IoT Hub") { populateDevicesCommand };

var rootCommand = new RootCommand("Provisioning app for Azure Digital Twins") { devicesCommand, twinCommand };
await rootCommand.InvokeAsync(args);

return returnCode;