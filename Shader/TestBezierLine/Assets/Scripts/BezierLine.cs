using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierLine : MonoBehaviour {

    public Vector3[] points;

    //根据进度t,获取曲线点坐标
    public Vector3 GetPoint(float t1)
    {
        Vector3 v = BezierInterface.GetPoint(
            points[0],
            points[1],
            points[2],
            points[3],
            t1
            );
        return transform.TransformPoint(v);
    }

    //获取速度
    Vector3 GetVelocity(float t){
        Vector3 v = transform.TransformPoint(
            BezierInterface.GetFirstDerivative (
                points[0],
                points[1],
                points[2],
                points[3],
                t)             
            );
        v -= transform.position;
        return v;
    }

    //根据进度t, 获取曲线点的方向向量.
    public Vector3 GetDir(float t)
    {
        return GetVelocity(t).normalized;
    }

}
