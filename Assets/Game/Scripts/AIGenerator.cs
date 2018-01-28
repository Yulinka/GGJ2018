using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGenerator : MonoBehaviour
{
    public AiPerson Template;
    public int Count = 40;

    // Use this for initialization
    void Start()
    {
        GeneratePartygoers(Count);
    }

    public void GeneratePartygoers(int count)
    {
        for (int i = 0; i < count; ++i)
            Instantiate(Template, transform);
    }
}
