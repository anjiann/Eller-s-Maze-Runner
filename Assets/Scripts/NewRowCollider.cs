using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRowCollider : MonoBehaviour {
    [SerializeField] public Maze maze;

    private void OnTriggerEnter(Collider other) {
        maze.GenerateRow(transform.position.z);
        Destroy(gameObject);
    }
}
