using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
[CanEditMultipleObjects]
public class EnemyEditor : Editor
{
    public void OnSceneGUI()
    {
        var enemy = (target as Enemy);
    
        var handlePos = enemy.transform.position;

        UnityEditor.Handles.color = new Color(1, 0, 0, 0.2f);
        UnityEditor.Handles.DrawSolidArc(
            handlePos,
            Vector3.back,
            Vector3.left,
            enemy.m_lookatRotationLimit,
            UnityEditor.HandleUtility.GetHandleSize(handlePos));

        UnityEditor.Handles.DrawSolidArc(
            handlePos,
            Vector3.back,
            Vector3.left,
            -enemy.m_lookatRotationLimit,
            UnityEditor.HandleUtility.GetHandleSize(handlePos));
    }
}
