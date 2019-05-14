using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Game", "Select Image Configuration", "Bla bla bla")]
public class SelectImageConfiguration : Command
{
    public Sprite backgroundSprite;

    public GameObject backgroundGObj;

    public GameObject objectiveGameObject;
    public Sprite objectiveImage;

    [Space]

    [SerializeField]
    public List<ImageAndSoundObject> rightOptions;

    [Space]

    [SerializeField]
    public List<FungusSprite> wrongOptions;

    [Space]

    public Image background;
    public Button rightButton;
    public Button wrongButton1;
    public Button wrongButton2;

    public override void OnEnter()
    {
        if(objectiveImage != null)
        {
            objectiveGameObject.GetComponent<Image>().sprite = objectiveImage;
        }
        
        

        int aux, aux2;

        /*
        if(backgroundSprite != null)
        {
            background.sprite = backgroundSprite;
        }*/
        

        if(backgroundGObj!= null)
        {
            backgroundGObj.SetActive(true);
        }
        

        //Sorteia a imagem correta e a coloca no botão determinado
        aux = Random.Range(0, rightOptions.Count);
        rightButton.image.sprite = rightOptions[aux].image;
        
        if(rightOptions[aux].highlightedImage != null)
        {
            rightButton.GetComponent<ButtonBehaviour>().normalSprite = rightOptions[aux].image;
            rightButton.GetComponent<ButtonBehaviour>().highlightedSprite = rightOptions[aux].highlightedImage;
        }

        //Seta o som correspondente a imagem como o padrão para aquele lvl

        //Sorteia a posição das imagens incorretas apenas se houverem mais imagens do que slots
        if (wrongOptions.Count == 2)
        {
            wrongButton1.image.sprite = wrongOptions[0].image;
            wrongButton2.image.sprite = wrongOptions[1].image;
        }
        else
        {
            //Sorteia a primeira imagem errada e a coloca no botão determinado
            aux = Random.Range(0, wrongOptions.Count);
            wrongButton1.image.sprite = wrongOptions[aux].image;

            //Sorteia uma nova imagem para a segunda posição, fica no loop até sortear uma imagem diferente da primeira
            do
            {
                aux2 = Random.Range(0, wrongOptions.Count);

            } while (aux2 == aux);

            //Coloca a imagem no botão determinado
            wrongButton2.image.sprite = wrongOptions[aux2].image;
        }
        

        Continue();
    }
}
