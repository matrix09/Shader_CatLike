using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BezierLine))]
public class BezierLineEditor : Editor {
    BezierLine curve;
    Transform curveTransform;
    int m_nSelectedIndex = -1;
    void OnEnable()
    {
        curve = target as BezierLine;
        curveTransform = curve.transform;
    }
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
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
      
    }

    Vector3 ShowPoint(int index)
    {
        /*
         * 将BeizerLine point[index]的坐标转换成世界坐标
         * 
         * 在Scene GUI中显示点的Handler
         * 
         * 在将可能发生改变的世界坐标转变成局部坐标 -> 
         * 保存到curve.points[index]中
         * 
         * 将新的世界坐标返回.
         * */

        Vector3 point = curveTransform.TransformPoint(curve.points[index]);
        
        if (Handles.Button(point, Quaternion.identity, 0.5f, 1f, Handles.DotHandleCap))
        {
            m_nSelectedIndex = index;
        }

        if (m_nSelectedIndex == index)
        {
            //检测在这个区域内，是否发生了改变
            EditorGUI.BeginChangeCheck();
            point = Handles.PositionHandle(point, Quaternion.identity);
            curve.AddForce(m_nSelectedIndex);
            //如果发生了改变，那么就返回true
            if (EditorGUI.EndChangeCheck())
            {
                curve.points[index] = curveTransform.InverseTransformPoint(point);
                //保存所有发生的改变.
                EditorUtility.SetDirty(curve);
            }
        }          
        return point;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button( "Add Points"))
        {
            //通过调用一个接口 -> 动态地增加points的长度
            curve.AddPoint();
        }



    }

}
