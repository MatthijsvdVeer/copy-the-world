namespace CopyTheWorld.Shared;

using System.Net;
using Azure;
using Azure.DigitalTwins.Core;

public sealed class DigitalTwinRepository
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public DigitalTwinRepository(DigitalTwinsClient digitalTwinsClient) =>
        this.digitalTwinsClient = digitalTwinsClient;

    public async Task AddTwinAsync(BasicDigitalTwin twin)
    {
        try
        {
            var response = await this.digitalTwinsClient.CreateOrReplaceDigitalTwinAsync(twin.Id, (object)twin);
            if (response.GetRawResponse().Status == (int)HttpStatusCode.OK)
            {
                await Console.Out.WriteLineAsync($"Added {twin.Id}");
                return;
            }

            await Console.Out.WriteLineAsync($"COULD NOT ADD {twin.Id} ({response.GetRawResponse().Status})");
        }
        catch (RequestFailedException exception)
        {
            await Console.Error.WriteLineAsync(exception.Message);
            throw;
        }
    }

    public async Task AddRelationshipAsync(BasicRelationship relationship)
    {
        try
        {
            var response = await this.digitalTwinsClient.CreateOrReplaceRelationshipAsync(relationship.SourceId, relationship.Id,
                relationship);
            if (response.GetRawResponse().Status == (int)HttpStatusCode.OK)
            {
                await Console.Out.WriteLineAsync($"Added {relationship.Id}");
                return;
            }

            await Console.Out.WriteLineAsync($"COULD NOT ADD {relationship.Id} ({response.GetRawResponse().Status})");
        }
        catch (RequestFailedException exception)
        {
            await Console.Error.WriteLineAsync(exception.Message);
            throw;
        }
    }
}