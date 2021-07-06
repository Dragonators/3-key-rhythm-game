using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private float dsp_Position;
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
    public GameObject Pause;

    public GameObject notes;
    public GameObject P;
    public GameObject Gr;

    public GameObject Gd;

    public GameObject miss;

    public Text showscore;
    public Text title;
    public GameObject loader;
    private RaycastHit2D hit;
    private Vector2 basepoint;
    private Vector3 fingerpoint;
    private float Length;
    private float delta;
    private int score=0;
    private int n_P=0;
    private int n_Gr=0;
    private int n_Gd=0;
    private int n_miss=0;
    private bool notpause=true;
    void Start()
    {
        mapnow.scale=942.7f/(942.7f*(1f+mapnow.judge_miss/mapnow.advanceposition));
        BGM=mapnow.BGM;
        BGM.loop=false;
        Length=BGM.clip.length;
        BPM=mapnow.BPM;
        title.text=mapnow.title;
        if(mapnow.difficulty=="easy")easymode();
        else if(mapnow.difficulty=="hard")hardmode();
        else if(mapnow.difficulty=="normal")normalmode();
        StartCoroutine(finishload());
        dsptimesong=(float)AudioSettings.dspTime + 5;
        BGM.PlayScheduled(dsptimesong+mapnow.offset);
    }
    void Update()
    {
        if(notpause)
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
                if(clicknotes[i].notetype==1)
                {
                    fingerpoint=clicknotes[i].transform.localPosition;
                    fingerpoint.x=-232f;
                    _x=Instantiate(miss,notes.transform);
                    _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                    _x.transform.localPosition=fingerpoint;
                }
                else if(clicknotes[i].notetype==2)
                {
                    fingerpoint=clicknotes[i].transform.localPosition;
                    fingerpoint.x=232f;
                    _x=Instantiate(miss,notes.transform);
                    _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                    _x.transform.localPosition=fingerpoint;
                }
                else if(clicknotes[i].notetype==3)
                {
                    fingerpoint.x=0f;fingerpoint.y=-415f;
                     _x=Instantiate(miss,notes.transform);
                    _x.transform.localPosition=fingerpoint;
                }
                n_miss++;
                clicknotes[i].del();
                clicknotes.RemoveAt(i);
            }
        }
        }
        if(songPosition>=BGM.clip.length)Invoke("loadscore",3f);
    }
    private IEnumerator finishload()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        loader.SetActive(false);
        yield break;
    }
    private void loadscore()
    {
        mapnow.score=score;
        mapnow.n_P=n_P;
        mapnow.n_Gr=n_Gr;
        mapnow.n_Gd=n_Gd;
        mapnow.n_miss=n_miss;
        //GameObject.DestroyImmediate(BGM.gameObject);
        SceneManager.LoadScene("Score");
    }
    public void pause()
    {
        dsp_Position=(float)AudioSettings.dspTime;
        notpause=false;
        BGM.Pause();
        Pause.SetActive(true);
    }
    public void goon()
    {
        dsptimesong+=(float)AudioSettings.dspTime-dsp_Position;
        BGM.Play();
        notpause=true;
        Pause.SetActive(false);
    }
    public void _out()
    {
        GameObject.Destroy(BGM.gameObject);
        SceneManager.LoadScene("Selsong");
    }
    public void retry()
    {
        BGM.Stop();
        SceneManager.LoadScene("Play");
    }
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
                    if(Physics2D.Raycast(basepoint,Vector2.left,800f,8))
                    {
                        hit=Physics2D.Raycast(basepoint,Vector2.left,800f,8);
                        delta=Mathf.Abs(songPosition-hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                        if(delta<mapnow.judge_on)
                        {
                            hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                            if(delta>mapnow.judge_miss)
                            {
                                fingerpoint.x=-232f;
                                _x=Instantiate(miss,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_miss++;
                            }
                            else if(delta<mapnow.judge_P)
                            {
                                score+=300;
                                fingerpoint.x=-232f;
                                _x=Instantiate(P,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_P++;
                            }
                            else if(delta>mapnow.judge_gd)
                            {
                                score+=100;
                                fingerpoint.x=-232f;
                                _x=Instantiate(Gd,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_Gd++;
                            }
                            else
                            {
                                score+=200;
                                fingerpoint.x=-232f;
                                _x=Instantiate(Gr,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_Gr++;
                            }
                        }
                    }
                }
                else if(fingerpoint.x>=154&&fingerpoint.x<=400&&fingerpoint.y>=-419)
                {
                    basepoint.y=fingerpoint.y;basepoint.x=137.5f;
                    basepoint=Camera.main.WorldToScreenPoint(basepoint);
                    if(Physics2D.Raycast(basepoint,Vector2.right,800f,8))
                    {
                        hit=Physics2D.Raycast(basepoint,Vector2.right,800f,8);
                        delta=Mathf.Abs(songPosition-hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                        if(delta<mapnow.judge_on)
                        {
                            hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                            if(delta>mapnow.judge_miss)
                            {
                                fingerpoint.x=232f;
                                _x=Instantiate(miss,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_miss++;
                            }
                            else if(delta<mapnow.judge_P)
                            {
                                score+=300;
                                fingerpoint.x=232f;
                                _x=Instantiate(P,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_P++;
                            }
                            else if(delta>mapnow.judge_gd)
                            {
                                score+=100;
                                fingerpoint.x=232f;
                                _x=Instantiate(Gd,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_Gd++;
                            }
                            else
                            {
                                score+=200;
                                fingerpoint.x=232f;
                                _x=Instantiate(Gr,notes.transform);
                                _x.transform.rotation=Quaternion.Euler(0f,0f,90f);
                                _x.transform.localPosition=fingerpoint;
                                n_Gr++;
                            }
                        }
                    }
                }
                else if(fingerpoint.x>=-130&&fingerpoint.x<=130)
                {
                    basepoint.y=-540f;basepoint.x=0f;
                    basepoint=Camera.main.WorldToScreenPoint(basepoint);
                    if(Physics2D.Raycast(basepoint,Vector2.up,400f,8))
                    {
                        hit=Physics2D.Raycast(basepoint,Vector2.up,400f,8);
                        delta=Mathf.Abs(songPosition-hit.collider.gameObject.GetComponent<clicknote>().noteposition);
                        if(delta<mapnow.judge_on)
                        {
                            hit.collider.gameObject.GetComponent<clicknote>().todestroy=true;
                            if(delta>mapnow.judge_miss)
                            {
                                fingerpoint.x=0f;fingerpoint.y=-415f;
                                _x=Instantiate(miss,notes.transform);
                                _x.transform.localPosition=fingerpoint;
                                n_miss++;
                            }
                            else if(delta<mapnow.judge_P)
                            {
                                score+=300;
                                fingerpoint.x=0f;fingerpoint.y=-415f;
                                _x=Instantiate(P,notes.transform);
                                _x.transform.localPosition=fingerpoint;
                                n_P++;
                            }
                            else if(delta>mapnow.judge_gd)
                            {
                                score+=100;
                                fingerpoint.x=0f;fingerpoint.y=-415f;
                                _x=Instantiate(Gd,notes.transform);
                                _x.transform.localPosition=fingerpoint;
                                n_Gd++;
                            }
                            else
                            {
                                score+=200;
                                fingerpoint.x=0f;fingerpoint.y=-415f;
                                _x=Instantiate(Gr,notes.transform);
                                _x.transform.localPosition=fingerpoint;
                                n_Gr++;
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
        float b=4f*60f/BPM;
        while(b<=Length-3f)
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
        float b=4f*60f/BPM;
        while(b<=Length-3f)
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
        private void normalmode()
    {
        noteinforms a;
        float b=4f*60f/BPM;
        while(b<=Length-3f)
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
            b+=c==1?60/BPM:c==0?60/BPM:30/BPM;
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
