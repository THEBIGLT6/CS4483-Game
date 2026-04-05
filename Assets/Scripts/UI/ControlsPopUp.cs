using UnityEngine;

public class ControlsPopup : MonoBehaviour
{
    void Start()
{
    gameObject.SetActive(true);
    Time.timeScale = 0f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // Find and wire up the button automatically
    UnityEngine.UI.Button closeBtn = GetComponentInChildren<UnityEngine.UI.Button>();
    if (closeBtn != null)
        closeBtn.onClick.AddListener(ClosePopup);
}

    public void ClosePopup()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}