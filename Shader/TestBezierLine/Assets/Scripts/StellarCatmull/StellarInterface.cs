using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarInterface {

    public static Vector3 Interp(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = p0;
        Vector3 b = p1;
        Vector3 c = p2;
        Vector3 d = p3;

        float u = t;

        return 0.5f * (
           (-a + 3f * b - 3f * c + d) * (u * u * u)
           + (2f * a - 5f * b + 4f * c - d) * (u * u)
           + (-a + c) * u
           + 2f * b
       );

    }

    public static Vector3 Velocity(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = p0;
        Vector3 b = p1;
        Vector3 c = p2;
        Vector3 d = p3;

        float u = t;

        return 1.5f * (-a + 3f * b - 3f * c + d) * (u * u)
              + (2f * a - 5f * b + 4f * c - d) * u
              + .5f * c - .5f * a;

    }

}
