﻿using UnityEngine;

public class Archetype4Bullet : Bullet
{
    public float ExplodeTime = 1.5f;
    public int GoldDamageOnPlayer = 10;
    public bool ShouldStun;
    private float m_ExplodeTimer;

    protected override void DoStart()
    {
        m_ExplodeTimer = ExplodeTime;
    }

    protected override void DoUpdate()
    {
        m_ExplodeTimer -= Time.deltaTime;
        if (m_ExplodeTimer < 0)
        {
            Explode();
        }
    }

    protected override void DoOnCollisionEnter(Collision collision)
    {
        if (!Helpers.CheckObjectTag(collision.gameObject, "AI") && collision.gameObject.GetComponent<Archetype4Bullet>() == null)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ResourceManager.GetPrefab("StunExplosion"), transform.position, Quaternion.identity).GetComponent<StunExplosion>().ShouldStun = ShouldStun;
        Destroy(gameObject);
    }
}