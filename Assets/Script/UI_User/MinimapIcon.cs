using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public GameObject iconPrefab; 
    private Transform playerTransform; 

    private RectTransform minimapRect; 
    private Camera minimapCamera;
    private RectTransform iconRect;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        minimapCamera = GameObject.Find("Mini-Map-Camera").GetComponent<Camera>();
        if (minimapCamera == null)
        {
            Debug.LogError("Mini-Map Camera not found!");
            return;
        }

        minimapRect = GameObject.Find("Mini-Map").GetComponent<RectTransform>();
        if (minimapRect == null)
        {
            Debug.LogError("Mini-Map RectTransform not found!");
            return;
        }

        if (iconPrefab != null)
        {
            GameObject iconInstance = Instantiate(iconPrefab, minimapRect);
            iconRect = iconInstance.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Icon prefab is null!");
        }
    }

    void Update()
    {
        if (iconRect != null && playerTransform != null)
        {
            Vector3 worldPosition = transform.position;

            Vector3 viewportPosition = minimapCamera.WorldToViewportPoint(worldPosition);

            if (viewportPosition.z > 0 && viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1)
            {
                iconRect.gameObject.SetActive(true);

                float xPos = (viewportPosition.x - 0.5f) * minimapRect.rect.width;
                float yPos = (viewportPosition.y - 0.5f) * minimapRect.rect.height;

                iconRect.anchoredPosition = new Vector2(xPos, yPos);

                iconRect.rotation = Quaternion.Euler(0, 0, -transform.eulerAngles.y);
            }
            else
            {
                iconRect.gameObject.SetActive(false);
            }
        }
    }
}