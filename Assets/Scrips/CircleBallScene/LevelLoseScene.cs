using TMPro;
using UnityEngine;

public class LevelLoseScene : MonoBehaviour
{
	[SerializeField] private TMP_Text gained;
	[SerializeField] private TMP_Text result;
	[SerializeField] private TMP_Text button;
	[SerializeField] private Animator viewAnimator;

	public void ShowWin(int coins)
	{
		gameObject.SetActive(true);
		gained.text = coins.ToString();
		result.text = "YOU WIN!";
		button.text = "NEXT LEVEL";
	}

	public void ShowLose()
	{
		gameObject.SetActive(true);
		gained.text = "0";
		result.text = "YOU LOSE";
		button.text = "RETRY";
	}

	public void MenuAnimation()
	{
		viewAnimator.SetTrigger("menu");
	}

	public void NextLevelAnimation()
	{
		viewAnimator.SetTrigger("next");
	}
}
