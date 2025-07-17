using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPlayerMovement : MonoBehaviour
{
    public float mouseSensibility, speedMove, jumpForce;
    public GameObject playerBody, groundChecker;
    private float mouseX, mouseY, xRotation, MoveX, MoveZ;

    // Gravedad de forma Manual
    private Vector3 gravityVelocity;
    public float gravity = -9.81f;

    public bool isGrounded;

    public LayerMask groundMask;

    /// Crea variables para los detectar Touch.
    private int leftFingerID, rightFingerID;
    private float halfScreenWidth; // variable para dividir la pantalla
    private Vector2 moveInput, moveTouchStartPosition, lookInput;
    [SerializeField] private float cameraSensibility;
   // private float cameraPitch;



    void Start()
    {
        leftFingerID = -1;
        rightFingerID = -1;
        halfScreenWidth = Screen.width / 2f;
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
        GetTouchInput();

        if(leftFingerID != -1)
        {
            // Movimiento del player
            movement();
        }

        if(rightFingerID != -1)
        {
            // rotación de vista
            rotateView();
        }

        isGrounded = Physics.CheckSphere(groundChecker.transform.position, 0.4f, groundMask);

        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2;
        }

       // Debug.Log(gravityVelocity.y)
        

        // Añadir Gravedade de forma Manual
        gravityManual();

        //Salto
        jump();
    }

    void rotateView ()
    {
        //  Valores de movimiento de la cámara
        //mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        //mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        mouseX = lookInput.normalized.x /** mouseSensibility * Time.deltaTime*/;
        mouseY = lookInput.normalized.y /** mouseSensibility * Time.deltaTime*/;

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
        //MoveX = Input.GetAxis("Horizontal");
        //MoveZ = Input.GetAxis("Vertical");

        MoveX = moveInput.normalized.x;
        MoveZ = moveInput.normalized.y;

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

    private void GetTouchInput()
    {
        //if (Input.touchCount > 0)
        //{
        //    Debug.Log($"Ahora {Input.touchCount} dedos tocan la pantalla");
        //}

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            if (t.phase == TouchPhase.Began)
            {
                if (t.position.x < halfScreenWidth && leftFingerID == -1)
                {
                    leftFingerID = t.fingerId;
                    moveTouchStartPosition = t.position;
                }
                else if (t.position.x > halfScreenWidth && rightFingerID == -1)
                {
                    rightFingerID = t.fingerId;
                }
            }
            if (t.phase == TouchPhase.Canceled)
            {

            }
            if (t.phase == TouchPhase.Moved)
            {
                if(leftFingerID == t.fingerId)
                {
                    moveInput = t.position - moveTouchStartPosition;
                }
                else if(rightFingerID == t.fingerId)
                {
                    lookInput = t.deltaPosition * mouseSensibility * Time.deltaTime /** cameraSensibility * Time.deltaTime*/;
                }
            }
            if (t.phase == TouchPhase.Stationary)
            {
                if(leftFingerID == t.fingerId)
                {
                    moveInput = Vector2.zero;
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (leftFingerID == t.fingerId)
                {
                    leftFingerID = -1;
                    moveInput = Vector2.zero;
                }
                else if (rightFingerID == t.fingerId)
                {
                    rightFingerID = -1;
                    lookInput = Vector2.zero;
                }
            }
        }
    }
}
