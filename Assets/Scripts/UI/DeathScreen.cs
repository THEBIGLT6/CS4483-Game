using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class DeathScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text m_moneyText;
    [SerializeField] private TMP_Text m_roundText;
    [SerializeField] private TMP_Text m_titleText;

    private static DeathScreen _instance;
    public static DeathScreen Instance => _instance;

    private void Awake()
    {
        _instance = this;
        gameObject.SetActive(false); 
    }

    public void Show()
    {
        gameObject.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Save progress
        //GameManager.Instance.Save();

        // Display stats
        int money = GameManager.Instance.getMoney();
        int round = GameManager.Instance.getCurrentStage();

        m_titleText.text = "YOU DIED";
        m_moneyText.text = "Money Collected: $" + money.ToString("D4");
        m_roundText.text = "Round Reached: " + round;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}