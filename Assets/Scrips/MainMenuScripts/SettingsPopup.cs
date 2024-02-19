using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
	[SerializeField] private GameObject checkMarkMusic;
	[SerializeField] private GameObject checkMarkEffects;
	private ActiveSound activeSound;

	private void Start()
	{
		activeSound = FindFirstObjectByType<ActiveSound>();

		checkMarkMusic.SetActive(KeyValueData.SavedData.f_volume);
		checkMarkEffects.SetActive(KeyValueData.SavedData.f_sfx);
	}

	public void ToggleMusicMark()
	{
		checkMarkMusic.SetActive(!checkMarkMusic.activeSelf);
		activeSound.ToggleActiveSound(checkMarkMusic.activeSelf);


		KeyValueData.SavedData.f_volume = checkMarkMusic.activeSelf;
		KeyValueData.SaveProgress();
	}

	public void ToggleEffectsMark()
	{
		checkMarkEffects.SetActive(!checkMarkEffects.activeSelf);
		KeyValueData.SavedData.f_sfx = checkMarkEffects.activeSelf;
		KeyValueData.SaveProgress();
	}
}
