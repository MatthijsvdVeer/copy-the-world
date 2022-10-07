namespace CopyTheWorld.Shared;

using System.Net;
using Azure;
using Azure.DigitalTwins.Core;

public sealed class DigitalTwinRepository
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public DigitalTwinRepository(DigitalTwinsClient digitalTwinsClient) =>
        this.digitalTwinsClient = digitalTwinsClient;

    public async Task AddTwinAsync(BasicDigitalTwin twin, CancellationToken cancellationToken)
    {
        try
        {
            var response = await this.digitalTwinsClient.CreateOrReplaceDigitalTwinAsync(twin.Id, (object)twin, cancellationToken: cancellationToken);
            if (response.GetRawResponse().Status == (int)HttpStatusCode.OK)
            {
                await Console.Out.WriteLineAsync($"Added {twin.Id}");
                return;
            }

            await Console.Out.WriteLineAsync(
                $"Could not add {twin.Id}. HTTP status: {response.GetRawResponse().Status}");
        }
        catch (RequestFailedException exception)
        {
            await Console.Error.WriteLineAsync($"Could not add twin {twin.Id}");
            await Console.Error.WriteLineAsync(exception.Message);
            throw;
        }
    }

    public async Task AddRelationshipAsync(BasicRelationship relationship, CancellationToken cancellationToken)
    {
        try
        {
            var response = await this.digitalTwinsClient.CreateOrReplaceRelationshipAsync(relationship.SourceId,
                relationship.Id,
                relationship, cancellationToken: cancellationToken);
            if (response.GetRawResponse().Status == (int)HttpStatusCode.OK)
            {
                await Console.Out.WriteLineAsync($"Added {relationship.Id}");
                return;
            }

            await Console.Out.WriteLineAsync($"COULD NOT ADD {relationship.Id} ({response.GetRawResponse().Status})");
        }
        catch (RequestFailedException exception)
        {
            await Console.Error.WriteLineAsync($"Could not add relation {relationship.Id}");
            await Console.Error.WriteLineAsync(exception.Message);
            throw;
        }
    }
}