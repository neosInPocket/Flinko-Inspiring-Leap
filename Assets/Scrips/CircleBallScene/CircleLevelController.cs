using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CircleLevelController : MonoBehaviour
{
	[SerializeField] private RunCircle circlePrefab;
	[SerializeField] private LineBarrier barrierPrefab;
	[SerializeField] private int positionsPerCircle;
	[SerializeField] private Vector2 radiusSizes;
	[SerializeField] private float arcSize;
	[SerializeField] private float arcRadiusDelta;
	[SerializeField] private Rotator rotator;
	[SerializeField] private float circlesDeltaWidth;
	[SerializeField] private CameraSmoothMovement cameraMovement;
	private List<RunCircle> circles;
	public List<RunCircle> Circles => circles;
	private int currentCameraCircleIndex;

	private void Awake()
	{
		circles = new List<RunCircle>();
		BuildLevel();
	}

	private void BuildLevel()
	{
		var x = KeyValueData.SavedData.f_level;
		int circlesSpawnCount = (int)Mathf.Sqrt(Mathf.Pow(x, 1.7f)) + 2;

		for (int i = 0; i < circlesSpawnCount; i++)
		{
			var circle = Instantiate(circlePrefab, transform);

			var randomSize = Random.Range(radiusSizes.x, radiusSizes.y);
			circle.Spawn(randomSize, positionsPerCircle, arcSize, randomSize * (1 + arcRadiusDelta));
			circle.EnableCollider(false);

			circles.Add(circle);

			if (i == 0)
			{
				circle.transform.position = Vector2.zero;
			}
			else
			{
				Vector2 spawnPosition;
				var prevCircle = circles[i - 1];

				if (i == 1)
				{
					spawnPosition.x = prevCircle.transform.position.x + prevCircle.Radius + 2 * circlesDeltaWidth + circle.Radius;
					spawnPosition.y = prevCircle.transform.position.y;
				}
				else
				{
					if (Random.Range(0, 2) == 1)
					{
						spawnPosition.x = prevCircle.transform.position.x + prevCircle.Radius + 2 * circlesDeltaWidth + circle.Radius;
						spawnPosition.y = prevCircle.transform.position.y;
					}
					else
					{
						spawnPosition.y = prevCircle.transform.position.y + prevCircle.Radius + 2 * circlesDeltaWidth + circle.Radius;
						spawnPosition.x = prevCircle.transform.position.x;
					}
				}

				circle.transform.position = spawnPosition;
			}
		}

		circles[0].EnableCollider(true);
		rotator.CurrentCircle = circles[0];
		PlaceRotatorInPosition();
		circles[0].Initial();

		int index = 1;
		bool isVertical = circles[index].transform.position.x - circles[index - 1].transform.position.x == 0;
		var first = circles[index - 1];
		var second = circles[index];

		if (isVertical)
		{
			Vector2 top = second.transform.position;
			Vector2 bottom = first.transform.position;

			top.y += second.Radius;
			bottom.y -= first.Radius;

			cameraMovement.SetCameraPosition(bottom, top, true);
		}
		else
		{
			Vector2 right = second.transform.position;
			Vector2 left = first.transform.position;

			right.x += second.Radius;
			left.x -= first.Radius;

			cameraMovement.SetCameraPosition(left, right, false);
		}

		currentCameraCircleIndex = 1;
		circles[0].IsInitial = true;
	}

	public void SetCameraPositions(RunCircle runCircle)
	{
		var index = circles.IndexOf(runCircle) + 1;
		if (index == currentCameraCircleIndex) return;

		SetCameraPositions(index);
	}

	public int GetIndex(RunCircle circle)
	{
		return circles.IndexOf(circle);
	}

	private int currentIndex = 0;

	public void SetCameraIndex()
	{
		SetCameraPositions(currentIndex + 1);
		currentIndex++;
	}

	public void SetCameraPositions(int index)
	{
		if (index >= circles.Count) return;

		currentCameraCircleIndex = index;
		bool isVertical = circles[index].transform.position.x - circles[index - 1].transform.position.x == 0;
		var first = circles[index - 1];
		var second = circles[index];

		if (isVertical)
		{
			Vector2 top = second.transform.position;
			Vector2 bottom = first.transform.position;

			top.y += second.Radius;
			bottom.y -= first.Radius;

			cameraMovement.MoveToDestination(bottom, top, true);
		}
		else
		{
			Vector2 right = second.transform.position;
			Vector2 left = first.transform.position;

			right.x += second.Radius;
			left.x -= first.Radius;

			cameraMovement.MoveToDestination(left, right, false);
		}
	}

	private void PlaceRotatorInPosition()
	{
		var linePoint = circles[0].GetLinePoint(0);
		var rotatorRadius = rotator.SpRenderer.size.x / 2;
		rotator.transform.position = new Vector2(linePoint.x - rotatorRadius, linePoint.y - rotatorRadius);
	}
}
