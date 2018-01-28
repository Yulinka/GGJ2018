using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum AiPersonState
{
	None,
	Idle,
	Standing,
	Smoking,
	StandAt,
	Conversation,
	ConversationMoving,
	Walking,
}

public class AiPerson : MonoBehaviour
{
    public bool IsAgent = false;
    public bool IsInfected = false;
    public bool IsConverted = false;
    public AiPerson InfectedBy = null;
    public float ConversationMaxTime = 2f;
    public float ConversationMinTime = 1f;
    public float InfectMaxTime = 20f;
    public float InfectMinTime = 7f;
    public float StandMaxTime = 5f;
    public float StandMinTime = 3f;
    public float ShoutMinTime = 5f;
    public float ShoutMaxTime = 15f;
    public BodyConfig BodyConfig;
    public HintBubble Hint;

    private StateMachine<AiPersonState> states;
    private Vector3 navDest = Vector3.zero;
    private NavMeshAgent navAgent;
    private List<AiPerson> talkedTo;
    private Director director;
    private Activity activity;
    private float infectedTime;
    private float activityTime;
    private float shoutTime;
    private float startingActivityTime;
    private PlayerController player;
    private bool didInfect;
    private Canvas hintCanvas;
    private Canvas characterCanvas;
    private float infectedTotalTime;
    private float lastMetTotalTime;

    public void MoveTo(Vector3 location)
    {
        if (states.State != AiPersonState.Conversation)
            throw new Exception("Requires AiState.Conversation");

        talkedTo = new List<AiPerson>();

        navDest = location;
        states.ChangeState(AiPersonState.Walking);
    }

    public void ConversationMove(Vector3 location)
    {
        navDest = location;
        states.ChangeState(AiPersonState.ConversationMoving);
    }

    public void InfectMe(AiPerson infectedBy)
    {
        IsInfected = true;
        InfectedBy = infectedBy;
        infectedTotalTime = Time.time;
        infectedTime = UnityEngine.Random.Range(InfectMinTime, InfectMaxTime);
        name += " (Infected)";
        Debug.Log("Infected", this);
    }

    public void Kill()
    {
        director.GetComponent<GameManager>().EndGame(this);
    }

    public void OnLose()
    {
        states.ChangeState(AiPersonState.None);
    }

    public void Interrogate()
    {
        if (talkedTo.Count == 0)
        {
            Hint.ShowNoHint();
            Debug.Log("Havent spoken to anyone");
        }
        else if (IsInfected)
        {
            ShowHintAbout(InfectedBy, infectedTotalTime);
        }
        else
        {
            ShowHintAbout(talkedTo.Last(), lastMetTotalTime);
        }
    }

    public void onOtherJoinConversation(AiPerson joiner)
    {
        if (talkedTo.Contains(joiner))
            talkedTo.Remove(joiner);

        talkedTo.Add(joiner);
        lastMetTotalTime = Time.time;
    }

    public void ShowHintAbout(AiPerson target, float hintHash)
    {
        int index = (int)(hintHash % 3f);

        if (index == 0)
            Hint.ShowClothesHint(target.BodyConfig.Clothes);
        else if (index == 1)
            Hint.ShowHatHint(target.BodyConfig.Hat);
        else if (index == 2)
            Hint.ShowGlassesHint(target.BodyConfig.Glasses);
        else
            throw new Exception("Shouldnt get here");
    }

    private void Start()
    {
        // FOR TESTING
        //ConversationMaxTime = 8f;
        //ConversationMinTime = 4f;
        //StandMaxTime = 3f;
        //StandMinTime = 2f;
        //InfectMaxTime = 3f;
        //InfectMinTime = 1f;

        // FOR RELEASE
        ConversationMaxTime = 20f;
        ConversationMinTime = 8f;
        StandMaxTime = 5f;
        StandMinTime = 10f;
        InfectMaxTime = 9f;
        InfectMinTime = 6f;

        if(IsAgent)
        {
            ConversationMaxTime = 9f;
            ConversationMinTime = 7f;
            StandMaxTime = 5f;
            StandMinTime = 10f;
        }

        talkedTo = new List<AiPerson>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
        navAgent.autoBraking = true;

        director = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        characterCanvas = transform.Find("Canvas").gameObject.GetComponent<Canvas>();

        hintCanvas = transform.Find("HintContainer").transform.Find("Hint").gameObject.GetComponent<Canvas>();
        Hint = hintCanvas.GetComponent<HintBubble>();

        BodyConfig = director.GetComponent<BodyGenerator>().GetNextConfig();
        ConstructCanvas(BodyConfig);

        states = StateMachine<AiPersonState>.Initialize(this);
        states.ChangeState(AiPersonState.Idle);
    }

    private void ConstructCanvas(BodyConfig config)
    {
        Image body = characterCanvas.transform.Find("Body").gameObject.GetComponent<Image>();
        Image hat = characterCanvas.transform.Find("Hat").gameObject.GetComponent<Image>();
        Image hair = characterCanvas.transform.Find("Hair").gameObject.GetComponent<Image>();
        Image glasses = characterCanvas.transform.Find("Glasses").gameObject.GetComponent<Image>();
        Image clothes = characterCanvas.transform.Find("Clothes").gameObject.GetComponent<Image>();

		if (config.Hat == BodyHatState.None)
			hat.enabled = false;
		else
        	hat.sprite = config.HatSprite;

		if (config.Glasses == BodyGlassesState.None)
			glasses.enabled = false;
		else
        	glasses.sprite = config.GlassesSprite;

		body.sprite = config.BodySprite;
		hair.sprite = config.HairSprite;
        clothes.sprite = config.ClothesSprite;
    }

    private void Update()
    {
        hintCanvas.transform.rotation = characterCanvas.transform.rotation;

        transform.position = navAgent.nextPosition;
        TickInfected();

        if (IsInfected)
        {
            var color = IsConverted ? Color.magenta : Color.cyan;
            var a = new Vector3(0, 0.1f, 0.1f);
            var b = new Vector3(0.1f, 0.1f, -0.1f);
            var c = new Vector3(-0.1f, 0.1f, -0.1f);
            Debug.DrawRay(transform.position + a, b - a, color);
            Debug.DrawRay(transform.position + c, a - c, color);
            Debug.DrawRay(transform.position + b, c - b, color);
        }

        if(IsConverted && states.State != AiPersonState.None)
        {
            shoutTime -= Time.deltaTime;

            if(shoutTime <= 0)
            {
                shoutTime = UnityEngine.Random.Range(ShoutMinTime, ShoutMaxTime);
                Hint.ShowFascistHint();
            }
        }
    }

    private void TickInfected()
    {
        if (!IsInfected || IsConverted)
            return;

        bool inActivityWithAgent = activity.Participants.Contains(InfectedBy);

        if (inActivityWithAgent)
            return;

        infectedTime -= Time.deltaTime;

        if (infectedTime <= 0)
        {
            IsConverted = true;
            shoutTime = UnityEngine.Random.Range(ShoutMinTime, ShoutMaxTime);
            Hint.ShowFascistConvert();
        }
    }

    private void PickNextActivity()
    {
        Activity newActivity = null;

        if(UnityEngine.Random.value <= 0.1f)
            newActivity = director.FindActivity(this, typeof(StandAt));
        else if (UnityEngine.Random.value > 0.1f)
            newActivity = director.FindActivity(this, typeof(Conversation));

        if(newActivity == null || !newActivity.CanJoin(this) || newActivity == activity)
            return;

        //Debug.Log("Picking new activity: " + activity.GetName());
        if(activity != null)
            activity.Leave(this);

        activity = newActivity;
        activity.Reserve(this);
        navDest = activity.transform.position;
        states.ChangeState(AiPersonState.Walking);
    }

	private void SetRandomSprite(SpriteRenderer renderer, Sprite[] sprites)
    {
        if (sprites.Length == 0)
            return;

        int index = (int)Mathf.Round(UnityEngine.Random.value * (sprites.Length - 1));
        renderer.sprite = sprites[index];
    }

    private void StartActivity(Activity activity)
    {
        if (activity is Conversation)
            states.ChangeState(AiPersonState.Conversation);
        if (activity is StandAt)
            states.ChangeState(AiPersonState.StandAt);
        didInfect = false;
    }

    private void InfectParticipant(Activity searchIn)
    {
        var activityRadius = Vector3.Distance(transform.position, activity.transform.position);
        var playerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerDistance < activityRadius + 1)
            return; //player is in viscinity

        AiPerson target = (searchIn.Participants
            .Where((p) => !p.IsInfected && p != this)
            .ToArray()
            .GetRandomItem());

        if (target == null)
            return;

        target.InfectMe(this);
    }

    private void Idle_Update()
    {
        PickNextActivity();
    }

    private void Walking_Enter()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(navDest);
    }

    private void Walking_Update()
    {
        if (navAgent.remainingDistance <= activity.GetApproachDistance())
            StartActivity(activity);
    }

    private void Walking_Exit()
    {
        navAgent.isStopped = true;
    }

    private void StandAt_Enter()
    {
        activityTime = UnityEngine.Random.Range(
            StandMinTime,
            StandMaxTime);
        startingActivityTime = activityTime;

        activity.Join(this);
    }

    private void StandAt_Update()
    {
        activityTime -= Time.deltaTime;

        if (activityTime <= 0)
            PickNextActivity();
    }

    private void Conversation_Enter()
    {
        if (states.LastState == AiPersonState.ConversationMoving)
            return;

        activityTime = UnityEngine.Random.Range(
            ConversationMinTime,
            ConversationMaxTime);

        startingActivityTime = activityTime;

        activity.Join(this);
        Hint.ShowDotDotDot();

        //Debug.Log(ConversationMinTime + ", " + ConversationMaxTime + ", " + conversationTime);
    }

    private void Conversation_Update()
    {
        activityTime -= Time.deltaTime;
        Debug.DrawRay(transform.position + new Vector3(0.1f, 0, 0), Vector3.up * (activityTime / startingActivityTime) * 4, activity.Color);

        if (activityTime <= 0) {
            if (IsAgent && !didInfect)
            {
                InfectParticipant(activity);
                didInfect = true;
            }

            PickNextActivity();
        }
    }

    private void ConversationMoving_Enter()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(navDest);
    }

    private void ConversationMoving_Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0.1f, 0, 0), Vector3.up * (activityTime / startingActivityTime) * 4, activity.Color);
        //transform.position = navDest;
        //navAgent.Warp(navDest);

        navAgent.SetDestination(navDest);

        if (navAgent.remainingDistance <= 0)
            states.ChangeState(AiPersonState.Conversation);
    }

    private void ConversationMove_Exit()
    {
        navAgent.SetDestination(transform.position);
        navAgent.isStopped = true;
    }
}