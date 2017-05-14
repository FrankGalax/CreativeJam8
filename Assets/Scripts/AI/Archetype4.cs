using UnityEngine;

public class Archetype4 : AI
{
    public float FireRange = 3.0f;
    public float FireCooldown = 2.0f;
    public float EnragedFireCooldown = 2.0f;
    public Vector3 GunOffset;
    public float EnragedNavMeshSpeed = 3.0f;

    enum State
    {
        Normal,
        Fire,
        Enraged
    }
    private State m_State;
    private float m_FireTimer;

    protected override void DoStart()
    {
        m_State = State.Normal;
    }

    protected override void DoUpdate()
    {
        if (m_State != State.Enraged)
        {
            if ((float)m_CurrentHp / (float)Hp < 0.5f)
            {
                m_State = State.Enraged;
                m_FireTimer = 0;
                m_NavMeshAgent.speed = EnragedNavMeshSpeed;
            }
        }

        switch (m_State)
        {
            case State.Normal:
                Normal();
                break;
            case State.Fire:
                Fire();
                break;
            case State.Enraged:
                Enraged();
                break;
        }
    }

    private void Normal()
    {
        m_NavMeshAgent.destination = m_Player.position;

        Vector3 toPlayer = m_Player.position - transform.position;
        if (toPlayer.magnitude < FireRange)
        {
            m_State = State.Fire;
            m_FireTimer = 0.0f;
        }
    }

    private void Fire()
    {
        m_NavMeshAgent.destination = transform.position;
        transform.rotation = Quaternion.LookRotation((m_Player.position - transform.position).normalized);

        m_FireTimer -= Time.deltaTime;
        if (m_FireTimer < 0)
        {
            Vector3 position = transform.position + transform.TransformVector(GunOffset);
            Instantiate(ResourceManager.GetPrefab("Archetype4Bullet"), position, transform.rotation).GetComponent<Archetype4Bullet>().ShouldStun = true;
            m_FireTimer = FireCooldown;
        }

        Vector3 toPlayer = m_Player.position - transform.position;
        if (toPlayer.magnitude >= FireRange)
        {
            m_State = State.Normal;
        }
    }

    private void Enraged()
    {
        m_FireTimer -= Time.deltaTime;
        if (m_FireTimer < 0)
        {
            Vector3 position = transform.position + transform.TransformVector(GunOffset);

            for (int i = 0; i < 6; ++i)
            {
                Quaternion rot = Quaternion.AngleAxis(i * 60, Vector3.up);
                Vector3 direction = rot * transform.forward;
                Instantiate(ResourceManager.GetPrefab("Archetype4Bullet"), transform.position + direction * 0.5f + Vector3.up * 0.5f, Quaternion.LookRotation(direction)).GetComponent<Archetype4Bullet>().ShouldStun = false;
            }

            m_FireTimer = FireCooldown;
        }

        m_NavMeshAgent.destination = m_Player.position;
    }
}