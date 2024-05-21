using System.Collections.Generic;
using UnityEngine;

public class ShellLevelManager : MonoBehaviour
{
	[SerializeField] public ShellRenderer shellPrefab;
	[SerializeField] public int posCounts;
	[SerializeField] public Vector2 radiuses;
	[SerializeField] public float arcMeasures;
	[SerializeField] public float arcSizeDelta;
	[SerializeField] public Spinning spinning;
	[SerializeField] public float shellDeltaSize;
	[SerializeField] public CameraShellFollow cameraShellFollow;
	[HideInInspector] public List<ShellRenderer> shells;
	public List<ShellRenderer> Shells => shells;
	[HideInInspector] public int currentCameraCircleIndex;

	private void Awake()
	{
		shells = new List<ShellRenderer>();
		SetLayerActivated();
	}

	public void SetLayerActivated()
	{
		int xValue = SerializedAudio.Singleton.Serializer.playOrder;
		int spawnAmount = (int)Mathf.Sqrt(Mathf.Pow(xValue, 1.7f)) + 2;

		for (int i = 0; i < spawnAmount; i++)
		{
			var circle = Instantiate(shellPrefab, transform);

			var randomSize = Random.Range(radiuses.x, radiuses.y);
			circle.ShellAppear(randomSize, posCounts, arcMeasures, randomSize * (1 + arcSizeDelta));
			circle.ActivateCollisions(false);

			shells.Add(circle);

			if (i == 0)
			{
				circle.transform.position = Vector2.zero;
			}
			else
			{
				Vector2 setPos;
				var previous = shells[i - 1];

				if (i == 1)
				{
					setPos.x = previous.transform.position.x + previous.ShellRadius + 2 * shellDeltaSize + circle.ShellRadius;
					setPos.y = previous.transform.position.y;
				}
				else
				{
					if (Random.Range(0, 2) == 1)
					{
						setPos.x = previous.transform.position.x + previous.ShellRadius + 2 * shellDeltaSize + circle.ShellRadius;
						setPos.y = previous.transform.position.y;
					}
					else
					{
						setPos.y = previous.transform.position.y + previous.ShellRadius + 2 * shellDeltaSize + circle.ShellRadius;
						setPos.x = previous.transform.position.x;
					}
				}

				circle.transform.position = setPos;
			}
		}

		shells[0].ActivateCollisions(true);
		spinning.CurrentShell = shells[0];
		SpinningPosition();
		shells[0].StartAction();

		int indexAmount = 1;
		bool isNonHorizontal = shells[indexAmount].transform.position.x - shells[indexAmount - 1].transform.position.x == 0;
		var s1 = shells[indexAmount - 1];
		var s2 = shells[indexAmount];

		if (isNonHorizontal)
		{
			Vector2 top = s2.transform.position;
			Vector2 bottom = s1.transform.position;

			top.y += s2.ShellRadius;
			bottom.y -= s1.ShellRadius;

			cameraShellFollow.SetCameraPosition(bottom, top, true);
		}
		else
		{
			Vector2 right = s2.transform.position;
			Vector2 left = s1.transform.position;

			right.x += s2.ShellRadius;
			left.x -= s1.ShellRadius;

			cameraShellFollow.SetCameraPosition(left, right, false);
		}

		currentCameraCircleIndex = 1;
		shells[0].IsDefault = true;
	}

	public void CameraPosSetter(ShellRenderer shell)
	{
		var index = shells.IndexOf(shell) + 1;
		if (index == currentCameraCircleIndex) return;

		CamSetter(index);
	}

	public int ShellIndex(ShellRenderer shell)
	{
		return shells.IndexOf(shell);
	}

	public int currentIndex = 0;

	public void CamIndex()
	{
		CamSetter(currentIndex + 1);
		currentIndex++;
	}

	public void CamSetter(int amount)
	{
		if (amount >= shells.Count) return;

		currentCameraCircleIndex = amount;
		bool isVertical = shells[amount].transform.position.x - shells[amount - 1].transform.position.x == 0;
		var first = shells[amount - 1];
		var second = shells[amount];

		if (isVertical)
		{
			Vector2 top = second.transform.position;
			Vector2 bottom = first.transform.position;

			top.y += second.ShellRadius;
			bottom.y -= first.ShellRadius;

			cameraShellFollow.TargetShell(bottom, top, true);
		}
		else
		{
			Vector2 right = second.transform.position;
			Vector2 left = first.transform.position;

			right.x += second.ShellRadius;
			left.x -= first.ShellRadius;

			cameraShellFollow.TargetShell(left, right, false);
		}
	}

	public void SpinningPosition()
	{
		var linePoint = shells[0].GetLinePoint(0);
		var rotatorRadius = spinning.SpRenderer.size.x / 2;
		spinning.transform.position = new Vector2(linePoint.x - rotatorRadius, linePoint.y - rotatorRadius);
	}
}
