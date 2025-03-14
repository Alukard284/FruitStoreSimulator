using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public GameObject pickupButton; // ������ �����������
    public float throwForce = 10f; // ���� ������
    public float pickupDistance = 3f; // ���������� ��� �������
    public Transform handTransform; // ������������ ������ (����)
    public float smoothSpeed = 10f; // �������� �������� �����������
    public float swipeThreshold = 50f; // ����������� ���������� ��� ������

    private GameObject heldObject;
    private bool isHolding;
    private Vector3 velocity = Vector3.zero;
    private Vector2 touchStartPos;
    private bool isSwiping;

    private void Update()
    {
        if (Input.touchCount > 0 && !isHolding)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // ���������� ��������� ������� �������
                touchStartPos = touch.position;
                isSwiping = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // ���������, �������� �� ������� �������
                if (Vector2.Distance(touchStartPos, touch.position) > swipeThreshold)
                {
                    isSwiping = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && !isSwiping)
            {
                // ���� ��� �� �����, ��������� ������ ��������
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
                {
                    if (hit.collider.CompareTag("Pickable"))
                    {
                        PickUp(hit.collider.gameObject);
                    }
                }
            }
        }

        // ������� ����������� ������� � ����
        if (isHolding && heldObject != null)
        {
            heldObject.transform.position = Vector3.SmoothDamp(
                heldObject.transform.position,
                handTransform.position,
                ref velocity,
                smoothSpeed * Time.deltaTime
            );
        }
    }

    private void PickUp(GameObject obj)
    {
        heldObject = obj;
        heldObject.GetComponent<Rigidbody>().isKinematic = true; // ��������� ������
        isHolding = true;
        pickupButton.SetActive(true);

        // ������ ������ �������� ��� ����
        heldObject.transform.SetParent(handTransform);
    }

    public void Throw()
    {
        if (heldObject == null) return;

        // ���������� ������
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        // ���������� ������������ ������
        heldObject.transform.SetParent(null);

        heldObject = null;
        isHolding = false;
        pickupButton.SetActive(false);
    }
}