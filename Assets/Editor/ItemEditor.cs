using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemData item = (ItemData)target;
        
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("itemSprite"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Shape", EditorStyles.boldLabel);

        SerializedProperty shapeProp = serializedObject.FindProperty("shape");

        float buttonSize = 30;
        EditorGUILayout.BeginVertical();

        for (int row = 0; row < 3; row++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int col = 0; col < 3; col++)
            {
                int index = row * 3 + col;
                SerializedProperty cellProp = shapeProp.GetArrayElementAtIndex(index);

                Color originalColor = GUI.backgroundColor;
                GUI.backgroundColor = cellProp.boolValue ? Color.green : Color.gray;

                if (GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    cellProp.boolValue = !cellProp.boolValue;
                }

                GUI.backgroundColor = originalColor;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}