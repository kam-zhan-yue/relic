using DG.Tweening;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private AudioSettings audioSettings;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private Material transparentMaterial;
    private Door _door;
    private GameState _gameState;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
        _gameState = new GameState(gameSettings);
        Messenger.Default.Subscribe<GameOverPayload>(OnGameOver);
        Messenger.Default.Subscribe<LevelCompletePayload>(OnLevelCompleted);
    }

    /// <summary>
    /// Massive bug where a new GameManager and ServiceLocator is being created at the start of a new
    /// scene because they cleaned up. This gets around that. Things are not saved in between scenes.
    /// TODO: Fix this if it becomes a problem lol
    /// </summary>
    private void Start()
    {
        InitGame();
        audioSettings.Init();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            TogglePause();
    }

    public void TogglePause()
    {
        _gameState.TogglePause();
    }

    public void InitGame()
    {
        ServiceLocator.Instance.Get<IAudioService>().Resume("START");
        _gameState.Init();
        GamePayload payload = new GamePayload
        {
            GameState = _gameState
        };
        Messenger.Default.Publish(payload);
    }

    public Material GetShadowMaterial()
    {
        return shadowMaterial;
    }

    public Material GetTransparentMaterial()
    {
        return transparentMaterial;
    }

    public void NextLevel()
    {
        int next = gameSettings.GetNextScene();
        LoadScene(next);
    }
    
    private void LoadScene(int buildIndex)
    {
        if (buildIndex >= 0)
        {
            // SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(buildIndex);
        }
    }

    private void OnLevelCompleted(LevelCompletePayload payload)
    {
        _gameState.LevelCompleted(payload);
    }

    private void OnGameOver(GameOverPayload payload)
    {
        _gameState.GameOver(payload);
    }
    
    public void RestartGame()
    {
        ServiceLocator.Instance.Get<IAudioService>().Resume("START");
        DOTween.KillAll();
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadLevel(LevelSettings level)
    {
        LoadScene(level.buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On Scene Loaded");
        InitGame();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<GameOverPayload>(OnGameOver);
        Messenger.Default.Unsubscribe<LevelCompletePayload>(OnLevelCompleted);
    }
}