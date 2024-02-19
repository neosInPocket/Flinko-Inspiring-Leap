using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private Image checkImage;
	[SerializeField] private TMP_Text text;

	public Image Check => checkImage;
	public TMP_Text Text => text;
	public Button Button => button;
}
