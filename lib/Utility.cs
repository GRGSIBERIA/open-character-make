using UnityEngine;
using System.Collections.Generic;

public class Utility {
	/// <summary>
	/// あるGameObjectの絶対パスを調べる
	/// </summary>
	/// <param name="obj">対象のGameObject</param>
	/// <returns>絶対パス</returns>
	public static string GetGameObjectPath(GameObject obj)
	{
		List<string> names = new List<string>();
		var watchdog = obj.transform;

		while (watchdog != null)
		{
			names.Add(watchdog.name);
			watchdog = watchdog.parent;
		}

		var path = "";
		for (int i = names.Count - 1; i >= 1; i--)
			path += names[i] + "/";
		return path + names[0];
	}

	/// <summary>
	/// 対象のGameObjectすべての子供とそのパスを収集する
	/// </summary>
	/// <param name="obj">対象のGameObject</param>
	/// <returns>子どもとパス</returns>
	public static Dictionary<string, GameObject> UnfoldGameObjectChildren(GameObject obj)
	{
		var collected_pairs = CollectRecursiveCallOfChildren(obj.transform);
		var result = new Dictionary<string, GameObject>();

		foreach (var v in collected_pairs)
			result.Add(v.Key, v.Value);
		return result;
	}

	/// <summary>
	/// 対象のTransformの中身から再帰的に子供を集めてくる
	/// </summary>
	/// <param name="t">対象のtransform</param>
	/// <returns>path->GameObjectのKVPairの配列</returns>
	static List<KeyValuePair<string, GameObject>> CollectRecursiveCallOfChildren(Transform t)
	{
		// dictionary型は結合できないので便宜的にListを使う
		List<KeyValuePair<string, GameObject>> result = new List<KeyValuePair<string,GameObject>>();

		if (t.childCount <= 0) return null;

		for (int i = 0; i < t.childCount; i++)
		{
			var path = GetGameObjectPath(t.gameObject);
			var child = t.GetChild(i);
			result.Add(new KeyValuePair<string,GameObject>(path, child.gameObject));

			var collected_children = CollectRecursiveCallOfChildren(child);
			if (collected_children != null)
				result.AddRange(collected_children);
		}

		return result;
	}
}
