using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagView : MonoBehaviour
{
    public int index = -1;
    public Button[] bagbutton;
    public GameObject[] bagpage;
    public GameObject[] buttonactiveimages;

    void Start()
    {
        OnSelectBag(0);
    }

    public void OnSelectBag(int bagindex)
    {
        if (this.index != bagindex)
        {
            this.index = bagindex;
            for (int i = 0; i < bagbutton.Length; i++)
            {
                buttonactiveimages[i].SetActive(i == bagindex);
                bagpage[i].SetActive(i == bagindex);
            }
        }
    }



}
