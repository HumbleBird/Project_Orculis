using UnityEditor;
using UnityEngine;

public class AutoAssignMeshCollider : MonoBehaviour
{
    [MenuItem("Tools/Assign Mesh Collider")]
    private static void AssignMeshColliders()
    {
        // 선택된 오브젝트를 가져옵니다.
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("선택된 오브젝트가 없습니다.");
            return;
        }

        foreach (Object obj in selectedObjects)
        {
            // Hierarchy 또는 Project에서 선택된 오브젝트를 GameObject로 캐스팅
            GameObject gameObject = obj as GameObject;
            if (gameObject == null)
            {
                Debug.LogWarning($"'{obj.name}'은(는) GameObject가 아닙니다. 건너뜁니다.");
                continue;
            }

            // MeshCollider 추가 (이미 존재하면 추가하지 않음)
            MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();
            }

            // MeshCollider에 메쉬 할당
            if (meshCollider.sharedMesh == null)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
                {
                    meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                    Debug.Log($"'{gameObject.name}'의 MeshCollider에 메쉬가 성공적으로 할당되었습니다.");
                }
                else
                {
                    Debug.LogWarning($"'{gameObject.name}'에 SkinnedMeshRenderer가 없거나 메쉬가 없습니다. 작업을 건너뜁니다.");
                }
            }
            else
            {
                Debug.Log($"'{gameObject.name}'의 MeshCollider는 이미 메쉬가 할당되어 있습니다.");
            }
        }
    }
}
