using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockColor { Red, Blue, Green, Yellow }

public class StickyBlock : MonoBehaviour
{
    public BlockColor blockColor;
    public HashSet<StickyBlock> touchingBlocks = new HashSet<StickyBlock>();
    public bool effectTriggered = false;


    [Header("Effect Settings")]
    public Material highlightMaterial; // 임시로 입힐 마테리얼
    public GameObject firstStack;
    public GameObject secondStack;
    public GameObject thirdStack;

    private void Start()
    {
        // 시작 시 기존 material 저장
        var renderer = GetComponent<MeshRenderer>();
    }

        private void OnCollisionEnter(Collision collision)
    {
        StickyBlock otherBlock = collision.collider.GetComponent<StickyBlock>();
        if (otherBlock != null && !otherBlock.effectTriggered)
        {
            touchingBlocks.Add(otherBlock);
            otherBlock.touchingBlocks.Add(this);
            Debug.Log("충돌 감지");

            CheckConnectionGroup();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        StickyBlock otherBlock = collision.collider.GetComponent<StickyBlock>();
        if (otherBlock != null)
        {
            touchingBlocks.Remove(otherBlock);
            otherBlock.touchingBlocks.Remove(this);

            //effectTriggered = false;
        }
    }

    private void CheckConnectionGroup()
    {
        var group = GetConnectedGroupByColor(this.blockColor);

        if (group.Count >= 3 && !effectTriggered)
        {
            Debug.Log($"{this.blockColor} 블록이 3개 이상 연결됨!");
            VfxManager.instance.stack++;

            // effectTriggered 먼저 설정해서 중복 방지
            foreach (var block in group)
            {
                block.effectTriggered = true;
                BoxManager.Instance.GoaledBoxes.Remove(block.gameObject);
                BoxManager.Instance.spawnedBoxes.Remove(block.gameObject);
            }

            // 연결된 그룹 전부 파괴
            foreach (var block in group)
            {
                Rigidbody rigidbody = block.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                }

                block.StartCoroutine(block.HighlightAndDestroy());
            }
        }
    }

    private HashSet<StickyBlock> GetConnectedGroupByColor(BlockColor targetColor)
    {
        HashSet<StickyBlock> visited = new HashSet<StickyBlock>();
        Queue<StickyBlock> queue = new Queue<StickyBlock>();

        visited.Add(this);
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            StickyBlock current = queue.Dequeue();

            foreach (StickyBlock neighbor in current.touchingBlocks)
            {
                if (!visited.Contains(neighbor) && neighbor.blockColor == targetColor)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.Log("visited group count: " + visited.Count);
        return visited;
    }

    private IEnumerator HighlightAndDestroy()
    {
        // 하이라이트 마테리얼로 바꾸기
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null && highlightMaterial != null)
            renderer.material = highlightMaterial;

        // 0.2초 대기
        yield return new WaitForSeconds(0.15f);

        switch (VfxManager.instance.stack)
        {
            case 1:
                Instantiate(firstStack, transform.position, firstStack.transform.rotation);
                Debug.Log("첫 콤보");
                break;
            case 2:
                Instantiate(secondStack, transform.position, secondStack.transform.rotation);
                Debug.Log("둘 콤보");

                break;
            case 3:
                Instantiate(thirdStack, transform.position, thirdStack.transform.rotation);
                InGameGoldUI.Instance.GetGold();
                Debug.Log("셋 콤보");

                break;
            default:
                Instantiate(thirdStack, transform.position, thirdStack.transform.rotation);
                InGameGoldUI.Instance.GetGold();
                Debug.Log("무겐 콤보");

                break;
        }

        // 원래 마테리얼로 되돌릴 필요 없으니 Destroy
        Destroy(gameObject);
    }
}
