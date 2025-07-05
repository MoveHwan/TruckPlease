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
    public Material highlightMaterial; // �ӽ÷� ���� ���׸���
    public GameObject firstStack;
    public GameObject secondStack;
    public GameObject thirdStack;

    private void Start()
    {
        // ���� �� ���� material ����
        var renderer = GetComponent<MeshRenderer>();
    }

        private void OnCollisionEnter(Collision collision)
    {
        StickyBlock otherBlock = collision.collider.GetComponent<StickyBlock>();
        if (otherBlock != null && !otherBlock.effectTriggered)
        {
            touchingBlocks.Add(otherBlock);
            otherBlock.touchingBlocks.Add(this);
            Debug.Log("�浹 ����");

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
            Debug.Log($"{this.blockColor} ����� 3�� �̻� �����!");
            VfxManager.instance.stack++;

            // effectTriggered ���� �����ؼ� �ߺ� ����
            foreach (var block in group)
            {
                block.effectTriggered = true;
                BoxManager.Instance.GoaledBoxes.Remove(block.gameObject);
                BoxManager.Instance.spawnedBoxes.Remove(block.gameObject);
            }

            // ����� �׷� ���� �ı�
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
        // ���̶���Ʈ ���׸���� �ٲٱ�
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null && highlightMaterial != null)
            renderer.material = highlightMaterial;

        // 0.2�� ���
        yield return new WaitForSeconds(0.15f);

        switch (VfxManager.instance.stack)
        {
            case 1:
                Instantiate(firstStack, transform.position, firstStack.transform.rotation);
                Debug.Log("ù �޺�");
                break;
            case 2:
                Instantiate(secondStack, transform.position, secondStack.transform.rotation);
                Debug.Log("�� �޺�");

                break;
            case 3:
                Instantiate(thirdStack, transform.position, thirdStack.transform.rotation);
                InGameGoldUI.Instance.GetGold();
                Debug.Log("�� �޺�");

                break;
            default:
                Instantiate(thirdStack, transform.position, thirdStack.transform.rotation);
                InGameGoldUI.Instance.GetGold();
                Debug.Log("���� �޺�");

                break;
        }

        // ���� ���׸���� �ǵ��� �ʿ� ������ Destroy
        Destroy(gameObject);
    }
}
