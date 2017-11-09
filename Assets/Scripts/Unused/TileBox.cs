using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBox : MonoBehaviour
{

    private Vector3 center;
    private Vector3 extents;
    private Vector3 size;

    private Color drawColor = Color.green;

    public void AddBox(Vector3 center, Vector3 size)
    {
        this.extents = new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);
        this.size = size;
        this.center = center;
    }

    public void OnDrawGizmos()
    {
        Collider[] hits = Physics.OverlapBox(center, extents);
        if (hits.Length == 0)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawCube(center, size);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(center, size);
    }
}
