using UnityEngine;

[CreateAssetMenu(menuName = "Defaulter")]
public class Defaulter : ScriptableObject
{
	[SerializeField] private int playOrderDefault;
	[SerializeField] private int shardsDefault;
	[SerializeField] private bool skill1Default;
	[SerializeField] private bool skill2Default;
	[SerializeField] private bool audioConnectionDefault;
	[SerializeField] private bool effectsConnectionDefault;
	[SerializeField] private bool gameInstructionsDefault;

	public int PlayOrderDefault => playOrderDefault;
	public int ShardsDefault => shardsDefault;
	public bool Skill1Default => skill1Default;
	public bool Skill2Default => skill2Default;
	public bool AudioConnectionDefault => audioConnectionDefault;
	public bool EffectsConnectionDefault => effectsConnectionDefault;
	public bool GameInstructionsDefault => gameInstructionsDefault;
}
