using UnityEngine;

public class TrolleyChoice : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveDistance = 3f; // ���E�̈ړ���
    public float moveSpeed = 2f;    // �ړ����x

    private Vector2 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position; // �����ʒu���L�^
    }

    void Update()
    {
        // X�����Ɉړ�
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // �͈͂𒴂�����������]
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
            float clampedX = Mathf.Clamp(transform.position.x, startPos.x - moveDistance, startPos.x + moveDistance);
            transform.position = new Vector2(clampedX, transform.position.y);
        }
    }
}
