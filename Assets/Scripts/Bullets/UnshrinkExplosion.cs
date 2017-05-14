using UnityEngine;
using System.Collections.Generic;

public class UnshrinkExplosion : MonoBehaviour
{
    public float MaxRadius = 1.5f;
    public float ExpandTime = 1.0f;
    public AudioClip AudioClip;
    
    private float m_ExpandTimer;
    private List<Transform> m_AffectedAIs;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        m_ExpandTimer = 0.0f;
        m_AffectedAIs = new List<Transform>();
        AudioSource.PlayClipAtPoint(AudioClip, transform.position);
    }

    void FixedUpdate()
    {
        m_ExpandTimer += Time.fixedDeltaTime;

        float scale = m_ExpandTimer / ExpandTime * MaxRadius;
        transform.localScale = new Vector3(scale, 1, scale);

        if (m_ExpandTimer > ExpandTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnAIEnter(AI ai)
    {
        if (m_AffectedAIs.Contains(ai.transform))
            return;
        m_AffectedAIs.Add(ai.transform);

        ai.Unshrink();
    }
}