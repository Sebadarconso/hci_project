using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private PlayerInput playerActionSystem;
    [SerializeField] private float movSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Animator playerAnimator;

    private void Awake()
    {
        playerActionSystem = new PlayerInput();
        playerActionSystem.PlayerActions.Enable();
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
        this.transform.position += movVec * movSpeed * Time.deltaTime;
        this.transform.forward = Vector3.Slerp(this.transform.forward, movVec, Time.deltaTime * turnSpeed);

        /*
         * La prof ha scritto questo, fa schifo. MI dispiace ma rimane come giù
         * 
        
        if (movVec.magnitude != 0)
        {
            playerAnimator.SetBool("isWalking", true);
        } else
        {
            playerAnimator.SetBool("isWalking", false);
        }*/

        playerAnimator.SetBool("isWalking", movVec.magnitude != 0);
    }
}
