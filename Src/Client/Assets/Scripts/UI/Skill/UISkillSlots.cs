using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour
{
    public UISkillSlot[] slots;


    // Start is called before the first frame update
    void Start()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        var Skills = DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class];
        int skillIdx = 0;
        foreach(var kv in Skills)
        {
            slots[skillIdx].SetSkill(kv.Value);
            skillIdx++;
        }
    }


}
