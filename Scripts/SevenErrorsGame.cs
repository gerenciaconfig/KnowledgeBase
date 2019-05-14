using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SevenErrorsGame : Game {

    #if UNITY_EDITOR
    [MenuItem("Tools/Fungus/Create/SevenErrorsGame")]
    public static void Create()
    {
        GameObject gObject = Instantiate(Resources.Load<GameObject>("Prefabs/SevenErrorsGame"));
        gObject.name = gObject.name.Replace("(Clone)", "");
    }
    #endif

}
