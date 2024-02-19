using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPanelManagerr : MonoBehaviour
{
	[SerializeField] private List<ButtonHandler> buttons;
	[SerializeField] private Button playButton;
	[SerializeField] private Color chooseColor;
	[SerializeField] private Color unavaliableColor;

	private void Start()
	{
		playButton.interactable = false;

		RefreshButtons();
	}

	private void RefreshButtons()
	{
		var currentLevelIndex = KeyValueData.SavedData.f_level - 1;

		for (int i = 0; i < buttons.Count; i++)
		{
			if (i == currentLevelIndex)
			{
				buttons[i].Button.interactable = true;
				buttons[i].Check.enabled = false;
				buttons[i].Text.enabled = true;
				buttons[i].Text.color = Color.white;
			}
			else
			{
				if (i < currentLevelIndex)
				{
					buttons[i].Button.interactable = false;
					buttons[i].Check.enabled = true;
					buttons[i].Text.enabled = false;
					buttons[i].Text.color = unavaliableColor;
				}
				else
				{
					buttons[i].Button.interactable = false;
					buttons[i].Check.enabled = false;
					buttons[i].Text.enabled = true;
					buttons[i].Text.color = unavaliableColor;
				}
			}


			buttons[i].Text.text = (i + 1).ToString();
		}
	}

	public void OnButtonClick(int index)
	{
		RefreshButtons();

		playButton.interactable = true;
		buttons[index].Text.text = "GO!";
		buttons[index].Text.color = chooseColor;
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("CircleRunScene");
	}
}
