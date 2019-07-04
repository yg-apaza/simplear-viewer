using PolyToolkit;
using UnityEngine;
using static VuforiaController;

public class PolyUtil
{
    public static void GetPolyResult(string assetId, GetResourceCallback callback)
    {
        PolyApi.GetAsset("assets/" + assetId, (getAssetResult) => GetAssetCallback(getAssetResult, callback));
    }

    private static void GetAssetCallback(PolyStatusOr<PolyAsset> getAssetResult, GetResourceCallback callback)
    {
        if (!getAssetResult.Ok)
        {
            Debug.LogError("<color=red>Failed to get assets. Reason: " + getAssetResult.Status + "</color>");
            return;
        }

        PolyImportOptions options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 5.0f;
        options.recenter = true;

        PolyApi.Import(getAssetResult.Value, options, (asset, importResult) => ImportAssetCallback(asset, importResult, callback));
    }

    private static void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result, GetResourceCallback callback)
    {
        if (!result.Ok)
        {
            Debug.LogError("<color=red> Failed to import asset. :( Reason: " + result.Status + "</color>");
            return;
        }
        // Default transformations
        GameObject polyResult = result.Value.gameObject;
        polyResult.SetActive(false);
        // TODO: Doesn't work for all models, set position, rotation and scale relative to marker size
        polyResult.transform.position = new Vector3(0f, 0f, 0f);
        polyResult.transform.rotation = Quaternion.Euler(0, 0, 0);
        polyResult.transform.localScale = new Vector3(500f, 500f, 500f);
        Debug.Log(">> POLY RESOURCE CREATED > " + asset.name);
        callback(polyResult);
    }
}