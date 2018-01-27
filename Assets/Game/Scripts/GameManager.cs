using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<AiPerson> people;
    public int InfectedCount = 0;
    public int ConvertedCount = 0;
    public int ConvertedPercent = 0;

    void Start ()
    {
        people = (GameObject
            .FindGameObjectsWithTag("Person")
            .Select((o) => o.GetComponent<AiPerson>())
            .ToList());
	}
	
	void Update ()
    {
        CalculateScore();
	}

    private void CalculateScore()
    {
        ConvertedCount = 0;
        InfectedCount = 0;
        ConvertedPercent = 0;

        if(people.Count > 0)
        {
            ConvertedCount = people.Count((p) => p.IsConverted);
            InfectedCount = people.Count((p) => p.IsInfected);
            ConvertedPercent = (int)(((float)ConvertedCount / (float)people.Count) * 100f);
        }
    }
}
