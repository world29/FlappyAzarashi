using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInputAction", menuName = "ScriptableObjects/InputAction")]
public class InputActionScriptableObject : ScriptableObject
{
    public string ActionName; // アクション定義名
}
