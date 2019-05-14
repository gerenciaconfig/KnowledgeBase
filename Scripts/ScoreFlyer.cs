using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class ScoreFlyer : MonoBehaviour
    {
        public Text scoreText;
        public RectTransform rTransform;

        public void StartFly(string score, Vector3 wPos)
        {
            scoreText.text = score;
            Canvas c = GameObject.Find("CanvasMain").GetComponent<Canvas>();
            rTransform.SetParent(c.transform);
            rTransform.anchoredPosition = Coordinats.WorldToCanvasCenterCenter(wPos, c);
            Vector2 pos = rTransform.anchoredPosition;
            float dist = Random.Range(300, 500);
            float time = Random.Range(1.2f, 2.2f);
            SimpleTween.Value(gameObject, 0f, dist, time).SetOnUpdate((float val) =>
            {
                Vector2 npos = pos + new Vector2(0, val);
               if(this) rTransform.anchoredPosition = npos;

            }).SetEase(EaseAnim.EaseOutCubic).
            SetDelay(Random.Range(0.0f, 0.1f)).
            AddCompleteCallBack(() =>
            {
                if(this)Destroy(gameObject);
            });
        }

        private void OnDestroy()
        {
            SimpleTween.Cancel(gameObject, false);
        }
    }
}
