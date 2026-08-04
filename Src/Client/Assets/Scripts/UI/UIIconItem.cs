using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour
{
    public Image IconImage;
    public Image SecondImage;
    public TMP_Text text;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetMainIcon(string iconName, string text)
    {
        this.IconImage.overrideSprite = Resloader.Load<Sprite>(iconName);
        this.text.text = text;
    }


}
