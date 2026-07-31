using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models;
using UnityEditor;
using Managers;

public class UIMinimap : MonoBehaviour
{
    public Collider minimapBoundingBox;
    public Image minimap;
    public Image arrow;

    public TMP_Text mapName;

    private Transform playerTransform;

    void Start()
    {
        Debug.LogWarning("UIMinimap Start " + this.GetInstanceID());
        MinimapManager.Instance.Minimap = this;
        this.UpdateMap();
    }



    public void UpdateMap()
    {
        mapName.text = User.Instance.CurrentMapData.Name;
        minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        minimap.SetNativeSize();
        minimap.transform.localPosition = Vector3.zero;
        minimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;
        playerTransform = null;

    }


    void Update()
    {
        if(playerTransform == null && User.Instance.CurrentCharacterObject != null)
        {
            playerTransform = MinimapManager.Instance.PlayerTransform;
        }

        if (minimapBoundingBox == null || playerTransform == null) return;
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);

    }

}
