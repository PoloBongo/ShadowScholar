using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    private Transform targetTransform; 
    private GameObject minimap; 
    private RectTransform minimapRect;
    private float minimapSize;

    public GameObject iconPrefab;
    private RectTransform iconRect; 

    void Start()
    {
        targetTransform = transform;
        minimap = GameObject.Find("Mini-Map");
        minimapRect = minimap.GetComponent<RectTransform>();
        minimapSize = GameObject.Find("Mini-Map-Camera").GetComponent<Camera>().orthographicSize * 2;

        if(iconPrefab != null)
        {
            Instantiate(iconPrefab, minimapRect);
            iconRect = iconPrefab.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("iconPrefab can't be null");
        }
    }

    void Update()
    {
        if (iconPrefab != null)
        {
            Vector3 targetPosition = targetTransform.position;

            float xPos = (targetPosition.x / minimapSize) * minimapRect.rect.width;
            float yPos = (targetPosition.z / minimapSize) * minimapRect.rect.height;

            iconRect.anchoredPosition = new Vector2(xPos, yPos);
            iconRect.rotation = Quaternion.Euler(0, 0, -targetTransform.eulerAngles.y);
        }
    }
}

