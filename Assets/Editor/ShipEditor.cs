using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShipData ship = (ShipData)target;
        
        EditorGUILayout.LabelField("Layout", EditorStyles.boldLabel);

        SerializedProperty layoutProp = serializedObject.FindProperty("layout");

        float buttonSize = 15;
        EditorGUILayout.BeginVertical();

        for (int row = 0; row < 3; row++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int col = 0; col < 12; col++)
            {
                int index = row * 3 + col;
                SerializedProperty cellProp = layoutProp.GetArrayElementAtIndex(index);

                Color originalColor = GUI.backgroundColor;

                switch (cellProp.intValue)
                {
                    case (0):   // Any
                        GUI.backgroundColor = Color.gray;
                        return;
                    case (1):   // Thruster
                        GUI.backgroundColor = Color.blue;
                        return;
                    case (2):   // Weapon
                        GUI.backgroundColor = Color.yellow;
                        return;
                    case (3):   // Invalid
                        GUI.backgroundColor = Color.red;
                        return;
                    default: 
                        GUI.backgroundColor = Color.gray;
                        return;
                }

                if (GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    cellProp.intValue++;
                }
            }
        }
        
    }
}
