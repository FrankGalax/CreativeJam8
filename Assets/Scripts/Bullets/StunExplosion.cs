using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class StunExplosion : MonoBehaviour
{
    public float MaxRadius = 1.5f;
    public float ExpandTime = 1.0f;
    public AudioClip AudioClip;
    public float StunTime = 0.5f;
    public int GoldDamageOnPlayer = 10;
    public bool ShouldStun { get; set; }

    private float m_ExpandTimer;
    private bool m_AffectedPlayer;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        m_ExpandTimer = 0.0f;
        m_AffectedPlayer = false;
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

    public void OnPlayerEnter(PlayerController playerController)
    {
        if (m_AffectedPlayer)
            return;
        
        m_AffectedPlayer = true;
        if (ShouldStun)
        {
            StartCoroutine(StunPlayer(playerController));
        }
        else
        {
            playerController.TakeDamage(GoldDamageOnPlayer);
        }
    }

    private IEnumerator StunPlayer(PlayerController playerController)
    {
        if (!playerController.IsStunned)
        {
            playerController.TakeDamage(GoldDamageOnPlayer);
            playerController.IsStunned = true;
            yield return new WaitForSeconds(StunTime);
            playerController.IsStunned = false;
        }
    }
}