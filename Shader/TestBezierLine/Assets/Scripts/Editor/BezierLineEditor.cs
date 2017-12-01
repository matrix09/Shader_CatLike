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
        Vector3 p0 =  ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);


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

}
