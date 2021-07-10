using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ExportAssetBundle
{
    [MenuItem("Export/Monster Asset Bundles")]
    public static void ExportMonsterBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "monster.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("monsters.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Monster", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/NPC Asset Bundles")]
    public static void ExportNPCBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "npc.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("npcs.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/NPC", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Player Asset Bundles")]
    public static void ExportPlayerBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "player.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("user.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Player", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Combined Object Asset Bundles")]
    public static void ExportCombinedObjectBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "combinedobject.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("combinedobjects.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/CombinedObject", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Particle Asset Bundles")]
    public static void ExportParticleBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "particle.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("particles.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Particle", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Props Asset Bundles")]
    public static void ExportPropsBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "props.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("prop.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Props", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Rocks Asset Bundles")]
    public static void ExportRocksBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "rocks.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("rock.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Rocks", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Terrain Asset Bundles")]
    public static void ExportTerrainBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "terrain.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("terrains.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Terrain", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Vegetation Asset Bundles")]
    public static void ExportVegetationBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "vegetation.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("vegetations.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Vegetation", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/Weapon Asset Bundles")]
    public static void ExportWeaponBundle()
    {
        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
        buildBundles[0].assetBundleName = "weapon.pak";
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle("weapons.assetbundle");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Weapon", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Export/All Asset Bundles")]
    public static void ExportAllBundle()
    {
        //ExportMonsterBundle();
        //ExportNPCBundle();
        //ExportPlayerBundle();
        ExportCombinedObjectBundle();
        ExportParticleBundle();
        ExportPropsBundle();
        ExportRocksBundle();
        ExportTerrainBundle();
        ExportVegetationBundle();
        ExportWeaponBundle();
    }
}
