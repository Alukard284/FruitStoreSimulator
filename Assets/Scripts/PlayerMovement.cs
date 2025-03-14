using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения персонажа
    public bl_Joystick joystick; // Ссылка на джойстик
    public Transform cameraTransform; // Ссылка на трансформ камеры

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Получаем ввод от джойстика
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Создаем вектор движения на основе ввода
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Если есть ввод от джойстика
        if (moveDirection != Vector3.zero)
        {
            // Преобразуем вектор движения в направление камеры
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            moveDirection.y = 0; // Обнуляем вертикальную составляющую

            // Двигаем персонажа
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Поворачиваем персонажа в направлении движения
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}