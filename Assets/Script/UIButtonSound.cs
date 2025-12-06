using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip clickSound; // ลากไฟล์เสียงที่ต้องการมาใส่ตรงนี้
    public string sfxName; // หรือใส่ชื่อ SFX ที่มีใน SoundManager

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        // 1. ถ้ามีคลิปเสียงใส่มาในช่อง clickSound ให้เล่นไฟล์นั้น
        if (clickSound != null)
        {
            if (SoundManager_Test1.instance != null)
            {
                SoundManager_Test1.instance.GetSFXPlayer().PlayOneShot(clickSound);
            }
            else
            {
                // Fallback ถ้าไม่มี SoundManager ให้สร้างเสียงเองแต่ยังอิง Volume SFX
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
                AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, sfxVolume);
            }
        }
        // 2. ถ้าไม่มีคลิป แต่มีชื่อ sfxName ให้เรียกผ่าน SoundManager
        else if (!string.IsNullOrEmpty(sfxName) && SoundManager_Test1.instance != null)
        {
            SoundManager_Test1.instance.PlaySFX(sfxName);
        }
    }
}
