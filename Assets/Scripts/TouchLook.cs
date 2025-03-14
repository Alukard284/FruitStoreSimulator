using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLook : MonoBehaviour
{
    public float sensitivity = 0.5f; // ���������������� ���������� �������
    public bl_Joystick joystick; // ������ �� ��������

    private Vector2 touchStartPos;
    private bool isTouching;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ���������, ��������� �� ������� �� ���������
            if (IsTouchOnJoystick(touch.position))
            {
                return; // ���������� ������� �� ���������
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

    // ��������, ��������� �� ������� �� ���������
    private bool IsTouchOnJoystick(Vector2 touchPosition)
    {
        if (joystick == null) return false;

        // �������� RectTransform ���������
        RectTransform joystickRect = joystick.GetComponent<RectTransform>();

        // ����������� ������� ������� � ��������� ���������� ���������
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickRect,
            touchPosition,
            null,
            out localPoint
        );

        // ���������, ��������� �� ����� ������ RectTransform ���������
        return joystickRect.rect.Contains(localPoint);
    }
}