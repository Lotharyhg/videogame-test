using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float mouseSensibility, speedMove, jumpForce;
    public GameObject playerBody, groundChecker;
    private float mouseX, mouseY, xRotation, MoveX, MoveZ;

    // Gravedad de forma Manual
    private Vector3 gravityVelocity;
    public float gravity = -9.81f;

    public bool isGrounded;

    public LayerMask groundMask;


    void Start()
    {
        //playerBody = FindObjectOfType<Transform>().gameObject;
        //playerBody = GameObject.FindGameObjectWithTag("Player");
        //playerBody = GameObject.Find("NameObject");
        //groundChecker = this.transform.GetChild(indexPosition).gameObject;

        playerBody = GameObject.Find("First_Person_Player");
        groundChecker = playerBody.transform.GetChild(1).gameObject;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundChecker.transform.position, 0.4f, groundMask);

        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2;
        }

       // Debug.Log(gravityVelocity.y);

        // rotación de vista
        rotateView();

        // Movimiento del player
        movement();

        // Añadir Gravedade de forma Manual
        gravityManual();

        //Salto
        jump();
    }

    void rotateView ()
    {
        //  Valores de movimiento de la cámara
        mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        // aplicar el movimiento al vista vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90); //limite de rotación
        this.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); //aplicar la rotación 

        // aplicar el l movimiento del vector al punto de rotación del playerbodyla vista horizontal
        playerBody.transform.Rotate(Vector3.up * mouseX);
    }

    void movement()
    {
        // se basa el movimiento con el objeto padre
        // Rescatar los valor de axis del eje horizontal y vertical de las teclas WASD
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        // crear un vector para guardar la información de dirección del character controller en el player del objeto padre.
        Vector3 move = playerBody.transform.right * MoveX + playerBody.transform.forward * MoveZ;
        
        //Utilizar el componemente charactercontroller para ejecutar el movimiento del player del objeto padre.
        playerBody.GetComponent<CharacterController>().Move( move * speedMove * Time.deltaTime );

    }

    void gravityManual()
    {
        gravityVelocity.y += gravity * Time.deltaTime;
        // Gravedad aplicada
        playerBody.GetComponent<CharacterController>().Move(gravityVelocity * Time.deltaTime);
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            gravityVelocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(groundChecker.transform.position, 0.4f);
    }
}
