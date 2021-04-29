using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyLookAt))]
[CanEditMultipleObjects]
public class EnemyLookAtEditor : Editor
{
    public void OnSceneGUI()
    {
        var lookAt = (target as EnemyLookAt);

        var handlePos = lookAt.transform.position;

        UnityEditor.Handles.color = new Color(1, 0, 0, 0.2f);
        UnityEditor.Handles.DrawSolidArc(
            handlePos,
            Vector3.back,
            Vector3.left,
            lookAt.m_lookatRotationLimit,
            UnityEditor.HandleUtility.GetHandleSize(handlePos));

        UnityEditor.Handles.DrawSolidArc(
            handlePos,
            Vector3.back,
            Vector3.left,
            -lookAt.m_lookatRotationLimit,
            UnityEditor.HandleUtility.GetHandleSize(handlePos));
    }
}
