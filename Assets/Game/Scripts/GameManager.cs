using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<AiPerson> people;
    public int InfectedCount = 0;
    public int ConvertedCount = 0;
    public int ConvertedPercent = 0;
    public int WinTarget = 10;
    public Slider Slider;
    public PlayerController Player;

    public AiPerson SpawnTemplate;
    public int SpawnCount = 40;
    public int AgentSpawnCount = 1;

    public Transform SpawnPoint;

    private float targetSlider;
    private bool hasLost = false;

    public void EndGame(AiPerson target)
    {
        if (target.IsAgent)
        {
            Debug.Log("YOU WIN");

            foreach (var p in people)
                p.Hint.ShowDotDotDot();
        } else
            OnLose();
    }

    private void Start ()
    {
        GeneratePartygoers(SpawnCount, AgentSpawnCount);

        people = (GameObject
            .FindGameObjectsWithTag("Person")
            .Select((o) => o.GetComponent<AiPerson>())
            .ToList());

        Slider = GameObject.FindObjectOfType<Slider>();
        Slider.maxValue = 0.5f;
	}

	private void Update ()
    {
        CalculateScore();

        float increment = 0.05f * Time.deltaTime;
        Slider.value = Mathf.Clamp(Slider.value + increment, 0, targetSlider);

        if (ConvertedCount >= WinTarget && !hasLost)
            OnLose();
	}

    private void OnLose()
    {
        hasLost = true;

        Player.enabled = false;

        foreach (var p in people) {
            p.Hint.ShowLose();
            p.OnLose();
        }
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

            int TotalCount = people.Count -1;
            float percent = (float)ConvertedCount / WinTarget;

            ConvertedPercent = (int)(percent * 100f);
            targetSlider = percent;
        }
    }

    public void GeneratePartygoers(int count, int agentCount)
    {
        for (int i = 0; i < count; ++i)
            Instantiate(SpawnTemplate, (SpawnPoint ? SpawnPoint : transform).position, Quaternion.identity);

        for (int i = 0; i < agentCount; ++i)
        {
            var agent = Instantiate(SpawnTemplate, (SpawnPoint ? SpawnPoint : transform).position, Quaternion.identity);
            agent.IsAgent = true;
            agent.name += " (Agent)";
        }
    }
}
