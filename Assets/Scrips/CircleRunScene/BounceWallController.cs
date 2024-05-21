using System.Collections.Generic;
using UnityEngine;

public class BounceWallController : MonoBehaviour
{
	[SerializeField] public BounceWallRenderer wallInstance;
	[SerializeField] public float bounceSize;
	List<BounceWallRenderer> walls;

	public void Awake()
	{
		walls = new List<BounceWallRenderer>();

		walls.Add(Instantiate(wallInstance, transform));
		walls.Add(Instantiate(wallInstance, transform));
		walls.Add(Instantiate(wallInstance, transform));
		walls.Add(Instantiate(wallInstance, transform));
	}

	public void RefreshWalls()
	{
		var screenSize = ScreenSizeGetter();

		walls[0].bounceWallRenderer.size = new Vector2(2 * screenSize.x, bounceSize);
		walls[1].bounceWallRenderer.size = new Vector2(2 * screenSize.x, bounceSize);
		walls[2].bounceWallRenderer.size = new Vector2(bounceSize, 2 * screenSize.y);
		walls[3].bounceWallRenderer.size = new Vector2(bounceSize, 2 * screenSize.y);

		walls[0].transform.localPosition = new Vector2(0, -screenSize.y - bounceSize / 2);
		walls[1].transform.localPosition = new Vector2(0, screenSize.y + bounceSize / 2);
		walls[2].transform.localPosition = new Vector2(screenSize.x + bounceSize / 2, 0);
		walls[3].transform.localPosition = new Vector2(-screenSize.x - bounceSize / 2, 0);
	}

	public Vector3 ScreenSizeGetter()
	{
		var screenSize = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
		return screenSize;
	}
}
