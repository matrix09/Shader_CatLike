
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierLine))]
public class BezierCurveEditor : Editor
{

    BezierLine curve;
    Transform curveTransform;

    const int lineSteps = 10;
    const float directionScale = 0.5f;

    float m_fSelectedIndex = -1;

    float size = 0.05f;
    float pickSize = 0.1f;


    void OnEnable()
    {
        curve = target as BezierLine;
        curveTransform = curve.transform;
    }

    /// <summary>
    /// 重载Scene UI
    /// </summary>
    void OnSceneGUI()
    {

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < curve.points.Length; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);
            Handles.color = Color.white;
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
       

    }

    /// <summary>
    /// 获取指定index点的世界坐标
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Vector3 ShowPoint(int index)
    {
        Vector3 point = curveTransform.TransformPoint(curve.points[index]);


        if (Handles.Button(point, Quaternion.identity, size, pickSize, Handles.DotHandleCap))
        {
            m_fSelectedIndex = index;
        }

        if (m_fSelectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();

            point = Handles.DoPositionHandle(point, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curveTransform, "Move Point");
                EditorUtility.SetDirty(curve);
                curve.points[index] = curveTransform.InverseTransformPoint(point);
            }
        }




        return point;

    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Points"))
        {
            //add called function
            curve.AddPoints();
        }

    }
    /*
     * 1 : 在Inspector 中添加一个按钮
     * 2 ： 声明定义添加顶点接口
     * 3 ： 绘制新添加的顶点
     * 
     * */




}
