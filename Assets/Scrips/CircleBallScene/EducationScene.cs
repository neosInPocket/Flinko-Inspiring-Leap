using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class EducationScene : MonoBehaviour
{
	[SerializeField] private CircleLevelController circlesController;
	[SerializeField] private Rotator rotator;
	[SerializeField] private TMP_Text characterText;
	[SerializeField] private Image arrow;
	[SerializeField] private float speed;
	[SerializeField] private float amplitude;
	[SerializeField] private float freq;
	[SerializeField] private Transform barPosition;
	private Action OnEducationEnd;
	private Action currentHandler;
	private bool arrowRotation;
	private bool arrowBlinking;

	private void Start()
	{
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();
	}

	private void Update()
	{
		if (!arrowRotation) return;

		arrow.transform.position = rotator.transform.position;
	}

	public void ShowMessage(Action educationEndAction)
	{
		OnEducationEnd = educationEndAction;
		gameObject.SetActive(true);

		Touch.onFingerDown += NextHandler;

		characterText.text = "WELCOME TO FLINKO INSPIRING LEAP!";
		currentHandler = FirstHandler;
	}

	private void NextHandler(Finger finger)
	{
		currentHandler();
	}

	private void FirstHandler()
	{
		currentHandler = SecondHandler;

		characterText.text = "LOOK! HERE IS YOUR BALL, WHICH MOVES IN A CLOSED CIRCLE";
		EnableArrowRotation();
	}

	private void SecondHandler()
	{
		currentHandler = ThirdHandler;
		DisableArrowRotation();

		characterText.text = "TAP THE SCREEN TO SHOOT IT. YOUR GOAL IS TO REACH NEXT GREEN CIRCLE";
		arrow.gameObject.SetActive(true);
		arrow.transform.position = circlesController.Circles[1].transform.position;
		arrowBlinking = true;
		StartCoroutine(BlinkArrow(false));
	}

	private void ThirdHandler()
	{
		currentHandler = ForthHandler;

		StopAllCoroutines();
		var euler = arrow.transform.eulerAngles;
		euler.z = 90f;
		arrow.transform.eulerAngles = euler;
		arrow.transform.position = barPosition.position;
		StartCoroutine(BlinkArrow(true));
		characterText.text = "AT THE BEGINNING OF EACH LEVEL YOU HAVE A CERTAIN NUMBER OF THROWS";
	}

	private void ForthHandler()
	{
		currentHandler = FifthHandler;

		StopAllCoroutines();
		arrowBlinking = false;
		arrow.gameObject.SetActive(false);
		characterText.text = "IF YOUR BALL FLYES OFF THE SCREEN, YOU WILL LOSE ONE THROW";
	}

	private void FifthHandler()
	{
		currentHandler = LastHandler;

		characterText.text = "GOOD LUCK!";
	}

	private void LastHandler()
	{
		OnEducationEnd();
		gameObject.SetActive(false);
		Touch.onFingerDown -= NextHandler;
	}

	private void EnableArrowRotation()
	{
		arrow.gameObject.SetActive(true);
		arrowRotation = true;
	}

	private void DisableArrowRotation()
	{
		arrow.gameObject.SetActive(false);
		arrowRotation = false;
	}

	private IEnumerator BlinkArrow(bool isVertical)
	{
		Vector2 startPosition = arrow.transform.position;
		Vector2 currentPosition = arrow.transform.position;
		float currentTime = Time.time;
		float addvalue = 0;

		if (isVertical)
		{
			while (arrowBlinking)
			{
				addvalue = amplitude * Mathf.Sin(currentTime * freq) - amplitude;
				currentTime += Time.deltaTime;
				arrow.transform.position = startPosition + addvalue * Vector2.up;
				yield return null;
			}
		}
		else
		{
			while (arrowBlinking)
			{
				addvalue = amplitude * Mathf.Sin(currentTime * freq) - amplitude;
				currentTime += Time.deltaTime;
				arrow.transform.position = startPosition + addvalue * Vector2.right;
				yield return null;
			}
		}


	}

	private void OnDestroy()
	{
		Touch.onFingerDown -= NextHandler;
	}
}
