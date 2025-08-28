using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed = 0.5f;

    [SerializeField] private  Renderer _bgRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _bgRenderer.material.mainTextureOffset += new Vector2(0, _scrollSpeed * Time.deltaTime);
    }
}
