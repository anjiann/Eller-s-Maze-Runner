using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 2500f;
    [SerializeField] private float lookSensitivity = 3f;

    [SerializeField] private PlayerMotor motor;

    public bool isGrounded = false;

    // Use this for initialization
    void Start() {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update() {
        //jump
        if (Input.GetKeyDown("space") && isGrounded) {
            print("space was pressed");
            motor.Move(new Vector3(0, 0, 0));
            isGrounded = false;
            motor.Jump(jumpForce);
        }
        else if (isGrounded) {
            //movement
            float xAxis = Input.GetAxisRaw("Horizontal");
            float zAxis = Input.GetAxisRaw("Vertical");

            Vector3 moveHorizontal = transform.right * xAxis;
            Vector3 moveVertical = transform.forward * zAxis;

            Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
            motor.Move(velocity);

            //shoot
            if (Input.GetKeyDown("f") && isGrounded) {
                print("f was pressed");

                GameStatus gameStatus = FindObjectOfType<GameStatus>();

                if (gameStatus.getNumProjectiles() > 0) {
                    gameStatus.MinusProjectile();
                    motor.Shoot();
                }
            }
        }

        //rotation around the y axis
        float yRot = Input.GetAxisRaw("Mouse X"); //when we move our mouse on the x, rotate around the y
        Vector3 rotationY = new Vector3(0f, yRot, 0f) * lookSensitivity;
        motor.RotateY(rotationY);

        //rotation around the x axis
        float xRot = Input.GetAxisRaw("Mouse Y"); //when we move our mouse on the x, rotate around the y

        Vector3 rotationX = new Vector3(xRot, 0f, 0f) * lookSensitivity;
        motor.RotateX(rotationX);
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.name == "Ground" || collision.gameObject.name == "Maze Row(Clone)") {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.name == "Ground" || collision.gameObject.name == "Maze Row(Clone)") {
            isGrounded = false;
        }
    }
}
