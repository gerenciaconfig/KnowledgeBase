using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;


namespace Arcolabs
{
    [CommandInfo("UI", "TextWriter", "Write Text in screen")]
    public class TextWriter : Command
    {
        public Text text;
        public string textValue;
        public float timeBetweenLetters;
        public Color textColor;
        public bool deactivateText;
        public float deactivateTime;
        public bool waitUntilFinish;

        public override void OnEnter()
        {
            StartCoroutine(WriteText());

            if (!waitUntilFinish)
            {
                Continue();
            }
        }

        IEnumerator WriteText()
        {
            text.text = null;
            text.gameObject.SetActive(true);
            text.color = textColor;

            foreach (char item in textValue)
            {
                text.text = text.text += item;
                yield return new WaitForSeconds(timeBetweenLetters);
            }

            if (deactivateText == true)
            {
                yield return new WaitForSeconds(deactivateTime
                    );
                text.gameObject.SetActive(false);
            }

            if (waitUntilFinish)
            {
                Continue();
            }
        }
    }
}
