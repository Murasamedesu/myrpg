using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    public int npcID;

    private Animator anim;

    private NpcDefine npc;

    private bool isBarCreated = false;
    private Quaternion originRotation;
    private bool inInteractive = false;
    private SkinnedMeshRenderer rendererr;

    NpcQuestStatus questStatus;


    void Start()
    {
        anim = GetComponent<Animator>();

        npc = NpcManager.Instance.GetNpcDefine(npcID);

        rendererr = GetComponentInChildren<SkinnedMeshRenderer>();
        originRotation = transform.rotation;
        NpcManager.Instance.UpdateNpcPosition(this.npcID, this.transform.position);
        this.StartCoroutine(Action());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;

    }

    void OnQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcID);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if(UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
        }
    }


    private void OnMouseEnter()
    {
        Highlight(true);

    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
        
    }


    private void OnMouseDown()
    {
        //if(Vector3.Distance(this.transform.position, User.Instance.CurrentCharacterObject.transform.position) > 2f)
        //{
        //    User.Instance.CurrentCharacterObject.StartNav(this.transform.position);
        //}
        Interactive();
    }

    IEnumerator Action()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
            this.Relax();
            yield return new WaitForSeconds(2.5f);
            this.Idle();
        }

    }

    void Relax()
    {
        anim.SetTrigger("Relax");
    }

    void Idle()
    {
        anim.SetTrigger("Idle");
    }


    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        yield return FaceToOrigin();
        inInteractive = false;
        this.Idle();
        
    }
    
    IEnumerator FaceToPlayer()
    {
        Vector3 playerPos = User.Instance.CurrentCharacterObject.transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(transform.forward, direction)) > 5f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);
            yield return null;
        }

    }

    IEnumerator FaceToOrigin()
    {
        while (Mathf.Abs(Quaternion.Angle(transform.rotation, originRotation)) > 1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originRotation, Time.deltaTime * 5f);
            yield return null;
        }
        
    }

    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (!isBarCreated)
            {
                isBarCreated = true;
                UIWorldElementManager.Instance.AddNPCChooseBar(transform);
            }
        }
        else
        {
            if (isBarCreated)
            {
                UIWorldElementManager.Instance.RemoveNPCChooseBar(transform);
                isBarCreated = false;
            }
        }
        
    }


}