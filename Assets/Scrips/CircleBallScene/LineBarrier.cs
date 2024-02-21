using UnityEngine;

public class LineBarrier : MonoBehaviour
{
	[SerializeField] private LineRenderer lineRenderer;

	public void Spawn(Vector2 from, Vector2 to, float zRotation)
	{
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, from);
		lineRenderer.SetPosition(1, to);

		var euler = transform.eulerAngles;
		euler.z = zRotation;
		transform.eulerAngles = euler;
	}
}
