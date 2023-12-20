using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private float pickUpDistance;
    [SerializeField] private float dropOffDistance; 
    [HideInInspector] public GameObject tableHeldCell;

    
    private GameObject swapCell;
    public GameObject dropPoint;
    // Start is called before the first frame update
    void Start()
    {
        PlayerActions.instance.OnInteractPerformed += Instance_OnInteractionPerformed;
    }

    private void Instance_OnInteractionPerformed(object sender, System.EventArgs e)
    {
        // player wants to drop off the energy cell
        if (PlayerActions.instance.heldCell != null &&  
                tableHeldCell == null && 
                Vector3.Distance(this.transform.position, PlayerActions.instance.transform.position) < dropOffDistance)
                {
                    tableHeldCell = PlayerActions.instance.heldCell;
                    tableHeldCell.transform.parent = dropPoint.transform;
                    tableHeldCell.transform.localPosition = Vector3.zero;
                    PlayerActions.instance.heldCell = null;
                }
        else if(PlayerActions.instance.heldCell == null &&  
                    tableHeldCell != null && 
                    Vector3.Distance(this.transform.position, PlayerActions.instance.transform.position) < pickUpDistance) // player wants to pick up the energy cell
        {
                    PlayerActions.instance.heldCell = tableHeldCell;
                    PlayerActions.instance.heldCell.transform.parent = PlayerActions.instance.holdPoint.transform;
                    PlayerActions.instance.heldCell.transform.localPosition = Vector3.zero;
                    tableHeldCell = null;
        }
        
        else if(PlayerActions.instance.heldCell != null && 
                tableHeldCell != null &&
                Vector3.Distance(this.transform.position, PlayerActions.instance.transform.position) < pickUpDistance)
                { 
                    swapCell = PlayerActions.instance.heldCell; 
                    PlayerActions.instance.heldCell = tableHeldCell; 
                    PlayerActions.instance.heldCell.transform.parent = PlayerActions.instance.holdPoint.transform;
                    PlayerActions.instance.heldCell.transform.localPosition = Vector3.zero;
                    tableHeldCell = swapCell;
                    tableHeldCell.transform.parent = dropPoint.transform;
                    tableHeldCell.transform.localPosition = Vector3.zero;
                }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable() 
    {
        PlayerActions.instance.OnInteractPerformed -= Instance_OnInteractionPerformed;
    }
}
