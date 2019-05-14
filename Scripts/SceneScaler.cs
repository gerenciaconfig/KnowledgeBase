using UnityEngine;

[ExecuteInEditMode]
public class SceneScaler : MonoBehaviour {
    [SerializeField]
    private int baseWidth = 1080;
    [SerializeField]
    private int baseHeight = 1920;

    private float baseRatio; //2048 x 2734
    private float currScrRatio;



    private int width = 0;
    private int height = 0;

    [HideInInspector]
    [SerializeField]
    private bool useBaseScaleOld = false;

    void Start()
    {
        BaseScaleBkg();
    }

    void Update()
    {
        if (width != Screen.width || height != Screen.height)
        {
            BaseScaleBkg();
        }
    }

    void BaseScaleBkg()
    {
        baseRatio = (float)baseWidth / baseHeight;
        width = Screen.width;
        height = Screen.height;
        currScrRatio = (height!=0) ? (float)width / height : 1;
        float k = 1f;
        if (baseRatio > currScrRatio)
        {
            k = currScrRatio /baseRatio ;
        }
        gameObject.transform.localScale = new Vector3(k, k, k);
        Debug.Log("new local scale " + k);
    }
}
