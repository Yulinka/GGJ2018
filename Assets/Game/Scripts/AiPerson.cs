using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public bool IsInfected = true;
    public AiPerson InfectedBy = null;
    public float ConversationMaxTime = 9f;
    public float ConversationMinTime = 5f;
    public float StandMaxTime = 5f;
    public float StandMinTime = 3f;
    public Sprite[] Sprites;

    private StateMachine<AiPersonState> states;
    private Vector3 navDest = Vector3.zero;
    private NavMeshAgent navAgent;
    private List<AiPerson> talkedTo;
    private SpriteRenderer spriteRenderer;
    private Director director;
    private Activity activity;
    private float conversationTime;
    private float standTime;

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

    public void Infect(AiPerson agent)
    {
        IsInfected = true;
        InfectedBy = agent;
    }

    public void Interrogate()
    {
        if (talkedTo.Count == 0)
        {
            Debug.Log("Havent spoken to anyone");
        }
        else if (IsInfected)
        {
            Debug.Log("INFECTED, AGENT", InfectedBy);
        }
        else
        {
            AiPerson lastSpokeTo = talkedTo.Last();
            Debug.Log("LAST SPOKE TO", lastSpokeTo);
        }
    }

    public void onOtherJoinConversation(AiPerson joiner)
    {
        if (talkedTo.Contains(joiner))
            talkedTo.Remove(joiner);

        talkedTo.Add(joiner);

    }

    private void Start()
    {
        ConversationMaxTime = 30f;
        ConversationMinTime = 10f;
        StandMaxTime = 20f;
        StandMinTime = 4f;

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
        navAgent.autoBraking = true;

        director = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SetRandomSprite();

        states = StateMachine<AiPersonState>.Initialize(this);
        states.ChangeState(AiPersonState.Idle);
    }

    private void Update()
    {
        transform.position = navAgent.nextPosition;
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

    private void SetRandomSprite()
    {
        if (Sprites.Length == 0)
            return;

        int index = (int)Mathf.Round(UnityEngine.Random.value * (Sprites.Length - 1));
        spriteRenderer.sprite = Sprites[index];
    }

    private void StartActivity(Activity activity)
    {
        if (activity is Conversation)
            states.ChangeState(AiPersonState.Conversation);
        if (activity is StandAt)
            states.ChangeState(AiPersonState.StandAt);
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
        standTime= UnityEngine.Random.Range(
            StandMinTime,
            StandMaxTime);

        activity.Join(this);
    }

    private void StandAt_Update()
    {
        standTime -= Time.deltaTime;

        if (standTime <= 0)
            PickNextActivity();
    }

    private void Conversation_Enter()
    {
        if (states.LastState == AiPersonState.ConversationMoving)
            return;

        conversationTime = UnityEngine.Random.Range(
            ConversationMinTime,
            ConversationMaxTime);

        //Debug.Log(ConversationMinTime + ", " + ConversationMaxTime + ", " + conversationTime);
        activity.Join(this);
    }

    private void Conversation_Update()
    {
        conversationTime -= Time.deltaTime;

        if (conversationTime <= 0)
            PickNextActivity();
    }

    private void ConversationMoving_Enter()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(navDest);
    }

    private void ConversationMoving_Update()
    {
        //transform.position = navDest;
        //navAgent.Warp(navDest);

        //Debug.Log(navDest);

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