using UnityEngine;

public class SerializedAudio : MonoBehaviour
{
	[SerializeField] private bool returnDefault;
	[SerializeField] private AudioSource serialized;
	[SerializeField] private Defaulter defaulter;
	public static SerializedAudio Singleton { get; private set; }
	public SettingsSerializer Serializer { get; private set; }

	private void Awake()
	{
		Application.targetFrameRate = 15;

		if (Singleton != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Singleton = this;
			DontDestroyOnLoad(gameObject);
		}

		Serializer = new SettingsSerializer(returnDefault, defaulter);
	}

	private void Start()
	{
		serialized.volume = Serializer.audioConnection ? 1 : 0;
	}

	public bool SetSoundsParams()
	{
		Serializer.audioConnection = !Serializer.audioConnection;
		serialized.volume = Serializer.audioConnection ? 1 : 0;
		Serializer.MaintainSettingsValues();

		return Serializer.audioConnection;
	}

	public bool SetEffectsParams()
	{
		Serializer.effectsConnection = !Serializer.effectsConnection;
		Serializer.MaintainSettingsValues();
		return Serializer.effectsConnection;
	}
}
