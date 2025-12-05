using System.Collections.Generic;
using UnityEngine;

public class SoundManager_Test1 : MonoBehaviour
{
    public static SoundManager_Test1 instance;
    public enum SoundType { BGM, SFX, VO }

    [System.Serializable]
    public class NamedClip
    {
        public string name;
        public AudioClip clip;
    }

    [Header("BGM Clips")]
    public List<NamedClip> bgmClips;

    [Header("SFX Clips")]
    public List<NamedClip> sfxClips;

    [Header("Voice Over Clips")]
    public List<NamedClip> VoiceOverClips;

    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> sfxDict;
    private Dictionary<string, AudioClip> voDict;
    private List<(AudioSource source, SoundType type)> externalSources = new List<(AudioSource, SoundType)>();

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;
    private AudioSource voPlayer;

    public AudioSource GetBGMPlayer() { return bgmPlayer; }
    public AudioSource GetSFXPlayer() { return sfxPlayer; }
    public AudioSource GetVOPlayer() { return voPlayer; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;

        sfxPlayer = gameObject.AddComponent<AudioSource>();

        voPlayer = gameObject.AddComponent<AudioSource>();

        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var clip in bgmClips)
            bgmDict[clip.name] = clip.clip;

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
            sfxDict[clip.name] = clip.clip;

        voDict = new Dictionary<string, AudioClip>();
        foreach (var clip in VoiceOverClips)
            voDict[clip.name] = clip.clip;
    }

    public void HandleSoundTag(string tag, string currentStoryName)
    {
        if (tag.StartsWith("play_bgm:"))
        {
            string name = tag.Substring("play_bgm:".Length).Trim();
            PlayBGM(name);
        }
        else if (tag == "stop_bgm")
        {
            StopBGM();
        }
        if (tag.StartsWith("play_sound:"))
        {
            string name = tag.Substring("play_sound:".Length).Trim();
            PlaySFX(name);
        }
        if (tag.StartsWith("VOICE:"))
        {
            string name = tag.Substring("VOICE:".Length).Trim();
            PlayVoiceClipByName(name, currentStoryName); // MODIFIED CALL
        }
    }

    private void PlayBGM(string name)
    {
        AudioClip clip = null;
        if (!bgmDict.TryGetValue(name, out clip))
        {
            clip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
        }

        if (clip != null)
        {
            if (bgmPlayer.clip != clip)
            {
                bgmPlayer.clip = clip;
                bgmPlayer.Play();
            }
        }
        else
        {
            Debug.LogWarning("❌ ไม่พบ BGM: " + name + " (Checked Dictionary and Resources/Sounds/BGM/)");
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = null;
        if (!sfxDict.TryGetValue(name, out clip))
        {
            clip = Resources.Load<AudioClip>("Sounds/SFX/" + name);
        }

        if (clip != null)
        {
            sfxPlayer.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("❌ ไม่พบ SFX: " + name + " (Checked Dictionary and Resources/Sounds/SFX/)");
        }
    }
    public void PlayVoiceClipByName(string fullTagName, string currentStoryName)
    {
<<<<<<< Updated upstream
        AudioClip clip = null;
        
        // 1. Try Dictionary
        if (voDict.TryGetValue(name, out clip))
        {
            // Found in manual list
        }
        else
        {
            // 2. Try Advanced Resources Load
            // Tag format: "Type-ID" e.g. "บรรยาย-0"
            string[] parts = name.Split('-');
            if (parts.Length >= 2)
            {
                string type = parts[0]; // "บรรยาย"
                string id = parts[1];   // "0"
                
                string currentStory = "Story1"; // Default fallback
                if (DialogueManager_Test1.GetInstance() != null && !string.IsNullOrEmpty(DialogueManager_Test1.GetInstance().CurrentStoryName))
                {
                    currentStory = DialogueManager_Test1.GetInstance().CurrentStoryName;
                }

                // Map Type to Folder Name
                string folderType = "";
                if (type == "บรรยาย") folderType = "เสียงบรรยาย";
                else if (type == "พากย์") folderType = "เสียงพากย์";
                else if (type.StartsWith("จบ")) folderType = "Ending" + type.Substring(2); // จบ1 -> Ending1

                if (!string.IsNullOrEmpty(folderType))
                {
                    // Path: sounds/Story1/เสียงบรรยาย story1/
                    string path = $"sounds/{currentStory}/{folderType} {currentStory}/";
                    
                    // Load all clips in folder
                    AudioClip[] allClips = Resources.LoadAll<AudioClip>(path);
                    
                    // Find clip starting with "ID-"
                    foreach (var c in allClips)
                    {
                        if (c.name.StartsWith(id + "-"))
                        {
                            clip = c;
                            break;
                        }
                    }

                    if (clip == null)
                    {
                         Debug.LogWarning($"❌ Search failed in: {path} for ID: {id}-");
                    }
                }
            }
            
            // 3. Fallback to simple path if complex logic failed
            if (clip == null)
            {
                 clip = Resources.Load<AudioClip>("Sounds/VO/" + name);
            }
        }

=======
        // 1. ดึงชื่อ Story Base Name (เช่น "Story2" -> "story2")
        string storyFolderName = currentStoryName;
        if (currentStoryName.StartsWith("Story"))
        {
            int idx = currentStoryName.IndexOf('_');
            if (idx > 0)
                storyFolderName = currentStoryName.Substring(0, idx);
        }

        // บังคับให้เป็นตัวพิมพ์เล็กตามมาตรฐานที่แนะนำ
        string storyBaseNameLower = storyFolderName.ToLower();

        // 2. กำหนด Path Variables
        string baseFolder = $"sounds/{storyBaseNameLower}/"; // เช่น sounds/story2/
        string assetTypeFolder = "";
        string searchFileName = "";

        // 3. ดึงหมายเลขไฟล์และรวมขีดกลาง (เช่น "0-" จาก "บรรยาย-0")
        string fileNumber = "";
        string[] parts = fullTagName.Split('-');
        if (parts.Length > 1)
        {
            // ✅ FIX: ดึงเฉพาะตัวเลขมาใช้ในการค้นหา (ไม่เอาขีดกลางมาต่อท้าย)
            fileNumber = parts[1].Trim();
        }
        else
        {
            fileNumber = fullTagName.Trim();
        }

        // 4. กำหนด Folder (ใช้ชื่อโฟลเดอร์ใหม่ที่ไม่มีช่องว่าง)
        if (fullTagName.StartsWith("บรรยาย-"))
        {
            assetTypeFolder = "narration";
        }
        else if (fullTagName.StartsWith("พากย์-"))
        {
            assetTypeFolder = "dialogue";
        }
        else if (fullTagName.StartsWith("จบ1-"))
        {
            assetTypeFolder = "ending1";
        }
        else if (fullTagName.StartsWith("จบ2-"))
        {
            assetTypeFolder = "ending2";
        }
        else if (fullTagName.StartsWith("จบ3-"))
        {
            assetTypeFolder = "ending3";
        }
        else
        {
            Debug.LogWarning($"❌ Tag VOICE ไม่ถูกต้อง: {fullTagName}");
            return;
        }

        // 5. สร้าง Search Path: (Path ใหม่ที่ถูกต้องและไม่ซ้ำซ้อน)
        // Path ตัวอย่าง: sounds/story2/narration/0- 
        string searchPath = baseFolder + assetTypeFolder + "/" + fileNumber;

        // 6. โหลดไฟล์เสียง
        AudioClip clip = Resources.Load<AudioClip>(searchPath);

>>>>>>> Stashed changes
        if (clip != null)
        {
            if (voPlayer.isPlaying)
            {
                voPlayer.Stop();
            }
            voPlayer.clip = clip;
            voPlayer.Play();
            Debug.Log($"✅ เล่นเสียงพากย์: {searchPath}");
        }
        else
        {
            Debug.LogWarning($"❌ ไม่พบ Voice Clip: {fullTagName}. Search Path ที่ค้นหา: {searchPath}");
        }
    }
    public void RegisterExternalAudio(AudioSource source, SoundType type)
    {
        // 1. หา Volume ที่ถูกตั้งค่าไว้ปัจจุบันจาก AudioSource หลัก
        float currentSystemVolume = GetVolumeByType(type);

        // 2. ตั้งค่า Volume ของแหล่งเสียงภายนอกให้ตรงกับ Volume ที่ระบบกำลังใช้
        source.volume = currentSystemVolume;

        externalSources.Add((source, type));
    }
    public void UnregisterExternalAudio(AudioSource source)
    {
        externalSources.RemoveAll(item => item.source == source);
    }
    private float GetVolumeByType(SoundType type)
    {
        if (type == SoundType.BGM) return bgmPlayer.volume;
        if (type == SoundType.SFX) return sfxPlayer.volume;
        if (type == SoundType.VO) return voPlayer.volume;
        return 1f;
    }
    public void SetVolume(SoundType type, float volumeValue)
    {
        // 1. อัปเดต AudioSource หลัก
        AudioSource player = null;
        if (type == SoundType.BGM) player = bgmPlayer;
        else if (type == SoundType.SFX) player = sfxPlayer;
        else if (type == SoundType.VO) player = voPlayer;

        if (player != null) player.volume = volumeValue;

        // 2. อัปเดต AudioSource ภายนอกทั้งหมดที่ลงทะเบียนไว้
        foreach (var item in externalSources)
        {
            if (item.type == type)
            {
                // อัปเดต volume ของแหล่งเสียงภายนอก
                // (เราสมมติว่า volumeValue คือค่า Final Volume ที่ต้องการ)
                item.source.volume = volumeValue;
            }
        }
    }
}
