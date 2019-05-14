namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TileProperties : MonoBehaviour
    {
        public GameObject pivotCenter;

        public GameObject actIconPrefab;

        public float posXRange;
        public float posYRange;


		public List<GameObject> configurations;

        public Vector3 GetPivotPos()
        {
            return pivotCenter.transform.position;
        }

        public void SortTileConfiguration()
        {
            if(configurations.Count > 0)
            {
                //Desativa todas as configurações
                for (int i = 0; i < configurations.Count; i++)
                {
                    configurations[i].SetActive(false);
                }

                //Ativa uma configuração aleatória
                int aux = Random.Range(0, configurations.Count);
                //configurations[aux].SetActive(true);


                // TODO MUDAR ESSA MERDA
                configurations[0].SetActive(true);
            }
        }

		public void PopulateTile(List<Activity> activities)
        {
			for (int i = 0; i < activities.Count; i++)
            {
                GameObject instatiatedIcon = Instantiate(actIconPrefab);

                Vector3 iconPos = new Vector3(pivotCenter.transform.position.x, pivotCenter.transform.position.y, pivotCenter.transform.position.z);

                ActivityButton activityButton = instatiatedIcon.GetComponent<ActivityButton>();

                activityButton.SetActivity(activities[i]);
                instatiatedIcon.transform.SetParent(pivotCenter.transform);

                iconPos.x = iconPos.x - posXRange + (posXRange * (i + 1)) / activities.Count;
                instatiatedIcon.transform.position = iconPos;
            }
        }
    }
}