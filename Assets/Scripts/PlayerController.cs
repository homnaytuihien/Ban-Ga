using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSmooth = 5f;// độ mượt khi di chuyển

    [SerializeField] private Transform _firePoint;// điểm bắn

    [SerializeField] private Bullet[] _bulletList;// danh sách đạn

    [SerializeField] int _bulletIndex;// chỉ số đạn hiện tại

    [SerializeField] private bool _isOnCooldown = false; // cờ cooldown

    [SerializeField] private GameObject _explosionEffect; // hiệu ứng nổ khi bị hủy

    private Vector2 _clampMin;// giới hạn min

    private Vector2 _clampMax;// giới hạn max

    private Camera _mainCam;// camera chính

    private Vector2 _targetPosition;// vị trí mục tiêu




    void Start()
    {
        _mainCam = Camera.main;
        _targetPosition = transform.position;
        float camHeight = _mainCam.orthographicSize;// nửa chiều cao
        float camWidth = camHeight * _mainCam.aspect;// nửa chiều rộng (tính theo tỉ lệ màn hình)
        _clampMin = new Vector2(-camWidth + 1f, -camHeight + 1f);// tránh tràn ra ngoài
        _clampMax = new Vector2(camWidth - 1f , camHeight - 1f);// tránh tràn ra ngoài
    }

    void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        HandleMouseControl();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchControl();
#endif

        // Di chuyển mượt
        Vector2 newPos = Vector2.Lerp(transform.position, _targetPosition, _moveSmooth * Time.deltaTime);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    void HandleMouseControl()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(mousePos);
        _targetPosition = ClampPosition(worldPos);
    }

    void HandleTouchControl()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Fire();
            }

            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;
            touchPos.z = 10f;
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(touchPos);
            _targetPosition = ClampPosition(worldPos);
        }
    }

    Vector3 ClampPosition(Vector3 pos)
    {
        float x = Mathf.Clamp(pos.x, _clampMin.x, _clampMax.x);
        float y = Mathf.Clamp(pos.y, _clampMin.y, _clampMax.y);
        return new Vector3(x, y, transform.position.z);
    }

    public void Fire()
    {
        if (_isOnCooldown) return; // đang hồi, không bắn được

        Bullet currentBullet = _bulletList[_bulletIndex];

        // Spawn đạn
        Instantiate(currentBullet, _firePoint.position, currentBullet.transform.rotation);

        // Bắt đầu cooldown
        StartCoroutine(FireCooldown(currentBullet.Cooldown));
    }

    private IEnumerator FireCooldown(float cooldown)
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        _isOnCooldown = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chicken"))
            Destroy(gameObject);
        else if (collision.CompareTag("Egg"))
        {
            Destroy(collision.gameObject); Destroy(gameObject);
        }
            
    }
    private void OnDestroy()
    {
        if(gameObject.scene.isLoaded) // Kiểm tra nếu đối tượng vẫn còn trong cảnh
        {
            var vfx = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(vfx, 1f); // Hủy hiệu ứng sau 1 giây
        }
    }
}
