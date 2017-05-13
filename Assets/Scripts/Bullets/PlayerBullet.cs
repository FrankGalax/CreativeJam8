using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override void DoOnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "AI"))
        {
            AI ai = collision.gameObject.GetComponent<AI>();
            ai.TakeDamage(1);
            if (!ai.IsShrunk)
                Destroy(gameObject);
        }
    }
}
