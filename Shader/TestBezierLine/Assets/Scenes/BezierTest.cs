using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTest : MonoBehaviour {

    public Transform m_tMajor;

    public BezierLine m_cBezierLine;

    public float m_fSpeed;

    float t;
	
	// Update is called once per frame
	void Update () {
        if (t >= 1f)
            t = 0f;
        m_tMajor.position = m_cBezierLine.GetPoint(t);
        m_tMajor.forward = m_cBezierLine.GetDir(t);
        t += Time.deltaTime * m_cBezierLine.GetDeltaT(t);
	}   
}

