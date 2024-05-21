using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class StartCutScene : MonoBehaviour
{
	public TMP_Text guideCaption;
	private Action CutSceneCompletedAction;
	public Animator shellAnimator;
	private Action CurrentOrderAction;

	private void Awake()
	{
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();
	}

	public void StartCutSceneAction(Action cutEnd)
	{
		CutSceneCompletedAction = cutEnd;
		gameObject.SetActive(true);

		Touch.onFingerDown += NextOrderAction;

		guideCaption.text = "WELCOME TO Lucky Jet: Sky Impulse!";
		CurrentOrderAction = InitialAction;
	}

	private void NextOrderAction(Finger finger)
	{
		CurrentOrderAction();
		shellAnimator.SetTrigger("actionPassed");
	}

	private void InitialAction()
	{
		CurrentOrderAction = SecondAction;
		guideCaption.text = "LOOK! HERE IS YOUR BALL, it MOVES IN its CLOSED CIRCLE";
	}

	private void SecondAction()
	{
		CurrentOrderAction = SecondPlusAction;
		guideCaption.text = "tap the screen to make it fly in the direction of its velocity. your goal is to get into the next circle";
	}

	private void SecondPlusAction()
	{
		CurrentOrderAction = FifthMinusAction;
		StopAllCoroutines();
		guideCaption.text = "launch your ball with caution! At the beginning of each level you have a certain number of throws, use them wisely!";
	}

	private void FifthMinusAction()
	{
		CurrentOrderAction = Fifth;

		StopAllCoroutines();
		guideCaption.text = "IF YOUR BALL FLYES OFF THE SCREEN, YOU WILL LOSE ONE THROW";
	}

	private void Fifth()
	{
		CurrentOrderAction = LastOrder;
		guideCaption.text = "Get to the end of the level and track your progress to earn shards that you can spend in the store! GOOD LUCK!";
	}

	private void LastOrder()
	{
		CutSceneCompletedAction();
		gameObject.SetActive(false);
		Touch.onFingerDown -= NextOrderAction;
	}

	private void OnDestroy()
	{
		Touch.onFingerDown -= NextOrderAction;
	}
}
