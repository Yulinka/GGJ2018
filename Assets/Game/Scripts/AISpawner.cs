using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public AiPerson Template;
    public int Count = 40;
    public int AgentCount = 1;

    public Transform SpawnPoint;

    // Use this for initialization
    void Start()
    {
        GeneratePartygoers(Count, AgentCount);
    }

    public void GeneratePartygoers(int count, int agentCount)
    {
        for (int i = 0; i < count; ++i)
            Instantiate(Template, (SpawnPoint ? SpawnPoint : transform).position, Quaternion.identity);

        for (int i = 0; i < agentCount; ++i)
        {
            var agent = Instantiate(Template, (SpawnPoint ? SpawnPoint : transform).position, Quaternion.identity);
            agent.IsAgent = true;
            agent.name += " (Agent)";
        }
    }
}
