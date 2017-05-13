using System.Collections;
using UnityEngine;

public class Archetype2 : AI
{
    public float FlightHeight = 2.0f;
    public float Speed = 1.0f;
    public float DropRadius = 1.0f;
    public float DropTime = 0.25f;
    public float LiftTime = 1.0f;
    public float InAirStunTime = 0.5f;
    public float OnGroundStunTime = 4.0f;

    enum State
    {
        Fly,
        WaitInAir,
        Drop,
        OnGround,
        Lift
    }
    private State m_State;
    private float m_Timer;

    protected override void DoStart()
    {
        m_State = State.Fly;
    }

    protected override void DoUpdate()
    {
        if (IsShrunk)
        {
            Vector3 position = transform.position;
            position.y = 0.0f;
            transform.position = position;
            return;
        }

        switch (m_State)
        {
            case State.Fly:
                Fly();
                break;
            case State.WaitInAir:
                WaitInAir();
                break;
            case State.Drop:
                Drop();
                break;
            case State.OnGround:
                OnGround();
                break;
            case State.Lift:
                Lift();
                break;
        }
    }

    private void Fly()
    {
        Vector3 playerDirection = m_Player.position - transform.position;
        playerDirection.y = 0;
        Vector3 playerDirectionNormalized = playerDirection.normalized;

        Vector3 newPosition = transform.position;

        newPosition.y = FlightHeight;
        newPosition += Speed * playerDirectionNormalized * Time.deltaTime;

        transform.position = newPosition;

        if (playerDirection.magnitude < DropRadius)
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000.0f, layerMask))
            {
                Debug.Log(hit.point);
                m_State = State.WaitInAir;
                m_Timer = InAirStunTime;
            }
        }
    }

    private void WaitInAir()
    {
        m_Timer -= Time.deltaTime;
        Vector3 position = transform.position;
        position.y = FlightHeight;
        transform.position = position;
        if (m_Timer <= 0)
        {
            m_State = State.Drop;
            m_Timer = DropTime;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    private void Drop()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer > 0)
        {
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(0, FlightHeight, m_Timer / DropTime);
            transform.position = position;
        }
        else
        {
            m_State = State.OnGround;
            m_Timer = OnGroundStunTime;
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    private void OnGround()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            m_State = State.Lift;
            m_Timer = LiftTime;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    private void Lift()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer > 0)
        {
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(FlightHeight, 0, m_Timer / LiftTime);
            transform.position = position;
        }
        else
        {
            m_State = State.Fly;
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}