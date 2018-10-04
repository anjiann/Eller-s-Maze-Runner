using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "New Row Collider(Clone)") Destroy(gameObject, 5);
        else if (collision.gameObject.name == "Side Wall(Clone)" || 
            collision.gameObject.name == "Front Wall(Clone)") {
            collision.gameObject.GetComponent<WallHealth>().decreaseHealth();
        }

        Destroy(gameObject);
    }
}
