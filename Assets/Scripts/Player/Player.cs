using UnityEngine;

public class Player : MonoBehaviour
{
    public int Gold;

    void Start()
    {
        Gold = 0;
    }

    public void RemoveGold(int gold)
    {
        Gold -= gold;
        if (Gold < 0)
            Gold = 0;
    }
}