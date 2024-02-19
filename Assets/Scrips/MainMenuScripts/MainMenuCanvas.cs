using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
	public void OnAnimationEnd()
	{
		SceneManager.LoadScene("CircleRunScene");
	}
}
