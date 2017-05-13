using UnityEngine;

public class Archetype1 : AI
{
    protected override void DoUpdate()
    {
        m_NavMeshAgent.destination = m_Player.position;
    }
}
