using UnityEngine;
using UnityEngine.SceneManagement;
public class ShopScreenManager : MonoBehaviour
{
    private GameObject[] allScreens;

    private AudioSource backgroundMusicSource;
    public AudioClip backgroundMusicClip; // Arka plan müziği için AudioClip


    void Start()
    {
        // Find all screens under the Screen object and deactivate them
        GameObject screenObject = GameObject.Find("Screen");
        if (screenObject != null)
        {
            allScreens = new GameObject[screenObject.transform.childCount];
            for (int i = 0; i < screenObject.transform.childCount; i++)
            {
                allScreens[i] = screenObject.transform.GetChild(i).gameObject;

            }
        }
        else
        {
            Debug.LogError("Screen object not found in scene!");
        }
        // Add an AudioSource component for background music
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        // Set the background music clip
        backgroundMusicSource.clip = backgroundMusicClip;

        // Set the background music to loop
        backgroundMusicSource.loop = true;

        // Play the background music
        backgroundMusicSource.Play();
    }

    // Activate a screen by its name and deactivate others
    public void ActivateScreenByTag(string screenTag)
    {
        foreach (GameObject screen in allScreens)
        {
            if (screen.CompareTag(screenTag))
            {
                screen.SetActive(true);
            }
            else
            {
                screen.SetActive(false);
            }
        }
    }

    // Deactivate a screen by its name
    public void DeactivateScreenByTag(string screenTag)
    {
        foreach (GameObject screen in allScreens)
        {
            if (screen.CompareTag(screenTag))
            {
                screen.SetActive(false);
            }
        }
    }
}
