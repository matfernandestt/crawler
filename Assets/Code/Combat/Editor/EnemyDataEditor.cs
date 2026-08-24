using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var parent = target as EnemyData;
        var texture = AssetPreview.GetAssetPreview(parent.icon);
        GUILayout.Label(texture);
        
        base.OnInspectorGUI();
    }
}
