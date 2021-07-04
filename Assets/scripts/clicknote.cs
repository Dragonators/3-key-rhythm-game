using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicknote : MonoBehaviour
{
    public float noteposition;
    public int notetype;
    public bool todestroy=false;
    private Vector3 spawn;
    private Vector3 judgeline;
    private Vector3 remove;
    public void manageupadate(float songPosition)
    {
        this.transform.localPosition=Vector3.Lerp(spawn,remove,(songPosition-(noteposition-mapnow.advanceposition))/mapnow.advanceposition*mapnow.scale);
    }
    public void del(){Destroy(this.gameObject);}
    public void manageStart()
    {
        spawn=this.transform.localPosition;
        if(notetype==1){judgeline=new Vector3(-233,spawn.y,spawn.z);remove=new Vector3(-1175.7f+942.7f/mapnow.scale,spawn.y,spawn.z);}
        if(notetype==2){judgeline=new Vector3(233,spawn.y,spawn.z);remove=new Vector3(1175.7f-942.7f/mapnow.scale,spawn.y,spawn.z);}
        if(notetype==3){judgeline=new Vector3(spawn.x,-420,spawn.z);remove=new Vector3(spawn.x,522.7f-942.7f/mapnow.scale,spawn.z);}
    }
}
