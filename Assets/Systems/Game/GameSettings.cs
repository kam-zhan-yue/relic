using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelSettings
{
    [TableColumnWidth(50, Resizable = false)]
    public bool active;
    
    [TableColumnWidth(50, Resizable = false)]
    public bool menu;

    [TableColumnWidth(150, Resizable = true)]
    public string name;

    [TextArea, TableColumnWidth(200, Resizable = true)]
    public string description;

    [TableColumnWidth(30, Resizable = false)]
    public int buildIndex;

    public bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(name) == 1;
    }

    public void Unlock()
    {
        PlayerPrefs.SetInt(name, 1);
        PlayerPrefs.Save();
    }
}

[CreateAssetMenu(menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public bool startAutomatically = false;
    public int startLevel = 1;
    [TableList] public LevelSettings[] levels = Array.Empty<LevelSettings>();

    public int GetNextScene()
    {
        // Find the current level with the active build index
        int currentBuild = SceneManager.GetActiveScene().buildIndex;
        int index = -1;
        for (int i = 0; i < levels.Length; ++i)
        {
            if (levels[i].buildIndex == currentBuild)
            {
                index = i;
                break;
            }
        }

        // If no existing level, don't bother
        if (index < 0)
            return index;

        // Return the next active level in the list
        for (int i = index + 1; i < levels.Length; ++i)
        {
            if (levels[i].active)
            {
                return levels[i].buildIndex;
            }
        }

        return -1;
    }

    public int GetLevelNum(LevelSettings level)
    {
        int num = 0;
        for (int i = 0; i < levels.Length; ++i)
        {
            if (levels[i] == level)
                return num;
            if (levels[i].active)
                num++;
        }

        return -1;
    }

    public LevelSettings GetCurrentLevel()
    {
        int currentBuild = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < levels.Length; ++i)
        {
            if (levels[i].buildIndex == currentBuild)
            {
                return levels[i];
            }
        }

        return null;
    }

    [Button]
    private void UnlockSave()
    {
        foreach(LevelSettings level in levels)
            level.Unlock();
    }

    [Button]
    private void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
}