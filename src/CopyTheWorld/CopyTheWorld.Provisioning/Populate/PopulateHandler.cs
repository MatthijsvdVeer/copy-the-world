namespace CopyTheWorld.Provisioning.Populate
{
    using Azure.Identity;
    using Shared;
    using System.Text.Json;
    using Azure.Core.Serialization;
    using Azure.DigitalTwins.Core;
    using System.Data;

    internal sealed class PopulateHandler
    {
        public async Task<int> Handle(FileInfo? file, string? adtEndpoint, CancellationToken cancellationToken)
        {
            if (file is {Exists: false})
            {
                await Console.Error.WriteLineAsync($"{file.FullName} does not exist.");
                return 1;
            }

            if (string.IsNullOrEmpty(adtEndpoint))
            {
                await Console.Error.WriteLineAsync("The adtEndpoint was not supplied.");
                return 1;
            }

            try
            {
                var dataSet = new DataSourceReader().GetDataSet(file!.FullName);
                foreach (DataTable table in dataSet.Tables)
                {
                    await Console.Out.WriteLineAsync($"{table.TableName}: {table.Rows.Count} items.");
                }

                var twinFactory = new TwinFactory();
                var twins = new List<BasicDigitalTwin>();
                var relationships = new List<BasicRelationship>();
                foreach (DataTable table in dataSet.Tables)
                {
                    var result = twinFactory.CreateTwinsAndRelationships(table);
                    twins.AddRange(result.Twins);
                    relationships.AddRange(result.Relationships);
                }

                var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var jsonObjectSerializer = new JsonObjectSerializer(jsonSerializerOptions);
                foreach (var twin in twins)
                {
                    Console.WriteLine(JsonSerializer.Serialize((object)twin, jsonSerializerOptions));
                }

                foreach (var relationship in relationships)
                {
                    Console.WriteLine(JsonSerializer.Serialize(relationship));
                }

                var digitalTwinsClient = new DigitalTwinsClient(new Uri(adtEndpoint), new DefaultAzureCredential(),
                    new DigitalTwinsClientOptions {Serializer = jsonObjectSerializer});
                var digitalTwinRepository = new DigitalTwinRepository(digitalTwinsClient);

                foreach (var twin in twins)
                {
                    await digitalTwinRepository.AddTwinAsync(twin, cancellationToken);
                }

                foreach (var relationship in relationships)
                {
                    await digitalTwinRepository.AddRelationshipAsync(relationship, cancellationToken);
                }
            }
            catch (Exception exception)
            {
                await Console.Error.WriteLineAsync(exception.Message);
                return 1;
            }

            return 0;
        }
    }
}