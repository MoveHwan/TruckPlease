using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchOutline : MonoBehaviour
{
    ThrowBox throwBox;

    private Material materialInstance;
    public bool isOutlined = false;

    [SerializeField] private float outlineWidth = 0.05f;

    // 감지할 레이어 설정 (에디터에서 바꿔도 됨)
    public LayerMask targetLayer;

    void Awake()
    {
        throwBox = GetComponent<ThrowBox>();
    }
    void Start()
    {
        materialInstance = GetComponent<Renderer>().material;
        materialInstance.SetFloat("_Outline", 0f);
        targetLayer = 1 << 10;
    }

    void Update()
    {
        if (!BoxManager.Instance.keepItem || !throwBox.throwDone)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 단 하나만 체크하고, 특정 레이어만 대상으로 함
            if (Physics.Raycast(ray, out hit, 100f, targetLayer))
            {
                if (hit.transform == transform)
                {
                    OnOutline();
                }
            }
        }
    }

    void OnOutline()
    {
        if (isOutlined)
        {
            OffOutLine();
        }
        else
        {
            List<GameObject> GoaledBoxes  = BoxManager.Instance.GoaledBoxes;
            foreach (GameObject go in GoaledBoxes) 
            {
                TouchOutline touchOutline = go.GetComponent<TouchOutline>();
                touchOutline.OffOutLine();
            }
        
            isOutlined = !isOutlined;

            materialInstance.SetFloat("_Outline", isOutlined ? outlineWidth : 0);
        }
    }

    void OffOutLine()
    {
        isOutlined = false;

        materialInstance.SetFloat("_Outline", 0f);
    }
}
