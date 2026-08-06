using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICharEquip : UIWindow
{
    public TMP_Text title;
    public GameObject ItemPrefab;
    public GameObject ItemEquipedPrefab;
    public Transform ItemListRoot;
    public List<Transform> slots;

    void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipedList();
        InitEquipedItems();
    }

    //初始化左侧装备列表
    void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                {
                    continue;
                }
                GameObject go = Instantiate(ItemPrefab, ItemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }

    void ClearAllEquipList()
    {
        foreach(var item in ItemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }

    void ClearEquipedList()
    {
        foreach(var item in slots)
        {
            if(item.childCount > 0)
            {
                Destroy(item.GetChild(0).gameObject);
            }
        }
    }


    // 初始化角色装备面板
    void InitEquipedItems()
    {
        for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if(item != null)
                {
                    GameObject go = Instantiate(ItemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }
    }




    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }


    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }

}
