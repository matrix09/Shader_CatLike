using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Stellar
{
    public class PathPoint : MonoBehaviour
    {

        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
         
            Gizmos.DrawWireSphere(transform.position, .25f);
        }
    }

}
