using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        
    }

    private void Instance_OnInteractPerformed(object sender, System.EventArgs e) 
    {
        Debug.Log("bla");
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerActions.instance.OnInteractPerformed += Instance_OnInteractPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = PlayerActions.instance.transform.position + offset;
        this.transform.LookAt(PlayerActions.instance.transform);

    }
}
