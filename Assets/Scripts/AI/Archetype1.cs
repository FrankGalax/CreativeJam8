using UnityEngine;

public class Archetype1 : AI
{
    public float StunTime = 1.0f;

    private bool m_IsStunned;
    private float m_StunTimer;

    protected override void DoStart()
    {
        m_IsStunned = false;
    }

    protected override void DoUpdate()
    {
        if (m_IsStunned)
        {
            m_NavMeshAgent.destination = transform.position;
            m_StunTimer -= Time.deltaTime;
            if (m_StunTimer < 0)
            {
                m_IsStunned = false;
            }
        }
        else
        {
            m_NavMeshAgent.destination = m_Player.position;
        }
    }

    public void Stun()
    {
        m_IsStunned = true;
        m_StunTimer = StunTime;
    }
}
