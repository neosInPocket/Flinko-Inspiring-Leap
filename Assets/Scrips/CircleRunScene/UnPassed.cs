using TMPro;
using UnityEngine;

public class UnPassed : MonoBehaviour
{
	public TMP_Text shards;
	public TMP_Text passedUhPassed;
	public TMP_Text passedButton;
	public Animator animatorController;

	public void Passed(int shardsAmount)
	{
		gameObject.SetActive(true);
		shards.text = shardsAmount.ToString();
		passedUhPassed.text = "level completed!";
		passedButton.text = "next level";
	}

	public void UnPassedShow(int shards = 0)
	{
		gameObject.SetActive(true);
		this.shards.text = shards.ToString();
		passedUhPassed.text = "level failed";
		passedButton.text = "try again";
	}
}
