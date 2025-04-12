using Unity.VisualScripting;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public enum PlatformType
    {
        Vertical,
        Horizontal,
        Crumbling
    }
    [SerializeField] public PlatformType type;
    [SerializeField] public Rigidbody2D _rb;
    [SerializeField] private float _timer = 3;
    [SerializeField] private bool _timerOn;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (_timerOn)
        {
            _timer -= Time.deltaTime;
        }

        if (type == PlatformType.Crumbling)
        {
            if (_timer <= 0)
            {
                _timerOn = false;
                _timer = 3;
                _rb.gravityScale = 1;
                _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (type == PlatformType.Crumbling)
            {
                if (!_timerOn)
                {
                    _timerOn = true;
                }
                else
                {
                    Debug.Log("Better Get off this thing.");
                }
            }
        }
        else if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Hey, I touched another wall/platform!!");
            Destroy(this.gameObject);
        }
        
        else if (collision.gameObject.CompareTag("Emergency"))
        {
            this.gameObject.SetActive(false);
        }

        
    }
}
