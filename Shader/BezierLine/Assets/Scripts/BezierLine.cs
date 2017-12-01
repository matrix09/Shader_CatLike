using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BezierLine : MonoBehaviour {


    public Vector3[] points;

    public int CurveNum
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }

    public void Reset()
    {
        points = new Vector3[] { 
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }


    public void AddPoints()
    {
        Vector3 pos = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        pos.x += 1f;
        points[points.Length - 3] = pos;
        pos.x += 1f;
        points[points.Length - 2] = pos;
        pos.x += 1f;
        points[points.Length - 1] = pos;

    }

    public Vector3 GetPoint(float t)
    {
        /*
         * 期望曲线在全路径下进行计算
         * 
         * @    @   @   @   @   @   @   
         * 0      1     2    3     4    5     6    
         * 
         * 目前曲线个数 : 2
         * 假如当前t = 0.5f; 0.6,
         * 
         * t * curveNum
         * 取整 -> 第几条曲线 -> N
         * 取余 -> 第N跳曲线的进度值
         * 
         * 
         * */
        int index = 0;
        if (t >= 1f)
        {
            t = 1f;
            index = points.Length - 4;
        }
        else
        {
            float n = t * CurveNum;
            index = (int)n;
            t = n - index;
            index *= 3;
        }

        Vector3 v = BezierInterface.GetPoint(points[index], points[index + 1], points[index + 2], points[index + 3], t);

        return transform.TransformPoint(v);

    }

    Vector3 GetVelocity(float t)
    {

        int index = 0;
        if (t >= 1f)
        {
            t = 1f;
            index = points.Length - 4;
        }
        else
        {
            float n = t * CurveNum;
            index = (int)n;
            t = n - index;
            index *= 3;
        }

        Vector3 v = transform.TransformPoint(BezierInterface.GetFirstDerivative(points[index], points[index + 1], points[index + 2], points[index + 3], t));

        v -= transform.position;

        return v;
    }

    public Vector3 GetDir(float t)
    {

        Vector3 v = GetVelocity (t);

        return v.normalized;
    }




}
