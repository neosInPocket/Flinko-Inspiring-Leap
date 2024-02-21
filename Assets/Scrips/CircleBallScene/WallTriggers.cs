using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallTriggers : MonoBehaviour
{
	[SerializeField] private WallTrigger wallPrefab;
	[SerializeField] private float wallSize;
	List<WallTrigger> triggers;

	private void Awake()
	{
		triggers = new List<WallTrigger>();

		for (int i = 0; i < 4; i++)
		{
			triggers.Add(Instantiate(wallPrefab, transform));
		}
	}

	public void RefreshWalls()
	{
		var screenSize = WorldPos();

		triggers[0].SpriteRenderer.size = new Vector2(2 * screenSize.x, wallSize);
		triggers[1].SpriteRenderer.size = new Vector2(2 * screenSize.x, wallSize);
		triggers[2].SpriteRenderer.size = new Vector2(wallSize, 2 * screenSize.y);
		triggers[3].SpriteRenderer.size = new Vector2(wallSize, 2 * screenSize.y);

		triggers[0].transform.localPosition = new Vector2(0, -screenSize.y - wallSize / 2);
		triggers[1].transform.localPosition = new Vector2(0, screenSize.y + wallSize / 2);
		triggers[2].transform.localPosition = new Vector2(screenSize.x + wallSize / 2, 0);
		triggers[3].transform.localPosition = new Vector2(-screenSize.x - wallSize / 2, 0);
	}

	public Vector3 WorldPos()
	{
		// var size = Camera.main.ScreenPointToRay(screenSize);

		// var alt = size.direction;
		// var originValue = size.origin;

		// Vector3 pointVectir = new Vector3(0, 0, 1);
		// Vector3 pos = new Vector3(0, 0, 0);

		// float productResult = Vector3.Dot(alt, pointVectir);

		// float magnitudeDistance = Vector3.Dot(pos - originValue, pointVectir) / productResult;

		// Vector3 result = originValue + magnitudeDistance * alt;
		// return result;

		float screenRatio = (float)Screen.width / (float)Screen.height;
		var ortho = Camera.main.orthographicSize;
		var screenSize = new Vector2(ortho * screenRatio, ortho);
		return screenSize;
	}
}
