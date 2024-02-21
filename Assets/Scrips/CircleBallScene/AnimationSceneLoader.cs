using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationSceneLoader : MonoBehaviour
{
	[SerializeField] private MainSceneLoader mainSceneLoader;

	public void OnAnimationEndCallBack()
	{
		mainSceneLoader.Enter();
	}

	public void GoMenu()
	{
		SceneManager.LoadScene("MenuSave");
	}

	public void GoNext()
	{
		SceneManager.LoadScene("CircleRunScene");
	}
}
