using UnityEngine;

public class Archetype3 : AI
{
    public float Range = 5.0f;

    enum State
    {
        GetClose,
        Spit
    }
    private State m_State;
    private float m_NavMeshSpeed;

    protected override void DoStart()
    {
        m_State = State.GetClose;
    }

    protected override void DoUpdate()
    {
        switch (m_State)
        {
            case State.GetClose:
                GetClose();
                break;
            case State.Spit:
                Spit();
                break;
        }
    }

    private void GetClose()
    {
        m_NavMeshAgent.destination = m_Player.position;

        float distanceSq = (m_Player.position - transform.position).sqrMagnitude;
        if (distanceSq < Range* Range)
        {
            m_State = State.Spit;
            m_NavMeshSpeed = m_NavMeshAgent.speed;
        }
    }

    private void Spit()
    {
        m_NavMeshAgent.destination = m_Player.position;
        m_NavMeshAgent.speed = 0.0f;

        float distanceSq = (m_Player.position - transform.position).sqrMagnitude;
        if (distanceSq > Range * Range)
        {
            m_State = State.GetClose;
            m_NavMeshAgent.speed = m_NavMeshSpeed;
        }
    }
}