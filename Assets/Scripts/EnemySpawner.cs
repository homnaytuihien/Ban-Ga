using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnDelay = 0.1f; // thời gian spawn từng con

    private List<Enemy> _enemies = new List<Enemy>();
    private int _rows;
    private int _cols;
    private Vector2 _center;

    void Start()
    {
        Camera cam = Camera.main;
        float halfScreenHeight = cam.orthographicSize;

        // Tâm đội hình ở giữa nửa trên màn hình
        _center = new Vector2(0, halfScreenHeight * 0.5f);

        StartCoroutine(SpawnGridSequential());
    }

    private IEnumerator SpawnGridSequential()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.orthographicSize * cam.aspect;
        float screenHalfHeight = cam.orthographicSize;

        // lấy kích thước enemy prefab
        float enemyWidth = _enemyPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float enemyHeight = _enemyPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        // thêm khoảng trống nhỏ giữa các enemy
        float paddingX = enemyWidth * 0.2f;
        float paddingY = enemyHeight * 0.2f;

        float cellWidth = enemyWidth + paddingX;
        float cellHeight = enemyHeight + paddingY;

        // tính số hàng và cột để lấp đầy nửa trên màn hình
        _cols = Mathf.FloorToInt((screenHalfWidth * 2f) / cellWidth);
        _rows = Mathf.FloorToInt((screenHalfHeight) / cellHeight);

        // tính lại kích thước đội hình
        float formationWidth = (_cols - 1) * cellWidth;
        float formationHeight = (_rows - 1) * cellHeight;

        float startX = _center.x - formationWidth / 2f;
        float startY = _center.y + formationHeight / 2f;

        // vị trí spawn ngoài màn hình
        float leftX = -screenHalfWidth - 2f;
        float rightX = screenHalfWidth + 2f;
        float spawnY = screenHalfHeight + 2f;

        int index = 0;
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                float spawnX = (index % 2 == 0) ? leftX : rightX;
                Vector2 spawnPos = new Vector2(spawnX, spawnY);

                Enemy enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                _enemies.Add(enemy);

                // Vị trí trong lưới
                Vector2 target = new Vector2(startX + j * cellWidth, startY - i * cellHeight);
                enemy.MoveTo(target);

                index++;
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}
