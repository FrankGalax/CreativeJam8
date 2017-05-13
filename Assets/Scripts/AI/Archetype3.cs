using UnityEngine;

public class Archetype3 : AI
{
    public float Range = 2.5f;
    public float FireCooldown = 3.0f;
    public Vector3 GunOffset;

    enum State
    {
        GetClose,
        Spit
    }
    private State m_State;
    private float m_NavMeshSpeed;
    private float m_FireTimer;

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
            m_FireTimer = FireCooldown;
        }
    }

    private void Spit()
    {
        transform.rotation = Quaternion.LookRotation((m_Player.position - transform.position).normalized);
        m_NavMeshAgent.speed = 0.0f;

        if (m_FireTimer > 0)
        {
            m_FireTimer -= Time.deltaTime;
        }
        else
        {
            Vector3 position = transform.position + transform.TransformVector(GunOffset);
            Instantiate(ResourceManager.GetPrefab("Archetype3Bullet"), position, transform.rotation);
            m_FireTimer = FireCooldown;
        }

        float distanceSq = (m_Player.position - transform.position).sqrMagnitude;
        if (distanceSq > Range * Range)
        {
            m_State = State.GetClose;
            m_NavMeshAgent.speed = m_NavMeshSpeed;
        }
    }
}