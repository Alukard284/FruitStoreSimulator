using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLook : MonoBehaviour
{
    public float sensitivity = 0.5f; // Чувствительность управления камерой
    public bl_Joystick joystick; // Ссылка на джойстик

    private Vector2 touchStartPos;
    private bool isTouching;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Проверяем, находится ли касание на джойстике
            if (IsTouchOnJoystick(touch.position))
            {
                return; // Игнорируем касание на джойстике
            }

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                Vector2 delta = touch.position - touchStartPos;
                transform.Rotate(0, delta.x * sensitivity, 0);
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
    }

    // Проверка, находится ли касание на джойстике
    private bool IsTouchOnJoystick(Vector2 touchPosition)
    {
        if (joystick == null) return false;

        // Получаем RectTransform джойстика
        RectTransform joystickRect = joystick.GetComponent<RectTransform>();

        // Преобразуем позицию касания в локальные координаты джойстика
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickRect,
            touchPosition,
            null,
            out localPoint
        );

        // Проверяем, находится ли точка внутри RectTransform джойстика
        return joystickRect.rect.Contains(localPoint);
    }
}