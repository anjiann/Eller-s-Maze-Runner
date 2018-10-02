using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public bool addFrontWall = false;
    public bool addRightWall = false;
    public bool isIsolated;
    public int set;
    public int indexX;
    public int indexZ;


    public Cell(int z, int x) {
        indexZ = z;
        indexX = x;
    }

    public int getX() {
        return indexX;
    }

    public Cell Clone() {
        Cell cell = new Cell(indexZ, indexX);
        cell.addFrontWall = addFrontWall;
        cell.addRightWall = addRightWall;
        cell.isIsolated = isIsolated;
        cell.set = set;

        return cell;
    }
}
