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
    Standing,
    Deciding,
    Smoking,
    Conversing,
    ConversationMoving,
    Walking, // to activity
}

public class AiPerson : MonoBehaviour
{
    public bool IsAgent = false;
    public bool IsInfected = true;
    public AiPerson InfectedBy = null;

    private StateMachine<AiPersonState> states;
    private Vector3 navDest = Vector3.zero;
    private NavMeshAgent navAgent;
    private List<AiPerson> talkedTo;

    private Director director;
    private Activity activity;

    public void MoveTo(Vector3 location)
    {
        if (states.State != AiPersonState.Conversing)
            throw new Exception("Requires AiState.Conversing");

        talkedTo = new List<AiPerson>();

        navDest = location;
        states.ChangeState(AiPersonState.Walking);
    }

    public void ConversationMove(Vector3 location)
    {
        navDest = location;
        isInConversation = true;
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
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
        navAgent.autoBraking = true;
        director = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();

        states = StateMachine<AiPersonState>.Initialize(this);
        states.ChangeState(AiPersonState.Deciding);
    }

    private void Update()
    {
        transform.position = navAgent.nextPosition;
    }

    private void Standing_Update()
    {
    }

    private void Deciding_Update()
    {
    }

    private void Deciding_Enter()
    {
        activity = director.FindActivity(this, typeof(Conversation));
        navDest = activity.transform.position;
        states.ChangeState(AiPersonState.Walking);
    }

    private void Walking_Enter()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(navDest);
    }

    private void Walking_Update()
    {
        if (Vector3.Distance(transform.position, activity.transform.position) <= activity.GetApproachDistance())
        {
            if (activity.CanJoin(this))
                states.ChangeState(AiPersonState.Conversing);
            else
                states.ChangeState(AiPersonState.Deciding);
        }
    }

    private void Walking_Exit()
    {
        navAgent.isStopped = true;
    }

    private void Smoking_Update()
    {

    }

    bool isInConversation = false;
    private void Conversing_Enter()
    {
        if (states.LastState != AiPersonState.ConversationMoving)
            activity.Join(this);
    }

    private void Conversing_Exit()
    {
        if (!isInConversation)
            activity.Leave(this);
    }

    private void ConversationMove_Enter()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(navDest);
        isInConversation = false;
    }

    private void ConversationMove_Exit()
    {
        navAgent.isStopped = true;
    }
}