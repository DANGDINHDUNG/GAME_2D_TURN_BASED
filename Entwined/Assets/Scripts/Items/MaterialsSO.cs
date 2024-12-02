using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Materials/Item")]
public class MaterialsSO : ScriptableObject
{
    public string materialName;
    public MaterialType materialType;
    public Rarity materialRarity;
    public int expValue;
    public Sprite materialIcon;
    public Class materialClass = Class.NoType;

}
public enum MaterialType
{
    LevelExp,
    CrystalExp,
    FragmentExp,
    Breakthrough,
}

#if UNITY_EDITOR
[CustomEditor(typeof(MaterialsSO)), CanEditMultipleObjects]
public class ItemDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MaterialsSO materialData = (MaterialsSO)target;

        // Display always
        materialData.materialName = EditorGUILayout.TextField("Display Name", materialData.materialName);

        materialData.materialIcon = EditorGUILayout.ObjectField(materialData.materialIcon, typeof(Sprite), true, GUILayout.Height(48), GUILayout.Width(48)) as Sprite;

        // Display dropdown
        materialData.materialType = (MaterialType)EditorGUILayout.EnumPopup("Type", materialData.materialType);
        materialData.materialRarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", materialData.materialRarity);

        // Display conditional for two
        if (materialData.materialType == MaterialType.Breakthrough)
        {
            materialData.materialClass = (Class)EditorGUILayout.EnumPopup("Class", materialData.materialClass);
        }
        else
        {
            materialData.expValue = EditorGUILayout.IntField("Exp Value", materialData.expValue);
        }

        // Apply changes and mark the object as dirty if it’s modified
        if (GUI.changed)
        {
            EditorUtility.SetDirty(materialData); // Marks the asset as modified
            AssetDatabase.SaveAssets();           // Saves all modified assets
        }
    }

}
#endif