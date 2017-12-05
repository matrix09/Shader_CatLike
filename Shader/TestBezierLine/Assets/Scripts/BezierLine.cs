using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BezierLine : MonoBehaviour {

    public Vector3[] points;


    public float m_fLength;


    public Vector3[] m_vConstants;

    //根据进度t,获取曲线点坐标
    public Vector3 GetPoint(float t1)
    {

        int index = 0;
        if (t1 >= 1f)
        {
            t1 = 1f;
            index = points.Length - 4;
        }
        else
        {
            t1 = t1 * ((points.Length - 1) / 3);
            index = (int)t1;
            t1 = t1 - index;
            index *= 3;
        }

        Vector3 v = BezierInterface.GetPoint(
            points[index],
            points[index + 1],
            points[index + 2],
            points[index + 3],
            t1
            );
        return transform.TransformPoint(v);

    }


    void Reset()
    {
        points = new Vector3[] {
            new Vector3(4f, 0f, 0f),
            new Vector3(8f, 0f, 0f),
            new Vector3(12f, 0f, 0f),
            new Vector3(16f, 0f, 0f),
        };

        m_vConstants = new Vector3[3];

    }

    //获取速度
    Vector3 GetVelocity(float t1){

        int index = 0;
        if (t1 >= 1f)
        {
            t1 = 1f;
            index = points.Length - 4;
        }
        else
        {
            t1 = t1 * ((points.Length - 1) / 3);
            index = (int)t1;
            t1 = t1 - index;
            index *= 3;
        }
        //v1 = -3A + 9B - 9C + 3D
        m_vConstants[0] = -3 * points[index] + 9 * points[index + 1] - 9 * points[index + 2] + 3 * points[index + 3];
        //v2 = 6A - 12B + 6C
        m_vConstants[1] = 6 * points[index] - 12 * points[index + 1] + 6 * points[index + 2];
        //v3 = -3A + 3B
        m_vConstants[2] = -3 * points[index] + 3 * points[index + 1];

        Vector3 v = BezierInterface.GetFirstDerivative(
            points[index],
            points[index + 1],
            points[index + 2],
            points[index + 3],
            t1
            );

        v = transform.TransformPoint(v);
            
        v -= transform.position;
        return v;
    }

    //根据进度t, 获取曲线点的方向向量.
    public Vector3 GetDir(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void AddPoint()
    {
        Vector3 pos = points[points.Length - 1];
        //增加points的长度
        Array.Resize(ref points, points.Length + 3);
        pos.y += 4;
        points[points.Length - 3] = pos;
        pos.y += 4;
        points[points.Length - 2] = pos;
        pos.y += 4;
        points[points.Length - 1] = pos;
        AddForce(points.Length - 3);



    }

    public void AddForce(int index)
    {
        /*
         * 0     1       2       3       4       5       6
         * 0      0      1       1       1       2       2
         * 1过滤器
         * 2计算fixedIndex, EnforcedIndex
         * 3计算Enforced Point Position.
         * */

        int modelIndex = (index + 1) / 3;

        modelIndex *= 3;

        if (modelIndex == 0 || modelIndex == points.Length - 1)
            return;


        int fixedIndex, enforcedIndex;
        if (index < modelIndex)
        {
            fixedIndex = modelIndex - 1;
            enforcedIndex = modelIndex + 1;
        }
        else
        {
            fixedIndex = modelIndex + 1;
            enforcedIndex = modelIndex - 1;
        }

        Vector3 middlePoint = points[modelIndex];
        Vector3 fixedPoint = points[fixedIndex];
        //points[enforcedIndex] = middlePoint + (middlePoint - fixedPoint).normalized * Vector3.Distance(middlePoint, fixedPoint);
        points[enforcedIndex] = middlePoint +
            (middlePoint - fixedPoint);
    }

    public float GetDeltaT(float t)
    {
        int index = 0;
        if (t >= 1f)
        {
            t = 1f;
        }
        else
        {
            t = t * ((points.Length - 1) / 3);
            index = (int)t;
            t = t - index;
        }

        return m_fLength / Vector3.Magnitude(t * t * m_vConstants[0] + t * m_vConstants[1] + m_vConstants[2]);
    }

        
}
