using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public int Hp = 3;
    public float ShrinkSpeed = 0.5f;
    public float ShrinkRatio = 0.25f;
    public int GoldReward = 10;

    protected NavMeshAgent m_NavMeshAgent;
    protected int m_CurrentHp;
    protected bool m_Shrunk;
    protected Transform m_Player;

    void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        m_Player = Helpers.GetObjectWithTag("Player").transform;
        m_CurrentHp = Hp;
        m_Shrunk = false;

        DoStart();
    }
    protected virtual void DoStart() { }

    void Update()
    {
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

    public void TakeDamage(int damage)
    {
        if (m_Shrunk)
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
        Destroy(gameObject);
    }

    protected virtual void Shrink()
    {
        m_Shrunk = true;
        StartCoroutine(ShrinkAnim());
    }

    public bool IsShrunk { get { return m_Shrunk; } }

    private IEnumerator ShrinkAnim()
    {
        float timer = ShrinkSpeed;
        float navMeshSpeed = m_NavMeshAgent.speed;
        while (timer > 0)
        {
            float scale = Mathf.Lerp(ShrinkRatio, 1.0f, timer / ShrinkSpeed);
            transform.localScale = new Vector3(scale, scale, scale);
            timer -= Time.fixedDeltaTime;

            m_NavMeshAgent.speed = scale * navMeshSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Unshrink()
    {
        m_CurrentHp = Hp;
        m_Shrunk = false;
        StartCoroutine(UnshrinkAnim());
    }

    private IEnumerator UnshrinkAnim()
    {
        float timer = ShrinkSpeed;
        float navMeshSpeed = m_NavMeshAgent.speed;
        while (timer > 0)
        {
            float scale = Mathf.Lerp(1.0f, ShrinkRatio, timer / ShrinkSpeed);
            transform.localScale = new Vector3(scale, scale, scale);
            timer -= Time.fixedDeltaTime;

            m_NavMeshAgent.speed = navMeshSpeed / scale;
            yield return new WaitForFixedUpdate();
        }
    }
}
