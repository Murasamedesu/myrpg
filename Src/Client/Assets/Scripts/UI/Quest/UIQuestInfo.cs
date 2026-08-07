using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text[] targets;
    public TMP_Text description;

    public UIIconItem rewardItems;
    public TMP_Text rewardMoney;
    public TMP_Text rewardExp;


    void Start()
    {
        
    }


    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}] {1}", quest.Define.Type, quest.Define.Name);
        if(quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.DialogFinish;
            }
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
