using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraSmoothMovement : MonoBehaviour
{
	[SerializeField] private Camera cam;
	[SerializeField] private float delta;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float orthoSpeed;
	[SerializeField] private float deltaDistance;
	[SerializeField] private float magnitudeThreshold;
	[SerializeField] private WallTriggers wallTriggers;

	public void MoveToDestination(Vector2 start, Vector2 end, bool isVertical)
	{
		Vector2 position = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);
		if ((Vector3)position == transform.position) return;

		StopAllCoroutines();


		float orthoSize = 0;

		if (isVertical)
		{
			orthoSize = end.y + delta - position.y;
			StartCoroutine(MoveOrthoRoutine(orthoSize));
			StartCoroutine(MovePositionRoutine(position));
		}
		else
		{
			float screenRatio = (float)Screen.height / (float)Screen.width;
			orthoSize = (end.x + delta - position.x) * screenRatio;
			StartCoroutine(MoveOrthoRoutine(orthoSize));
			StartCoroutine(MovePositionRoutine(position));
		}
	}

	public void SetCameraPosition(Vector2 start, Vector2 end, bool isVertical)
	{
		StopAllCoroutines();
		Vector2 position = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);
		float orthoSize = 0;

		if (isVertical)
		{
			orthoSize = end.y + delta - position.y;
			cam.orthographicSize = orthoSize;
			transform.position = position;
		}
		else
		{
			float screenRatio = (float)Screen.height / (float)Screen.width;
			orthoSize = (end.x + delta - position.x) * screenRatio;
			cam.orthographicSize = orthoSize;
			transform.position = position;
		}

		wallTriggers.RefreshWalls();
	}

	private IEnumerator MoveOrthoRoutine(float destination)
	{
		var currentSize = cam.orthographicSize;
		var startSize = currentSize;
		int direction = (int)((destination - currentSize) / Mathf.Abs(destination - currentSize));
		float magnitude = 1;

		while ((currentSize < destination && direction > 0) || (currentSize > destination && direction < 0))
		{
			currentSize += direction * orthoSpeed * Time.deltaTime * (magnitude + magnitudeThreshold) * (magnitude + magnitudeThreshold);
			cam.orthographicSize = currentSize;
			magnitude = Mathf.Abs((destination - currentSize) / destination);
			wallTriggers.RefreshWalls();
			yield return null;
		}

		cam.orthographicSize = destination;
		wallTriggers.RefreshWalls();
	}

	private IEnumerator MovePositionRoutine(Vector3 position)
	{
		var startPosition = transform.position;
		var currentPosition = transform.position;
		Vector3 direction = (position - currentPosition).normalized;
		float currentDistance = Vector2.Distance(position, currentPosition);
		float startDistance = currentDistance;
		float magnitude = 1;

		if (startDistance < float.Epsilon)
		{
			Debug.Log("error");
		}

		while (currentDistance > deltaDistance)
		{
			if (magnitude > 1)
			{
				transform.position = position;
				wallTriggers.RefreshWalls();
				yield break;
			}

			currentPosition += direction * moveSpeed * Time.deltaTime * (magnitude + magnitudeThreshold) * (magnitude + magnitudeThreshold);
			transform.position = currentPosition;
			currentDistance = Vector2.Distance(position, currentPosition);
			magnitude = currentDistance / startDistance;
			wallTriggers.RefreshWalls();
			yield return null;
		}

		transform.position = position;
		wallTriggers.RefreshWalls();
	}
}
