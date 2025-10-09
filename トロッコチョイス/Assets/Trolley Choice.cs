using UnityEngine;

public class TrolleyChoice : MonoBehaviour
{
    [Header("ˆÚ“®Ý’è")]
    public float moveDistance = 3f; // ¶‰E‚ÌˆÚ“®•
    public float moveSpeed = 2f;    // ˆÚ“®‘¬“x

    private Vector2 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position; // ‰ŠúˆÊ’u‚ð‹L˜^
    }

    void Update()
    {
        // X•ûŒü‚ÉˆÚ“®
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // ”ÍˆÍ‚ð’´‚¦‚½‚ç•ûŒü”½“]
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
            float clampedX = Mathf.Clamp(transform.position.x, startPos.x - moveDistance, startPos.x + moveDistance);
            transform.position = new Vector2(clampedX, transform.position.y);
        }
    }
}
