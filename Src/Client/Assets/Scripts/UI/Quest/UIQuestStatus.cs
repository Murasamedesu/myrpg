using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestStatus : MonoBehaviour
{
    public Image[] statusImages;
    
    public NpcQuestStatus questStatus;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetQuestStatus(NpcQuestStatus status)
    {
        questStatus = status;

        for(int i = 0; i< 4; i++)
        {
            if (statusImages[i] != null)
            {
                statusImages[i].gameObject.SetActive(i == (int)status);
            }
        }

    }


}
