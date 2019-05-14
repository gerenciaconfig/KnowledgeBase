using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Perfil Image Database", menuName = "Database/Perfil Image Database", order = 1)]
public class PerfilImageScriptable : SerializedScriptableObject
{
    [Space(10)]
    public Dictionary<int, Sprite> perfilImageList;
}
