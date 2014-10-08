using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public GameObject level;
	public GameObject levelContainer;
	public GameObject screenFader;
	public List<GameObject> generatedObjects;

	private delegate void BlockGenerator(Vector3 pos, Quaternion rotation, GameObject type);

	void Start()
	{
		generatedObjects = new List<GameObject>();
		BlockGenerator generator = StandardGenerator;
		GenerateLevel(level, generator);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.L)) {
			GameObject levelToLoad = (level == GameObject.Find("Level 1")) ? GameObject.Find("Level 2") : levelToLoad = GameObject.Find("Level 1");
			level = levelToLoad;
			StartCoroutine(ChangeLevel(levelToLoad));
		}
  }

	void OnDrawGizmos()
	{
		BlockGenerator generator = GizmoGenerator;
		GenerateLevel(level, generator);
	}

	IEnumerator ChangeLevel(GameObject newLevel)
	{
		foreach (var child in generatedObjects) { GameObject.Destroy(child); }

		yield return StartCoroutine(screenFader.GetComponent<FadeInOut>().Fade());

		BlockGenerator generator = StandardGenerator;
		GenerateLevel(newLevel, generator);

		yield return StartCoroutine(screenFader.GetComponent<FadeInOut>().Fade());

	}

	private void GenerateLevel(GameObject level, BlockGenerator generator)
	{
		Level levelObj = level.GetComponent<Level>();
		string[] levelAry = levelObj.levelMap.text.Replace("\r\n","\n").Split('\n');

		int levelHeight = levelAry.Length - 1;
		
		Vector3 currentPos = new Vector3(0, (float)levelHeight, 0);
		
		foreach (string line in levelAry) {
			currentPos.x = 0;
			char[] blocks = line.ToCharArray();
			foreach (char c in blocks) {
				LevelBlock b = levelObj.getBlock(c);
				
				if (b != null)
					generator(currentPos, levelContainer.transform.rotation, b.blockObject);
				
				currentPos.x++;
			}
			currentPos.y--;
		}
	}

	private void StandardGenerator(Vector3 pos, Quaternion rotation, GameObject type)
	{
		GameObject currentBlock = Instantiate(type, pos, rotation) as GameObject;
		generatedObjects.Add(currentBlock);
	}

	private void GizmoGenerator(Vector3 pos, Quaternion rotation, GameObject type)
	{
		Gizmos.DrawWireCube(pos, new Vector3(1.0f, 1.0f, 1.0f));
	}
}
