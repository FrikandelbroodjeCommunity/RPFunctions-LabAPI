using FrikanUtils.ProjectMer;
using ProjectMER.Features.Serializable.Schematics;

namespace RPF;

internal static class Assets
{
    public static SchematicObjectDataList FemurBreaker;
    public static SchematicObjectDataList HackDevice;

    public static bool Loaded;

    public static async void Load()
    {
        FemurBreaker = await MerUtilities.LoadSchematicData("FemurBreaker.json");
        HackDevice = await MerUtilities.LoadSchematicData("HackDevice.json");
        Loaded = true;
    }
}