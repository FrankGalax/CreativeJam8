using UnityEngine;

public class Archetype1 : AI
{
    private Transform m_Player;

    protected override void DoStart()
    {
        m_Player = Helpers.GetObjectWithTag("Player").transform;
    }

    protected override void DoUpdate()
    {
        m_NavMeshAgent.destination = m_Player.position;
    }

    protected override void Shrink()
    {
        base.Shrink();
    }
}
