using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Archetype3 : AI
{
    public float Range = 2.5f;
    public float FireCooldown = 3.0f;
    public Vector3 GunOffset;
   
    private float m_FireTimer;
    private Animator m_Animator;

    protected override void DoStart()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    protected override void DoUpdate()
    {
        if (m_FireTimer > 0)
        {
            m_FireTimer -= Time.deltaTime;
            m_Animator.SetBool("Shoot", false);
        }
        else
        {
            Vector3 position = transform.position + transform.TransformVector(GunOffset);
            Instantiate(ResourceManager.GetPrefab("Archetype3Bullet"), position, transform.rotation);
            m_FireTimer = FireCooldown;
            m_Animator.SetBool("Shoot", true);
        }

        List<AI> ais = FindObjectsOfType<AI>().Where(p => p.IsShrunk).ToList();
        if (ais.Count == 0)
            return;

        Vector3 averagePosition = Vector3.zero;
        ais.ForEach(p => averagePosition += new Vector3(p.transform.position.x, 0, p.transform.position.z));
        averagePosition /= ais.Count;
        m_NavMeshAgent.destination = averagePosition;
    }
}