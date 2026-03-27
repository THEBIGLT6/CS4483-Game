using TMPro;
using UnityEngine;

public class RegisterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_moneyText;

    void Start()
    {
        GameManager.Instance.RegisterUI(this);
    }

    public TMP_Text GetMoneyText()
    {
        return m_moneyText;
    }
}
