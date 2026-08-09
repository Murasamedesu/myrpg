using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour
{
    public int ID;
    Mesh mesh = null;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 pos = this.transform.position + Vector3.up * transform.localScale.y * .5f;
        Gizmos.color = Color.red;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh, pos, transform.rotation, transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, pos,transform.rotation, 1f, EventType.Repaint);
        UnityEditor.Handles.Label(pos, "SpawnPoint:" + this.ID);
    }
#endif




}
