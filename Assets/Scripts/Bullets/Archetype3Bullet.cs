using UnityEngine;

public class Archetype3Bullet : Bullet
{
    public float ExplodeTime = 1.5f;
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
        if (!Helpers.CheckObjectTag(collision.gameObject, "AI"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ResourceManager.GetPrefab("UnshrinkExplosion"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}