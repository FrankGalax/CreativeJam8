using UnityEngine;

public class Archetype3Bullet : Bullet
{
    protected override void DoOnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "Player"))
        {
            Instantiate(ResourceManager.GetPrefab("UnshrinkExplosion"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}