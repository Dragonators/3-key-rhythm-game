using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;

public class Playing : MonoBehaviour
{
    private struct noteinforms
    {
        public float timeposition;
        public int type;
        public float position_y;
    }
    private float dsptimesong;
    private float songPosition;
    private float BPM;
    private AudioSource BGM;
    private GameObject _x;
    private LinkedList<int> notenum=new LinkedList<int>();//每一个时间轴的note数量
    private LinkedList<noteinforms> nextnotes=new LinkedList<noteinforms>();//数据库排列的note信息
    private List<clicknote> clicknotes=new List<clicknote>();
    public GameObject sideclick;
    public GameObject middleclick;
    public GameObject sideclick_m;
    public GameObject middleclick_m;
    public GameObject notes;
    public Text showscore;
    private RaycastHit2D hit;
    private Vector2 basepoint;
    private Vector3 fingerpoint;
    private float Length;
    private float delta;
    private float score=0;
    void Start()
    {
        mapnow.scale=942.7f/(942.7f*(1f+mapnow.judge_miss/mapnow.advanceposition));
        BGM=mapnow.BGM;
        Length=BGM.clip.length;
        BPM=mapnow.BPM;
        easymode();
        dsptimesong=(float)AudioSettings.dspTime + 2;
        BGM.PlayScheduled(dsptimesong);
    }
    void Update()
    {
        songPosition = (float) (AudioSettings.dspTime - dsptimesong);
        judgement();
        if(notenum.Count>0)createnote();
        for(int i=0;i<clicknotes.Count;i++)
        {
            if(clicknotes[i].todestroy)
            {
                clicknotes[i].del();
                clicknotes.RemoveAt(i);
                continue;
            }
            clicknotes[i].manageupadate(songPosition);
            if((songPosition-(clicknotes[i].noteposition-mapnow.advanceposition))/mapnow.advanceposition*mapnow.scale>=1)
            {
                 clicknotes[i].del();
                 clicknotes.RemoveAt(i);
            }
        }
    }
/*    private void test()
    {
        for(int i=0;i<Input.touchCount;i++)
        {
            if(Input.GetTouch(i).phase == TouchPhase.Began)
            {
                a=Input.GetTouch(i).position;
                a.x=1060f;
            Vector3 b=new Vector3();
            b.x=a.x;
            b.y=a.y;
            b.z=0;
            if(Physics2D.Raycast(a,Vector2.left,130f))
            {
            hit=Physics2D.Raycast(a,Vector2.left,130f);
            Debug.Log(hit.collider.gameObject.transform.position);
            if(true)
            {
                Debug.Log("Perfect");
                hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                hit.collider.gameObject.GetComponent<clicknote>().del();
            }
            }
            Debug.DrawRay(b,Vector3.left*130,Color.red,3f);
            }
        }
    }
    */
    private void judgement()
    {
        for(int i=0;i<Input.touchCount;i++)
        {
            if(Input.GetTouch(i).phase == TouchPhase.Began)
            {
                fingerpoint=Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                if(fingerpoint.x<=-154&&fingerpoint.x>=-400&&fingerpoint.y>=-419)
                {
                    basepoint.y=fingerpoint.y;basepoint.x=-137.5f;
                    basepoint=Camera.main.WorldToScreenPoint(basepoint);
                    if(Physics2D.Raycast(basepoint,Vector2.left,800f))
                    {
                        hit=Physics2D.Raycast(basepoint,Vector2.left,800f);
                        delta=Mathf.Abs(songPosition-hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                        if(delta<mapnow.judge_on)
                        {
                            hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                            if(delta>mapnow.judge_miss)
                            {

                            }
                            else if(delta<mapnow.judge_P)
                            {
                                score+=300;
                            }
                            else if(delta>mapnow.judge_gd)
                            {
                                score+=100;
                            }
                            else
                            {
                                score+=200;
                            }
                        }
                    }
                }
                else if(fingerpoint.x>=154&&fingerpoint.x<=400&&fingerpoint.y>=-419)
                {
                    basepoint.y=fingerpoint.y;basepoint.x=137.5f;
                    basepoint=Camera.main.WorldToScreenPoint(basepoint);
                    if(Physics2D.Raycast(basepoint,Vector2.right,800f))
                    {
                        hit=Physics2D.Raycast(basepoint,Vector2.right,800f);
                        delta=Mathf.Abs(songPosition-hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                        if(delta<mapnow.judge_on)
                        {
                            hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                            if(delta>mapnow.judge_miss)
                            {

                            }
                            else if(delta<mapnow.judge_P)
                            {
                                score+=300;
                            }
                            else if(delta>mapnow.judge_gd)
                            {
                                score+=100;
                            }
                            else
                            {
                                score+=200;
                            }
                        }
                    }
                }
                else if(fingerpoint.x>=-130&&fingerpoint.x<=130&&fingerpoint.y<=-350)
                {
                    basepoint.y=-540f;basepoint.x=0f;
                    basepoint=Camera.main.WorldToScreenPoint(basepoint);
                    //Debug.Log(hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                    if(Physics2D.Raycast(basepoint,Vector2.up,400f))
                    {
                        hit=Physics2D.Raycast(basepoint,Vector2.up,400f);
                        delta=Mathf.Abs(songPosition-hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                        if(delta<mapnow.judge_on)
                        {
                            hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                            if(delta>mapnow.judge_miss)
                            {

                            }
                            else if(delta<mapnow.judge_P)
                            {
                                score+=300;
                            }
                            else if(delta>mapnow.judge_gd)
                            {
                                score+=100;
                            }
                            else
                            {
                                score+=200;
                            }
                        }
                    }
                }
                showscore.text=score.ToString();
            }
        }
    }
    private void createnote()
    {
        if(songPosition>=nextnotes.First.Value.timeposition-mapnow.advanceposition)
        {
            if(notenum.First.Value==1)crearesingletap();
            else creaemultytap();
        }
    }
    private void hardmode()
    {
        noteinforms a;
        float b=5f;
        while(b<=Length-5f)
        {
            a.timeposition=b;
            int c=Random.Range(0,20);
            if(c==5)
            {
                notenum.AddLast(3);
                a.type=1;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=2;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=3;
                nextnotes.AddLast(a);
            }
            else if(c==4)
            {
                notenum.AddLast(2);
                a.type=2;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=2;
                if(a.position_y<=-16)a.position_y=Random.Range((int)(a.position_y+263.5f),386);
                else if(a.position_y>=137.7)a.position_y=Random.Range(-260,(int)(a.position_y-263.5f));
                else a.position_y=Random.Range(0,2)==1?Random.Range((int)(a.position_y+263.5f),386):Random.Range(-260,(int)(a.position_y-263.5f));
                nextnotes.AddLast(a);
            }
            else if(c==10)
            {
                notenum.AddLast(2);
                a.type=1;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=1;
                if(a.position_y<=-16)a.position_y=Random.Range((int)(a.position_y+263.5f),386);
                else if(a.position_y>=137.7)a.position_y=Random.Range(-260,(int)(a.position_y-263.5f));
                else a.position_y=Random.Range(0,2)==1?Random.Range((int)(a.position_y+263.5f),386):Random.Range(-260,(int)(a.position_y-263.5f));
                nextnotes.AddLast(a);                
            }
            else if(c==15)
            {
                notenum.AddLast(2);
                a.type=1;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=2;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);                
            }            
            else
            {
                notenum.AddLast(1);
                a.type=Random.Range(1,4);
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);   
            }
            c=Random.Range(0,3);
            b+=c==1?60/BPM:c==0?30/BPM:15/BPM;
        }
    }
    private void easymode()
    {
        noteinforms a;
        float b=5f;
        while(b<=Length-5f)
        {
            a.timeposition=b;
            int c=Random.Range(0,20);
            if(c==4)
            {
                notenum.AddLast(2);
                a.type=2;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=2;
                if(a.position_y<=-16)a.position_y=Random.Range((int)(a.position_y+263.5f),386);
                else if(a.position_y>=137.7)a.position_y=Random.Range(-260,(int)(a.position_y-263.5f));
                else a.position_y=Random.Range(0,2)==1?Random.Range((int)(a.position_y+263.5f),386):Random.Range(-260,(int)(a.position_y-263.5f));
                nextnotes.AddLast(a);
            }
            else if(c==10)
            {
                notenum.AddLast(2);
                a.type=1;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=1;
                if(a.position_y<=-16)a.position_y=Random.Range((int)(a.position_y+263.5f),386);
                else if(a.position_y>=137.7)a.position_y=Random.Range(-260,(int)(a.position_y-263.5f));
                else a.position_y=Random.Range(0,2)==1?Random.Range((int)(a.position_y+263.5f),386):Random.Range(-260,(int)(a.position_y-263.5f));
                nextnotes.AddLast(a);                
            }
            else if(c==15)
            {
                notenum.AddLast(2);
                a.type=1;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);
                a.type=2;
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);                
            }            
            else
            {
                notenum.AddLast(1);
                a.type=Random.Range(1,4);
                a.position_y=Random.Range(-260,386);
                nextnotes.AddLast(a);   
            }
            c=Random.Range(0,3);
            b+=c==1?60/BPM:c==0?120/BPM:240/BPM;
        }
    }
    private void crearesingletap()
    {
        if(nextnotes.First.Value.type==3)
        {
            Vector3 a=new Vector3(0f,522.7f,0f);
            _x=Instantiate(middleclick,notes.transform);
            _x.transform.localPosition=a;
        }
        else
        {
            Vector3 a=new Vector3(0f,0f,0f);
            if(nextnotes.First.Value.type==1)a=new Vector3(-1175.7f,nextnotes.First.Value.position_y,0f);
            else a=new Vector3(1175.7f,nextnotes.First.Value.position_y,0f);
            _x=Instantiate(sideclick,notes.transform);
            _x.transform.localPosition=a;
        }
        _x.transform.SetSiblingIndex(0);
        _x.GetComponent<clicknote>().noteposition=nextnotes.First.Value.timeposition;
        _x.GetComponent<clicknote>().notetype=nextnotes.First.Value.type;
        _x.GetComponent<clicknote>().manageStart();
        clicknotes.Add(_x.GetComponent<clicknote>());
        nextnotes.RemoveFirst();
        notenum.RemoveFirst();
    }
    private void creaemultytap()
    {
        for(int i=1;i<=notenum.First.Value;i++)
        {
            if(nextnotes.First.Value.type==3)
            {
                Vector3 a=new Vector3(0f,522.7f,0f);
                _x=Instantiate(middleclick_m,notes.transform);
                _x.transform.localPosition=a;
            }
            else
            {
                Vector3 a=new Vector3(0f,0f,0f);
                if(nextnotes.First.Value.type==1)a=new Vector3(-1175.7f,nextnotes.First.Value.position_y,0f);
                else a=new Vector3(1175.7f,nextnotes.First.Value.position_y,0f);
                _x=Instantiate(sideclick_m,notes.transform);
                _x.transform.localPosition=a;
            }
            _x.transform.SetSiblingIndex(0);
            _x.GetComponent<clicknote>().noteposition=nextnotes.First.Value.timeposition;
            _x.GetComponent<clicknote>().notetype=nextnotes.First.Value.type;
            _x.GetComponent<clicknote>().manageStart();
            clicknotes.Add(_x.GetComponent<clicknote>());
            nextnotes.RemoveFirst();
        }
        notenum.RemoveFirst();
    }
}
