	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;

	public class GetNumberObject : MonoBehaviour
	{
		[ContextMenu("Sort Ascending")]
		public void SortAscending()
		{
			if (objectsInfo != null)
				objectsInfo = objectsInfo.OrderBy(obj => obj.number).ToArray();
		}

		[ContextMenu("Sort Descending")]
		public void SortDescending()
		{
			if (objectsInfo != null)
				objectsInfo = objectsInfo.OrderByDescending(obj => obj.number).ToArray();
		}
	[System.Serializable]
	public class ObjectInfo
	{
		public string name;
		public int number;
		public bool createMission;
		public Sprite sprite;
	}

	public ObjectInfo[] objectsInfo;

	void Reset()
	{
		GetObjectsInfo();
		AssignSprites();
	}

	[ContextMenu("Get Objects Info")]
	public void GetObjectsInfo()
	{
		Dictionary<string, int> dict = new Dictionary<string, int>();
		foreach (Transform child in transform)
		{
			string cleanName = child.name.Replace("(Clone)", "").Trim();
			if (dict.ContainsKey(cleanName))
				dict[cleanName]++;
			else
				dict[cleanName] = 1;
		}
		objectsInfo = dict.Select(kv => new ObjectInfo { name = kv.Key, number = kv.Value, createMission = false, sprite = null })
			.OrderBy(obj => obj.number)
			.ToArray();
	}

	[ContextMenu("Assign Sprites")]
	public void AssignSprites()
	{
		foreach (var obj in objectsInfo)
		{
			obj.sprite = FindSpriteByName(obj.name);
		}
	}

	public Sprite FindSpriteByName(string objectName)
	{
		// Tìm tất cả sprite trong Assets bằng Resources hoặc AssetDatabase (Editor)
		// Nếu dùng Resources, sprite phải nằm trong thư mục Resources
		Sprite sprite = Resources.Load<Sprite>(objectName);
		if (sprite != null)
			return sprite;
#if UNITY_EDITOR
		// Nếu không tìm thấy, thử tìm bằng AssetDatabase (chỉ dùng trong Editor)
		string[] guids = UnityEditor.AssetDatabase.FindAssets(objectName + " t:Sprite");
		foreach (string guid in guids)
		{
			string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
			Sprite s = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
			if (s != null && s.name == objectName)
				return s;
		}
#endif
		return null;
	}
}
