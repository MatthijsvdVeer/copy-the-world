using CopyTheWorld.Provisioning.Populate;
using System.CommandLine;

var returnCode = 0;
var rootCommand = new RootCommand("Provisioning app for Azure Digital Twins");


var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The xlsx file to read the twin data from.");
var twinInstanceOption = new Option<string>("--adtEndpoint", "The URL of the Azure Digital Twins instance");
var populateCommand = new Command("populate", "Upload twin and relationships to Azure Digital Twins")
{
    fileOption,
    twinInstanceOption
};

populateCommand.SetHandler(async context =>
{
    var populateHandler = new PopulateHandler();
    var cancellationToken = context.GetCancellationToken();
    var file = context.ParseResult.GetValueForOption(fileOption);
    var twinInstance = context.ParseResult.GetValueForOption(twinInstanceOption);
    returnCode = await populateHandler.Handle(file, twinInstance, cancellationToken);
});

rootCommand.Add(populateCommand);
await rootCommand.InvokeAsync(args);

return returnCode;