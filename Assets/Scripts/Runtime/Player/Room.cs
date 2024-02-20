using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Id;

    [SerializeField,Disable]
    List<EntrancePosition> Entrances;

    public bool HasEntrance(int id)
    {
        return Entrances.Any(x => x.Id == id);
    }

    public EntrancePosition GetEntrance(int id)
    {
        return Entrances.Find(x => x.Id == id);
    }

#if UNITY_EDITOR
    void OnValidate()
    {

            Entrances = GetComponentsInChildren<EntrancePosition>().ToList();
          
            //UnityEditor.EditorUtility.SetDirty(this);
        

    }
#endif
}
