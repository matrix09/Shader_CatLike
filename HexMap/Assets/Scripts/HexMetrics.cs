using UnityEngine;

public static class HexMetrics {

	public const float outerRadius = 10f;

	public const float innerRadius = outerRadius * 0.866025404f;

    public const float SolidFactor = 0.75f;

    public const float BlendFactor = 1f - SolidFactor;

	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};


    /// <summary>
    /// get distance between v1 and v3
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetBridge(HexDir dir)
    {
        return (corners[(int)dir] + corners[(int)dir + 1]) * 0.5f * BlendFactor;
    }


    public static Vector3 GetFirstDirCorner(HexDir dir)
    {
        return corners[(int)dir];
    }

    public static Vector3 GetSecDirCorner(HexDir dir)
    {
        return corners[(int)dir + 1];
    }

    public static Vector3 GetFirstSolidDirCorner(HexDir dir)
    {
        return corners[(int)dir] * SolidFactor;
    }

    public static Vector3 GetSecSolidDirCorner(HexDir dir)
    {
        return corners[(int)dir + 1] * SolidFactor;
    }





}