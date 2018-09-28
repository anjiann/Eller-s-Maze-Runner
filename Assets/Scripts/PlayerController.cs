using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;

    [SerializeField] private PlayerMotor motor;

	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update () {
        //movement
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xAxis;
        Vector3 moveVertical = transform.forward * zAxis;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
        motor.Move(velocity);

        //rotation
        float yRot = Input.GetAxisRaw("Mouse X"); //when we move our mouse on the x, rotate around the y
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        motor.Rotate(rotation);

        //camera rotation
        float xRot = Input.GetAxisRaw("Mouse Y"); //when we move our mouse on the x, rotate around the y

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * lookSensitivity;
        motor.RotateCamera(cameraRotation);
    }
}
