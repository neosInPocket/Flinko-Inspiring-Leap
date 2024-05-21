using UnityEngine;

public class SettingsSerializer
{
	public int playOrder;
	public int shards;
	public bool skill1;
	public bool skill2;
	public bool audioConnection;
	public bool effectsConnection;
	public bool gameInstructions;

	public Defaulter defaulter;

	public SettingsSerializer(bool returnDefault, Defaulter defaulter)
	{
		this.defaulter = defaulter;
		if (returnDefault)
		{
			LoadFromDefaultSettings();
		}
		else
		{
			LoadFromSerializedSettings();
		}
	}

	private void LoadFromSerializedSettings()
	{
		playOrder = PlayerPrefs.GetInt("playOrder", defaulter.PlayOrderDefault);
		shards = PlayerPrefs.GetInt("shards", defaulter.ShardsDefault);
		skill1 = PlayerPrefs.GetInt("skill1", defaulter.Skill1Default ? 1 : 0) == 1;
		skill2 = PlayerPrefs.GetInt("skill2", defaulter.Skill2Default ? 1 : 0) == 1;
		audioConnection = PlayerPrefs.GetInt("audioConnection", defaulter.AudioConnectionDefault ? 1 : 0) == 1;
		effectsConnection = PlayerPrefs.GetInt("effectsConnection", defaulter.EffectsConnectionDefault ? 1 : 0) == 1;
		gameInstructions = PlayerPrefs.GetInt("gameInstructions", defaulter.GameInstructionsDefault ? 1 : 0) == 1;
	}

	private void LoadFromDefaultSettings()
	{
		playOrder = defaulter.PlayOrderDefault;
		shards = defaulter.ShardsDefault;
		skill1 = defaulter.Skill1Default;
		skill2 = defaulter.Skill2Default;
		audioConnection = defaulter.AudioConnectionDefault;
		effectsConnection = defaulter.EffectsConnectionDefault;
		gameInstructions = defaulter.GameInstructionsDefault;

		MaintainSettingsValues();
	}

	public void MaintainSettingsValues()
	{
		PlayerPrefs.SetInt("playOrder", playOrder);
		PlayerPrefs.SetInt("shards", shards);
		PlayerPrefs.SetInt("skill1", skill1 ? 1 : 0);
		PlayerPrefs.SetInt("skill2", skill2 ? 1 : 0);
		PlayerPrefs.SetInt("audioConnection", audioConnection ? 1 : 0);
		PlayerPrefs.SetInt("effectsConnection", effectsConnection ? 1 : 0);
		PlayerPrefs.SetInt("gameInstructions", gameInstructions ? 1 : 0);
	}
}
