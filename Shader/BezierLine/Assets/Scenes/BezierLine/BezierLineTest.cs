using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierLineTest : MonoBehaviour {


    public Transform m_tMajor;

    public BezierLine m_cBeizerLine;

    float t = 0f;

    float speed = 0.3f;

	// Use this for initialization
	void Start () {
         m_tMajor.position = m_cBeizerLine.GetPoint(0f);
         m_tMajor.forward = m_cBeizerLine.GetDir(0f);
	}
	
	// Update is called once per frame
	void Update () {


        if (t >= 1f)
            t = 0f;

        Vector3 pos = m_cBeizerLine.GetPoint(t);

        /*
         * 
         * 1根据进度获取当前曲线点坐标，和当前点的方向
         * 
         * 2将点坐标和方向赋值给主角

         * */

        m_tMajor.position = pos;
        m_tMajor.forward = m_cBeizerLine.GetDir(t);
        t += Time.deltaTime * speed;

	}
}
