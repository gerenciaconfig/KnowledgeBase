using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MemoryGame : Game {

    #if UNITY_EDITOR
    [MenuItem("Tools/Fungus/Create/MemoryGame")]
    public static void Create()
    {
        GameObject gObject = Instantiate(Resources.Load<GameObject>("Prefabs/MemoryGame"));
        gObject.name = gObject.name.Replace("(Clone)", "");
    }
    #endif

}
