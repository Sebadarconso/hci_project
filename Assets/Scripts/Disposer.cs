using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Disposer : MonoBehaviour
{
    [SerializeField] Animator animator;
    [HideInInspector] public GameObject disposerHeldCell;
    [SerializeField] private float dropOffDistance; 
    public GameObject dropPoint;
    private bool disposing;
    private float disposeTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerActions.instance.OnInteractPerformed += Instance_OnInteractPerformed;
    }

    private void Instance_OnInteractPerformed(object sender, System.EventArgs e) 
    {
        if (!disposing &&
            PlayerActions.instance.heldCell != null &&
            Vector3.Distance(this.transform.position, PlayerActions.instance.heldCell.transform.position) < dropOffDistance)
            {
                disposerHeldCell = PlayerActions.instance.heldCell;
                disposerHeldCell.transform.parent = dropPoint.transform;
                disposerHeldCell.transform.localPosition = Vector3.zero;
                PlayerActions.instance.heldCell = null;
                disposing = true;
                // animator.SetBool("disposecell", true);
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (disposing) 
        {
            disposeTime -= Time.deltaTime;
            if (disposeTime < 0f)
            {
                disposing = false;
                // animator.SetBool("disposeCell", false);
                disposeTime = 2f;
            }
            else if(disposeTime < 1f)
            {
                Destroy(disposerHeldCell);
                disposerHeldCell = null;
            }
        }
    }
}
