using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;
    public Image Overlay;
    public TMP_Text cdText;
    public SkillDefine skill;

    float overlaySpeed = 0;
    float cdRemain = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Overlay.fillAmount > 0)
        {
            Overlay.fillAmount = this.cdRemain / this.skill.CD;
            this.cdText.text = ((int)Math.Ceiling(this.cdRemain)).ToString();
            this.cdRemain -= Time.deltaTime;
        }
        else
        {
            if(Overlay.enabled) Overlay.enabled = false;
            if(this.cdText.enabled) cdText.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.Overlay.fillAmount > 0)
        {

        }
        else
        {
            this.SetCD(this.skill.CD);
        }
    }

    public void SetCD(float cd)
    {
        if(!Overlay.enabled) Overlay.enabled=true;
        if(!cdText.enabled) cdText.enabled=true;
        cdText.text = ((int)Math.Floor(this.cdRemain)).ToString();
        Overlay.fillAmount = 1f;
        overlaySpeed = 1f/cd;
        cdRemain = cd;
    }

    internal void SetSkill(SkillDefine value)
    {
        this.skill = value;
        if (this.Icon != null) Icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Icon);
        this.SetCD(this.skill.CD);
    }

}
