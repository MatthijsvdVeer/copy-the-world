﻿namespace CopyTheWorld.Shared.TwinModels.Components;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:rec_3_3:SpaceCO2;1")]
public sealed class SpaceCo2 : BasicDigitalTwinComponent
{
    public double CO2Sensor { get; set; }
}