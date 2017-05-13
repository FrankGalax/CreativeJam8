using UnityEngine;

public class Player : MonoBehaviour
{
    public int Gold { get; set; }
    public int Bombs { get; set; }

    void Start()
    {
        Gold = 0;
        Bombs = 0;
    }

    public void RemoveGold(int gold)
    {
        Gold -= gold;
        if (Gold < 0)
            Gold = 0;
    }
}