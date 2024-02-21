using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneLoader : MonoBehaviour
{
	[SerializeField] private Rotator rotator;
	[SerializeField] private EducationScene educationScene;
	[SerializeField] private CircleLevelController circleLevelController;
	[SerializeField] private Image throwsImage;
	[SerializeField] private Image progressImage;
	[SerializeField] private TMP_Text throwsText;
	[SerializeField] private TMP_Text progressText;
	[SerializeField] private LevelLoseScene levelLoseScene;
	private int levelReward;
	private int throwsLeft;
	private int allThrows;
	private int allProgress;
	private int currentProgress;

	public void Enter()
	{
		GetLevelStatistics();
	}

	private void Start()
	{
		if (KeyValueData.SavedData.f_tutorial)
		{
			KeyValueData.SavedData.f_tutorial = false;
			KeyValueData.SaveProgress();

			educationScene.ShowMessage(OnEducationEnd);
		}
		else
		{
			OnEducationEnd();
		}

		GetLevelStatistics();
		RefreshBars();
	}

	private void OnEducationEnd()
	{
		rotator.AllowTouchInput();
		rotator.AllowedExternalInput = true;

		rotator.TriggerEnter += OnRotatorTriggerEnter;
		rotator.CirclePass += OnCirclePassed;
	}

	private void OnRotatorTriggerEnter()
	{
		throwsLeft--;

		if (throwsLeft < 0)
		{
			throwsLeft = 0;
			LoseLevel();
		}

		RefreshBars();
	}

	private void LoseLevel()
	{
		UnSubscribeFromRotator();
		levelLoseScene.ShowLose();
	}

	private void WinLevel()
	{
		UnSubscribeFromRotator();
		levelLoseScene.ShowWin(levelReward);

		KeyValueData.SavedData.f_gold += levelReward;
		KeyValueData.SavedData.f_level += 1;
		KeyValueData.SaveProgress();
	}

	private void OnCirclePassed(RunCircle runCircle)
	{
		if (runCircle.IsPointed) return;
		runCircle.Point();

		currentProgress = circleLevelController.GetIndex(runCircle);

		if (currentProgress >= allProgress)
		{
			WinLevel();
		}

		RefreshBars();
	}

	private void UnSubscribeFromRotator()
	{
		rotator.TriggerEnter -= OnRotatorTriggerEnter;
		rotator.CirclePass -= OnCirclePassed;
	}

	private void RefreshBars()
	{
		throwsImage.fillAmount = (float)throwsLeft / (float)allThrows;
		throwsText.text = $"{throwsLeft}/{allThrows}";

		progressImage.fillAmount = (float)currentProgress / (float)allProgress;
		progressText.text = $"{currentProgress}/{allProgress}";
	}

	private void GetLevelStatistics()
	{
		var x = (float)KeyValueData.SavedData.f_level;
		throwsLeft = (int)Mathf.Sqrt(Mathf.Pow(x, 2.2f)) + 2;
		allThrows = (int)Mathf.Sqrt(Mathf.Pow(x, 2.2f)) + 2;
		allProgress = circleLevelController.Circles.Count - 1;
		levelReward = 1;
	}
}
