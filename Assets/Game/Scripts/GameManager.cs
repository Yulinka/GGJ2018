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
	public Canvas IntroScreen;
    public PlayerController Player;
    public float IncrementSpeed = 0.05f;

    public AiPerson SpawnTemplate;
    public int SpawnCount = 40;
	public int AgentSpawnCount = 1;
	public bool gameStarted = false;
    public float gameTotalTime = 0f;

    public AudioClip LoseSound;

    public Transform SpawnPoint;

    private float targetSlider;
    private bool hasLost = false;
    private bool gameIsOver = false;

    public void EndGame(AiPerson target)
    {
        if (gameIsOver || hasLost)
            return;

        if (target.IsAgent)
            StartCoroutine(OnWin());
        else
            StartCoroutine(OnLose());
    }

    private void Start ()
    {
        Slider = GameObject.FindObjectOfType<Slider>();
        Slider.minValue = 0f;
        Slider.maxValue = 1f;
        Slider.gameObject.SetActive(false);
	}

	private void Update ()
    {

        if (gameStarted)
            gameTotalTime += Time.deltaTime;
        
        if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(StartGame());
		}

        if (!hasLost)
            CalculateScore();

        float increment = IncrementSpeed * Time.deltaTime;
        Slider.value = Mathf.Clamp(Slider.value + increment, 0, targetSlider);

        if (ConvertedCount >= WinTarget && !hasLost)
            StartCoroutine(OnLose());
	}

    IEnumerator StartGame()
	{
		GeneratePartygoers(SpawnCount, AgentSpawnCount);

		people = (GameObject
			.FindGameObjectsWithTag("Person")
			.Select((o) => o.GetComponent<AiPerson>())
			.ToList());

		Image[] images = IntroScreen.GetComponentsInChildren<Image> ();
		images[0].CrossFadeAlpha (0, 2.0f, false);
		images[1].CrossFadeAlpha (0, 2.0f, false);
		images[2].CrossFadeAlpha (0, 2.0f, false);

		gameStarted = true;

        yield return new WaitForSeconds(2.0f);
        Slider.gameObject.SetActive(true);
	}

    private IEnumerator OnWin()
    {
        Player.enabled = false;

		foreach (var p in people)
        {
            if (p.IsAgent)
                p.Hint.ShowLose(true);
            else
                p.Hint.ShowWin(false);

            p.OnWin();
        }

        yield return new WaitForSeconds(8.0f);
        Application.LoadLevel(Application.loadedLevel);
    }

    private IEnumerator OnLose()
    {
        hasLost = true;

        Player.enabled = false;

        targetSlider = 1;
        IncrementSpeed = 1;

        foreach (var p in people) {
            p.Hint.ShowLose(p.IsAgent);

            if (p.IsAgent)
                Camera.main.GetComponentInParent<NewCameraSystem>().LookAtAnimate(p.gameObject);

            p.OnLose();
        }

        AudioSource.PlayClipAtPoint(LoseSound, Camera.main.transform.position);

        yield return new WaitForSeconds(8.0f);
        Application.LoadLevel(Application.loadedLevel);
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

            //int TotalCount = people.Count -1;
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
