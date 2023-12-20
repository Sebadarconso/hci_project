using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    public static Reactor Instance { get; private set; }

    public enum CellType 
    {
        Red, 
        Green, 
        Blue,
        Yellow
    }

    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GameObject[] dropPoints;
    [SerializeField] private CellType[] acceptedCells;
    [SerializeField] private float dropDistance;

    private GameObject heldCell;

    private float gameTime = 0;
    private float currentTime;
    private bool consuming = false;
    private bool consumed = false;
    private float consumingTime = 2f;
    private bool crossedIn;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        acceptedCells = new CellType[4];

        SetDropPoint(0);
        SetDropPoint(1);
        SetDropPoint(2);    
        SetDropPoint(3);

        PlayerActions.instance.OnInteractPerformed += Instance_OnInteractPerformed;

    }

    private void Instance_OnInteractPerformed(object sender, System.EventArgs e)
    {
       for (int i = 0; i < dropPoints.Length; i++)
       {
          if (PlayerActions.instance.heldCell != null && 
          Vector3.Distance(PlayerActions.instance.heldCell.transform.position, dropPoints[i].transform.position) < dropDistance &&
          acceptedCells[i] == PlayerActions.instance.heldCell.GetComponent<EnergyCell>().cellType &&
          !consuming)
          {
            PlayerActions.instance.heldCell.transform.SetParent(dropPoints[i].transform);
            PlayerActions.instance.heldCell.transform.localPosition = Vector3.zero;
            heldCell = PlayerActions.instance.heldCell;
            PlayerActions.instance.heldCell = null;
            consuming = true;

            SetDropPoint(i);
          }
       }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDropPoint(int id) 
    {
        int rndTypeID = UnityEngine.Random.Range(0, 4);
        switch (rndTypeID)
        {
            case 0:
                acceptedCells[id] = CellType.Red;
                dropPoints[id].GetComponentInChildren<SpriteRenderer>().color = Color.red;
                break;
            case 1:
                acceptedCells[id] = CellType.Green; 
                dropPoints[id].GetComponentInChildren<SpriteRenderer>().color = Color.green;
                break;
            case 2:
                acceptedCells[id] = CellType.Blue;
                dropPoints[id].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                break;
            case 3:
                acceptedCells[id] = CellType.Yellow;
                dropPoints[id].GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                break;
        }
    }
}
