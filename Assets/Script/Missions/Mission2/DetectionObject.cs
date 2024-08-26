using UnityEngine;

public class DetectionObject : MonoBehaviour
{
    private Camera playerCamera;
    private bool canSearch;
    private bool objectIsView;
    private ObjectifMission objectifMission;

    private void Start()
    {
        canSearch = false;
        objectIsView = false;

        GameObject findObjectifMission = GameObject.Find("ObjectifMission");
        if (findObjectifMission != null )
            objectifMission = findObjectifMission.GetComponent<ObjectifMission>();
    }

    public void InitDetectionObject(Camera _camera, bool _canSearch)
    {
        playerCamera = _camera;
        canSearch = _canSearch;
    }

    void Update()
    {
        if (canSearch)
        {
            if (IsObjectInView())
            {
                if (!objectIsView)
                {
                    objectifMission.UpdateObjectif(1, 1);
                    objectifMission.ShowTextMotivation();
                    objectIsView = true;
                }
            }
        }
    }

    private bool IsObjectInView()
    {
        if (Vector3.Distance(this.gameObject.transform.position, playerCamera.transform.position) <= 30)
        {
            Vector3 screenPoint = playerCamera.WorldToViewportPoint(this.gameObject.transform.position);
            bool isInView = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (isInView)
            {
                Ray ray = playerCamera.ScreenPointToRay(playerCamera.WorldToScreenPoint(this.gameObject.transform.position));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
