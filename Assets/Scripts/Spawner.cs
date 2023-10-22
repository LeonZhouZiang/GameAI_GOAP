using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPatients;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numPatients; i++)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        }

        Invoke(nameof(SpawnPatient), 5);
    }

    private void SpawnPatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(SpawnPatient), Random.Range(2, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
