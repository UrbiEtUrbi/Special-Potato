using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControllerRooms : MonoBehaviour
{

    //rooms

    [SerializeField]
    SpriteAnimator transitionIn;

    [SerializeField]
    SpriteAnimator transitionOut;

    Room m_CurrentRoom;
    EntrancePosition m_CurrentEntrance;

    [SerializeField]
    List<Room> m_Rooms;

    int nextEntrance;
    Room NextRoom;


    public void LoadRoom(int room, int entrance)
    {
        NextRoom = m_Rooms.Find(x => x.Id == room);
        nextEntrance = entrance;
        if (NextRoom && NextRoom.HasEntrance(nextEntrance))
        {
            StartCoroutine(LoadingCoroutine());
        }
    }

    public void InitialRoom(int room, int entrance)
    {
        NextRoom = m_Rooms.Find(x => x.Id == room);
        nextEntrance = entrance;
        StartCoroutine(FirstLoad());
    }


    IEnumerator LoadingCoroutine()
    {

        ControllerGame.Instance.IsGamePlaying = false;
        transitionIn.gameObject.SetActive(true);
        transitionIn.Reset(false);
        SoundManager.Instance.Play("room_transition_in");


        yield return new WaitUntil(() => !transitionIn.IsAnimating);
        Destroy(m_CurrentRoom.gameObject);

        Load();
        
        yield return new WaitForSeconds(0.5f);
        transitionIn.gameObject.SetActive(false);
        yield return LoadNext();
    }

    void Load()
    {
        m_CurrentRoom = Instantiate(NextRoom);
        m_CurrentEntrance = m_CurrentRoom.GetEntrance(nextEntrance);
        ControllerGame.Instance.AssignConfiner(m_CurrentRoom.GetComponentInChildren<PolygonCollider2D>());

      //  m_CurrentRoom.GetComponentInChildren<ParalaxBkg>().Init(ControllerGame.Instance.GetCCamera.transform);
        ControllerGame.Player.transform.position = m_CurrentEntrance.transform.position;
    }

    IEnumerator LoadNext()
    {
        transitionOut.gameObject.SetActive(true);
        transitionOut.Reset(false);
        SoundManager.Instance.Play("room_transition_out");
        yield return new WaitUntil(() => !transitionOut.IsAnimating);
        transitionOut.gameObject.SetActive(false);

        ControllerGame.Instance.IsGamePlaying = true;
    }

    IEnumerator FirstLoad()
    {

        Load();
        yield return LoadNext();
    }


}
