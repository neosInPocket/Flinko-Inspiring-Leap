using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainLayerManager : MonoBehaviour
{
	[SerializeField] private Spinning spinning;
	[SerializeField] private StartCutScene cutScene;
	[SerializeField] private ShellLevelManager shellLevel;
	[SerializeField] private Image passesLeft;
	[SerializeField] private Image alreadyCompletedImage;
	[SerializeField] private TMP_Text passesLeftText;
	[SerializeField] private TMP_Text alreadyCompletedText;
	[SerializeField] private TMP_Text shardsRewardText;
	[SerializeField] private TMP_Text currentPlayOrderText;
	[SerializeField] private UnPassed unPassed;
	[HideInInspector] public int shardsRewarded;
	[HideInInspector] public int passesLeftAmount;
	[HideInInspector] public int maximumPassesAmount;
	[HideInInspector] public int maximumProgressAmount;
	[HideInInspector] public int currentCompletedAmount;

	public void Start()
	{
		CheckInstructionsPassed();
		ResizeTopHolder();
	}

	public void CheckInstructionsPassed()
	{
		float xLayer = SerializedAudio.Singleton.Serializer.playOrder;
		passesLeftAmount = (int)Mathf.Sqrt(Mathf.Pow(xLayer, 2.4f)) + 2;
		maximumPassesAmount = (int)Mathf.Sqrt(Mathf.Pow(xLayer, 2.4f)) + 2;
		maximumProgressAmount = shellLevel.Shells.Count - 1;
		shardsRewarded = 30 + (int)xLayer * 3;
		shardsRewardText.text = shardsRewarded.ToString();
		currentPlayOrderText.text = $"level {(int)xLayer}";

		bool gameInstructions = SerializedAudio.Singleton.Serializer.gameInstructions;

		if (SerializedAudio.Singleton.Serializer.gameInstructions)
		{
			SerializedAudio.Singleton.Serializer.gameInstructions = false;
			SerializedAudio.Singleton.Serializer.MaintainSettingsValues();

			cutScene.StartCutSceneAction(CutSceneCompleted);
		}
		else
		{
			CutSceneCompleted();
		}
	}

	public void CutSceneCompleted()
	{
		spinning.EnableTouchPass();
		spinning.TouchInputEnabled = true;

		spinning.ShellEnterAction += SpinningShellEntered;
		spinning.ShellPassedAction += SpinningPassedAction;
	}

	public void SpinningShellEntered()
	{
		passesLeftAmount--;

		if (passesLeftAmount == 0)
		{
			passesLeftAmount = 0;
			LoseLayer();
		}

		ResizeTopHolder();
	}

	public void LoseLayer()
	{
		ReloadEventFunctions();
		unPassed.UnPassedShow();
	}

	public void PassLayer()
	{
		ReloadEventFunctions();
		unPassed.Passed(shardsRewarded);

		SerializedAudio.Singleton.Serializer.shards += shardsRewarded;
		SerializedAudio.Singleton.Serializer.playOrder += 1;
		SerializedAudio.Singleton.Serializer.MaintainSettingsValues();
	}

	public void SpinningPassedAction(ShellRenderer shellRenderer)
	{
		if (shellRenderer.IsPassed) return;
		shellRenderer.ActivatePointPass();

		currentCompletedAmount = shellLevel.ShellIndex(shellRenderer);

		if (currentCompletedAmount >= maximumProgressAmount)
		{
			PassLayer();
		}

		ResizeTopHolder();
	}

	public void ReloadEventFunctions()
	{
		spinning.ShellEnterAction -= SpinningShellEntered;
		spinning.ShellPassedAction -= SpinningPassedAction;
	}

	public void ResizeTopHolder()
	{
		passesLeft.fillAmount = (float)(passesLeftAmount) / (float)(maximumPassesAmount);
		passesLeftText.text = $"{passesLeftAmount}/{maximumPassesAmount}";

		alreadyCompletedImage.fillAmount = (float)currentCompletedAmount / (float)maximumProgressAmount;
		alreadyCompletedText.text = $"{currentCompletedAmount}/{maximumProgressAmount}";
	}
}
