using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForthTransform : MonoBehaviour
{
    [SerializeField]
    private float xRange = 5f;
    [SerializeField]
    private Transform[] transforms;
    private float[] originalPositions;
    private float[] targetPositions;
    private float[] randomOffset;
    private float stateTime = 0f;
    private float offset;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        stateTime = 0f;
        count = transforms.Length;
        originalPositions = new float[count];
        randomOffset = new float[count];
        targetPositions = new float[count];
        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = transforms[i].localPosition.x;
            randomOffset[i] = Random.value * 6 - 3f;
            targetPositions[i] = originalPositions[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            float dst = targetPositions[i] - originalPositions[i];
            if (Mathf.Abs(dst) > 5f)
            {
                randomOffset[i] = -Mathf.Sign(dst) * Time.deltaTime * .1f;
                targetPositions[i] = originalPositions[i] + xRange * Mathf.Sign(dst);
            }
            targetPositions[i] += randomOffset[i] + Random.value * Time.deltaTime * Mathf.Sign(randomOffset[i]) * 2;
            Vector3 pos = transforms[i].localPosition;
            pos.x = Mathf.Lerp(pos.x, targetPositions[i], Random.Range(.02f, .1f));
            transforms[i].localPosition = pos;
        }
    }
}
