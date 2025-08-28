using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string bulletName;      // Tên viên đạn
    public float BulletName => _cooldown;// Thuộc tính chỉ đọc để truy cập tên viên đạn

    [SerializeField] private float _speed;
    public float Speed => _speed;// Thuộc tính chỉ đọc để truy cập tốc độ viên đạn

    [SerializeField] private float _damage;
    public float Damage => _damage;// Thuộc tính chỉ đọc để truy cập sát thương viên đạn

    [SerializeField] private float _lifetime;
    public float Lifetime => _lifetime;// Thuộc tính chỉ đọc để truy cập thời gian sống của viên đạn

    [SerializeField] private float _knockback;
    public float Knockback => _knockback;// Thuộc tính chỉ đọc để truy cập lực đẩy lùi của viên đạn
    [SerializeField] private float _cooldown; // Thời gian hồi (giây)
    public float Cooldown => _cooldown;// Thuộc tính chỉ đọc để truy cập thời gian hồi của viên đạn

    [SerializeField] private float _rotationSpeed; // Tốc độ quay (độ/giây)
    public float RotationSpeed => _rotationSpeed;// Thuộc tính chỉ đọc để truy cập tốc độ quay của viên đạn

    private GameObject visualEffect;
    public GameObject VisualEffect => visualEffect;

    private void Awake()
    {
        bulletName = gameObject.name; // Gán tên bằng tên của GameObject
        Destroy(gameObject, _lifetime); // Hủy viên đạn sau một khoảng thời gian nhất định
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector2.up * RotationSpeed * Time.deltaTime);
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }
}
