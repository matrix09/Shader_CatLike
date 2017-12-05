using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Stellar;
[CustomEditor(typeof(Stellar))]
public class StellarRouteEditor : Editor {


    Stellar stellar;
    Transform transStellar;
    int m_nSelectedIndex = -1;
    float Size = 0.05f;
    float PickedSize = 0.1f;

    int DrawPieces = 60;

    float scaleDir = 0.5f;

    void OnEnable()
    {
        stellar = target as Stellar;
        transStellar  = stellar.transform;
    }

    void OnSceneGUI()
    {
        Vector3 p0 = ShowPoint(0);
        Vector3 p1, p2;
        for (int i = 1; i < stellar.Points.Length - 1; i+=3)
        {
            p1 = ShowPoint(i);
            p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);
            DrawStellarCurve(p0, p1, p2, p3);

            Handles.color = Color.gray;
            //(绘制p0 - p2) == (p1的dir)
            Handles.DrawLine(p0, p2);
            Handles.color = Color.blue;
            Handles.DrawLine(p1, p1 + scaleDir * stellar.GetDir(0f));
            //(绘制pPreLast2 - pLast) == (pPreLast的dir)
            Handles.color = Color.gray;
            Handles.DrawLine(p1, p3);
            Handles.color = Color.blue;
            Handles.DrawLine(p2, p2 + scaleDir * stellar.GetDir(1f));

            p0 = p3;
        }
    }

    Vector3 ShowPoint(int index)
    {

        Vector3 point = transStellar.TransformPoint (stellar.Points[index]);            //获取点的世界坐标

        if (index == 0 || index == stellar.Points.Length - 1)
            Handles.color = Color.red;
        else
            Handles.color = Color.white;
        if (Handles.Button(point, Quaternion.identity, Size, PickedSize, Handles.DotHandleCap))
        {
            m_nSelectedIndex = index;
        }

        if (m_nSelectedIndex == index)
        {

            EditorGUI.BeginChangeCheck();

            point = Handles.PositionHandle(point, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(stellar, "Stellar");
                stellar.Points[index] = transStellar.InverseTransformPoint(point);
                EditorUtility.SetDirty(stellar);
            }

        }

        Handles.Label(point, "P" + index.ToString());


        return point;         
  
    }

    void DrawStellarCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {

        float t = 0f;
        Vector3 Pre = StellarInterface.Interp(p0, p1, p2, p3, 0);
        for (int i = 1; i < DrawPieces; i++)
        {
            t = i / 60f;
            Vector3 Cur = StellarInterface.Interp(p0, p1, p2, p3, t);
            Handles.DrawLine(Pre, Cur);
            Pre = Cur;
        }

    }

}


/*
 * 
区别 : stellar                    Bezier
 * 只绘制中间两个点 vs 绘制四个点
 * p1 dir = (p2 - p0)           p0 dir = p1 dir
 * 
*/