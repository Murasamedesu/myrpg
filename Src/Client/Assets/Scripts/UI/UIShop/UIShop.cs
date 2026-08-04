using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
    public TMP_Text Title;
    public GameObject ShopItem;
    public TMP_Text Money;
    public Transform Pages;
    ShopDefine shop;

    private UIShopItem selectedItem;

    void Start()
    {
        StartCoroutine(InitItems());
    }


    IEnumerator InitItems()
    {
        foreach(var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if(kv.Value.Status > 0)
            {
                GameObject go = Instantiate(ShopItem, Pages);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
            }
        }
        yield return null;
    }

    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.Title.text = shop.Name;
        this.Money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    public void SelectShopItem(UIShopItem item)
    {
        if(selectedItem != null)
        {
            selectedItem.Selected = false;
        }
        selectedItem = item;
    }

    public void OnClickBuy()
    {
        if(this.selectedItem == null)
        {
            MessageBox.Show("请先选择要购买的物品", "提示", MessageBoxType.Information);
            return;
        }
        
    }   


}
