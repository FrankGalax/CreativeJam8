using System.Collections;
using UnityEngine;

public class UnlockInteraction : MonoBehaviour
{
    public Barrier Barrier;
    public int Cost;
    public Transform MovePlayer;

    private bool m_PlayerInside;
    private PlayerController m_PlayerController;

    void Awake()
    {
        m_PlayerController = Helpers.GetObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        m_PlayerInside = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Helpers.CheckObjectTag(collider.gameObject, "Player"))
        {
            m_PlayerInside = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (Helpers.CheckObjectTag(collider.gameObject, "Player"))
        {
            m_PlayerInside = false;
        }
    }

    void Update()
    {
        if (!WaveManager.Instance.IsCooldown)
            return;

        if (m_PlayerInside && m_PlayerController.IsInteracting && Barrier != null && !Barrier.Unlocked)
        {
            Player player = m_PlayerController.GetComponent<Player>();
            if (player.Gold >= Cost)
            {
                player.RemoveGold(Cost);
                Barrier.Unlock();
                StartCoroutine(MovePlayerAnim());
            }
        }
    }

    private IEnumerator MovePlayerAnim()
    {
        Vector3 initialPlayerPosition = m_PlayerController.transform.position;
        Vector3 move = MovePlayer.position - initialPlayerPosition;
        float animTime = move.magnitude / m_PlayerController.Speed;
        float time = animTime;

        m_PlayerController.enabled = false;

        while (time > 0)
        {
            m_PlayerController.transform.position = Vector3.Lerp(MovePlayer.position, initialPlayerPosition, time / animTime);
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        m_PlayerController.enabled = true;
        Destroy(gameObject);
    }
}