using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float gravity = -30.123f;
    public float tilt = 5f;
    public float maxLift = 4f;
    public float smoothSpeed = 4f;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;
    private float lift;

    public float topLimit = 5f;
    public float bottomLimit = -5f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        float pitch = SerialController.soundLevel;

        float mapped = Mathf.InverseLerp(200f, 1000f, pitch);
        float force = mapped * maxLift;

        direction.y += force * Time.deltaTime;
        direction.y += gravity * Time.deltaTime;

        transform.position += direction * Time.deltaTime;

        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;

        if (transform.position.y > topLimit || transform.position.y < bottomLimit)
        {
            GameManager.Instance.GameOver();
        }
    }


    private void AnimateSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length) spriteIndex = 0;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
            GameManager.Instance.GameOver();
        else if (other.gameObject.CompareTag("Scoring"))
            GameManager.Instance.IncreaseScore();
    }
}
