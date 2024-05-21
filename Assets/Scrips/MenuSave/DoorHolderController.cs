using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHolderController : MonoBehaviour
{
	[SerializeField] private Animator doorAnimator;
	[SerializeField] private string playOpenIndex;
	[SerializeField] private string storeOpenIndex;
	[SerializeField] private string closeIndex;

	public void DoorClosed()
	{
		SceneManager.LoadScene("GameEventStart");
	}

	public void OpenStore()
	{
		doorAnimator.SetTrigger(storeOpenIndex);
	}

	public void CloseDoors()
	{
		doorAnimator.SetTrigger(closeIndex);
	}

	public void OpenPlay()
	{
		doorAnimator.SetTrigger(playOpenIndex);
	}
}
