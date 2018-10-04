using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour {
    [SerializeField] private int health;

    private void Start() {
        health = 3;
    }

    public void decreaseHealth() {
        health--;
        if (health <= 0) Destroy(gameObject);
    }
}
