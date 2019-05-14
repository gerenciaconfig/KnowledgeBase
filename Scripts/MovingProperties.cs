using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProperties : MonoBehaviour
{
    public IEnumerator MoveAndRotateTo(float seconds, Vector3 targetPos, Quaternion targetQuaternion)
    {
        float elapsedTime = 0;
        Vector3 originalPos = transform.position;
        Quaternion originalQuaternion = transform.rotation;

        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / seconds));
            transform.rotation = Quaternion.Slerp(originalQuaternion, targetQuaternion, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = targetQuaternion;
        transform.position = targetPos;
    }

    //Sets the object position
    public void SetPosAndRotation(Vector3 pos, Quaternion quaternion)
    {
        transform.position = pos;
        transform.rotation = quaternion;
    }
}
