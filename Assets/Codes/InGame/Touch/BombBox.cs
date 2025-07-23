using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BombBox : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float pushForce = 10f;

    bool startEffect;

    [Header("Effect Settings")]
    MeshRenderer meshRenderer;
    public Material highlightMaterial; // 임시로 입힐 마테리얼
    Material baseMaterial;
    public GameObject bombEffect;

    void Start()
    {
        // 시작 시 기존 material 저장
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = meshRenderer.material;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if(!startEffect)
            {
                startEffect = true;
                StartCoroutine(HighlightAndDestroy());
            }
        }
    }

    public void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] hitObjects = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in hitObjects)
        {
            if (hit.CompareTag("Box"))
            {
                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null && rb.gameObject != this.gameObject)
                {
                    Vector3 dir = (rb.position - explosionPos).normalized;
                    rb.AddForce(dir * pushForce, ForceMode.Impulse);
                }
            }
        }

        // 이펙트나 사운드 추가
        Destroy(gameObject); // 터지면서 사라짐
    }
    
    IEnumerator HighlightAndDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material = highlightMaterial;

        yield return new WaitForSeconds(0.3f);
        meshRenderer.material = baseMaterial;

        yield return new WaitForSeconds(0.3f);
        meshRenderer.material = highlightMaterial;

        yield return new WaitForSeconds(0.1f);
        BoxManager.Instance.spawnedBoxes.Remove(gameObject);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.boxBomb);
        }

        Instantiate(bombEffect, transform.position, Quaternion.identity);
        Explode();
    }

}
