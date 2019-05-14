namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WorldManager : MonoBehaviour
    {
        public ActivitiesDatabase activitiesDatabase;

        public static int maxInstances = 2;
        public static int curInstances;

        public GameObject tileStart;
        public GameObject tileMidle;
        public GameObject tileEnd;

        public GameObject worldLights;
        private Camera cam;

        public int iconsPerTile;

        public float tilesDistance;

        public int iconsStart;
        public int iconsEnd;

		public SpriteRenderer groundSprite;
		public float groundTileMultiplier = 1.3f;

		public float endTileRatio = 0.8f;

		public SpriteRenderer skySprite;
		public float skyTileMultiplier = 40f;

        public GameObject actIconPrefab;

        public GameObject pivotCenter;

        public float posXRange;
        public float posYRange;

        public List<GameObject> interactbleObjects;

        public float intObjMarginX;
        public float intObjMarginY;

        public bool skipFirstInteractble = false;

        public bool onlySpawnAbove;

        public int skipIcons;

        private Vector3 camMaxPos;
        public Vector3 fixedCamLimit;

        public string worldArea;

        public void Awake()
        {
            PopulateWordNew(ActivitiesDatabase.GetActivitiesFromArea(ActivitiesDatabase.serverActivities, worldArea));
            cam = Camera.main;
        }

        public void OnEnable()
        {
            if (fixedCamLimit != null)
            {
                Camera.main.GetComponent<NewCameraMovement>().SetMaxPos(camMaxPos - fixedCamLimit);
            }
            else
            {
                Camera.main.GetComponent<NewCameraMovement>().SetMaxPos(camMaxPos);
            }
            
            //Passa para a camera a luzes daquele mundo
            GameObject instantiatedLights;
            instantiatedLights = Instantiate(worldLights);
            instantiatedLights.transform.SetParent(cam.GetComponent<CamLightsHelper>().camWorldLights.transform);
            instantiatedLights.SetActive(true);

        }

        public void PopulateWordNew(List<Activity> activities)
        {
            //Calcula a qtd de tiles de meio necessários
            int midTilesCount = (activities.Count - iconsStart - iconsEnd) / iconsPerTile;

            Vector3 initialPosIcon;
            Vector3 finalPosIcon;

            GameObject instantiatedTile;
            Vector3 tilePos = Vector3.zero;

            Vector3 intObjPos = Vector3.zero;

            //Instanciando o Tile inicial
            instantiatedTile = Instantiate(tileStart, tilePos, Quaternion.identity, this.transform);

            //Qguarda a posição onde deve ser instanciado o primeiro icone
            initialPosIcon = instantiatedTile.GetComponent<TileProperties>().GetPivotPos();            

            //Instancia a qtd de mid tiles calculada anteriormente
            for (int i = 0; i < midTilesCount; i++)
            {
                tilePos.x += tilesDistance;
                instantiatedTile = Instantiate(tileMidle, tilePos, Quaternion.identity, this.transform);

                instantiatedTile.GetComponent<TileProperties>().SortTileConfiguration();
            }

            //Instancia o Tile final
            tilePos.x += tilesDistance;
            instantiatedTile = Instantiate(tileEnd, tilePos, Quaternion.identity, this.transform);

            //Guarda a posiçao onde deve ser instanciado o ultimo icone
            finalPosIcon = instantiatedTile.GetComponent<TileProperties>().GetPivotPos();

            //Atualiza a posição máxima da câmera
            tilePos.x += tilesDistance * (endTileRatio);
            camMaxPos = tilePos;
            if (fixedCamLimit != null)
            {
                Camera.main.GetComponent<NewCameraMovement>().SetMaxPos(camMaxPos-fixedCamLimit);
            }
            else
            {
                Camera.main.GetComponent<NewCameraMovement>().SetMaxPos(camMaxPos);
            }

            //Configuração do chão, para englobar todos os tiles instanciados (menos o ultimo)
            if (midTilesCount>1)
            {
                float midConvert = midTilesCount;
                groundSprite.size = new Vector2((midTilesCount * (groundTileMultiplier * 0.8f)) + (groundTileMultiplier), groundSprite.size.y);
                groundSprite.transform.position = new Vector3((midConvert / 2 + 0.5f) * tilesDistance, groundSprite.transform.position.y, groundSprite.transform.position.z);
            }
            else
            {
                groundSprite.size = new Vector2((midTilesCount+1) * (groundTileMultiplier), groundSprite.size.y);
                groundSprite.transform.position = new Vector3((midTilesCount / 2 + 1) * tilesDistance, groundSprite.transform.position.y, groundSprite.transform.position.z);
            }

            //Configuração do céu, para englobar todos os tiles 
            skySprite.size = new Vector2((midTilesCount + 2) * skyTileMultiplier, skySprite.size.y);
            skySprite.transform.position = new Vector3(((midTilesCount + 2) / 2) * tilesDistance, skySprite.transform.position.y, skySprite.transform.position.z);


            //Instanciando os icones
            //Calcula a distância de um para o outro
            posXRange = (finalPosIcon.x - initialPosIcon.x)/ activities.Count;

            Vector3 iconPos = new Vector3(initialPosIcon.x, initialPosIcon.y, initialPosIcon.z);

            int notSpawnedInteractble = 0;

            int lastInstantiated = 0;

            List<GameObject> interactbleObjectsAux = new List<GameObject>();
            

            if(skipFirstInteractble)
            {
                interactbleObjectsAux.AddRange(interactbleObjects.GetRange(1, interactbleObjects.Count - 1));
            }

            for (int i = 0; i < activities.Count; i++)
            {
                GameObject instatiatedIcon = Instantiate(actIconPrefab);

                ActivityButton activityButton = instatiatedIcon.GetComponent<ActivityButton>();

                activityButton.SetActivity(activities[i]);
                instatiatedIcon.transform.SetParent(pivotCenter.transform);
                
                iconPos.y = initialPosIcon.y + Random.Range(-posYRange, posYRange);
                instatiatedIcon.transform.position = iconPos;
                iconPos.x += posXRange;

                if (i >= skipIcons)
                {
                    activityButton.midItem = true;
                }

                //Sorteia 50% a chance de instanciar um Interactble object
                int randomChance = Random.Range(0, 2);
                
                if ((randomChance == 0 || notSpawnedInteractble >= 2) && (interactbleObjects.Count > 0) && i >= skipIcons && (iconPos.x < finalPosIcon.x))
                {                    
                    if (onlySpawnAbove && (iconPos.y > initialPosIcon.y))
                    {
                        continue;
                    }

                    if(interactbleObjectsAux.Count == 0)
                    {
                        interactbleObjectsAux.AddRange(interactbleObjects.GetRange(0, interactbleObjects.Count));
                    }


                    int randomIndex;

                    //Sorteia um objeto da lista para instanciar
                    /*do
                    {
                        

                    } while (randomIndex == lastInstantiated);
                    */
                    randomIndex = Random.Range(0, interactbleObjectsAux.Count);
                    lastInstantiated = randomIndex;

                    GameObject interactbleObject = Instantiate(interactbleObjectsAux[randomIndex]);

                    interactbleObjectsAux.RemoveAt(randomIndex);

                    intObjPos.z = iconPos.z;
                    //Verifica se o ultimo item está a cima ou abaixo e define o range Y do item a partir disso
                    if (iconPos.y > initialPosIcon.y)
                    {
                        //O Icone foi instanciado a cima do meio
                        intObjPos.y = Random.Range(initialPosIcon.y - posYRange, iconPos.y - interactbleObject.GetComponent<ClickCollider>().securityMargin);
                    }
                    else
                    {
                        //O Icone foi instanciado abaixo do meio
                        intObjPos.y = Random.Range(iconPos.y + intObjMarginY, initialPosIcon.y + interactbleObject.GetComponent<ClickCollider>().securityMargin);                        
                    }

                    intObjPos.x = Random.Range(iconPos.x, (iconPos.x - 2 * intObjMarginX + posXRange));
                    
                    interactbleObject.transform.position = intObjPos;
                    interactbleObject.transform.SetParent(pivotCenter.transform);

                    notSpawnedInteractble = 0;
                }
                else
                {
                    notSpawnedInteractble++;
                }
            }
        }



        public void PopulateWorld(List<Activity> activities)
        {
            GameObject instantiatedTile;
            Vector3 tilePos = Vector3.zero;

            //Instanciando o Tile inicial
            instantiatedTile = Instantiate(tileStart, tilePos, Quaternion.identity, this.transform);
            //Adiciona as atividades ao Tile inicial
            instantiatedTile.GetComponent<TileProperties>().PopulateTile(activities.GetRange(0, iconsStart));
            //Remove as atividades adicionados da lista de icones
            activities.RemoveRange(0, iconsStart);

            //Pega as atividades para o tile final e adiciona a lista auxiliar
            List<Activity> activitiesEnd = activities.GetRange(0, iconsEnd);
            //Remove essas atividades da lista geral
            activities.RemoveRange(0, iconsEnd);

            //Calculo para saber o número de Mid Tiles necessários para comportar todas as atividades
            int midTilesCount = (activities.Count + (iconsPerTile - 1)) / iconsPerTile;

            for (int i = 0; i < midTilesCount; i++)
            {
                tilePos.x += tilesDistance;
                instantiatedTile = Instantiate(tileMidle, tilePos, Quaternion.identity, this.transform);

                instantiatedTile.GetComponent<TileProperties>().SortTileConfiguration();

                if ((i * iconsPerTile + iconsPerTile) < activities.Count)
                {
                    instantiatedTile.GetComponent<TileProperties>().PopulateTile(activities.GetRange(i * iconsPerTile, iconsPerTile));
                }
                else
                {
                    instantiatedTile.GetComponent<TileProperties>().PopulateTile(activities.GetRange(i * iconsPerTile, (activities.Count - i * iconsPerTile)));
                }
            }

            //Adicionta a posição x e instancia o Tile final
            //tilePos.x += tilesDistance;
            instantiatedTile = Instantiate(tileEnd, tilePos, Quaternion.identity, this.transform);

            instantiatedTile.GetComponent<TileProperties>().PopulateTile(activitiesEnd);

            //Atualiza a posição máxima da câmera
            tilePos.x += tilesDistance * 0.8f;
            Camera.main.GetComponent<NewCameraMovement>().SetMaxPos(tilePos);


			//Configuração do chão, para englobar todos os tiles instanciados (menos o ultimo)
			groundSprite.size = new Vector2((midTilesCount + 1) * groundTileMultiplier, groundSprite.size.y);
			groundSprite.transform.position = new Vector3((midTilesCount/2 + 0.5f) * tilesDistance, groundSprite.transform.position.y, groundSprite.transform.position.z);

			//Configuração do céu, para englobar todos os tiles 
			skySprite.size = new Vector2((midTilesCount + 2) * skyTileMultiplier, skySprite.size.y);
			skySprite.transform.position = new Vector3(((midTilesCount  + 2.5f )/ 2) * tilesDistance, skySprite.transform.position.y, skySprite.transform.position.z);

		}
    }
}