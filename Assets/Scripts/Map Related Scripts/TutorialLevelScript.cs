using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelScript : MonoBehaviour
{
    // Scripts
    [SerializeField] public PlayerScript Player;
    [SerializeField] public GameObject PlayerObj;
    [SerializeField] private GameObject _deathTutorialText;
    [SerializeField] private GameObject[] _emergencyPlatform;

    public bool hasFallen;

    void Start()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        Player = PlayerObj.GetComponent<PlayerScript>();
        _deathTutorialText = GameObject.FindGameObjectWithTag("Death Tutorial");
        _emergencyPlatform = GameObject.FindGameObjectsWithTag("Emergency");
        _deathTutorialText.SetActive(false);
        _emergencyPlatform[0].SetActive(false);
        _emergencyPlatform[1].SetActive(false);
    }
    void Update()
    {
        if (hasFallen)
        {
            _deathTutorialText.SetActive(true);
            EmergencyPlatformsOn();
            hasFallen = false;
        }
    }

    public void EmergencyPlatformsOn()
    {
        _emergencyPlatform[0].SetActive(true);
        _emergencyPlatform[1].SetActive(true);
    }
}
