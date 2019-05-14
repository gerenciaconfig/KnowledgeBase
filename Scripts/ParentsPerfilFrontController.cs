using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentsPerfilFrontController : PerfilFrontController
{
    public override bool CarregarPerfis()
    {
        try
        {
            var currentKidList = CurrentStatsInfo.currentUser.kids;

            if (currentKidList != null)
            {
                foreach (var item in currentKidList)
                {
                    PerfilButtonClass perfilButton = Instantiate(perfilPrefab, gridPerfil.transform).GetComponent<PerfilButtonClass>();
                    perfilButton.kid = item;
                    perfilButton.perfilName.text = item.name;

                    foreach (var image in perfilImageList.perfilImageList)
                    {
                        if (image.Key == item.imageId)
                        {
                            perfilButton.perfilImage.sprite = image.Value;
                            break;
                        }
                    }
                }
            }
        }
        catch
        {

        }
        return true;
    }
}
