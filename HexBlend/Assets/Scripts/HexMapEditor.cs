﻿using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;

    int ActiveElevation;

	void Awake () {
		SelectColor(0);
	}

	void Update () {
		if (
			Input.GetMouseButton(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
            EditCell(hexGrid.GetCell (hit.point));
			//hexGrid.ColorCell(hit.point, activeColor);
		}
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}

    public void SetElevation(float elevation)
    {
        ActiveElevation = (int)elevation;
    }

    void EditCell(HexCell cell)
    {
        cell.color = activeColor;
        cell.Elevation = ActiveElevation;
        hexGrid.Refresh();
    }


}