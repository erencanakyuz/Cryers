using UnityEngine;
using UnityEngine.SceneManagement;
public class ShoppingEvents : MonoBehaviour
{
    public Texture2D newCursorTexture;
    private ShopScreenManager shopScreenManager;
    private GameObject screenObject;
    private AudioSource audioSource; // Declare the AudioSource variable
    public AudioClip clickSound;
    private bool hasPlayedSound = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        screenObject = GameObject.Find("Screen");
        if (screenObject != null)
        {
            shopScreenManager = screenObject.GetComponent<ShopScreenManager>();
        }

        // Optional: Handle the case where ShopScreenManager is still not found
        if (shopScreenManager == null)
        {
            Debug.LogError("ShopScreenManager component not found in scene!");
        }
    }

    private void OnMouseOver()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("PlayA1");
        if (!hasPlayedSound)
        {
            audioSource.PlayOneShot(clickSound);
            hasPlayedSound = true;
        }

        string objTag = gameObject.tag; // Get the tag of the current object

        // Activate child Screen object based on object tag
        if (shopScreenManager != null)
        {
            // Activate the screen corresponding to the object's tag and deactivate others
            shopScreenManager.ActivateScreenByTag(objTag + "Screen");
        }
    }

    private void OnMouseExit()
    {
        Animator animator = GetComponent<Animator>();
        animator.ResetTrigger("PlayA1");
        animator.SetTrigger("PlayA2");

        string objTag = gameObject.tag; // Get the tag of the current object

        // Deactivate the screen corresponding to the object's tag
        if (shopScreenManager != null)
        {
            shopScreenManager.DeactivateScreenByTag(objTag + "Screen");
        }
        hasPlayedSound = false;
    }
}
