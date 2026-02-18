using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public float distanceScaleFactor = 0.1f;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - (player1.transform.position + player2.transform.position) / 2;
    }

    void Update()
    {
        Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2;
        float distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        
        float scaleFactor = 0.5f + (distance * distanceScaleFactor);
        Vector3 scaledOffset = offset * scaleFactor;
        
        transform.position = midpoint + scaledOffset;
    }
}
