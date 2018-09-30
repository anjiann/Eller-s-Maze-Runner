using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRowCollider : MonoBehaviour {
    [SerializeField] private GameObject ground;

    private void OnTriggerEnter(Collider other) {
        print("a collision");
        Vector3 pos = new Vector3(0, 0, transform.position.z + 2);
        Instantiate(ground, pos, Quaternion.identity);
    }
}
