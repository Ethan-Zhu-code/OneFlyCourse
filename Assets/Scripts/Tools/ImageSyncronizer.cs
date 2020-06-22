using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSyncronizer : MonoBehaviour
{
    [Header("同步下屏的Image组件")]
    public Image target_image;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        image.sprite = target_image.sprite;
    }
}
