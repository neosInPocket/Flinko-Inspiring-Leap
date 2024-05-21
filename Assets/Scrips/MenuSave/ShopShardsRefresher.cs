using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopShardsRefresher : MonoBehaviour
{
	[SerializeField] private TMP_Text skill1ButtonInner;
	[SerializeField] private TMP_Text skill2ButtonInner;
	[SerializeField] private ShardsRefresher shardContainer;
	[SerializeField] private Button skillSpeedRefreshButton;
	[SerializeField] private Button skillEffectsRefreshButton;
	[SerializeField] private TMP_Text skillCost1Text;
	[SerializeField] private TMP_Text skillCost2Text;
	[SerializeField] private int skill1Cost;
	[SerializeField] private int skill2Cost;

	private void Start()
	{
		ResetSkillStatuses();
	}

	public void ResetSkillStatuses()
	{
		skillCost1Text.text = skill1Cost.ToString();
		skillCost2Text.text = skill2Cost.ToString();

		shardContainer.SetShardsAmount();

		if (SerializedAudio.Singleton.Serializer.skill1)
		{
			skillSpeedRefreshButton.interactable = false;
			skill1ButtonInner.text = "UPGRADED";
			skill1ButtonInner.color = Color.green;
		}
		else
		{
			if (SerializedAudio.Singleton.Serializer.shards >= skill1Cost)
			{
				skillSpeedRefreshButton.interactable = true;
				skill1ButtonInner.text = "UPGRADE";
				skill1ButtonInner.color = Color.white;
			}
			else
			{
				skillSpeedRefreshButton.interactable = false;
				skill1ButtonInner.text = "NOT ENOUGH SHARDS";
				skill1ButtonInner.color = Color.red;
			}
		}

		if (SerializedAudio.Singleton.Serializer.skill2)
		{
			skillEffectsRefreshButton.interactable = false;
			skill2ButtonInner.text = "UPGRADED";
			skill2ButtonInner.color = Color.green;
		}
		else
		{
			if (SerializedAudio.Singleton.Serializer.shards >= skill2Cost)
			{
				skillEffectsRefreshButton.interactable = true;
				skill2ButtonInner.text = "UPGRADE";
				skill2ButtonInner.color = Color.white;
			}
			else
			{
				skillEffectsRefreshButton.interactable = false;
				skill2ButtonInner.text = "NOT ENOUGH SHARDS";
				skill2ButtonInner.color = Color.red;
			}
		}
	}

	public void Upgrade1Skill()
	{
		PurchaseSingleSkill(ref SerializedAudio.Singleton.Serializer.skill1, skill1Cost);
		ResetSkillStatuses();
	}

	public void Upgrade2Skill()
	{
		PurchaseSingleSkill(ref SerializedAudio.Singleton.Serializer.skill2, skill2Cost);
		ResetSkillStatuses();
	}

	public void PurchaseSingleSkill(ref bool skill, int skillCost)
	{
		skill = true;
		SerializedAudio.Singleton.Serializer.shards -= skillCost;
		SerializedAudio.Singleton.Serializer.MaintainSettingsValues();
	}
}
