using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public Cell leftCell = null;
    public Cell rightCell = null;

    public bool addFrontWall = false;
    public int indexX;
    public int indexZ;
    public int set;

    public Cell(int z, int x) {
        indexZ = z;
        indexX = x;
    }

    public bool IsLast() {
        if (rightCell == null) return true;
        return false;
    }
}
