using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] private float unitsPerGridCell = 4;
    [SerializeField] private float cellsPerMazeRow = 8;

    [SerializeField] private GameObject mazeRow;
    [SerializeField] private GameObject sideWall;
    [SerializeField] private GameObject frontWall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float getUnitsPerGridCell() {
        return unitsPerGridCell;
    }
}
