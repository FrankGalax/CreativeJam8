using System.Collections;
using UnityEngine;

public class Archetype2 : AI
{
    public float FlightHeight = 2.0f;
    public float Speed = 1.0f;
    public float DropRadius = 1.0f;
    public float DropTime = 0.25f;
    public float LiftTime = 1.0f;

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
            m_State = State.WaitInAir;
            m_Timer = 1;
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
            m_Timer = 4;
        }
    }

    private void OnGround()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            m_State = State.Lift;
            m_Timer = LiftTime;
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
        }
    }
}