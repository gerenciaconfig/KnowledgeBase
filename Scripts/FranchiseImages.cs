using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Franchise Image Database", menuName = "Database/Franchise Image Database", order = 1)]
public class FranchiseImages : SerializedScriptableObject
{
    public Sprite defaultImage;
    [Space(10)]
    public Dictionary<string, Sprite> imageList;
}
