using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class FVT : MonoBehaviour {

    [SerializeField] int size = 16;

    [SerializeField] Vector2 eye = new Vector2(0f, 0f);
    [SerializeField] Vector2 dir = new Vector2(0.5f, 0.5f);

    float frac0(float x)
    {
        return x - Mathf.Floor(x);
    }

    float frac1(float x)
    {
        return 1f - x + Mathf.Floor(x);
    }

    void OnDrawGizmos ()
    {
        DrawGrid();
        Traverse(eye, dir);
    }

    void DrawGrid ()
    {
        Gizmos.color = Color.gray;
        var unit = Vector3.one;
        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                Gizmos.DrawWireCube(new Vector3(x, y, 0), unit);
            }
        }
    }

    void Traverse(Vector2 eye, Vector2 dir)
    {
        int x, y, z;
        int dx, dy, dz;

        x = Mathf.FloorToInt(eye.x);
        y = Mathf.FloorToInt(eye.y);
        z = 0;

        dx = Mathf.FloorToInt(dir.x * size);
        dy = Mathf.FloorToInt(dir.y * size);
        dz = 0;

        int n, sx, sy, sz, ax, ay, az, bx, by, bz;
        int exy, exz, ezy;

        sx = (int)Mathf.Sign(dx);
        sy = (int)Mathf.Sign(dy);
        sz = (int)Mathf.Sign(dz);

        ax = Mathf.Abs(dx);
        ay = Mathf.Abs(dy);
        az = Mathf.Abs(dz);

        bx = 2 * ax;
        by = 2 * ay;
        bz = 2 * az;

        exy = ay - ax;
        exz = az - ax;
        ezy = ay - az;

        Gizmos.color = Color.white;

        var start = new Vector3(x, y, z);
        var end = new Vector3(x + dx, y + dy, z + dz);
        Gizmos.DrawLine(start, end);

        n = ax + ay + az;
        int iter = 0;
        while (
            n-- >= 0 &&
            0 <= x && x < size && 
            0 <= y && y < size && 
            0 <= z && z < size
        )
        {
            Handles.Label(new Vector3(x, y, z), (iter++).ToString());

            if (exy < 0)
            {
                if (exz < 0)
                {
                    x += sx;
                    exy += by; exz += bz;
                }
                else
                {
                    z += sz;
                    exz -= bx; ezy += by;
                }
            }
            else
            {
                if (ezy < 0)
                {
                    z += sz;
                    exz -= bx; ezy += by;
                }
                else
                {
                    y += sy;
                    exy -= bx; ezy -= bz;
                }
            }
        }

    }

}
