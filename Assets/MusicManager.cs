using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager instance = null;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    private AudioSource[] sounds;
    public static AudioClip[] sfx;

    public static float bps;

    AudioSource BGM_Player;
    AudioSource SFX_Player;

    public void PlaySound(int sound)
    {
        SFX_Player.PlayOneShot(sfx[sound]);
    }

    public static int soundlist_bgm = 0;
    public static int soundlist_totalsounds = 1;

    public void mute()
    {
        BGM_Player.volume = 0f;
        SFX_Player.volume = 0f;
    }

    public void unmute()
    {
        BGM_Player.volume = 1f;
        SFX_Player.volume = 1f;
    }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        sounds = GetComponents<AudioSource>();
        BGM_Player = sounds[0];
        SFX_Player = sounds[1];

        sfx = new AudioClip[soundlist_totalsounds];

        //BGM_Player.clip = sfx[0];
        //BGM_Player.loop = true;
        //BGM_Player.Play();
    }

    void Start()
    {
        //sfx[1] = (AudioClip)Resources.Load("8bit_battle_bgm");
        //sfx[2] = (AudioClip)Resources.Load("8bit_button_press");
        //sfx[3] = (AudioClip)Resources.Load("8bit_sound_select");
        //sfx[4] = (AudioClip)Resources.Load("8bit_swipe");
        //sfx[5] = (AudioClip)Resources.Load("8bit_control_select");
        //sfx[6] = (AudioClip)Resources.Load("8bit_options_change");
        //sfx[7] = (AudioClip)Resources.Load("8bit_game_start");
        //sfx[8] = (AudioClip)Resources.Load("8bit_invalid_press");
    }

    public void pause()
    {
        AudioListener.pause = true;
    }

    public void unpause()
    {
        AudioListener.pause = false;
    }
}