using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] private int unitsPerCell = 4;
    [SerializeField] private int cellsPerRow = 8;
    [SerializeField] private int mazeDepth = 8;
    [SerializeField] private int mazeBias = 80; //b/w 0 and 100

    [SerializeField] private GameObject mazeRow;
    [SerializeField] private GameObject boundaryWall;
    [SerializeField] private GameObject terminatingWall;
    [SerializeField] private GameObject sideWall;
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject newRowCollider;
    [SerializeField] private GameObject projectile;

    private Cell[,] maze = new Cell[8, 8]; //mxn matrix of Cells

    private int rowNumber;
    private float newRowPosZ;
    private Cell firstCell;
    private List<List<Cell>> setsOfCells; //each value in the dictionary is a connected set of tiles. the nested dictionary represents the zx coordinate

    private void Start() {
        rowNumber = -1; 

        setsOfCells = new List<List<Cell>>();
        for (int i = 0; i < cellsPerRow; i++) {
            setsOfCells.Add(new List<Cell>());
        }
    }

    public void GenerateRow(float colliderPosZ) {
        newRowPosZ = colliderPosZ;
        rowNumber++;
        ConnectVertical();
        FillRow();
        MergeCells();
        InstantiatePrefabs();
    }


    //nonempty sets must at least have one cell from the new row added to it. (vertical connection)
    private void ConnectVertical() {
        //first row
        if (rowNumber == 0) {
            for (int i = 0; i < setsOfCells.Count; i++) {
                Cell newCell = new Cell(rowNumber, i);
                newCell.set = i;

                setsOfCells[i].Add(newCell);
                maze[rowNumber, i] = newCell;
            }
            return;
        }

        //iterate through each set and add vertical connections
        for (int i = 0; i < setsOfCells.Count; i++) {
            int connection = UnityEngine.Random.Range(0, setsOfCells[i].Count); // choose a vertical connection

            for (int j = 0; j < setsOfCells[i].Count; j++) {
                Cell currentCell = setsOfCells[i][j];
                int indexX = currentCell.getX();
                maze[rowNumber, indexX] = new Cell(rowNumber, indexX);

                //if (connections == 0 || mazeBias > UnityEngine.Random.Range(0, 100)) {
                if(connection == j) { 
                    maze[rowNumber, indexX].set = currentCell.set;

                    setsOfCells[i][j] = maze[rowNumber, indexX];
                }
                else {
                    maze[rowNumber - 1, indexX].addFrontWall = true;

                    maze[rowNumber, indexX].set = -1;
                    //maze[rowNumber, indexX].isIsolated = true;

                    setsOfCells[i][j] = maze[rowNumber, indexX];
                }
            }
        }
    }

    private void FillRow() {

        for (int i = 0; i < setsOfCells.Count; i++) {
            for (int j = setsOfCells[i].Count - 1; j >= 0; j--) {
                if (setsOfCells[i][j].set == -1) {
                    setsOfCells[i].RemoveAt(j);
                }
            }
        }

        for(int i = 0; i < 8; i++) {
            Cell cell = maze[rowNumber, i];
            if (cell.set == -1) {
                int x = FindEmptySet();
                cell.set = x;
                setsOfCells[x].Add(cell);
            }
        }
    }

    //return the index of the first empty set in the list of sets, -1 if no empty set was found.
    private int FindEmptySet(){
        for (int i = 0; i < setsOfCells.Count; i++) {
            if (setsOfCells[i].Count == 0) return i;
        }
        return -1;
    }

    private void MergeCells() {

        for (int i = 0; i < cellsPerRow - 1; i++) {
            Cell currentCell = maze[rowNumber, i];
            print("the index of currentCell is " + currentCell.indexX);
            Cell rightCell = maze[rowNumber, i + 1];
            print("the index of rightCell is " + rightCell.indexX);
            //end of the maze, merge all cells into the same set
            if (rowNumber + 1 == mazeDepth && currentCell.set != rightCell.set) {
                removeFromSet(rightCell.indexX);

                rightCell.set = currentCell.set;
                setsOfCells[currentCell.set].Add(rightCell);
            } //otherwise merge adjacent cells randomly
            else if (mazeBias < UnityEngine.Random.Range(0, 100) && currentCell.set != rightCell.set) {
                removeFromSet(rightCell.indexX);

                rightCell.set = currentCell.set;
                setsOfCells[currentCell.set].Add(rightCell);
            }
            else {
                maze[rowNumber, i].addRightWall = true;
            }
        }
    }

    private void removeFromSet(int cellIndexX) {
        for (int i = 0; i < setsOfCells.Count; i++) {
            for (int j = 0; j < setsOfCells[i].Count; j++) {
                if (setsOfCells[i][j].indexX == cellIndexX) {
                    setsOfCells[i].RemoveAt(j);
                    return;
                }
            }
        }
        return;
    }

    private void InstantiatePrefabs() {
        int endBound = cellsPerRow * unitsPerCell / 2;

        //populate first row with projectiles
        Vector3 pos = new Vector3(-endBound + unitsPerCell / 2, 1, newRowPosZ + unitsPerCell / 2);
        if (rowNumber == 0) {
            for(int i = 0; i < cellsPerRow; i++) {
                pos.x = -endBound + unitsPerCell / 2 + i * unitsPerCell;
                Instantiate(projectile, pos, Quaternion.identity);
            }
        }

        //ground
        pos = new Vector3(0, 0, newRowPosZ + unitsPerCell / 2);
        Instantiate(mazeRow, pos, Quaternion.identity);

        //walls
        pos.y = 2;
        pos.z = newRowPosZ + unitsPerCell / 2;

        for (int i = 0; i < cellsPerRow; i++) {
            //print(maze[rowNumber, i].addRightWall);
            if(maze[rowNumber, i].addRightWall) {
                pos.x = -endBound + unitsPerCell + unitsPerCell * maze[rowNumber, i].getX();
                Instantiate(sideWall, pos, Quaternion.identity);
            }
            if (rowNumber != 0 && maze[rowNumber - 1, i].addFrontWall) {
                Vector3 wallPos = new Vector3(-endBound + unitsPerCell / 2 + unitsPerCell * maze[rowNumber - 1, i].indexX, 2, newRowPosZ);
                Instantiate(frontWall, wallPos, Quaternion.identity);
            }
        }

        //generate leftmost and rightmost wall
        pos.x = -endBound;
        Instantiate(boundaryWall, pos, Quaternion.identity);
        pos.x = endBound;
        Instantiate(boundaryWall, pos, Quaternion.identity);

        //generate terminating wall or a new row collider
        pos.x = 0;
        pos.y = 2;
        pos.z = newRowPosZ + unitsPerCell;
        if (rowNumber + 1 == mazeDepth) {
            Instantiate(terminatingWall, pos, Quaternion.identity);
        }
        else {
            Instantiate(newRowCollider, pos, Quaternion.identity);
            FindObjectOfType<NewRowCollider>().maze = this;
        }
    }

    ////for testing
    //private void printIndices() {
    //    print("-------printing indices -----");

    //    string s = "";
    //    for (int i = 0; i < setsOfCells.Count; i++) {
    //        for (int j = 0; j < setsOfCells[i].Count; j++) {
    //            s = s + setsOfCells[i][j].indexX + " ";
    //        }
    //    }
    //    Debug.Log(s);
    //    print("-----------------------------");
    //}

    ////for testing
    //private void printSets() {
    //    print("------printing maze row------");
    //    string row = "";
    //    for (int j = 0; j < 8; j++) {
    //        row = row + maze[rowNumber, j].set + " ";
    //    }
    //    Debug.Log(row);
    //    print("-------------------------");
    //}

}