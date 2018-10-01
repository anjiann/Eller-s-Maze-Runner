using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] private int unitsPerGridCell = 4;
    [SerializeField] private int cellsPerMazeRow = 8;
    [SerializeField] private int mazeBias = 100; //b/w 0 and 100

    [SerializeField] private GameObject mazeRow;
    [SerializeField] private GameObject sideWall;
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject newRowCollider;

    private int rowNumber;
    private float newRowPosZ;
    private ArrayList cellMatrix; //(0,0) at the maze entrance and (m, n) where m is the maze row, n is the cell in the row
    private Dictionary<int, List<Cell>> setsOfCells; //each value in the dictionary is a connected set of tiles. the nested dictionary represents the zx coordinate

    private void Start() {
        rowNumber = 0;

        setsOfCells = new Dictionary<int, List<Cell>>();
    }

    public void GenerateRow(float colliderPosZ) {
        newRowPosZ = colliderPosZ;

        AssignCellsToSets();
        MergeCells();
        for (int i = 0; i < setsOfCells.Count; i++) {
            for (int j = 0; j < setsOfCells[i].Count; j++) {
                print("Inside set " + i + ": " + setsOfCells[i][j].getXCoord());
            }
        }
        AddVerticalConnections();
        FillRowWithCells();
        InstantiatePrefabs();

        rowNumber++;
    }



    private void AssignCellsToSets() {
        for (int i = 0; i < cellsPerMazeRow; i++) {
            setsOfCells.Add(i, new List<Cell>() { new Tuple(rowNumber, i) });
        }
    }

    private void MergeCells() {
        if (rowNumber == 0) return;

        for(int i = 1; i < setsOfCells.Count; i++) {
            if (mazeBias < UnityEngine.Random.Range(1, 100)) { //merge with neighbouring cell
                appendList(setsOfCells[i], setsOfCells[i - 1]);

                //empty the set
                setsOfCells.Remove(i - 1);
                setsOfCells.Add(i - 1, new List<Cell>());
            }
        }
    }

    //appends l2 onto l1
    private void appendList(List<Cell> l1, List<Cell> l2) {
        for (int i = 0; i < l2.Count; i++) {
            l1.Add(l2[i]);
        }
    }

    private void AddVerticalConnections() {

    }

    private void FillRowWithCells() {

    }

    private void InstantiatePrefabs() {
        //ground
        Vector3 pos = new Vector3(0, 0, newRowPosZ + unitsPerGridCell / 2);
        Instantiate(mazeRow, pos, Quaternion.identity);

        //side walls
        int endBound = cellsPerMazeRow * unitsPerGridCell / 2;
        pos.y = 2;
        pos.z = newRowPosZ + unitsPerGridCell / 2;
        //generate a side wall per set of cells
        for (int i = 0; i < setsOfCells.Count; i++) {
            int x = 0;
            //find the rightmost(east side or the +x axis) cell in the set
            for (int j = 0; j < setsOfCells[i].Count; j++) {
                int num = setsOfCells[i][j].getXCoord();
                x = (num > x) ? num : x;
            }
            if (x != 0) { //if the set isn't empty
                pos.x = -endBound + x * unitsPerGridCell;
                Instantiate(sideWall, pos, Quaternion.identity);
            }
        }
        //generate leftmost and rightmost wall
        pos.x = -endBound;
        Instantiate(sideWall, pos, Quaternion.identity);
        pos.x = endBound;
        Instantiate(sideWall, pos, Quaternion.identity);

        //generate collider
        pos.x = 0;
        pos.y = 0;
        pos.z = newRowPosZ + unitsPerGridCell;
        Instantiate(newRowCollider, pos, Quaternion.identity);
        FindObjectOfType<NewRowCollider>().maze = this;
    }
}