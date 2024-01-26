using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<RequestStartGameEvent>(StartGame);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<RequestStartGameEvent>(StartGame);
    }

    private void StartGame(RequestStartGameEvent obj)
    {
        Time.timeScale = 1;
        Messenger.Default.Publish(new GameStartedEvent());
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
