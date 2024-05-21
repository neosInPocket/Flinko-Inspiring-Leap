using System.Collections;
using UnityEngine;

public class CameraShellFollow : MonoBehaviour
{
	public Camera MainCamera => Camera.main;
	[SerializeField] public float deltaSize;
	[SerializeField] public float cameraSpeed;
	[SerializeField] public float cameraLensVelocity;
	[SerializeField] public float epsMagnitude;
	[SerializeField] public float magnitudeLimit;
	[SerializeField] public BounceWallController bounceWalls;

	public void TargetShell(Vector2 currentPosition, Vector2 shellTargetPosition, bool isNonHorizontalMovement)
	{
		Vector2 changePosition = new Vector2((currentPosition.x + shellTargetPosition.x) / 2, (currentPosition.y + shellTargetPosition.y) / 2);
		if ((Vector3)changePosition == transform.position) return;

		StopAllCoroutines();

		var ortho = 0f;

		if (isNonHorizontalMovement)
		{
			ortho = shellTargetPosition.y + deltaSize - changePosition.y;
			StartCoroutine(ChangeCameraScale(ortho));
			StartCoroutine(ChangeCameraPosition(changePosition));
		}
		else
		{
			float ratio = 1 / MainCamera.aspect;
			ortho = (shellTargetPosition.x + deltaSize - changePosition.x) * ratio;
			StartCoroutine(ChangeCameraScale(ortho));
			StartCoroutine(ChangeCameraPosition(changePosition));
		}
	}

	public void SetCameraPosition(Vector2 start, Vector2 end, bool isVertical)
	{
		StopAllCoroutines();
		Vector2 position = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);
		var ortho = 0f;

		if (isVertical)
		{
			ortho = end.y + deltaSize - position.y;
			MainCamera.orthographicSize = ortho;
			transform.position = position;
		}
		else
		{
			float ratio = 1 / MainCamera.aspect;
			ortho = (end.x + deltaSize - position.x) * ratio;
			MainCamera.orthographicSize = ortho;
			transform.position = position;
		}

		bounceWalls.RefreshWalls();
	}

	public IEnumerator ChangeCameraScale(float destination)
	{
		var current = MainCamera.orthographicSize;
		var start = current;
		var dir = (int)((destination - current) / Mathf.Abs(destination - current));
		var magn = 1f;

		bool firstCondition = current < destination && dir > 0;
		bool secondCondition = current > destination && dir < 0;
		while (secondCondition || firstCondition)
		{
			current += dir * cameraLensVelocity * Time.deltaTime * (magn + magnitudeLimit) * (magn + magnitudeLimit);
			MainCamera.orthographicSize = current;
			magn = Mathf.Abs((destination - current) / destination);
			bounceWalls.RefreshWalls();
			firstCondition = current < destination && dir > 0;
			secondCondition = current > destination && dir < 0;
			Debug.Log(current);
			yield return null;
		}

		MainCamera.orthographicSize = destination;
		bounceWalls.RefreshWalls();
	}

	private IEnumerator ChangeCameraPosition(Vector3 target)
	{
		var start = transform.position;
		var current = transform.position;
		Vector3 dir = (target - current).normalized;
		float distance = Vector2.Distance(target, current);
		float startDist = distance;
		float magn = 1;

		while (distance > epsMagnitude)
		{
			if (magn > 1)
			{
				transform.position = target;
				bounceWalls.RefreshWalls();
				yield break;
			}

			current += dir * cameraSpeed * Time.deltaTime * (magn + magnitudeLimit) * (magn + magnitudeLimit);
			transform.position = current;
			distance = Vector2.Distance(target, current);
			magn = distance / startDist;
			bounceWalls.RefreshWalls();
			yield return null;
		}

		transform.position = target;
		bounceWalls.RefreshWalls();
	}
}
