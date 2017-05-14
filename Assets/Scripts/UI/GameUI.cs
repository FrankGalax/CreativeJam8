using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Text m_GoldText;
    private Text m_WaveText;
    private Text m_WaveMessage;
    private RectTransform m_WavePanel;
    private RectTransform m_GoldPanel;
    private RectTransform m_EndPanel;

    private Player m_Player;
    private bool m_IsEnded;

    void Awake()
    {
        m_EndPanel = transform.Find("EndPanel").GetComponent<RectTransform>();
        m_EndPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        m_Player = Helpers.GetObjectWithTag("Player").GetComponent<Player>();
        m_GoldPanel = transform.Find("GoldPanel").GetComponent<RectTransform>();
        m_GoldText = transform.Find("GoldPanel").Find("GoldText").GetComponent<Text>();
        m_WavePanel = transform.Find("WavePanel").GetComponent<RectTransform>();
        m_WaveText = transform.Find("WavePanel").Find("WaveText").GetComponent<Text>();
        m_WaveMessage = transform.Find("WavePanel").Find("WaveMessage").GetComponent<Text>();
        
        m_IsEnded = false;
    }

    void Update()
    {
        if (m_IsEnded)
            return;

        if (WaveManager.Instance.IsEnded)
        {
            End();
        }
        else
        {
            m_GoldText.text = "Gold : " + m_Player.Gold + ", Bombs : " + m_Player.Bombs;

            bool isCooldown = WaveManager.Instance.IsCooldown;
            if (isCooldown)
            {
                m_WaveText.text = FormatTime(WaveManager.Instance.CooldownTimer);
                if (WaveManager.Instance.IsBossWave)
                {
                    m_WaveMessage.text = "Boss Wave Incoming";
                }
                else
                {
                    m_WaveMessage.text = "Wave " + (WaveManager.Instance.CurrentWave + 1) + " Incoming";
                }
            }
            else
            {
                m_WaveText.text = WaveManager.Instance.AIKills + " / " + WaveManager.Instance.AICount;
                m_WaveMessage.text = "SHRINK'EM UP!";
            }
        }
    }

    private string FormatTime(float seconds)
    {
        int secondsInt = (int)Mathf.Ceil(seconds);
        int minutes = secondsInt / 60;
        secondsInt = secondsInt % 60;
        return (minutes >= 10 ? "" : "0") + minutes + ":" + (secondsInt >= 10 ? "" : "0") + secondsInt;
    }

    private void End()
    {
        m_GoldPanel.gameObject.SetActive(false);
        m_WavePanel.gameObject.SetActive(false);
        m_EndPanel.gameObject.SetActive(true);
        m_EndPanel.transform.Find("GoldText").GetComponent<Text>().text = "You shrunk'em up good and still have " + m_Player.Gold + " gold!";
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }
}