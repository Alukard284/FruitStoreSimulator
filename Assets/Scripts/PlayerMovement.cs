using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� �������� ���������
    public bl_Joystick joystick; // ������ �� ��������
    public Transform cameraTransform; // ������ �� ��������� ������

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �������� ���� �� ���������
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // ������� ������ �������� �� ������ �����
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // ���� ���� ���� �� ���������
        if (moveDirection != Vector3.zero)
        {
            // ����������� ������ �������� � ����������� ������
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            moveDirection.y = 0; // �������� ������������ ������������

            // ������� ���������
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // ������������ ��������� � ����������� ��������
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}