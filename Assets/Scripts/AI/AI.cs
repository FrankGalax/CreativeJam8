﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public int Hp = 3;
    public float ShrinkSpeed = 0.5f;
    public float ShrinkRatio = 0.25f;
    public int GoldReward = 10;
    public float DamageCooldown;
    public int GoldDamage = 5;
    public AudioClip SpawnClip;
    public AudioClip KillClip;

    protected NavMeshAgent m_NavMeshAgent;
    protected float m_NavMeshSpeed;
    protected int m_CurrentHp;
    protected bool m_IsUpdatingSize;
    protected bool m_Shrunk;
    protected Transform m_Player;

    private float m_DamageTimer;

    void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        m_Player = Helpers.GetObjectWithTag("Player").transform;
        m_CurrentHp = Hp;
        m_Shrunk = false;
        m_IsUpdatingSize = false;
        m_NavMeshSpeed = m_NavMeshAgent != null ? m_NavMeshAgent.speed : 0.0f;

        AudioSource.PlayClipAtPoint(SpawnClip, Camera.main.transform.position);
        DoStart();
    }
    protected virtual void DoStart() { }

    void Update()
    {
        m_DamageTimer = Mathf.Max(m_DamageTimer - Time.deltaTime, 0.0f);

        DoUpdate();
    }
    protected virtual void DoUpdate() { }

    void OnGUI()
    {
        float width = (float)m_CurrentHp / (float)Hp * 40.0f;
        Rect position = new Rect(Camera.main.WorldToScreenPoint(transform.position), new Vector2(width, 5));
        position.y = Screen.height - position.y - 50;
        position.x -= width / 2.0f;

        GUI.DrawTexture(position, ResourceManager.GetTexture("red"));
    }

    void OnTriggerEnter(Collider collider)
    {
        UnshrinkExplosion unshrinkExplosion = collider.GetComponent<UnshrinkExplosion>();
        if (unshrinkExplosion != null && m_Shrunk)
        {
            unshrinkExplosion.OnAIEnter(this);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "Player"))
        {
            if (!m_Shrunk && m_DamageTimer <= 0)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(GoldDamage);
                m_DamageTimer = DamageCooldown;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (m_Shrunk || m_IsUpdatingSize)
            return;

        m_CurrentHp -= damage;
        if (m_CurrentHp <= 0)
        {
            m_CurrentHp = 0;
            Shrink();
        }
    }

    public virtual void Die()
    {
        WaveManager.Instance.AddKill();
        AudioSource.PlayClipAtPoint(KillClip, Camera.main.transform.position);
        Destroy(gameObject);
    }

    protected virtual void Shrink()
    {
        m_IsUpdatingSize = true;
        StartCoroutine(ShrinkAnim());
    }

    public bool IsShrunk { get { return m_Shrunk; } }

    private IEnumerator ShrinkAnim()
    {
        float timer = ShrinkSpeed;
        while (timer > 0)
        {
            float scale = Mathf.Lerp(ShrinkRatio, 1.0f, timer / ShrinkSpeed);
            transform.localScale = new Vector3(scale, scale, scale);
            timer -= Time.fixedDeltaTime;

            if (m_NavMeshAgent != null)
            {
                m_NavMeshAgent.speed = scale * m_NavMeshSpeed;
            }
            yield return new WaitForFixedUpdate();
        }
        m_Shrunk = true;
        m_IsUpdatingSize = false;
    }

    public void Unshrink()
    {
        m_CurrentHp = Hp;
        m_IsUpdatingSize = true;
        StartCoroutine(UnshrinkAnim());
    }

    private IEnumerator UnshrinkAnim()
    {
        float timer = ShrinkSpeed;
        float navMeshSpeed = m_NavMeshAgent != null ? m_NavMeshAgent.speed : 0.0f;
        while (timer > 0)
        {
            float scale = Mathf.Lerp(1.0f, ShrinkRatio, timer / ShrinkSpeed);
            transform.localScale = new Vector3(scale, scale, scale);
            timer -= Time.fixedDeltaTime;

            if (m_NavMeshAgent != null)
            {
                m_NavMeshAgent.speed = navMeshSpeed / scale;
            }
            yield return new WaitForFixedUpdate();
        }
        m_Shrunk = false;
        m_IsUpdatingSize = false;
    }
}
