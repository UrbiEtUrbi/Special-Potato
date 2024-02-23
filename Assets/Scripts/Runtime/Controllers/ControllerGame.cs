using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControllerGame : ControllerLocal
{

    public static ControllerGame Instance;


    [BeginGroup("Prefabs")]
    [EndGroup]
    [SerializeField]
    Player PlayerPrefab;


    [BeginGroup("References")]
    [SerializeField]
    [EndGroup]
    CinemachineVirtualCamera VCamera;

    [BeginGroup("Settings")]
    [EndGroup]
    [SerializeField]
    Vector3 StartPosition = new Vector3(-9.7f, -5.4f, 0);


    public CinemachineVirtualCamera GetCCamera => VCamera;
    private CinemachineConfiner2D Confiner;


    [HideInInspector]
    public static Player Player => Instance.player;

    Player player;

    [Hide]
    public bool IsGameOver;

    [Hide]
    public bool IsGamePlaying;



    #region Controllers
    ControllerEntities m_ControllerEntities;
    public static ControllerEntities ControllerEntities => Instance.m_ControllerEntities;

    SceneLoader m_SceneLoader;
    public static SceneLoader SceneLoader => Instance.m_SceneLoader;


    ControllerRespawn m_ControllerRespawn;
    public static ControllerRespawn ControllerRespawn => Instance.m_ControllerRespawn;


    ControllerAttack m_ControllerAttack;
    public static ControllerAttack ControllerAttack => Instance.m_ControllerAttack;

    ControllerRooms m_ControllerRooms;
    public static ControllerRooms Rooms => Instance.m_ControllerRooms;

    #endregion

    public static bool Initialized
    {
        get
        {
            if (!Instance)
            {
                return false;

            }
            else
            {
                return Instance.isInitialized;
            }
        }

    }

    public override void Init()
    {
       
     

        m_ControllerEntities = GatherComponent<ControllerEntities>();
        m_SceneLoader = GetComponent<SceneLoader>();
        m_ControllerRespawn = GatherComponent<ControllerRespawn>();

        m_ControllerAttack = GetComponent<ControllerAttack>();
        m_ControllerRooms = GetComponent<ControllerRooms>();
        Instance = this;



        // MusicPlayer.Instance.PlayPlaylist("overworld");

        

        base.Init();
        StartCoroutine(WaitForSceneLoad());
    }

    public void AssignConfiner(PolygonCollider2D polygonCollider2D)
    {
        if (Confiner == null)
        {
            Confiner = VCamera.GetComponent<CinemachineConfiner2D>();
        }
        if (polygonCollider2D == null)
        {
            Confiner.enabled = false;
        }
        else
        {
            Confiner.enabled = true;
            Confiner.m_BoundingShape2D = polygonCollider2D;

        }
        Confiner.InvalidateCache();
    }

    IEnumerator WaitForSceneLoad() {
        yield return null;
        player = Instantiate(PlayerPrefab);
      
        Rooms.InitialRoom(0,0);
       
       
    }

    public void GameOver()
    {
        IsGameOver = true;

        //reload whole game scenes or entities
        SoundManager.Instance.PlayDelayed("transform", 3f); 
        Invoke(nameof(Reload), 9.5f);
    }

  

    void Reload()
    {
        MusicPlayer.Instance.StopPlaying(1);
        ControllerGameFlow.Instance.ResetCurrentScene();

    }

    public void PlayerDie()
    {
        //reload scenes or entities
    }

    public T GatherComponent<T>() where T : MonoBehaviour {
        var component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }
}
