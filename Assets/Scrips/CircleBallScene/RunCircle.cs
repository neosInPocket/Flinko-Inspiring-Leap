using System.Collections.Generic;
using UnityEngine;

public class RunCircle : MonoBehaviour
{
	[SerializeField] private LineRenderer mainLine;
	[SerializeField] private LineRenderer breakLine;
	[SerializeField] private EdgeCollider2D mainCollider;
	[SerializeField] private EdgeCollider2D breakCollider;
	[SerializeField] private Vector2 rotationSpeeds;
	[SerializeField] private CircleCollider2D circleCollider2D;
	[SerializeField] private float circleColliderRadiusDecrease;
	[SerializeField] private Gradient pointedGradient;
	[SerializeField] private Gradient defaultGradient;

	public float Radius { get; private set; }
	public bool IsPointed { get; set; }
	public bool IsInitial { get; set; }
	private float currentRotationSpeed;
	private Vector3 currentEuler;

	private void Start()
	{
		currentRotationSpeed = Random.Range(rotationSpeeds.x, rotationSpeeds.y);
		currentEuler = breakLine.transform.eulerAngles;
		mainLine.colorGradient = defaultGradient;

		if (IsInitial)
		{
			Point();
		}
	}

	private void Update()
	{
		currentEuler.z += currentRotationSpeed * Time.deltaTime;
		breakLine.transform.eulerAngles = currentEuler;
	}

	public void Point()
	{
		mainLine.colorGradient = pointedGradient;
		IsPointed = true;
	}

	public void EnableCollider(bool value)
	{
		mainCollider.enabled = value;
	}

	public void Spawn(float radius, int positionCount, float arcSize, float arcRadius)
	{
		List<Vector2> mainLinePoints = new List<Vector2>();
		List<Vector2> breakLinePoints = new List<Vector2>();

		mainLine.positionCount = positionCount;
		breakLine.positionCount = positionCount;
		Vector2 currentPosition;
		Vector2 currentArcPosition;
		float currentAngle = 0;
		float currentArcAngle = 0;
		float deltaAngle = 2 * Mathf.PI / positionCount;
		float deltaArcAngle = arcSize * Mathf.Deg2Rad / positionCount;

		for (int i = 0; i < positionCount; i++)
		{
			currentPosition.x = Mathf.Cos(currentAngle);
			currentPosition.y = Mathf.Sin(currentAngle);
			currentPosition *= radius;
			mainLine.SetPosition(i, currentPosition);

			currentAngle += deltaAngle;
			mainLinePoints.Add(currentPosition);
		}

		mainLinePoints.Add(mainLine.GetPosition(0));
		mainCollider.SetPoints(mainLinePoints);

		if (Random.Range(0, 2) == 1)
		{
			for (int i = 0; i < positionCount; i++)
			{
				currentArcPosition.x = Mathf.Cos(currentArcAngle);
				currentArcPosition.y = Mathf.Sin(currentArcAngle);
				currentArcPosition *= arcRadius;
				breakLine.SetPosition(i, currentArcPosition);

				currentArcAngle += deltaArcAngle;
				breakLinePoints.Add(currentArcPosition);
			}

			breakCollider.SetPoints(breakLinePoints);
		}
		else
		{
			breakCollider.enabled = false;
			breakLine.enabled = false;
		}

		circleCollider2D.radius = radius * (1 - radius * circleColliderRadiusDecrease);
		Radius = radius;
	}

	public void Initial()
	{
		breakCollider.enabled = false;
		breakLine.enabled = false;
	}

	public Vector2 GetLinePoint(int index)
	{
		return mainLine.GetPosition(index);
	}
}
