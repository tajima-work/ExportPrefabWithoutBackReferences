using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ExportPrefabWithoutBackReferences : MonoBehaviour
{
    [MenuItem("Tools/Export Prefab Without Back References")]
    public static void ExportSelectedPrefabWithoutBackReferences()
    {
        // �I������Prefab���擾
        GameObject selectedPrefab = Selection.activeGameObject;
        if (selectedPrefab == null)
        {
            Debug.LogError("Prefab���I������Ă��܂���I");
            return;
        }

        // �Q�Ƃ��Ă���ˑ��֌W�����W
        string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);
        string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);

        // �G�N�X�|�[�g����A�Z�b�g���X�g���쐬�iPrefab�Ƃ��̈ˑ��֌W�̂݁j
        List<string> assetsToExport = new List<string>();
        foreach (string dependency in dependencies)
        {
            // Prefab���̂Ƃ��̎Q�Ƃ݂̂�ǉ�
            if (dependency != prefabPath && AssetDatabase.GetMainAssetTypeAtPath(dependency) != typeof(GameObject))
            {
                assetsToExport.Add(dependency);
            }
        }
        assetsToExport.Add(prefabPath);

        // �G�N�X�|�[�g�p�b�P�[�W���쐬
        string packagePath = "Assets/ExportedPrefab.unitypackage";
        AssetDatabase.ExportPackage(assetsToExport.ToArray(), packagePath, ExportPackageOptions.Interactive);
        Debug.Log("Prefab�Ƃ��̈ˑ��֌W���G�N�X�|�[�g����܂����i�t�Q�Ƃ͊܂܂�Ă��܂���j�F" + packagePath);
    }
}