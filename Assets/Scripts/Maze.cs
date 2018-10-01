using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] private float unitsPerGridCell = 4;
    [SerializeField] private int cellsPerMazeRow = 8;


    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject mazeRow;
    [SerializeField] private GameObject sideWall;
    [SerializeField] private GameObject frontWall;

    //linked list each node containing a row stored as float array of all the cell positions of that row
    // when collider hits and a new row is created, a new node on linked list is created
    // theres a hash table with each index key as the set
    // the extended cell is added to hash table with the same index key as its parent
    // set merge grabs to lists from different keys in the has table and appends them
    // add all the 

    private int numOfRowsCreated;
    private float newRowPosZ;
    private float[] positionXOfCells; //indexing starts on the west side of the max
    private ArrayList cellMatrix; //(0,0) at the maze entrance and (m, n) where m is the maze row, n is the cell in the row
    private Dictionary<int, List<Tuple>> setsOfCells; //each value in the dictionary is a connected set of tiles. the nested dictionary represents the zx coordinate

    private void Start() {
        numOfRowsCreated = 0;

        positionXOfCells = new float[cellsPerMazeRow];
        float cellPos = -cellsPerMazeRow + unitsPerGridCell / 2;
        for (int i = 0; i < cellsPerMazeRow; i++, cellPos += unitsPerGridCell) {
            positionXOfCells[i] = cellPos;
        }

        setsOfCells = new Dictionary<int, List<Tuple>>();
        for (int i = 0; i < cellsPerMazeRow; i++) {
            setsOfCells.Add(i, new List<Tuple>() {new Tuple(numOfRowsCreated, i)});
        }
    }

    public void GenerateRow(float colliderPosZ) {
        numOfRowsCreated++;
        newRowPosZ = colliderPosZ;

        AssignCellsToSets();
        MergeCells();
        AddVerticalConnections();
        FillRowWithCells();
        InstantiatePrefabs();
    }

    private void InstantiatePrefabs() {
        //ground
        Vector3 pos = new Vector3(0, 0, newRowPosZ + unitsPerGridCell / 2);
        Instantiate(ground, pos, Quaternion.identity);
    }

    private void AssignCellsToSets() {

    }

    private void MergeCells() {

    }

    private void AddVerticalConnections() {

    }

    private void FillRowWithCells() {

    }
}