using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BankruptcyImminentText : MonoBehaviour
{
    public TMP_Text text;
    public float fadeInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        while (true)
        {
            while (text.color.a > 0)
            {
                Color newColor = text.color;
                newColor.a -= Time.deltaTime / fadeInterval;
                text.color = newColor;

                yield return null;
            }

            while (text.color.a < 1)
            {
                Color newColor = text.color;
                newColor.a += Time.deltaTime / fadeInterval;
                text.color = newColor;

                yield return null;
            }
        }
    }
}
