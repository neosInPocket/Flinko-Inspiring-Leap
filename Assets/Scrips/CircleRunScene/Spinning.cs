using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Spinning : MonoBehaviour
{
	[SerializeField] public Vector2 angleVelocities;
	[SerializeField] public SpriteRenderer spinningRenderer;
	[SerializeField] public Rigidbody2D spinRigidbody;
	[SerializeField] public float gravityVectorAmplitude;
	[SerializeField] public Vector2 initialGravity;
	[SerializeField] public ShellLevelManager SpinningLayerManager;
	[SerializeField] public AudioSource cutOffAudio;
	[SerializeField] public AudioSource shellPassedAudio;
	[SerializeField] public GameObject loseVisuals;
	[SerializeField] public GameObject passVisuals;
	public SpriteRenderer SpRenderer => spinningRenderer;
	public ShellRenderer CurrentShell;
	[HideInInspector] private float angleVelocity;
	[HideInInspector] private bool isBeingShooted;
	public bool TouchInputEnabled;
	public Action ShellEnterAction;
	public Action<ShellRenderer> ShellPassedAction;

	private void Start()
	{
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();

		bool sfxEnabled = SerializedAudio.Singleton.Serializer.effectsConnection;
		cutOffAudio.enabled = sfxEnabled;
		shellPassedAudio.enabled = sfxEnabled;
		angleVelocity = SerializedAudio.Singleton.Serializer.skill1 ? angleVelocities.y : angleVelocities.x;
	}

	public void Update()
	{
		if (!CurrentShell) return;
		if (isBeingShooted) return;

		var shellNormal = GetNormalToVector2(CurrentShell.transform.position - transform.position);
		Physics2D.gravity = -(CurrentShell.transform.position - transform.position).normalized * gravityVectorAmplitude;
		spinRigidbody.AddForce(shellNormal * angleVelocity * Time.deltaTime);
	}

	public Vector3 GetNormalToVector2(Vector2 toVector)
	{
		var toX = toVector.y;
		var toY = -toVector.x;
		Vector3 returnResult = new Vector2(toX, toY);
		return returnResult.normalized;
	}

	public void OnTouchInputCompleted(Finger deviceFinger)
	{
		DisableTouchPass();
		///////////
		isBeingShooted = true;
		CurrentShell.ActivateCollisions(false);
		Physics2D.gravity = initialGravity;
		cutOffAudio.Stop();
		cutOffAudio.Play();
	}

	public void OnTriggerEnter2D(Collider2D entered)
	{
		if (!TouchInputEnabled) return;

		EnableTouchPass();
		isBeingShooted = false;

		if (entered.TryGetComponent<ShellRenderer>(out ShellRenderer circle))
		{
			if (circle == CurrentShell)
			{
				CurrentShell.ActivateCollisions(true);
				return;
			}

			CurrentShell = circle;
			CurrentShell.ActivateCollisions(true);
			SpinningLayerManager.CameraPosSetter(CurrentShell);
			ShellPassedAction?.Invoke(circle);
			transform.position = CurrentShell.transform.position;
			shellPassedAudio.Stop();
			shellPassedAudio.Play();
			SetVisualsActive(passVisuals, transform.position);
			ShellEnterAction?.Invoke();
			return;
		}

		if (entered.TryGetComponent<BounceWallRenderer>(out BounceWallRenderer trigger))
		{
			transform.position = CurrentShell.transform.position;
			SetVisualsActive(loseVisuals, transform.position);
			ShellEnterAction?.Invoke();
		}
	}

	public void EnableTouchPass()
	{
		Touch.onFingerDown += OnTouchInputCompleted;
	}

	public void DisableTouchPass()
	{
		Touch.onFingerDown -= OnTouchInputCompleted;
	}

	public void OnDestroy()
	{
		DisableTouchPass();
	}

	public void SetVisualsActive(GameObject effect, Vector2 targetPos)
	{
		if (!SerializedAudio.Singleton.Serializer.skill2) return;

		effect.SetActive(false);
		effect.transform.position = targetPos;
		effect.SetActive(true);
	}
}
