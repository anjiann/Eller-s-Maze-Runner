using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        SceneManager.LoadScene("Win Screen");
    }
}
