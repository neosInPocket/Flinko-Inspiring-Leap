using UnityEngine;
using UnityEngine.SceneManagement;

public class EventEnteredAnimationManager : MonoBehaviour
{
	[SerializeField] private MainLayerManager mainSceneLoader;

	public void MenuOrder()
	{
		SceneManager.LoadScene("MenuEvent");
	}

	public void NextOrder()
	{
		SceneManager.LoadScene("GameEventStart");
	}
}
