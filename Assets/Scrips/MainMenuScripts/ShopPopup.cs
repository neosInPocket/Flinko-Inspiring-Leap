using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShopPopup : MonoBehaviour
{
	[SerializeField] private GameObject trajectoryStatus;
	[SerializeField] private GameObject ballSpeedStatus;
	[SerializeField] private GameObject ballSpeedStatusPurchased;
	[SerializeField] private GameObject trajectoryStatusPurchased;
	[SerializeField] private CharacterCoinsHolder characterCoinsHolder;
	[SerializeField] private int trajectoryCost;
	[SerializeField] private int speedCost;

	private void Start()
	{
		RestartStore();
	}

	public void RestartStore()
	{
		characterCoinsHolder.RestartCoins();

		if (KeyValueData.SavedData.f_visualEffect)
		{
			trajectoryStatus.SetActive(false);
			trajectoryStatusPurchased.SetActive(true);
		}
		else
		{
			if (KeyValueData.SavedData.f_gold >= trajectoryCost)
			{
				trajectoryStatus.SetActive(false);
				trajectoryStatusPurchased.SetActive(false);
			}
			else
			{
				trajectoryStatus.SetActive(true);
				trajectoryStatusPurchased.SetActive(false);
			}
		}

		if (KeyValueData.SavedData.f_speed)
		{
			ballSpeedStatus.SetActive(false);
			ballSpeedStatusPurchased.SetActive(true);
		}
		else
		{
			if (KeyValueData.SavedData.f_gold >= speedCost)
			{
				ballSpeedStatus.SetActive(false);
				ballSpeedStatusPurchased.SetActive(false);
			}
			else
			{
				ballSpeedStatus.SetActive(true);
				ballSpeedStatusPurchased.SetActive(false);
			}
		}
	}

	public void Buy(bool isTrajectory)
	{
		if (isTrajectory)
		{
			KeyValueData.SavedData.f_visualEffect = true;
			KeyValueData.SavedData.f_gold -= trajectoryCost;
			KeyValueData.SaveProgress();
		}
		else
		{
			KeyValueData.SavedData.f_speed = true;
			KeyValueData.SavedData.f_gold -= speedCost;
			KeyValueData.SaveProgress();
		}

		RestartStore();
	}
}
