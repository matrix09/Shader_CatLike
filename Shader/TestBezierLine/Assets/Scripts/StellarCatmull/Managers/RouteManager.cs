using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Stellar
{
    public class RouteManager : MonoBehaviour
    {

        public Transform m_tMajor;

        public float m_fSpeed = 2f;

        public Stellar m_cStellar;

        float m_fCurPer = 0f;
        // Use this for initialization
        void Start()
        {
            m_tMajor.transform.position = transform.position;
            m_tMajor.transform.forward = transform.forward;

        }

        // Update is called once per frame
        void Update()
        {

            //判断进度是否超过1，如果超过那么直接归0.
            if (m_fCurPer >= 1f)
                m_fCurPer = 0f;

            //读取曲线位置.
            m_tMajor.transform.position = m_cStellar.Interp(m_fCurPer);
            //读取曲线方向.
            m_tMajor.transform.forward = m_cStellar.GetDir(m_fCurPer);
            //增加进度.
            m_fCurPer += m_fSpeed * Time.deltaTime;
        }
    }

}


//void OnDrawGizmos()
//{
//    if(PathPoints == null || PathPoints.Length == 0)
//        PathPoints = Stellar.StellarInst.InitializePathPoints(m_tMajorPathPoints);          //初始化路线
//    Stellar.StellarInst.GizmoDraw(PathPoints, m_fCurPer);
//}