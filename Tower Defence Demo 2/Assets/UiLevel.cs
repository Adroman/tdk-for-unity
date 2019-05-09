using Scrips;
using Scrips.Audio;
using Scrips.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiLevel : MonoBehaviour
{
    public LevelConfiguration TargetLevel;

    public UiFader Fader;

    public AudioAnimator AudioTransition;

    public PlayerData PlayerData;
    
    public void LoadLevel()
    {
        if (Fader != null)
        {
            if (AudioTransition != null)
            {
                AudioTransition.TurnOffAudio();
            }

            Fader.FadeToLevel(TargetLevel.TargetScenePath);
        }
        else
        {
            SceneManager.LoadScene(TargetLevel.TargetScenePath);
        }
    }
}
