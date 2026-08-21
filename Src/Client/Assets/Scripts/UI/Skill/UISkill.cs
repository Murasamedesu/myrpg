using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Common;
using Common.Battle;

public class UISkill : UIWindow
{
    public TMP_Text descript;
    public GameObject ItemPrefab;
    public ListView listMain;
    private UISkillItem selectedItem;

    void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnDestroy()
    {

    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UISkillItem;
        this.descript.text = this.selectedItem.item.Description;
    }



    void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    //初始化左侧列表
    void InitItems()
    {
        var Skills = DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class];
        foreach (var kv in Skills)
        {
            if (kv.Value.Type == SkillType.Skill)
            {
                GameObject go = Instantiate(ItemPrefab, listMain.transform);
                UISkillItem ui = go.GetComponent<UISkillItem>();
                ui.SetSkillItem(kv.Value, this, false);
                this.listMain.AddItem(ui);
            }
        }
    }

    void ClearItems()
    {
        this.listMain.RemoveAll();
    }


}
