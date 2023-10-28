using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    private void Awake()
    {
        if(instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject patientPrefab;
    public GameObject nursePrefab;
    public int numPatients;
    public int numNurse;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numPatients; i++)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < numNurse; i++)
        {
            Instantiate(nursePrefab, transform.position, Quaternion.identity);
        }

        Invoke(nameof(SpawnPatient), 5);
    }

    private void SpawnPatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(SpawnPatient), Random.Range(10, 15));
    }

    public void SpawnNurse()
    {
        Instantiate(nursePrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
