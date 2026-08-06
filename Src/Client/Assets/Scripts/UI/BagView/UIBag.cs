using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindow
{
    public TMP_Text PlayerGold;
    public Transform[] Pages;

    public GameObject bagItem;
    

    List<Image> slots;


    void Start()
    {
        if(slots == null)
        {
            slots = new List<Image>();
            for(int page = 0; page < this.Pages.Length; page++)
            {
                slots.AddRange(this.Pages[page].GetComponentsInChildren<Image>(true));
            }
        }
        StartCoroutine(InitBags());
    }

    IEnumerator InitBags()
    {
        for(int i = 0; i < BagManager.Instance.Items.Length; i++)
        {
            var item  = BagManager.Instance.Items[i];
            if(item.ItemId > 0)
            {
                GameObject go = Instantiate(bagItem, slots[i].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].Define;
                ui.SetMainIcon(def.Icon, item.Count.ToString());
            }
            
        }

        for(int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
        {
            slots[i].color = Color.gray;
        }
        SetTitle();
        yield return null;
    }


    public void SetTitle()
    {
        this.PlayerGold.text = User.Instance.CurrentCharacter.Gold.ToString();

    }

    public void OnReset()
    {
        BagManager.Instance.Reset();
        Clear();
        StartCoroutine(InitBags());
    }

    void Clear()
    {
        for (int i = 0;i < slots.Count; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }
        }
    }





}
