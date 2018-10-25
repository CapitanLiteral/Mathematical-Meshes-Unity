using UnityEngine;

public class Graph : MonoBehaviour
{
	[SerializeField] private Transform pointPrefab;
	[Range(10, 100)]
	[SerializeField] private int resolution = 10;

	private Transform[] points;

	private bool resetPoints = false;

	private void Start()
	{
		SetPoints();
	}
	private void Update()
	{
		if (resetPoints)
			ResetPoints();

		UpdatePoints();
	}
	private void OnValidate()
	{
		resetPoints = true;
	}

	void SetPoints()
	{
		if (points != null)
		{
			foreach (var item in points)
			{
				if (item != null)
				{
					Destroy(item.gameObject);
				}
			}
		}
		points = new Transform[resolution];

		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		Vector3 position;
		position.y = 0f;
		position.z = 0f;

		for (int i = 0; i < resolution; i++)
		{
			points[i] = Instantiate(pointPrefab);
			points[i].SetParent(transform);
			points[i].localScale = scale;
			position.x = (i + 0.5f) * step - 1f;
			points[i].localPosition = position;
		}

	}
	void ResetPoints()
	{
		resetPoints = false;
		SetPoints();
	}

	void UpdatePoints()
	{
		foreach (var point in points)
		{
			Vector3 position = point.localPosition;
			position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
			point.localPosition = position;
		}
	}

}
