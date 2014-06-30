using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject level;
	public GameObject levelContainer;
	public GameObject screenFader;

	private delegate void BlockGenerator(Vector3 pos, string type, GameObject parent);

	void Start()
	{
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
		foreach (Transform child in levelContainer.transform)
			GameObject.Destroy(child.gameObject);

		Debug.Log("Waiting for fade: " + Time.time);
		yield return StartCoroutine(screenFader.GetComponent<FadeInOut>().Fade());
		Debug.Log("Fade is complete: " + Time.time);

		BlockGenerator generator = StandardGenerator;
		GenerateLevel(newLevel, generator);

		yield return StartCoroutine(screenFader.GetComponent<FadeInOut>().Fade());

	}

	private void GenerateLevel(GameObject level, BlockGenerator generator)
	{
		Level levelObj = level.GetComponent<Level>();
		string[] levelAry = levelObj.levelMap.text.Split();
		int levelHeight = levelAry.Length - 1;
		
		Vector3 currentPos = new Vector3(0, (float)levelHeight, 0);
		
		foreach (string line in levelAry) {
			currentPos.x = 0;
			char[] blocks = line.ToCharArray();
			foreach (char c in blocks) {
				LevelBlock b = levelObj.getBlock(c);
				
				if (b != null)
					generator(currentPos, b.blockObject.name, levelContainer);
				
				currentPos.x++;
			}
			currentPos.y--;
		}
	}

	private void StandardGenerator(Vector3 pos, string type, GameObject parent)
	{
		GameObject currentBlock = Instantiate(Resources.Load<GameObject>(type)) as GameObject;
		currentBlock.transform.position = pos;
		currentBlock.transform.parent = parent.transform;
	}

	private void GizmoGenerator(Vector3 pos, string type, GameObject parent)
	{
		Gizmos.DrawWireCube(pos, new Vector3(1.0f, 1.0f, 1.0f));
	}
}
