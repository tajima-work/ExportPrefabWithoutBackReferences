using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ExportPrefabWithoutBackReferences : MonoBehaviour
{
    [MenuItem("Tools/Export Prefab Without Back References")]
    public static void ExportSelectedPrefabWithoutBackReferences()
    {
        // 選択したPrefabを取得
        GameObject selectedPrefab = Selection.activeGameObject;
        if (selectedPrefab == null)
        {
            Debug.LogError("Prefabが選択されていません！");
            return;
        }

        // 参照している依存関係を収集
        string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);
        string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);

        // エクスポートするアセットリストを作成（Prefabとその依存関係のみ）
        List<string> assetsToExport = new List<string>();
        foreach (string dependency in dependencies)
        {
            // Prefab自体とその参照のみを追加
            if (dependency != prefabPath && AssetDatabase.GetMainAssetTypeAtPath(dependency) != typeof(GameObject))
            {
                assetsToExport.Add(dependency);
            }
        }
        assetsToExport.Add(prefabPath);

        // エクスポートパッケージを作成
        string packagePath = "Assets/ExportedPrefab.unitypackage";
        AssetDatabase.ExportPackage(assetsToExport.ToArray(), packagePath, ExportPackageOptions.Interactive);
        Debug.Log("Prefabとその依存関係がエクスポートされました（逆参照は含まれていません）：" + packagePath);
    }
}