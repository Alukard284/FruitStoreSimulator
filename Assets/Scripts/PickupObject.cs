using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public GameObject pickupButton; // Кнопка выкидывания
    public float throwForce = 10f; // Сила броска
    public float pickupDistance = 3f; // Расстояние для подбора
    public Transform handTransform; // Родительский объект (рука)
    public float smoothSpeed = 10f; // Скорость плавного перемещения
    public float swipeThreshold = 50f; // Минимальное расстояние для свайпа

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
                // Запоминаем начальную позицию касания
                touchStartPos = touch.position;
                isSwiping = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Проверяем, является ли касание свайпом
                if (Vector2.Distance(touchStartPos, touch.position) > swipeThreshold)
                {
                    isSwiping = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && !isSwiping)
            {
                // Если это не свайп, проверяем подбор предмета
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

        // Плавное перемещение объекта в руку
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
        heldObject.GetComponent<Rigidbody>().isKinematic = true; // Отключаем физику
        isHolding = true;
        pickupButton.SetActive(true);

        // Делаем объект дочерним для руки
        heldObject.transform.SetParent(handTransform);
    }

    public void Throw()
    {
        if (heldObject == null) return;

        // Возвращаем физику
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        // Сбрасываем родительский объект
        heldObject.transform.SetParent(null);

        heldObject = null;
        isHolding = false;
        pickupButton.SetActive(false);
    }
}