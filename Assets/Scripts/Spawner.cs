using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public enum SpawnerState
    {
        Waiting,
        Spawning,
        Spawned
    }

    SpawnerState spawnerState;

    private float spawnTimer;
    private bool isSpawned;

    [SerializeField] private GameObject[] cellPrefabs;
    [SerializeField] private GameObject spawnPoint;

    // [SerializeField] private Animator animatorController;

    [HideInInspector] public GameObject spawnedCell;

    [SerializeField] private float pickupDistance;

    // Start is called before the first frame update
    void Start()
    {
        PlayerActions.instance.OnInteractPerformed += OnInteractPerformed;
        spawnTimer = UnityEngine.Random.Range(2, 5);
    }

    private void OnDestroy()
    {
        PlayerActions.instance.OnInteractPerformed -= OnInteractPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerState == SpawnerState.Waiting)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnerState = SpawnerState.Spawning;
                spawnTimer = 1;

                if (spawnedCell == null)
                {
                    // animatorController.SetBool("isSpawned", true);
                    spawnedCell = Instantiate(cellPrefabs[UnityEngine.Random.Range(0, cellPrefabs.Length)]);
                    spawnedCell.transform.parent = spawnPoint.transform;
                    spawnedCell.transform.localPosition = Vector3.zero;
                    
                }
            }
        }
        else if (spawnerState == SpawnerState.Spawning)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                isSpawned = true;
                spawnerState = SpawnerState.Spawned;
            }
        }
        else
        {

        }
    }

    private bool CanPickupCell()
    {
        return spawnerState == SpawnerState.Spawned && Vector3.Distance(this.transform.position, PlayerActions.instance.transform.position) < pickupDistance && PlayerActions.instance.heldCell == null;
    }

    private void OnInteractPerformed(object sender, System.EventArgs e)
    {
        if (CanPickupCell())
        {
            spawnedCell.transform.parent = PlayerActions.instance.holdPoint.transform;
            PlayerActions.instance.heldCell = spawnedCell;
            spawnedCell.transform.localPosition = Vector3.zero;
            spawnedCell.transform.rotation = PlayerActions.instance.holdPoint.transform.rotation;

            spawnedCell = null;
            spawnTimer = UnityEngine.Random.Range(2, 5);
            spawnerState = SpawnerState.Waiting;
            
        }
    }
}