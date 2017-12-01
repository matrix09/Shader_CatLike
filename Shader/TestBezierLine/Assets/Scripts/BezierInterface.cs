using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierInterface {



    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {

        Vector3 v = Vector3.zero;

        t = Mathf.Clamp01(t);

        float oneMinusT = 1f - t;

        v = oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;

        return v;

    }


    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {

        Vector3 v = Vector3.zero;
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        v = 3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);


        return v;

    }

}
