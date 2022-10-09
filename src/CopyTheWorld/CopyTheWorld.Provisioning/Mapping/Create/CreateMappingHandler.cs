namespace CopyTheWorld.Provisioning.Mapping.Create;

using Azure.Data.Tables;
using Azure.Identity;
using Shared;
using System.Data;
using System.Text.Json;

internal class CreateMappingHandler
{
    public async Task<int> Handle(FileInfo? file, string? storageEndpoint, CancellationToken cancellationToken)
    {
        if (file is { Exists: false })
        {
            await Console.Error.WriteLineAsync($"{file.FullName} does not exist.");
            return 1;
        }

        if (string.IsNullOrEmpty(storageEndpoint))
        {
            await Console.Error.WriteLineAsync("The storageEndpoint was not supplied.");
            return 1;
        }

        var dataSet = new DataSourceReader().GetDataSet(file!.FullName);
        var definitions = new List<MappingEntity>();
        definitions.AddRange(GetMappingEntities(dataSet, "RoomMotionSensors"));
        definitions.AddRange(GetMappingEntities(dataSet, "TemperatureSensors"));
        definitions.AddRange(GetMappingEntities(dataSet, "Co2Sensors"));
        definitions.AddRange(GetMappingEntities(dataSet, "DeskMotionSensors"));

        var tableClient = new TableClient(
            new Uri(storageEndpoint),
            "mapping",
            new DefaultAzureCredential());

        foreach (var mappingEntity in definitions)
        {
            _ = await tableClient.UpsertEntityAsync(mappingEntity,TableUpdateMode.Replace, cancellationToken);
            await Console.Out.WriteLineAsync($"Wrote {mappingEntity.RowKey} to mapping table.");
        }

        return 0;
    }

    private static IEnumerable<MappingEntity> GetMappingEntities(DataSet dataSet, string tableName)
    {
        var entities = new List<MappingEntity>();
        var table = dataSet.Tables[tableName];
        foreach (DataRow tableRow in table.Rows)
        {
            var id = tableRow.GetStringValue("ID");
            var definitions = GetMappingDefinitions(id, tableName);
            var mappingEntity = new MappingEntity { PartitionKey = id, RowKey = id, Mapping = JsonSerializer.Serialize(definitions) };
            entities.Add(mappingEntity);
        }

        return entities;
    }

    private static MappingDefinition[] GetMappingDefinitions(string id, string tableName)
    {
        switch (tableName)
        {
            case "RoomMotionSensors":
                {
                    return new MappingDefinition[]
                    {
                        new()
                        {
                            DataType = "bool", Property = "MotionDetected", TwinId = id, TwinProperty = "/lastValue"
                        },
                        new()
                        {
                            DataType = "double", Property = "BatteryLevel", TwinId = id, TwinProperty = "/batteryLevel"
                        }
                    };
                }
            case "DeskMotionSensors":
                {
                    return new MappingDefinition[]
                    {
                        new()
                        {
                            DataType = "bool", Property = "Motion", TwinId = id, TwinProperty = "/lastValue"
                        },
                        new()
                        {
                            DataType = "double", Property = "Metadata/BatteryLevel", TwinId = id, TwinProperty = "/batteryLevel"
                        }
                    };
                }
            case "TemperatureSensors":
                {
                    return new MappingDefinition[]
                    {
                        new()
                        {
                            DataType = "double", Property = "Temperature", TwinId = id, TwinProperty = "/lastValue"
                        }
                    };
                }
            case "Co2Sensors":
                {
                    return new MappingDefinition[]
                    {
                        new()
                        {
                            DataType = "int", Property = "Co2", TwinId = id, TwinProperty = "/lastValue"
                        }
                    };
                }
            default:
                throw new InvalidOperationException("This table name was not recognized");
        }
        
    }
}