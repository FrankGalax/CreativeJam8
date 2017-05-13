using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Text m_GoldText;
    private Text m_WaveText;
    private Text m_WaveMessage;
    private Image m_WavePanel;
    private Image m_GoldPanel;

    private Player m_Player;

    void Start()
    {
        m_Player = Helpers.GetObjectWithTag("Player").GetComponent<Player>();
        m_GoldPanel = transform.Find("GoldPanel").GetComponent<Image>();
        m_GoldText = transform.Find("GoldPanel").Find("GoldText").GetComponent<Text>();
        m_WavePanel = transform.Find("WavePanel").GetComponent<Image>();
        m_WaveText = transform.Find("WavePanel").Find("WaveText").GetComponent<Text>();
        m_WaveMessage = transform.Find("WavePanel").Find("WaveMessage").GetComponent<Text>();
    }

    void Update()
    {
        m_GoldText.text = "Gold : " + m_Player.Gold.ToString();

        bool isCooldown = WaveManager.Instance.IsCooldown;
        if (isCooldown)
        {
            m_WaveText.text = FormatTime(WaveManager.Instance.CooldownTimer);
            m_WaveMessage.text = "Wave " + (WaveManager.Instance.CurrentWave + 1) + " Incoming";
        }
        else
        {
            m_WaveText.text = WaveManager.Instance.AIKills + " / " + WaveManager.Instance.AICount;
            m_WaveMessage.text = "SHRINK'EM UP!";
        }
    }

    private string FormatTime(float seconds)
    {
        int secondsInt = (int)Mathf.Ceil(seconds);
        int minutes = secondsInt / 60;
        secondsInt = secondsInt % 60;
        return (minutes >= 10 ? "" : "0") + minutes + ":" + (secondsInt >= 10 ? "" : "0") + secondsInt;
    }
}