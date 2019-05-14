using UnityEngine;
using System.Collections;


namespace Mkey
{
    //particle system callback
    public class AnimCallBack : MonoBehaviour
    {
        private System.Action cBack;

        private float kCallBackTime = 0.1f;

        public void EndCallBack()
        {
            if (cBack != null) cBack();
        }

        public void SetEndCallBack(System.Action cBack)
        {
            this.cBack = cBack;
        }

        private ParticleSystem ps;
        private void Start()
        {
            ps = GetComponent<ParticleSystem>();
            if (ps) StartCoroutine(CheckCallBack());
        }

        IEnumerator CheckCallBack()
        {
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax * kCallBackTime);
            EndCallBack();
        }
    }
}