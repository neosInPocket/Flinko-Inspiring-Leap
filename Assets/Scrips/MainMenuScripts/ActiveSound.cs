using System.Linq;
using UnityEngine;

public class ActiveSound : MonoBehaviour
{
	[SerializeField] private AudioSource sourceOfMusic;

	private void Awake()
	{
		var manager = FindObjectsByType<ActiveSound>(sortMode: FindObjectsSortMode.None).FirstOrDefault(x => x.gameObject.scene.name == "DontDestroyOnLoad");
		var condition = manager != null && manager != this;

		if (!condition)
		{
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	private void Start()
	{
		ToggleActiveSound(KeyValueData.SavedData.f_volume);
	}

	public void ToggleActiveSound(bool volumeEnabled)
	{
		sourceOfMusic.enabled = volumeEnabled;
	}
}
