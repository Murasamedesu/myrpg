using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour
{
    public Transform owner;
    public float heght = 2.1f;


    void Start()
    {
        
    }

    void Update()
    {
        if(owner != null)
        {
            transform.position = owner.position + Vector3.up * heght;
        }
        if (Camera.main != null)
            this.transform.forward = Camera.main.transform.forward;
    }
}
