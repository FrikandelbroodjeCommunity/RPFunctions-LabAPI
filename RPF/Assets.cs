using FrikanUtils.ProjectMer;
using ProjectMER.Features.Serializable.Schematics;

namespace RPF;

internal static class Assets
{
    public static SchematicObjectDataList FemurBreaker;

    public static bool Loaded;

    public static async void Load()
    {
        FemurBreaker = await MerUtilities.LoadSchematicData("FemurBreaker.json");
        Loaded = true;
    }
}