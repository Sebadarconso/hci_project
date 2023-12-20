using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera2 : MonoBehaviour
{

    [SerializeField]  private CinemachineVirtualCamera virtualCamera;
    [SerializeField]  private CinemachineDollyCart cameraDolly;
    [SerializeField] private float baseFOV;
    [SerializeField] private float FOVmulti;

    private float lengthScale; // scaling factor for the lenght of the track 
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        lengthScale = cameraDolly.m_Path.PathLength;
        radius = new Vector3(virtualCamera.transform.position.x, 0, virtualCamera.transform.position.z).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 player2DPos = new Vector2(PlayerActions.instance.transform.position.x, PlayerActions.instance.transform.position.z);
        float player2DMag = player2DPos.magnitude;

        player2DPos = new Vector2(player2DPos.x / player2DMag, player2DPos.y / player2DMag);
        float posLength = Vector2.SignedAngle(player2DPos, new Vector2(1, 1));

        float FOV = baseFOV + (PlayerActions.instance.transform.position.magnitude * FOVmulti);

        if (posLength > 0) SetCameraPos(posLength / 360, FOV);
        else SetCameraPos((posLength + 360) / 360, FOV);

    }

    private void SetCameraPos(float pos, float scaledFOV) 
    {
        virtualCamera.m_Lens.FieldOfView = scaledFOV;
        cameraDolly.m_Position = pos * lengthScale; // transform the pos to [0,1] range to [0, lengthScale]
    }

}
