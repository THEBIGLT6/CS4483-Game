using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private Slider m_musicSlider;
    [SerializeField] private Slider m_fxSlider;
    [SerializeField] private Slider m_sensitivitySlider;

    void Start()
    {
        setSliderPositions();
    }

    private void setSliderPositions()
    {
        GameSettings settings = SettingsManager.Instance.GetSettings();
        m_musicSlider.value = settings.musicVolume;
        m_fxSlider.value = settings.sfxVolume;

        m_sensitivitySlider.minValue = GameSettings.MIN_MOUSE_SENSITVITY;
        m_sensitivitySlider.maxValue = GameSettings.MAX_MOUSE_SENSITVITY;
        m_sensitivitySlider.value = settings.mouseSensitivity;
    }

    public void saveSettings()
    {
        GameSettings settings = SettingsManager.Instance.GetSettings();
        settings.musicVolume = m_musicSlider.value;
        settings.sfxVolume = m_fxSlider.value;
        settings.mouseSensitivity = m_sensitivitySlider.value;
        SettingsManager.Instance.applySettings();
    }

}
