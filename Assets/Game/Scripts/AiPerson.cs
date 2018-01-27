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
    Smoking,
    Conversing,
    Walking
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

    public void MoveTo(Vector3 location)
    {
        if (states.State != AiPersonState.Conversing)
            throw new Exception("Requires AiState.Conversing");

        talkedTo = new List<AiPerson>();

        navDest = location;
        states.ChangeState(AiPersonState.Walking);
    }

    public void Infect(AiPerson agent)
    {
        IsInfected = true;
        InfectedBy = agent;
    }

    public void Interrogate()
    {
        if(talkedTo.Count == 0)
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

        states = StateMachine<AiPersonState>.Initialize(this);
        states.ChangeState(AiPersonState.Standing);
	}

	private void Update()
	{
			
	}

    private void Standing_Update()
    {
    }

    private void DecideAction_Update()
    {
        states.ChangeState(AiPersonState.Conversing);
    }

    private void Walking_Enter()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(navDest);
    }

    private void Walking_Update()
    {
        navAgent.SetDestination(navDest);
    }

    private void Walking_Exit()
    {
        navAgent.isStopped = true;
    }

    private void Smoking_Update()
    {
        
    }

    private void Conversing_Enter()
    {
        
    }
}
