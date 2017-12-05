using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Stellar
{
    public class RouteManager : MonoBehaviour
    {

        public Transform m_tMajor;

        public Transform m_tMajorPathPoints;

        public float m_fSpeed = 2f;

        Vector3[] PathPoints;
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
            m_tMajor.transform.position = Stellar.StellarInst.Interp(PathPoints, m_fCurPer);
            //读取曲线方向.
            m_tMajor.transform.forward = Stellar.StellarInst.GetDir(PathPoints, m_fCurPer);
            //增加进度.
            m_fCurPer += m_fSpeed * Time.deltaTime;
        }

        void OnDrawGizmos()
        {
            if(PathPoints == null || PathPoints.Length == 0)
                PathPoints = Stellar.StellarInst.InitializePathPoints(m_tMajorPathPoints);          //初始化路线

            Stellar.StellarInst.GizmoDraw(PathPoints, m_fCurPer);
        }

    }

}
