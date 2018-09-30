using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {
    [SerializeField] private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotationY = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;


    [SerializeField] private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    public void Move (Vector3 _velocity) {
        velocity = _velocity;
    }

    public void RotateY(Vector3 _rotationY) {
        rotationY = _rotationY;
    }

    public void RotateX(Vector3 _rotationX) {
        cameraRotation = _rotationX;
    }

    public void Jump(float jumpForce) {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //run every physics iteration
    private void FixedUpdate() {
        PerformMovement();
        PerformRotation();
    }

    //perform movement based on velocity variable
    private void PerformMovement() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationY));
        if (cam != null) {
            cam.transform.Rotate(-cameraRotation);
        }
    }
}
