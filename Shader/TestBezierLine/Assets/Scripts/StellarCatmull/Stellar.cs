using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Assets.Scripts.Stellar
{
    public class Stellar : MonoBehaviour
    {

        public Vector3[] Points;

        public Vector3 Interp(float t)
        {
            int index = 0;
            if (t >= 1)
            {
                t = 1;
                index = Points.Length - 4;
            }
            else
            {
                t = t * (Points.Length - 1) / 3;
                index = (int)t;
                t = t - index;
            }

            return transform.TransformPoint(StellarInterface.Interp(Points[index], Points[index + 1], Points[index + 2], Points[index + 3], t));
        }

        public Vector3 Velocity(float t)
        {
            int index = 0;
            if (t >= 1)
            {
                t = 1;
                index = Points.Length - 4;
            }
            else
            {
                t = t * (Points.Length - 1) / 3;
                index = (int)t;
                t = t - index;
            }

            return transform.TransformPoint(StellarInterface.Velocity(Points[index], Points[index + 1], Points[index + 2], Points[index + 3], t)) - transform.position;
        }

        public Vector3 GetDir(float t)
        {
            return Velocity(t).normalized;
        }

    }
}



//public void GizmoDraw(Vector3[] source, float t)
//{
//    Gizmos.color = Color.white;
//    Vector3 prevPt = Interp(source, 0);

//    for (int i = 1; i <= 40; i++)
//    {
//        float pm = (float)i / 40f;
//        Vector3 currPt = Interp(source, pm);
//        Gizmos.DrawLine(currPt, prevPt);
//        prevPt = currPt;
//    }

//    Gizmos.color = Color.blue;
//    Vector3 pos = Interp(source, t);
//    Gizmos.DrawLine(pos, pos + Velocity(source, t).normalized);
//}

//public Vector3[] InitializePathPoints(Transform points)
//        {
//            Vector3[] source = new Vector3[points.childCount];
//            for (int i = 0; i < source.Length; i++)
//            {
//                source[i] = points.GetChild(i).position;
//            }

//            Vector3[] outputs = new Vector3[source.Length + 2];

//            Array.Copy(source, 0, outputs, 1, source.Length);

//            outputs[0] = outputs[1] + outputs[1] - outputs[2];

//            outputs[outputs.Length - 1] = outputs[outputs.Length - 2] + outputs[outputs.Length - 2] - outputs[outputs.Length - 3];


//            m_bIsInitializePoints = true;

//            return outputs;
//        }

//public Vector3 Interp(Vector3[] source, float per) 
//{

//    int numSections = source.Length - 3;
//    int currPt = Mathf.Min(Mathf.FloorToInt(per * (float)numSections), numSections - 1);

//    float u = per * (float)numSections - (float)currPt;
//    if (currPt < 0 || currPt > source.Length)
//    {
//        return Vector3.zero;
//    }
//    Vector3 a = source[currPt];
//    Vector3 b = source[currPt + 1];
//    Vector3 c = source[currPt + 2];
//    Vector3 d = source[currPt + 3];

//    return 0.5f * (
//        (-a + 3f * b - 3f * c + d) * (u * u * u)
//        + (2f * a - 5f * b + 4f * c - d) * (u * u)
//        + (-a + c) * u
//        + 2f * b
//    );
//}

        //public  Vector3 Velocity(Vector3[] source, float t)
        //{
        //    int numSections = source.Length - 3;
        //    int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
        //    float u = t * (float)numSections - (float)currPt;

        //    Vector3 a = source[currPt];
        //    Vector3 b = source[currPt + 1];
        //    Vector3 c = source[currPt + 2];
        //    Vector3 d = source[currPt + 3];

        //    return 1.5f * (-a + 3f * b - 3f * c + d) * (u * u)
        //            + (2f * a - 5f * b + 4f * c - d) * u
        //            + .5f * c - .5f * a;
        //}
