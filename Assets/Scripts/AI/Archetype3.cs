using UnityEngine;

public class Archetype3 : AI
{
    protected override void DoUpdate()
    {
        m_NavMeshAgent.destination = m_Player.position;
    }
}