using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

[ShowOdinSerializedPropertiesInInspector]
[System.Serializable]
public abstract class SavedClass
{
    public abstract void UpdateClass();
}
