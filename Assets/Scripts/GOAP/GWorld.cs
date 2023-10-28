using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static List<GameObject> patients;
    private static Queue<GameObject> cubicles;
    private static Queue<GameObject> vendingMachines;

    static GWorld()
    {
        world = new WorldStates();
        patients = new List<GameObject>();
        cubicles = new Queue<GameObject>();
        vendingMachines = new Queue<GameObject>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach(GameObject c in cubes)
        {
            cubicles.Enqueue(c);
        }

        GameObject[] machines = GameObject.FindGameObjectsWithTag("VendingMachine");
        foreach (GameObject c in machines)
        {
            vendingMachines.Enqueue(c);
        }

        if (cubes.Length > 0)
        {
            world.ModifyState("FreeCubicle", cubes.Length);
        }

        if (machines.Length > 0)
        {
            world.ModifyState("FreeVendingMachine", machines.Length);
        }
    }

    private GWorld()
    {

    }

    public void AddMachine(GameObject machine)
    {
        vendingMachines.Enqueue(machine);
    }

    public GameObject RemoveMachine()
    {
        if (vendingMachines.Count == 0) return null;
        return vendingMachines.Dequeue();
    }

    public void AddPatient(GameObject patient)
    {
        patients.Add(patient);
    }

    public GameObject RemovePatient()
    {
        if(patients.Count == 0) return null;
        GameObject result = patients[0];
        patients.RemoveAt(0);
        return result;
    }

    public bool RemovePatient(GameObject patient)
    {
        return patients.Remove(patient);
    }


    public void AddCubicles(GameObject cubicle)
    {
        cubicles.Enqueue(cubicle);
    }

    public GameObject RemoveCubicles()
    {
        if (cubicles.Count == 0) return null;
        return cubicles.Dequeue();
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
