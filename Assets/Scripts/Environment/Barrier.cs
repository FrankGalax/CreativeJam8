using System.Collections;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public bool Unlocked { get; private set; }
    public float LiftTime = 2.0f;

    public void Unlock()
    {
        Unlocked = true;
        StartCoroutine(UnlockAnim());
    }

    private IEnumerator UnlockAnim()
    {
        float time = LiftTime;
        Vector3 initialPosition = transform.position;
        while (time > 0)
        {
            Vector3 position = initialPosition;
            position.y = Mathf.Lerp(0.0f, initialPosition.y, time / LiftTime);
            transform.position = position;
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}