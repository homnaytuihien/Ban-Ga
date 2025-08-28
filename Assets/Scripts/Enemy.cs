using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _egg;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        StartCoroutine(SpawnEgg());
    }

    public void MoveTo(Vector2 target)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveRoutine(target));
    }

    private IEnumerator MoveRoutine(Vector2 target)
    {
        while ((Vector2)transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator SpawnEgg()
    {
        while (true)
        {
            Instantiate(_egg, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2, 7));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                _health -= bullet.Damage;
                if (_health <= 0)
                {
                    Destroy(gameObject);
                    GameManager.Instance.AddScore(10); // Thêm điểm khi tiêu diệt kẻ địch
                }
                Destroy(collision.gameObject);
            }
        }
    }
}
