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
    public int WinTarget = 11;
    public Slider Slider;
    public PlayerController Player;
    public float IncrementSpeed = 0.05f;

    public AiPerson SpawnTemplate;
    public int SpawnCount = 40;
    public int AgentSpawnCount = 1;


    public Transform SpawnPoint;

    private float targetSlider;
    private bool hasLost = false;

    public void EndGame(AiPerson target)
    {
        if (target.IsAgent)
            OnWin();
        else
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
        Slider.minValue = 0f;
        Slider.maxValue = 1f;
	}

	private void Update ()
    {
        if (!hasLost)
            CalculateScore();

        float increment = IncrementSpeed * Time.deltaTime;
        Slider.value = Mathf.Clamp(Slider.value + increment, 0, targetSlider);

        if (ConvertedCount >= WinTarget && !hasLost)
            OnLose();
	}


    private void OnWin()
    {
        foreach (var p in people)
            p.Hint.ShowDotDotDot();
    }

    private void OnLose()
    {
        hasLost = true;

        Player.enabled = false;

        targetSlider = 1;
        IncrementSpeed = 1;

        foreach (var p in people) {
            p.Hint.ShowLose(p.IsAgent);

            if (p.IsAgent)
                Camera.main.GetComponent<LookAtTarget>().Target = p.transform;

            p.OnLose();
        }
    }

    private void CalculateScore()
    {
        ConvertedCount = 0;
        InfectedCount = 0;
        ConvertedPercent = 0;
        targetSlider = 0;

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
