using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
	
	
	
	public CircleCollider2D  hole2DColider;
	public PolygonCollider2D ground2DColider;
	public MeshCollider GeneratedMeshColider;
	public float initialScale = 0.5f;
	Mesh GenerateMesh;


	private void Start() {
		initialScale = transform.localScale.x;
	}

	private void FixedUpdate() {
		if(transform.hasChanged == true) {
			transform.hasChanged = false;
			hole2DColider.transform.position = new Vector2(transform.position.x, transform.position.z);
			hole2DColider.transform.localScale = transform.localScale * initialScale;
			MakeHole2D();
			Make3DMeshColider();
		}
	}

	public void MakeHole2D() {
		int segments = 16; 
		Vector2[] points = new Vector2[segments];
		float angleStep = 360f / segments;
		float currentAngle = 0f;
		float currentRadius = (initialScale - 0.1f) / 2f;
		
		Vector2 holeWorldPosition = hole2DColider.transform.position;

		for (int i = 0; i < segments; i++) {
			float radians = Mathf.Deg2Rad * currentAngle;
			points[i] = new Vector2(holeWorldPosition.x + currentRadius * Mathf.Cos(radians),
				holeWorldPosition.y + currentRadius * Mathf.Sin(radians));
			currentAngle += angleStep;
		}
		ground2DColider.pathCount = 2;
		ground2DColider.SetPath(1, points);
	}
	public void Make3DMeshColider() {
		if (GenerateMesh != null) Destroy(GenerateMesh);
		GenerateMesh = ground2DColider.CreateMesh(true, true);
		GeneratedMeshColider.sharedMesh = GenerateMesh;
		
		
		// Test make Collider under the Gameobject 
		
		
		
	}

	public void changeInitialScale(float scale) {
		this.initialScale = scale;
	}
	
	


}
