using TMPro;
using UnityEngine;

public class CharacterCoinsHolder : MonoBehaviour
{
	[SerializeField] private TMP_Text amount;

	private void Start()
	{
		RestartCoins();
	}

	public void RestartCoins()
	{
		amount.text = KeyValueData.SavedData.f_gold.ToString();
	}
}
