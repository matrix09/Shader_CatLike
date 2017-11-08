using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public Color color;


    [SerializeField]
    HexCell[] Neighbours;


    public HexCell GetHexNeighbour(HexDir dir)
    {
        return Neighbours[(int)dir];
    }

    public void SetHexNeighbour(HexDir dir, HexCell cell)
    {
        Neighbours[(int)dir] = cell;
        cell.Neighbours[(int)dir.Opposite()] = this;
    }

}