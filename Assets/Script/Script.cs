using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class Script : MonoBehaviour
{
    public Transform taget;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null) 
        { 
            agent.SetDestination(taget.position); 
        }
    }
}
