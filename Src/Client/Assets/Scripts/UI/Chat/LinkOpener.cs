using Candlelight;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkOpener : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text pTextMeshPro = GetComponent<TMP_Text>();

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);

        if(linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();
            if (string.IsNullOrEmpty(linkId)) return;
            string[] strs = linkId.Split(":".ToCharArray());
            UIPopCharMenu menu = UIManager.Instance.ShoW<UIPopCharMenu>();
            menu.targetId = int.Parse(strs[0]);
            menu.targetName = strs[1];
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }




}
