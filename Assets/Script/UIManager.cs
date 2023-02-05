using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private Image hpBar;
	[SerializeField] private Image countingEnemyBar;
	[SerializeField] private Image BossBar;
	[SerializeField] private Image specialMovementBar;
	[SerializeField] private Image buffDurationBar;
	[SerializeField] private GameObject gameOverWindow;
	[SerializeField] private GameObject BossBarHpGameobject;
	[SerializeField] private GameObject BossBarCountingGameobject;
	[SerializeField] TextMeshProUGUI textGameOver;

	public void UpdateHpBar(float playerHp) {
		hpBar.fillAmount = playerHp / 10;
	}

	public void GameOver(string text) {
		gameOverWindow.SetActive(true);
		textGameOver.text = text;
	}

	public void EnemyCounting(float value, int maxEnemyCount) {
		countingEnemyBar.fillAmount = value / maxEnemyCount;
	}

	public void SpecialMovement(float duration, float maxDuration) {
		specialMovementBar.fillAmount = (maxDuration - duration) / maxDuration;
	}

	public void BuffDuration(float duration, float maxDuration) {
		buffDurationBar.fillAmount = duration / maxDuration;
	}

	public void BossHpBar(float value) {
		BossBar.fillAmount = value / 100;
	}

	public void BossBattles(bool value) {
		if (value){BossBarHpGameobject.SetActive(true); BossBarCountingGameobject.SetActive(false); BossHpBar(100); }
		else BossBarHpGameobject.SetActive(false); BossBarCountingGameobject.SetActive(true);
	}
}
