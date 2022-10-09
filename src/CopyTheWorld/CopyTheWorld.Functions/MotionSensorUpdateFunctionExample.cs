namespace CopyTheWorld.Functions;

using Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.DigitalTwins.Core;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Shared.DeviceMessages;
using System.Linq;

public class MotionSensorUpdateFunctionExample
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public MotionSensorUpdateFunctionExample(DigitalTwinsClient digitalTwinsClient) =>
        this.digitalTwinsClient = digitalTwinsClient;

    [FunctionName("MotionSensorUpdateFunctionExample"), Disable]
    public async Task Run(
        [EventHubTrigger("ingress", Connection = "IngressListen", ConsumerGroup = "function")]
        EventData eventData,
        ILogger log)
    {
        #region Receive Message

        var message = JsonSerializer.Deserialize<RoomMotionSensorMessage>(eventData.EventBody);
        var deviceId = eventData.SystemProperties["iothub-connection-device-id"].ToString();

        #endregion

        #region Update The Sensor

        const string sensorQueryTemplate = @"
SELECT sensor.$dtId FROM DIGITALTWINS
WHERE sensor.externalIds.deviceId = {0}";

        var sensorQuery = string.Format(sensorQueryTemplate, deviceId);
        var sensorTwin = this.digitalTwinsClient.Query<BasicDigitalTwin>(sensorQuery).Single();

        var sensorPatchDocument = new JsonPatchDocument();
        sensorPatchDocument.AppendReplace("/lastValue", message.MotionDetected);
        sensorPatchDocument.AppendReplace("/lastValue", message.BatteryLevel);
        _ = await this.digitalTwinsClient.UpdateDigitalTwinAsync(sensorTwin.Id, sensorPatchDocument);

        #endregion

        #region Update The Room

        const string roomQueryTemplate = @"
SELECT room.$dtId FROM DIGITALTWINS.room
JOIN sensor RELATED sensor.observes
WHERE sensor.$dtId = {0}";

        var roomQuery = string.Format(sensorQueryTemplate, sensorTwin.Id);
        var roomTwin = this.digitalTwinsClient.Query<BasicDigitalTwin>(roomQuery).Single();
        var roomPatchDocument = new JsonPatchDocument();
        roomPatchDocument.AppendReplace("/occupancy/isOccupied", message.MotionDetected);
        _ = await this.digitalTwinsClient.UpdateDigitalTwinAsync(roomTwin.Id, roomPatchDocument);

        #endregion
    }
}