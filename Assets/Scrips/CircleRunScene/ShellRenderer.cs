using System.Collections.Generic;
using UnityEngine;

public class ShellRenderer : MonoBehaviour
{
	[SerializeField] private LineRenderer line;
	[SerializeField] private LineRenderer loseLine;
	[SerializeField] private EdgeCollider2D shellCollider;
	[SerializeField] private EdgeCollider2D loseLineCollider;
	[SerializeField] private Vector2 spinningVelocities;
	[SerializeField] private CircleCollider2D aminCollider;
	[SerializeField] private float shellColliderDecrease;
	[SerializeField] private Gradient passedGrad;
	[SerializeField] private Gradient initialGrad;

	public float ShellRadius;
	public bool IsPassed;
	public bool IsDefault;
	private float spinningVelocity;
	private Vector3 transformAngles;

	public void Start()
	{
		spinningVelocity = Random.Range(spinningVelocities.x, spinningVelocities.y);
		transformAngles = loseLine.transform.eulerAngles;
		line.colorGradient = initialGrad;

		if (IsDefault) ActivatePointPass();
	}

	public void Update()
	{
		transformAngles.z += spinningVelocity * Time.deltaTime;
		loseLine.transform.eulerAngles = transformAngles;
	}

	public void ActivatePointPass()
	{
		line.colorGradient = passedGrad;
		IsPassed = true;
	}

	public void ActivateCollisions(bool value)
	{
		shellCollider.enabled = value;
	}

	public void ShellAppear(float shellRadius, int positionsNumber, float angleSize, float angleRadius)
	{
		List<Vector2> lineVectors = new List<Vector2>();
		List<Vector2> loseLineVectors = new List<Vector2>();

		line.positionCount = positionsNumber;
		loseLine.positionCount = positionsNumber;
		Vector2 current;
		Vector2 currentAngleArc;
		float currentRotationAngle = 0;
		float currentArcAngle = 0;
		float delta = 2 * Mathf.PI / positionsNumber;
		float deltaArc = angleSize * Mathf.Deg2Rad / positionsNumber;

		for (int i = 0; i < positionsNumber; i++)
		{
			current.x = Mathf.Cos(currentRotationAngle);
			current.y = Mathf.Sin(currentRotationAngle);
			current *= shellRadius;
			line.SetPosition(i, current);

			currentRotationAngle += delta;
			lineVectors.Add(current);
		}

		lineVectors.Add(line.GetPosition(0));
		shellCollider.SetPoints(lineVectors);

		if (Random.Range(0, 2) == 1)
		{
			for (int i = 0; i < positionsNumber; i++)
			{
				currentAngleArc.x = Mathf.Cos(currentArcAngle);
				currentAngleArc.y = Mathf.Sin(currentArcAngle);
				currentAngleArc *= angleRadius;
				loseLine.SetPosition(i, currentAngleArc);

				currentArcAngle += deltaArc;
				loseLineVectors.Add(currentAngleArc);
			}

			loseLineCollider.SetPoints(loseLineVectors);
		}
		else
		{
			loseLineCollider.enabled = false;
			loseLine.enabled = false;
		}

		aminCollider.radius = shellRadius * (1 - shellRadius * shellColliderDecrease);
		ShellRadius = shellRadius;
	}

	public void StartAction()
	{
		loseLineCollider.enabled = false;
		loseLine.enabled = false;
	}

	public Vector2 GetLinePoint(int index) => line.GetPosition(index);
}
