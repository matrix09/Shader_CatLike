using UnityEngine;

public static class HexMetrics {

	public const float outerRadius = 10f;

	public const float innerRadius = outerRadius * 0.866025404f;

	public const float solidFactor = 0.75f;

	public const float blendFactor = 1f - solidFactor;

	static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};

    public const float ElevationStpe = 5f;                  //提高高度的步长

    public const int TerraceFlat = 2;                          //平坡的数目

    public const int TerraceSlopeTotalSteps = TerraceFlat * 2 + 1;      //A - B斜坡总个数

    public const float horizontalTerraceStepSize = 1f / TerraceSlopeTotalSteps;//水平方向移动步长

    public const float verticalTerraceStepSize = 1f / (TerraceFlat + 1);//垂直方向移动步长

	public static Vector3 GetFirstCorner (HexDirection direction) {
		return corners[(int)direction];
	}

	public static Vector3 GetSecondCorner (HexDirection direction) {
		return corners[(int)direction + 1];
	}

	public static Vector3 GetFirstSolidCorner (HexDirection direction) {
		return corners[(int)direction] * solidFactor;
	}

	public static Vector3 GetSecondSolidCorner (HexDirection direction) {
		return corners[(int)direction + 1] * solidFactor;
	}

	public static Vector3 GetBridge (HexDirection direction) {
		return (corners[(int)direction] + corners[(int)direction + 1]) *
			blendFactor;
	}

    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)//rectangle lerp function
    {
        float h = step * horizontalTerraceStepSize;//5次

        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;

        float v = ((step + 1) / 2) * verticalTerraceStepSize;//3次
        a.y +=  ((b.y - a.y) * v);

        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step)
    {
        float h = step * horizontalTerraceStepSize;

        return Color.Lerp(a, b, h);
    }


}