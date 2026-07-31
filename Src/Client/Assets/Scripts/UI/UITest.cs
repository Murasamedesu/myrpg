using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITest : UIWindow
{

    public TMP_Text title;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SetTitle(string title)
    {
        this.title.text = title;
    }


}
