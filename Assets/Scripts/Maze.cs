using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] private int unitsPerCell = 4;
    [SerializeField] private int cellsPerRow = 8;
    [SerializeField] private int mazeBias = 50; //b/w 0 and 100

    [SerializeField] private GameObject mazeRow;
    [SerializeField] private GameObject sideWall;
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject newRowCollider;

    private int rowNumber;
    private float newRowPosZ;
    private Cell firstCell;
    private List<List<Cell>> setsOfCells; //each value in the dictionary is a connected set of tiles. the nested dictionary represents the zx coordinate

    private void Start() {
        rowNumber = 0;

        setsOfCells = new List<List<Cell>>();
        for (int i = 0; i < cellsPerRow; i++) {
            setsOfCells.Add(new List<Cell>());
        }
    }

    public void GenerateRow(float colliderPosZ) {
        newRowPosZ = colliderPosZ;

        AddNewCellsToSets();
        MergeCells();
        print("\nrow number is " + rowNumber);
        for (int i = 0; i < setsOfCells.Count; i++) {
            //for (int j = 0; j < setsOfCells[i].Count; j++) {
            //    print("Inside set " + i + ": " + setsOfCells[i][j].indexX);
            //}
            print("set " + i + " has " + setsOfCells[i].Count + " elements");
        }
        InstantiatePrefabs();

        rowNumber++;
    }


    //nonempty sets must at least have one cell from the new row added to it. (vertical connection)
    private void AddNewCellsToSets() {
        //first row
        if (rowNumber == 0) {
            firstCell = new Cell(rowNumber, 0);
            firstCell.set = 0;
            setsOfCells[0].Add(firstCell);
            for (int i = 1; i < setsOfCells.Count; i++) {
                Cell newCell = new Cell(rowNumber, i);
                newCell.set = i;
                setsOfCells[i].Add(newCell);
                setsOfCells[i][0].leftCell = setsOfCells[i - 1][0];
                setsOfCells[i - 1][0].rightCell = setsOfCells[i][0];

            }
            return;
        }

        //add vertical connections
        Cell prevCell = null;
        for (int i = 0; i < setsOfCells.Count; i++) {
            int verticalConnections = 0;

            for (int j = 0; j < setsOfCells[i].Count; j++) {
                Cell oldCell = setsOfCells[i][j];
                Cell newCell = new Cell(rowNumber, oldCell.indexX);

                if (verticalConnections == 0) {
                    //making a vertical connection
                    newCell.set = oldCell.set;
                    setsOfCells[i][j] = newCell;

                    verticalConnections++;
                }
                else {
                    //no vertical connection made, therefore add a wall in b/w current newCell and OldCell
                    //remove oldCell and add newCell to an empty set
                    
                    newCell.addFrontWall = true;
                    int x = FindEmptySetIndex();
                    newCell.set = x;
                    setsOfCells[x].Add(newCell);

                    setsOfCells[i].RemoveAt(j);
                }

                if (newCell.indexX == 0) {
                    firstCell = newCell;
                }
                else {
                    prevCell.rightCell = newCell;
                }

                newCell.leftCell = prevCell;
                prevCell = newCell;
            }
        }
    }

    private int FindEmptySetIndex() {
        for(int i = 0; i < setsOfCells.Count; i++) {
            if (setsOfCells[i].Count == 0) return i;
        }
        return 1;
    }

    private void MergeCells() {

        Cell cell = firstCell;
        while (cell.indexX != 7) {
            //print("cell index is " + cell.indexX);
            //float num = UnityEngine.Random.Range(0, 100);
            //print(mazeBias < num && cell.rightCell.set != cell.set);
            //print("right cell index is " + cell.rightCell.indexX);
            //print("mazeBias is " + mazeBias + " ... random is " + num);
            //print("my set is " + cell.set + "... my right cell set is " + cell.rightCell.set);
            if (mazeBias < UnityEngine.Random.Range(0, 100) && cell.rightCell.set != cell.set) {//merge
                int s = cell.rightCell.set;
                setsOfCells[cell.set].Add(cell.rightCell);
                setsOfCells[cell.rightCell.set].Remove(cell.rightCell);

                cell.rightCell.set = cell.set;
            }
            cell = cell.rightCell;
        }
    }

    //appends l2 onto l1
    private void appendList(List<Cell> l1, List<Cell> l2) {
        for (int i = 0; i < l2.Count; i++) {
            l1.Add(l2[i]);
        }
    }

    

    private void InstantiatePrefabs() {
        //ground
        Vector3 pos = new Vector3(0, 0, newRowPosZ + unitsPerCell / 2);
        Instantiate(mazeRow, pos, Quaternion.identity);

        //walls
        int endBound = cellsPerRow * unitsPerCell / 2;
        pos.y = 2;
        pos.z = newRowPosZ + unitsPerCell / 2;

        Cell cell = firstCell;
        while (cell.indexX != 7) {
            if (cell.rightCell.set != cell.set) { // side wall
                pos.x = -endBound + unitsPerCell + cell.indexX * unitsPerCell;
                Instantiate(sideWall, pos, Quaternion.identity);
            }
            if (cell.addFrontWall) {
                float posX = -endBound + unitsPerCell / 2 + cell.indexX * unitsPerCell;
                Instantiate(frontWall, new Vector3(posX, 2, newRowPosZ), Quaternion.identity);
            }
            print("cell " + cell.indexX + "value of addFrontWall is " + cell.addFrontWall);

            cell = cell.rightCell;
        }

        //generate leftmost and rightmost wall
        pos.x = -endBound;
        Instantiate(sideWall, pos, Quaternion.identity);
        pos.x = endBound;
        Instantiate(sideWall, pos, Quaternion.identity);

        //generate collider
        pos.x = 0;
        pos.y = 0;
        pos.z = newRowPosZ + unitsPerCell;
        Instantiate(newRowCollider, pos, Quaternion.identity);
        FindObjectOfType<NewRowCollider>().maze = this;
    }
}