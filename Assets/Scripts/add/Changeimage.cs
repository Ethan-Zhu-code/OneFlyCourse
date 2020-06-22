using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Changeimage : MonoBehaviour
{
    public Sprite[] images;
    public GameObject card;
    public GameObject cardhighlight;

    public void Change(int i)
    {
        card.GetComponent<Image>().sprite = images[i];
        cardhighlight.GetComponent<Image>().sprite = images[i + 1];
    }
}
