using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Add this to use TextMeshPro

public class CameraFlash : MonoBehaviour
{
    public Image flashImage; // Assign the UI Image here in the Inspector
    public TextMeshProUGUI flashText; // Assign the TextMeshProUGUI text here in the Inspector
	public GameObject player;
    public float flashDuration = 0.5f; // Duration of the flash

    void Update()
    {
        // Check if the Control key is pressed
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            StartCoroutine(FlashRed());
			if (player != null)
        	{
            	player.GetComponent<PlayerHealth>().TakeDamage(10);
        	}
        	else
        	{
            	Debug.LogError("Player GameObject is not assigned!");
        	}
        }
    }

    private IEnumerator FlashRed()
    {
        // Enable the flash image and set text alpha to 1 (fully visible)
        flashImage.enabled = true;

        // Enable the text
        flashText.enabled = true;

        // Make the text fully visible by setting alpha to 1
        Color textColor = flashText.color;
        textColor.a = 1f;
        flashText.color = textColor;

        // Optionally, reset the alpha of the image to 1 before starting the flash
        Color flashColor = flashImage.color;
        flashColor.a = 1f;
        flashImage.color = flashColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(flashDuration);

        // Fade out the flash and the text
        float fadeTime = 0.5f; // Duration for fading out
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            // Set the alpha of the flash image based on elapsed time
            flashColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            flashImage.color = flashColor;

            // Set the alpha of the text based on elapsed time
            textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            flashText.color = textColor;

            yield return null;
        }

        // Disable the flash image after fading out
        flashImage.enabled = false;

        // Disable the text after fading out
        flashText.enabled = false;
    }
}
