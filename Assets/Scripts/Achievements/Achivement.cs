using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achivement : MonoBehaviour
{
    [SerializeField] protected Constants.Achievements.Difficulty achievementDifficulty;
    [SerializeField] protected string achievementName;

    private void OnEnable()
    {
        string record = PlayerPrefs.GetString(achievementName, "");
        if (string.IsNullOrEmpty(record)) return;
        if (record == "true")
        {
            // obtained
        } else
        {
            // not obtained
        }
    }

    private void OnDisable()
    {

    }
}
