using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{   
    public float m_fSpeed;
    public Vector3 m_vDir;
    public Vector3 m_vOffSet;
    public Transform m_tPlayer;


    Vector3 vTmp;
    void LateUpdate()
    {
        //相机位置=主角位置+offset
        vTmp = m_tPlayer.position + m_vOffSet;
        transform.position = Vector3.Lerp(transform.position, vTmp, m_fSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(m_vDir), m_fSpeed * Time.deltaTime);

    }
}
