using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Text m_GoldText;
    private Player m_Player;

    void Start()
    {
        m_Player = Helpers.GetObjectWithTag("Player").GetComponent<Player>();
        m_GoldText = transform.Find("GoldPanel").Find("GoldText").GetComponent<Text>();
    }

    void Update()
    {
        m_GoldText.text = "Gold : " + m_Player.Gold.ToString();
    }
}