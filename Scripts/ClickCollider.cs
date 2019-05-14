using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arcolabs.Home
{
    public class ClickCollider : MonoBehaviour
    {
        private Vector3 screenPoint;
        private Vector3 curPosition;
        private Camera cam;
        public GameObject ObjPrefab;
        private GameObject instantPrefab;
        public GameObject pivotSpawn;

        public float securityMargin;

        public bool sortSize = false;

        public float minMultiplier;
        public float maxMultiplier;

        public bool selfAnimate = false;

        public void Awake()
        {
            if(sortSize)
            {
                transform.localScale = new Vector3(transform.localScale.x * Random.Range(minMultiplier, maxMultiplier), transform.localScale.y, transform.localScale.z);
            }
        }

        private void Start()
        {
            cam = Camera.main;
        }
        void OnMouseDown()
        {
            if (WorldManager.curInstances <= WorldManager.maxInstances)
            {
                InstanteObj();
            }
        }

        void InstanteObj()
        {
            if (pivotSpawn!=null)
            {
                instantPrefab = Instantiate(ObjPrefab, new Vector3(pivotSpawn.transform.position.x, pivotSpawn.transform.position.y, pivotSpawn.transform.position.z), Quaternion.identity);
                WorldManager.curInstances++;
                instantPrefab.GetComponent<Animator>().enabled = true;
            }
            else
            {
                if (selfAnimate)
                {                   
                    this.GetComponent<Animator>().SetTrigger("OnClick");
                }
                else
                {
                    screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);
                    curPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

                    instantPrefab = Instantiate(ObjPrefab, new Vector3(curPosition.x, curPosition.y, curPosition.z), Quaternion.identity);
                    WorldManager.curInstances++;
                    instantPrefab.GetComponent<Animator>().enabled = true;
                }
            }
        }
    }
}
