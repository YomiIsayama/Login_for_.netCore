using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private float changeTime = 0.0f;
    private Sprite loadingImg1;
    private Sprite loadingImg2;

    // Start is called before the first frame update
    void Start()
    {
        loadingImg1 = Resources.Load("ButtonTimescaleFullUpSprite", typeof(Sprite)) as Sprite;
        loadingImg1 = Resources.Load("ButtonTimescaleSlowUpSprite", typeof(Sprite)) as Sprite;

    }

    // Update is called once per frame
    void Update()
    {
        Image image = this.GetComponent<Image>();

        if(image.enabled)
        {
            changeTime += Time.deltaTime;
            if(changeTime>0.1)
            {
                changeTime = 0;
                if (image.sprite == loadingImg1)
                {
                    image.sprite = loadingImg2;
                }
                else
                {
                    image.sprite = loadingImg1;
                }
            }
        }
    }
}
