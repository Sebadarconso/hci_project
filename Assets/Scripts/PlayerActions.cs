using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions instance {get; private set;}
    private PlayerInput playerActionSystem;
    [SerializeField] private float movSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float playerHeight;
    [SerializeField] private float playerRadius;
    [SerializeField] private float movDist;

    public event EventHandler OnInteractPerformed;

    public GameObject holdPoint;
    [HideInInspector] public GameObject heldCell;

    private void Awake()
    {
        instance = this;
        playerActionSystem = new PlayerInput();
        playerActionSystem.PlayerActions.Enable();
        playerActionSystem.PlayerActions.Interact.performed += Interact_performed;

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
    {
        OnInteractPerformed.Invoke(this, EventArgs.Empty);
    }

    // Start is called before the first frame update
    void Start()
    {

    }
  

    // Update is called once per frame
    void Update()
    {
        Vector2 readVec = playerActionSystem.PlayerActions.Movement.ReadValue<Vector2>();
        Vector3 movVec = new Vector3(readVec.x, 0, readVec.y);

        Quaternion cameraRot = Camera.main.transform.rotation;
        float cameraAngle;
        Vector3 cameraAxis;
        cameraRot.ToAngleAxis(out cameraAngle, out cameraAxis);


        Quaternion cameraRotFlat = Quaternion.Euler(0, cameraAngle * cameraAxis.y, 0);
        
        movVec = cameraRotFlat * movVec;
        float movDist = movSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(this.transform.position, 
                                            this.transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            movVec,
                                            movDist
                                            );


        this.transform.forward = Vector3.Slerp(this.transform.forward, movVec, Time.deltaTime * turnSpeed);

        if (!canMove)
        {
            Vector3 movVecx = new Vector3(movVec.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(this.transform.position, 
                                            this.transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            movVecx,
                                            movDist
                                            );
            if (canMove)
            {
                movVec = movVecx;
            }
        }


        if (!canMove)
        {
            Vector3 movVecz = new Vector3(0, 0, movVec.z).normalized;
            canMove = !Physics.CapsuleCast(this.transform.position, 
                                            this.transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            movVecz,
                                            movDist
                                            );
            if (canMove)
            {
                movVec = movVecz;
            }
        }

        if (canMove)
        {
            this.transform.position += movVec * movSpeed * Time.deltaTime;
        }

        playerAnimator.SetBool("isWalking", movVec.magnitude != 0);
        
    }

    private void OnDisable()
    {
        playerActionSystem.PlayerActions.Interact.performed -= Interact_performed;
    }

}
