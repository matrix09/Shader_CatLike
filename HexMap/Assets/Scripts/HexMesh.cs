using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh hexMesh;
	List<Vector3> vertices;
	List<Color> colors;
	List<int> triangles;

	MeshCollider meshCollider;

	void Awake () {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		colors = new List<Color>();
		triangles = new List<int>();
	}

	public void Triangulate (HexCell[] cells) {
		hexMesh.Clear();
		vertices.Clear();
		colors.Clear();
		triangles.Clear();
		for (int i = 0; i < cells.Length; i++) {
			Triangulate(cells[i]);
		}
		hexMesh.vertices = vertices.ToArray();
		hexMesh.colors = colors.ToArray();
		hexMesh.triangles = triangles.ToArray();
		hexMesh.RecalculateNormals();
		meshCollider.sharedMesh = hexMesh;
	}

	void Triangulate (HexCell cell) {
		Vector3 center = cell.transform.localPosition;
		for (HexDir i = 0; i < HexDir.Size; i++) {
			AddTriangle(
				center,
				center + HexMetrics.GetFirstDirCorner(i),
				center + HexMetrics.GetSecDirCorner(i)
			);

            //get cell neighbor
            HexCell neighbor = cell.GetHexNeighbour(i) ?? cell;// neighbor == null ? cell : neighbor;;
            HexCell preneighbor = cell.GetHexNeighbour(HexDirectionExtensions.GetPreviousDir(i)) ?? cell;
            HexCell nexneighbor = cell.GetHexNeighbour(HexDirectionExtensions.GetNextDir(i)) ?? cell;

            //set triangle color
            AddTriangleColor(cell.color, (neighbor.color + cell.color + preneighbor.color) / 3f, (neighbor.color + cell.color + nexneighbor.color) / 3f);
		}   
	}

	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

    void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

}