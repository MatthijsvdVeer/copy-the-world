namespace CopyTheWorld.Shared.TwinModels;

[Dtmi("dtmi:digitaltwins:rec_3_3:core:Level;1")]
public sealed class Level : Space
{
    public int LevelNumber { get; set; }
}