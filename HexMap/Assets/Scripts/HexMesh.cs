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

            Vector3 v1 = center + HexMetrics.GetFirstSolidDirCorner(i);
            Vector3 v2 = center + HexMetrics.GetSecSolidDirCorner(i);

			AddTriangle(
				center,
				 v1,
				 v2
			);

            AddTriangleColor(cell.color);

            if (i <= HexDir.SE)
                TriangulateConnection(i, cell, v1, v2);
            //Vector3 v3 = center + HexMetrics.GetFirstDirCorner(i);
            //Vector3 v4 = center + HexMetrics.GetSecDirCorner(i);

            //Vector3 v3 = v1 + HexMetrics.GetBridge(i);
            //Vector3 v4 = v2 + HexMetrics.GetBridge(i);


            //AddQuad(v1, v2, v3, v4);

            ////get cell neighbor
            //HexCell neighbor = cell.GetHexNeighbour(i) ?? cell;// neighbor == null ? cell : neighbor;;

            ////set triangle color
            ////AddQuadColor(cell.color, cell.color, (neighbor.color + cell.color + preneighbor.color) / 3f, (neighbor.color + cell.color + nexneighbor.color) / 3f);
            //AddQuadColor(cell.color, (cell.color + neighbor.color) * 0.5f);
            

            ////----------------------------Filling the gaps
            //HexCell preneighbor = cell.GetHexNeighbour(HexDirectionExtensions.GetPreviousDir(i)) ?? cell;
            //HexCell nexneighbor = cell.GetHexNeighbour(HexDirectionExtensions.GetNextDir(i)) ?? cell;
            //AddTriangle(v1, center + HexMetrics.GetFirstDirCorner(i), v3);
            //AddTriangleColor(cell.color, (cell.color + preneighbor.color + neighbor.color) / 3f, (cell.color + neighbor.color) * 0.5f);

            //AddTriangle(v2, v4, center + HexMetrics.GetSecDirCorner(i));
            //AddTriangleColor(cell.color, (cell.color + neighbor.color) * 0.5f, (cell.color + nexneighbor.color + neighbor.color) / 3f);
            ////----------------------------Filling the gaps
		}   
	}


    void TriangulateConnection(HexDir dir, HexCell cell, Vector3 v1, Vector3 v2)
    {

        HexCell Neighbor = cell.GetHexNeighbour(dir);
        if (null == Neighbor)
            return;
        //add rectangle
        Vector3 v3 = v1 + HexMetrics.GetBridge(dir);
        Vector3 v4 = v2 + HexMetrics.GetBridge(dir);
        AddQuad(v1, v2, v3, v4);

      
        AddQuadColor(cell.color, Neighbor.color);

        HexCell nextNeighbor = cell.GetHexNeighbour(HexDirectionExtensions.GetNextDir(dir));
        
        if (null != nextNeighbor)
        {
            AddTriangle(v2, v4, v2 + HexMetrics.GetBridge(HexDirectionExtensions.GetNextDir(dir)));
            AddTriangleColor(cell.color, Neighbor.color, nextNeighbor.color);
        }


    }

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }

    void AddQuadColor(Color c1, Color c2, Color c3, Color c4) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }


    void AddQuadColor(Color c1, Color c2)
    {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);

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