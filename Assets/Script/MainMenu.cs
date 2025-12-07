using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip clickSound; // ลากไฟล์เสียงใส่จาก Inspector
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        // 1. ลองใช้ SoundManager ก่อน (เพื่อให้ดังตาม Volume ที่ตั้ง และลดปัญหาเสียงหายตอนเปลี่ยนฉาก)
        if (SoundManager_Test1.instance != null && clickSound != null)
        {
            SoundManager_Test1.instance.GetSFXPlayer().PlayOneShot(clickSound);
        }
        else if (clickSound != null)
        {
            // 2. Fallback: ถ้าไม่มี SoundManager ใช้ PlayClipAtPoint 
            // วิธีนี้จะสร้าง AudioSource ชั่วคราว ทำให้ไม่ติด Error "disabled source"
            float volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, volume);
        }
        // 3. Fallback สุดท้าย: ใช้ AudioSource ตัวเอง (แต่เพิ่มการเช็คเพื่อกัน Crash)
        else if (audioSource != null && audioSource.isActiveAndEnabled && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void OnPlayButtonClicked()
    {
        PlayClickSound();
        SceneManager.LoadScene("Selectstory");
    }

    public void OnExitButtonClicked()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void OnBackToMainButtonClicked()
    {
        PlayClickSound();

        // 1. ทำลาย DialogueManager เพื่อล้างค่า Ink Story และ History ทั้งหมด
        if (DialogueManager_Test1.GetInstance() != null)
        {
            // ต้องทำลาย GameObject เพื่อให้ Instance หายไป 
            // พอเริ่มเกมใหม่ ระบบจะสร้าง DialogueManager ตัวใหม่ที่สดใสกว่าเดิม
            Destroy(DialogueManager_Test1.GetInstance().gameObject);
        }

        // 2. (ทางเลือก) ถ้าต้องการรีเซ็ตเสียงด้วย (เช่น หยุดเพลง BGM เก่า) ให้เอา Comment ออก
        // แต่ระวัง! ถ้าทำลาย SoundManager เสียงกดปุ่ม (Click) อาจจะขาดหายไปกลางคันได้
        /*
        if (SoundManager_Test1.instance != null)
        {
            Destroy(SoundManager_Test1.instance.gameObject);
        }
        */

        // 3. โหลดฉากเมนูหลัก
        SceneManager.LoadScene("Mainmenu");
    }
}