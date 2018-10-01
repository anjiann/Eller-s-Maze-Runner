using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public Cell nextCell;

    public bool addWall = false;
    public int indexX;
    public int indexZ;
    public int set;

    public Cell(int z, int x) {
        indexZ = z;
        indexX = x;
    }
}
