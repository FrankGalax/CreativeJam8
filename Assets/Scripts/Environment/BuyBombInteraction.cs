using System.Collections;
using UnityEngine;

public class BuyBombInteraction : MonoBehaviour
{
    public int Cost;

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

        if (m_PlayerInside && m_PlayerController.IsInteracting)
        {
            Player player = m_PlayerController.GetComponent<Player>();
            if (player.Gold >= Cost)
            {
                player.RemoveGold(Cost);
                player.Bombs++;
            }
        }
    }
}