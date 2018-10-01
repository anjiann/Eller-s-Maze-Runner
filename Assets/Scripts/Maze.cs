using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] private int unitsPerCell = 4;
    [SerializeField] private int cellsPerRow = 8;
    [SerializeField] private int mazeBias = 20; //b/w 0 and 100

    [SerializeField] private GameObject mazeRow;
    [SerializeField] private GameObject sideWall;
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject newRowCollider;

    private int rowNumber;
    private float newRowPosZ;
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
            for (int i = 0; i < setsOfCells.Count; i++) {
                setsOfCells[i].Add(new Cell(rowNumber, i));
            }
            return;
        }

        for (int i = 0; i < setsOfCells.Count; i++) {
            int verticalConnections = 0;

            for (int j = 0; j < setsOfCells[i].Count; j++) {
                Cell oldCell = setsOfCells[i][j];
                Cell newCell = new Cell(rowNumber, oldCell.indexX);
                if (mazeBias < UnityEngine.Random.Range(0, 100)) {
                    //making a vertical connection and replacing the cell from the previous row with the cell from the new row
                    newCell.set = oldCell.set;
                    setsOfCells[i][j] = newCell;

                    verticalConnections++;
                }
                else if(verticalConnections > 0) {
                    //no vertical connection made, therefore add a wall in b/w current newCell and OldCell
                    //remove oldCell and add newCell to an empty set
                    newCell.addWall = true;
                    int x = FindEmptySetIndex();
                    newCell.set = x;
                    setsOfCells[x].Add(newCell);

                    setsOfCells[i].RemoveAt(j);
                }
                
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
        for(int i = 1; i < setsOfCells.Count; i++) {
            List<Cell> s1 = setsOfCells[i - 1];
            List<Cell> s2 = setsOfCells[i];
            if (mazeBias < UnityEngine.Random.Range(1, 100)) { //merge with neighbouring cell
                appendList(s2, s1);

                //empty the set
                s1.Clear();
            }
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

        //wall between rows
        
    
        //side walls
        int endBound = cellsPerRow * unitsPerCell / 2;
        pos.y = 2;
        pos.z = newRowPosZ + unitsPerCell / 2;
        //generate a side wall per set of cells and a front wall if the cell requires one
        for (int i = 0; i < setsOfCells.Count; i++) {
            int x = 0;
            //find the rightmost(east side or the +x axis) cell in the set
            for (int j = 0; j < setsOfCells[i].Count; j++) {
                int num = setsOfCells[i][j].indexX;
                x = (num > x) ? num : x;

                //front wall
                if (setsOfCells[i][j].addWall) {
                    float wallPosX = -endBound + unitsPerCell / 2 + setsOfCells[i][j].indexX * unitsPerCell;
                    Vector3 wallPos = new Vector3(wallPosX, 2, newRowPosZ);
                    Instantiate(frontWall, wallPos, Quaternion.identity);
                }
            }
            if (x != 0) { //if the set isn't empty
                pos.x = -endBound + x * unitsPerCell;
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
        pos.z = newRowPosZ + unitsPerCell;
        Instantiate(newRowCollider, pos, Quaternion.identity);
        FindObjectOfType<NewRowCollider>().maze = this;
    }
}