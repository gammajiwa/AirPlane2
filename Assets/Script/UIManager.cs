using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject gameOverWindow;

    public void UpdateHpBar(float playerHp) {
        hpBar.fillAmount = playerHp / 10;
    }

    public void GameOver() {
        gameOverWindow.SetActive(true);
    }
}
