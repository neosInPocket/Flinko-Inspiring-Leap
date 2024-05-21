using TMPro;
using UnityEngine;

public class ShardsRefresher : MonoBehaviour
{
	[SerializeField] private TMP_Text shardsText;

	private void Start()
	{
		SetShardsAmount();
	}

	public void SetShardsAmount()
	{
		shardsText.text = SerializedAudio.Singleton.Serializer.shards.ToString();
	}
}
