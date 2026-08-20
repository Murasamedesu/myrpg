using Common.Battle;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow
{
    public TMP_Text title;
    public GameObject ItemPrefab;
    public GameObject ItemEquipedPrefab;
    public Transform ItemListRoot;
    public List<Transform> slots;


    public TMP_Text hp;
    public Slider hpBar;
    public TMP_Text mp;
    public Slider mpBar;

    public TMP_Text[] attrs;


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
        InitAttributes();
    }

    //初始化左侧装备列表
    void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacterInfo.Class)
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

    void InitAttributes()
    {
        var charattr = User.Instance.CurrentCharacter.Attributes;
        this.hp.text = string.Format("{0}/{1}", charattr.HP, charattr.MaxHP);
        this.mp.text = string.Format("{0}/{1}", charattr.MP, charattr.MaxMP);
        this.hpBar.maxValue = charattr.MaxHP;
        this.hpBar.value = charattr.HP;
        this.mpBar.maxValue = charattr.MaxMP;
        this.mpBar.value = charattr.MP;

        for (int i = (int)AttributeType.STR; i < (int)AttributeType.MAX; i++)
        {
            if (i == (int)AttributeType.CRI)
            {
                this.attrs[i - 2].text = string.Format("{0:f2}%", charattr.Final.Data[i] * 100);
            }
            else
            {
                this.attrs[i - 2].text = ((int)charattr.Final.Data[i]).ToString();
            }
        }
    }


}
