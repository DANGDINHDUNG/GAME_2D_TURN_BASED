using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;
    public float m_ShrinkSpeed = 0.01f;  // Speed at which the image shrinks
    public float minScale = 0.1f;  // Minimum scale before disappearing

    private int m_IndexSprite;
    private Coroutine m_CoroutineAnim;
    private bool isAnimating;

    private void OnEnable()
    {
        UIManager.OnTransitionDone += Execute;
    }

    private void OnDisable()
    {
        UIManager.OnTransitionDone -= Execute;
    }

    private void Execute()
    {
        isAnimating = true;
        if (m_SpriteArray != null && m_SpriteArray.Length > 0 && m_Image != null)
        {
            ResetScale();
        }
    }

    private IEnumerator Func_PlayAnimUI()
    {
        RectTransform imageRect = m_Image.GetComponent<RectTransform>();  // Get RectTransform to modify scale
        Vector3 originalScale = imageRect.localScale;  // Save the original scale of the Image

        while (isAnimating)
        {
            yield return new WaitForSeconds(m_Speed);

            // Sprite Animation
            if (m_IndexSprite >= m_SpriteArray.Length)
            {
                m_IndexSprite = 0;
            }
            m_Image.sprite = m_SpriteArray[m_IndexSprite];
            m_IndexSprite += 1;

            // Shrinking Logic (using scale)
            if (imageRect.localScale.x > minScale && imageRect.localScale.y > minScale)
            {
                imageRect.localScale -= new Vector3(m_ShrinkSpeed, m_ShrinkSpeed, 0);  // Reduce scale uniformly on X and Y
            }
            else
            {
                isAnimating = false;  // Stop the animation when scale is below the minimum
                m_Image.enabled = false;  // Hide the image when it is fully shrunk
            }
        }
    }

    public void ResetScale()
    {
        // Optional: Reset the scale to the original size
        RectTransform imageRect = m_Image.GetComponent<RectTransform>();
        imageRect.localScale = Vector3.one;  // Reset to original scale
        m_Image.enabled = true;  // Make the image visible again
        isAnimating = true;  // Restart the animation
        m_CoroutineAnim = StartCoroutine(Func_PlayAnimUI());  // Restart animation if needed
    }
}