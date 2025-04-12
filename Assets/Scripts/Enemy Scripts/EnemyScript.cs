using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    public PlayerScript playerScript;
    public Rigidbody2D enemyRb;
    [SerializeField] private float _moveSpeed = 0.5f, _Accelerant;

    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        enemyRb = GetComponent<Rigidbody2D>();
        _Accelerant = 10;
    }

    void Update()
    {
        _Accelerant -= Time.deltaTime;

        if (_Accelerant <= 0)
        {
            _Accelerant = 5;
            _moveSpeed += 1f;
        }

        enemyRb.position = Vector2.Lerp(transform.position, player.transform.position, _moveSpeed);
    }
}
