using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Conversation : Activity
{
    public float ActorSize;
    public float RingPadding;

    private void Start()
    {
        ActorSize = 1f;
        RingPadding = 0.1f;
    }

    public override string GetName()
    {
        return "Conversation";
    }

    public override float GetApproachDistance()
    {
        var theta = (1f / Participants.Count) * Mathf.PI * 2;
        var radius = (ActorSize + RingPadding * 2) / theta;

        return radius + 2;

    }

    void DistributeParticipants()
    {
        var theta = (1f / Participants.Count) * Mathf.PI * 2;
        var radius = (ActorSize + RingPadding * 2) / theta;
        for (int i = 0; i < Participants.Count; ++i)
        {
            var unit = new Vector3(
                Mathf.Cos(i * theta),
                0,
                Mathf.Sin(i * theta)
            );
            Participants[i].ConversationMove(transform.position + radius * unit);
        }
    }

    public override void Join(AiPerson participant)
    {
        if (!Reserved.Remove(participant))
            throw new System.IndexOutOfRangeException("Participant's spot was not reserved");

        //var diff = Vector3.Normalize(transform.position - participant.transform.position);
        //var spot = ((Vector3.Cross(diff, Vector3.right).y + 1) / 2);
        //Participants.Insert(Mathf.RoundToInt(spot * Participants.Count), participant);

        var min = float.PositiveInfinity;
        var mini = 0;
        for (int i = 0; i < Participants.Count; ++i)
        {
            var dist = Vector3.Distance(
                Participants[i].transform.position,
                participant.transform.position
            );
            if (dist < min)
            {
                min = dist;
                mini = i;
            }
        }
        Participants.Insert(mini, participant);
        notifyJoined(participant);

        DistributeParticipants();
    }

    public override bool Leave(AiPerson participant)
    {
        if (base.Leave(participant))
        {
            DistributeParticipants();
            return true;
        }
        return false;
    }
}

