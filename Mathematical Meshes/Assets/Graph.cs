using UnityEngine;

public class Graph : MonoBehaviour
{
	[SerializeField] private Transform pointPrefab;
	[Range(10, 100)]
	[SerializeField] private int resolution = 10;
	[SerializeField] GraphFunctionName function;

	private Transform[] points;
	static private GraphFunction[] functions = { SineFunction, MultiSineFunction, Sine2DFunction, MultiSine2DFunction, Ripple, Cylinder, Sphere,
	Torus};
	private GraphFunction delegateFunction;

	private bool resetPoints = false;
	float time = 0;

	const float pi = Mathf.PI;

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
		UpdateSelectedFunction();
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
		points = new Transform[resolution*resolution];
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;

		for (int i = 0; i < points.Length; i++)
		{
			Transform point = Instantiate(pointPrefab);
			point.localScale = scale;
			point.SetParent(transform, false);
			points[i] = point;
		}
	}
	void ResetPoints()
	{
		resetPoints = false;
		SetPoints();
	}

	void UpdatePoints()
	{
		time = Time.time;
		float step = 2f / resolution;
		for (int i = 0, z = 0; z < resolution; z++)
		{
			float v = (z + 0.5f) * step - 1f;
			for (int x = 0; x < resolution; x++, i++)
			{
				float u = (x + 0.5f) * step - 1f;
				points[i].localPosition = delegateFunction(u, v, time);
			}
		}
	}
	void UpdateSelectedFunction()
	{
		delegateFunction = functions[(int)function];
	}

	static Vector3 SineFunction(float x, float z, float time)
	{
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + time));
		p.z = z;
		return p;
	}
	static Vector3 MultiSineFunction(float x, float z, float time)
	{
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + time));
		p.y += Mathf.Sin(2f * pi * (x + 2f * time)) / 2f;
		p.y *= 2f / 3f;
		p.z = z;
		return p;
	}
	static Vector3 Sine2DFunction(float x, float z, float time)
	{
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + time));
		p.y += Mathf.Sin(pi * (z + time));
		p.y *= 0.5f;
		p.z = z;
		return p;
	}
	static Vector3 MultiSine2DFunction(float x, float z, float time)
	{
		Vector3 p;
		p.x = x;
		p.y = 4f * Mathf.Sin(pi * (x + z + time / 2f));
		p.y += Mathf.Sin(pi * (x + time));
		p.y += Mathf.Sin(2f * pi * (z + 2f * time)) * 0.5f;
		p.y *= 1f / 5.5f;
		p.z = z;
		return p;
	}
	static Vector3 Ripple(float x, float z, float time)
	{
		Vector3 p;
		float d = Mathf.Sqrt(x * x + z * z);
		p.x = x;
		p.y = Mathf.Sin(pi * (4f * d - time));
		p.y /= 1f + 10f * d;
		p.z = z;
		return p;
	}
	static Vector3 Cylinder(float u, float v, float time)
	{
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + time)) * 0.2f;
		p.x = r * Mathf.Sin(pi * u);
		p.y = v;
		p.z = r * Mathf.Cos(pi * u);
		return p;
	}
	static Vector3 Sphere(float u, float v, float t)
	{
		Vector3 p;
		//float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		//float r = Mathf.Cos(pi * 0.5f * v);
		float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
		float s = r * Mathf.Cos(pi * 0.5f * v);
		
		p.x = s * Mathf.Sin(pi * u);
		p.y = Mathf.Sin(pi * 0.5f * v);

		p.z = s * Mathf.Cos(pi * u);
		return p;

		/*
		Vector3 p;
		float r = Mathf.Cos(pi * 0.5f * v);
		p.x = r * Mathf.Sin(pi * u);
		p.y = Mathf.Sin(pi * 0.5f * v);
		p.z = r * Mathf.Cos(pi * u);
		return p;
		*/
	}
	static Vector3 Torus(float u, float v, float t)
	{
		Vector3 p;
		float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}
}
