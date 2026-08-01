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

    void Start()
    {
        anim = GetComponent<Animator>();

        npc = NpcManager.Instance.GetNpcDefine(npcID);

        rendererr = GetComponentInChildren<SkinnedMeshRenderer>();
        originRotation = transform.rotation;
        this.StartCoroutine(Action());

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