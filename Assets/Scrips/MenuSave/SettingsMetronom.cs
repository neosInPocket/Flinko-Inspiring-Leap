using UnityEngine;
using UnityEngine.UI;

public class SettingsMetronom : MonoBehaviour
{
	[SerializeField] private Image iconForMusic;
	[SerializeField] private Image iconForEffects;
	[SerializeField] private Color nonEnabledColor;

	private void Start()
	{
		bool iconMusic = SerializedAudio.Singleton.Serializer.audioConnection;
		bool iconEffects = SerializedAudio.Singleton.Serializer.effectsConnection;

		iconForMusic.color = iconMusic ? Color.white : nonEnabledColor;
		iconForEffects.color = iconEffects ? Color.white : nonEnabledColor;
	}

	public void ChangeMusicIcon()
	{
		SerializedAudio.Singleton.SetSoundsParams();
		bool iconMusic = SerializedAudio.Singleton.Serializer.audioConnection;
		iconForMusic.color = iconMusic ? Color.white : nonEnabledColor;
	}

	public void ChangeEffectsMark()
	{
		SerializedAudio.Singleton.SetEffectsParams();
		bool iconEffects = SerializedAudio.Singleton.Serializer.effectsConnection;
		iconForEffects.color = iconEffects ? Color.white : nonEnabledColor;
	}
}
