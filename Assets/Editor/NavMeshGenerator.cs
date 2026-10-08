using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.AI;
using System.Reflection;
using System;

public class NavMeshGenerator : Editor
{
	[MenuItem("NavMesh Generator/Generate NavMesh")]
	public static void GenerateNavMesh()
	{
		SerializedObject settings = new SerializedObject(UnityEditor.AI.NavMeshBuilder.navMeshSettingsObject);

		settings.FindProperty("m_BuildSettings.agentRadius").floatValue = 0.5f;
		settings.FindProperty("m_BuildSettings.agentHeight").floatValue = 2f;
		settings.FindProperty("m_BuildSettings.agentSlope").floatValue = 45f;
		settings.FindProperty("m_BuildSettings.agentClimb").floatValue = 0.4f;
		settings.FindProperty("m_BuildSettings.manualCellSize").boolValue = false;

		settings.ApplyModifiedProperties();
		settings.Update();

		List<GameObject> generatedObjects = new List<GameObject>();

		foreach (Transform child in GetChildren(GameObject.Find("Triggers").transform))
		{
			SetNavStatic(child.gameObject, false);

			if (child.name.StartsWith("SliderDoor"))
			{
				GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
				plane.transform.parent = child;

				plane.transform.localScale = new Vector3(1.36f, 1f, EditorSceneManager.GetActiveScene().name.StartsWith("MAP2") ? 0.185f : EditorSceneManager.GetActiveScene().name.StartsWith("MAP4") ? 0.175f : 0.14f);
				plane.transform.localRotation = Quaternion.identity;
				plane.transform.localPosition = Vector3.zero;

				plane.isStatic = true;
				GameObjectUtility.SetNavMeshArea(plane, 3);

				generatedObjects.Add(plane);
			}
		}
		
		foreach (Transform child in GetChildren(GameObject.Find("ROOM--DOOR").transform))
		{
			string[] split = child.name.Split('_');

			if (split.Contains("back"))
			{
				SetNavStatic(child.gameObject, false);
			}
			else
			{
				if (EditorSceneManager.GetActiveScene().name.StartsWith("MAP1"))
				{
					if (split.Contains("Open"))
					{
						SetNavStatic(child.gameObject, false);

						GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
						plane.transform.parent = child;

						plane.transform.localScale = new Vector3(1f, 1f, 0.4f);
						plane.transform.localRotation = Quaternion.Euler(0f, 15f, 0f);
						plane.transform.localPosition = new Vector3(0f, 0.15f, -1.5f);

						plane.isStatic = true;
						GameObjectUtility.SetNavMeshArea(plane, 3);

						generatedObjects.Add(plane);
					}
					else
					{
						SetNavStatic(child.gameObject, true);
					}
				}
			}
		}

		List<GameObject> disabledSpawnPoints = DisablePlayerSpawnPoints();

		try
		{
			UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
		}
		finally
		{
			foreach (GameObject spawnPoint in disabledSpawnPoints)
			{
				if (spawnPoint != null)
				{
					spawnPoint.SetActive(true);
				}
			}

			foreach (GameObject generatedObject in generatedObjects)
			{
				DestroyImmediate(generatedObject);
			}
		}
	}

	private static List<GameObject> DisablePlayerSpawnPoints()
	{
		List<GameObject> disabled = new List<GameObject>();

		for (int i = 0; i < EditorSceneManager.sceneCount; i++)
		{
			UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetSceneAt(i);
			if (!scene.isLoaded)
			{
				continue;
			}

			foreach (GameObject root in scene.GetRootGameObjects())
			{
				foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
				{
					GameObject go = transform.gameObject;
					if (go.activeSelf && (go.tag == "PlayerSpawnPoint" || go.name == "PlayerSpawnPoint"))
					{
						go.SetActive(false);
						disabled.Add(go);
					}
				}
			}
		}

		if (disabled.Count == 0)
		{
			Debug.LogWarning("NavMeshGenerator: no PlayerSpawnPoint found in " + EditorSceneManager.GetActiveScene().name + ".");
		}

		return disabled;
	}

	private static void SetNavStatic(GameObject root, bool isStatic)
	{
		root.isStatic = isStatic;

		foreach (Transform child in GetChildren(root.transform))
		{
			SetNavStatic(child.gameObject, isStatic);
		}
	}

	private static List<Transform> GetChildren(Transform root)
	{
		List<Transform> children = new List<Transform>();

		for (int i = 0; i < root.childCount; i++)
		{
			children.Add(root.GetChild(i));
		}

		return children;
	}
}
