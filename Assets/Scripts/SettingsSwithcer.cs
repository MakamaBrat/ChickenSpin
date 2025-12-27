using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitcher : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    [Header("UI Images")]
    public Image musicImage;
    public Image sfxImage;
    public Image dummyImage; // Пустышковая кнопка

    [Header("Sprites")]
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;
    public Sprite dummyOnSprite;
    public Sprite dummyOffSprite;

    [Header("States")]
    public bool musicOn = true;
    public bool sfxOn = true;
    public bool dummyOn = true; // состояние пустышки

    private void Start()
    {
        ApplyMusicState();
        ApplySfxState();
        ApplyDummyState();
    }

    // --------------------
    // SWITCHERS
    // --------------------

    public void SwitchMusic()
    {
        musicOn = !musicOn;
        ApplyMusicState();
    }

    public void SwitchSfx()
    {
        sfxOn = !sfxOn;
        ApplySfxState();
    }

    public void SwitchDummy()
    {
        dummyOn = !dummyOn;
        ApplyDummyState();
    }

    // --------------------
    // APPLY STATES
    // --------------------

    void ApplyMusicState()
    {
        if (musicSource != null)
            musicSource.mute = !musicOn;

        if (musicImage != null)
        {
            if (musicOn && musicOnSprite != null)
                musicImage.sprite = musicOnSprite;
            else if (!musicOn && musicOffSprite != null)
                musicImage.sprite = musicOffSprite;
        }
    }

    void ApplySfxState()
    {
        foreach (AudioSource sfx in sfxSources)
        {
            if (sfx != null)
                sfx.mute = !sfxOn;
        }

        if (sfxImage != null)
        {
            if (sfxOn && sfxOnSprite != null)
                sfxImage.sprite = sfxOnSprite;
            else if (!sfxOn && sfxOffSprite != null)
                sfxImage.sprite = sfxOffSprite;
        }
    }

    void ApplyDummyState()
    {
        if (dummyImage != null)
        {
            if (dummyOn && dummyOnSprite != null)
                dummyImage.sprite = dummyOnSprite;
            else if (!dummyOn && dummyOffSprite != null)
                dummyImage.sprite = dummyOffSprite;
        }
    }
}
