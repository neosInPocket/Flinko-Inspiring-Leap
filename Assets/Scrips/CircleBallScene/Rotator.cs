using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Rotator : MonoBehaviour
{
	[SerializeField] private Vector2 radialSpeeds;
	[SerializeField] private SpriteRenderer spRenderer;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private float gravityForce;
	[SerializeField] private Vector2 defaultGravity;
	[SerializeField] private CircleLevelController circleLevelController;
	[SerializeField] private AudioSource shootSource;
	[SerializeField] private AudioSource passedSource;
	[SerializeField] private GameObject stunEffect;
	[SerializeField] private GameObject passEffect;
	public SpriteRenderer SpRenderer => spRenderer;
	public RunCircle CurrentCircle { get; set; }
	private float radialSpeed;
	private bool shooted;
	public bool AllowedExternalInput { get; set; }
	public Action TriggerEnter { get; set; }
	public Action<RunCircle> CirclePass { get; set; }

	private void Start()
	{
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();

		bool sfxEnabled = KeyValueData.SavedData.f_sfx;
		shootSource.enabled = sfxEnabled;
		passedSource.enabled = sfxEnabled;
		radialSpeed = KeyValueData.SavedData.f_speed ? radialSpeeds.y : radialSpeeds.x;
	}

	private void Update()
	{
		if (CurrentCircle == null || shooted) return;

		var normal = GetNormal(CurrentCircle.transform.position - transform.position);
		Physics2D.gravity = -(CurrentCircle.transform.position - transform.position).normalized * gravityForce;
		rb.AddForce(normal * radialSpeed);
	}

	private Vector3 GetNormal(Vector2 vector)
	{
		var newX = vector.y;
		var newY = -vector.x;
		return new Vector2(newX, newY).normalized;
	}

	private void OnFingerClick(Finger finger)
	{
		DenyTouchInput();
		TriggerEnter?.Invoke();
		shooted = true;
		CurrentCircle.EnableCollider(false);
		Physics2D.gravity = defaultGravity;
		shootSource.Stop();
		shootSource.Play();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!AllowedExternalInput) return;

		AllowTouchInput();
		shooted = false;

		if (collider.TryGetComponent<RunCircle>(out RunCircle circle))
		{
			if (circle == CurrentCircle)
			{
				CurrentCircle.EnableCollider(true);
				return;
			}

			CurrentCircle = circle;
			CurrentCircle.EnableCollider(true);
			circleLevelController.SetCameraPositions(CurrentCircle);
			CirclePass?.Invoke(circle);
			transform.position = CurrentCircle.transform.position;
			passedSource.Stop();
			passedSource.Play();
			EnableEffect(passEffect, transform.position);
			return;
		}

		if (collider.TryGetComponent<WallTrigger>(out WallTrigger trigger))
		{
			transform.position = CurrentCircle.transform.position;
			EnableEffect(stunEffect, transform.position);
		}
	}

	public void AllowTouchInput()
	{
		Touch.onFingerDown += OnFingerClick;
	}

	public void DenyTouchInput()
	{
		Touch.onFingerDown -= OnFingerClick;
	}

	private void OnDestroy()
	{
		DenyTouchInput();
	}

	private void EnableEffect(GameObject effect, Vector2 position)
	{
		if (!KeyValueData.SavedData.f_visualEffect) return;

		effect.SetActive(false);
		effect.transform.position = position;
		effect.SetActive(true);
	}
}
