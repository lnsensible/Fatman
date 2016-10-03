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

    public void PlayBGM(int sound)
    {
        BGM_Player.clip = sfx[sound];
        BGM_Player.Stop();
        BGM_Player.Play();
    }

    public static int soundlist_bgm = 0;
    public static int soundlist_bgm_game = 1;
    public static int soundlist_bgm_gameover = 2;
    public static int soundlist_bgm_quit = 3;
    public static int soundlist_bgm_select = 4;
    public static int soundlist_eat = 5;
    public static int soundlist_hitenemy = 6;
    public static int soundlist_enemyfly = 7;
    public static int soundlist_wallhit = 8;
    public static int soundlist_fever = 9;
    public static int soundlist_hitbynurse = 10;
    public static int soundlist_enemyspawn = 11;
    public static int soundlist_resultscore = 12;
    public static int soundlist_highscore = 13;
    public static int soundlist_totalsounds = 14;

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
        sfx[0] = (AudioClip)Resources.Load("Audio/bgm_maoudamashii_cyber11");
        BGM_Player.clip = sfx[0];
        BGM_Player.loop = true;
        BGM_Player.Play();
    }

    void Start()
    {
        sfx[1] = (AudioClip)Resources.Load("Audio/bgm_loop4");
        sfx[2] = (AudioClip)Resources.Load("Audio/gameover");
        sfx[3] = (AudioClip)Resources.Load("SE/quit1");
        sfx[4] = (AudioClip)Resources.Load("SE/decision");
        sfx[5] = (AudioClip)Resources.Load("SE/bite1");
        sfx[6] = (AudioClip)Resources.Load("SE/sen_ge_panchi10");
        sfx[7] = (AudioClip)Resources.Load("SE/flee1");
        sfx[8] = (AudioClip)Resources.Load("SE/hit_wall");
        sfx[9] = (AudioClip)Resources.Load("SE/levelup");
        sfx[10] = (AudioClip)Resources.Load("SE/down1");
        sfx[11] = (AudioClip)Resources.Load("SE/spawn");
        sfx[12] = (AudioClip)Resources.Load("coin");
        sfx[13] = (AudioClip)Resources.Load("High Score");
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