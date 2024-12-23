using Kuroneko.UtilityDelivery;
using UnityEngine;

public interface IGameManager : IGameService
{
    public void InitGame();
    public Material GetShadowMaterial();
    public Material GetTransparentMaterial();
    public void NextLevel();
    public void TogglePause();
    public void RestartGame();
    public void LoadLevel(LevelSettings level);
}