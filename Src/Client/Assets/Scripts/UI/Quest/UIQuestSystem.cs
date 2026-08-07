using Common.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuestSystem : UIWindow
{
    public TMP_Text title;
    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView listMain;
    public ListView listBranch;

    public UIQuestInfo questInfo;

    private bool showAvailableList = false;


    void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
        //QuestManager.Instance.OnQuestChanged += RefreshUI;
    }


    void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questItem.quest);
    }



    private void OnDestroy()
    {
        //QuestManager.Instance.OnQuestChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllQuestList();
        //InitAllQuestItems();
    }

    void ClearAllQuestList()
    {
        this.listMain.RemoveAll();
        this.listBranch.RemoveAll();
    }

    //void InitAllQuestItems()
    //{
    //    foreach(var kv in QuestManager.Instance.allQuests)
    //    {
    //        if (showAvailableList)
    //        {
    //            if(kv.Value.Info != null)
    //            {
    //                continue;
    //            }
    //        }
    //        else
    //        {
    //            if (kv.Value.Info == null)
    //            {
    //                continue;
    //            }
    //        }

    //        GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
    //        UIQuestItem ui = go.GetComponent<UIQuestItem>();
    //        ui.SetQuestInfo(kv.Value);
    //        if (kv.Value.Define.Type == QuestType.Main)
    //        {
    //            this.listMain.AddItem(ui);
    //        }
    //        else
    //        {
    //            this.listBranch.AddItem(ui);
    //        }
    //    }
    //}





}
