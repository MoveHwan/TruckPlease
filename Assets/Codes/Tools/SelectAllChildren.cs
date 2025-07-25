using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectAllChildren : MonoBehaviour
{
    [MenuItem("Tools/Select All First Children")]
    static void SelectAllFirstChildren()
    {
        GameObject[] selectedParents = Selection.gameObjects;
        List<GameObject> childObjects = new List<GameObject>();

        foreach (GameObject parent in selectedParents)
        {
            if (parent.transform.childCount > 0)
            {
                // 자식 중 첫 번째 자식 선택 (혹은 특정 이름으로 검색 가능)
                Transform child = parent.transform.GetChild(0); // 첫 번째 자식
                // 또는 이름으로 찾기:
                // Transform child = parent.transform.Find("Text_UserNameEter");
                if (child != null)
                {
                    childObjects.Add(child.gameObject);
                }
            }
        }

        if (childObjects.Count > 0)
        {
            Selection.objects = childObjects.ToArray();
            Debug.Log($"자식 오브젝트 {childObjects.Count}개를 선택했습니다.");
        }
        else
        {
            Debug.LogWarning("선택된 부모 오브젝트에서 자식을 찾을 수 없습니다.");
        }
    }
}
