using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public Color color;

	[SerializeField]
	HexCell[] neighbors;

    public RectTransform uiRect;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            elevation = value;

            Vector3 pos = transform.localPosition;
            pos.y = value * HexMetrics.ElevationStpe;
            transform.localPosition = pos;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.ElevationStpe;
            uiRect.localPosition = uiPosition;
        }
    }
    int elevation;

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}
}