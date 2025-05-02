using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject wavePanel;
    public GameObject resultPanel;

    public TextMeshProUGUI waveText;
    public Button startButton;
    public TextMeshProUGUI waveLabel;
    public TextMeshProUGUI scoreCount;
    public TextMeshProUGUI customerCount;

    GameManager gm;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }

        gm = GameManager.Instance;

        resultPanel.SetActive(false);
        wavePanel.SetActive(true);

    }

    public bool StopCamera()
    {
        return wavePanel.activeInHierarchy;
    }

    public void ShowUI(bool value)
    {
        resultPanel.SetActive(value);
        wavePanel.SetActive(value);

        Cursor.visible = UIManager.Instance.StopCamera();
        Cursor.lockState = UIManager.Instance.StopCamera() ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void UpdateUI()
    {
        waveText.text = "Wave " + gm.currentWave;
        waveLabel.text = "Wave " + gm.currentWave;
        scoreCount.text = "" + gm.score;
        customerCount.text = "" + gm.customersServed;
    }

}
