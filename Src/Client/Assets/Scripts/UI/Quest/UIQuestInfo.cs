using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text targets;
    public TMP_Text description;

    public GameObject rewardItems;
    public TMP_Text rewardMoney;
    public TMP_Text rewardExp;

    public TMP_Text overview;

    List<Image> slots;
    public Transform[] Pages;
    void Start()
    {
    }


    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}] {1}", quest.Define.Type, quest.Define.Name);
        if(this.overview != null) this.overview.text = quest.Define.Overview;
        
        if (slots == null)
        {
            slots = new List<Image>();
            for (int page = 0; page < this.Pages.Length; page++)
            {
                slots.AddRange(this.Pages[page].GetComponentsInChildren<Image>(true));
            }
        }
        if (slots != null)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].transform.childCount > 0)
                {
                    Destroy(slots[i].transform.GetChild(0).gameObject);
                }
            }
        }

        if (this.description != null)
        {
            if (quest.Info == null)
            {
                this.description.text = quest.Define.Dialog;
                this.targets.text = quest.Define.Overview;

            }
            else
            {
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.Finished)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.InProgress)
                {
                    this.description.text = quest.Define.DialogIncomplete;
                }
            }
        }

        if (quest.Define.RewardItem1 > 0)
        {
            GameObject go = Instantiate(rewardItems, slots[0].transform);
            var ui = go.GetComponent<UIIconItem>();
            var def = DataManager.Instance.Items[quest.Define.RewardItem1];
            ui.SetMainIcon(def.Icon, quest.Define.RewardItem1Count.ToString());
        }
        if (quest.Define.RewardItem2 > 0)
        {
            GameObject go = Instantiate(rewardItems, slots[1].transform);
            var ui = go.GetComponent<UIIconItem>();
            var def = DataManager.Instance.Items[quest.Define.RewardItem2];
            ui.SetMainIcon(def.Icon, quest.Define.RewardItem2Count.ToString());
        }
        if (quest.Define.RewardItem3 > 0)
        {
            GameObject go = Instantiate(rewardItems, slots[2].transform);
            var ui = go.GetComponent<UIIconItem>();
            var def = DataManager.Instance.Items[quest.Define.RewardItem3];
            ui.SetMainIcon(def.Icon, quest.Define.RewardItem3Count.ToString());
        }

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        foreach(var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }



    }


    public void OnClickAbandon()
    {

    }






}
