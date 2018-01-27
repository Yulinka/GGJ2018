using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Conversation : Activity
{
    public float RingScale;
    public float ActorSize;
    public float RingPadding;

    public Sprite[] Sprites;

    private void Start()
    {
        RingScale = 2f;
        ActorSize = 0.75f;
        RingPadding = 0.4f;
    }

    public override string GetName()
    {
        return "Conversation";
    }

    public override float GetApproachDistance()
    {
        if (Participants.Count == 0)
            return 0;

        float scale = (float)Participants.Count / (float)MaxParticipants;
        float radius = (ActorSize + RingPadding + 1f * RingScale) * scale;

        return radius;

    }

    void DistributeParticipants()
    {
        if (Participants.Count == 0)
            return;
        
        if(Participants.Count == 1)
        {
            Participants[0].ConversationMove(transform.position);
            return;
        }

        float theta = (360 / Participants.Count);
        float scale = (float)Participants.Count / (float)MaxParticipants;

        for (int i = 0; i < Participants.Count; ++i)
        {
            float angle = theta * i;
            float radius = (ActorSize + RingPadding * RingScale) * scale;

            Vector3 offset = Vector3.zero;
            offset.x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            offset.z = radius * Mathf.Cos(angle * Mathf.Deg2Rad);

            Participants[i].ConversationMove(transform.position + offset);
        }
    }

    public override void Join(AiPerson participant)
    {
        base.Join(participant);
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
