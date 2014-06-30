using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelBlock
{
	public string character;
	public GameObject blockObject;
}

public class Level : MonoBehaviour
{
	public TextAsset levelMap;
	public LevelBlock[] blocks;

	public LevelBlock getBlock(char c) {
		LevelBlock block = null;
		foreach (LevelBlock lb in blocks) {
			if (c.ToString().Equals(lb.character)) {
				block = lb;
			}
		}
		return block;
	}
}