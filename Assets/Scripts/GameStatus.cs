using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour {
    [SerializeField] private int numOfProjectiles = 0;
    [SerializeField] Text numOfBulletsText;

    private void Start() {
        
    }

    public int getNumProjectiles() {
        return numOfProjectiles;
    }

    public void AddProjectile() {
        numOfProjectiles++;
        numOfBulletsText.text = "bullets: " + numOfProjectiles.ToString();
    }

    public void MinusProjectile() {
        numOfProjectiles--;
        numOfBulletsText.text = "bullets: " + numOfProjectiles.ToString();
    }
}
