using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleScrollSnap;
using System.IO;
using UnityEngine.SceneManagement;

    public class menu : MonoBehaviour
    {
        public Text title;
        public Text bscore;
        public Text easy;
        public Text normal;
        public Text hard;
        public GameObject auto;
        public GameObject read;
        public InputField offset;
        public int selectid;
        public AudioSource[] allbgm=new AudioSource[6];
        public AudioSource nowbgm;
        public AudioClip turn;
        public GameObject settings;
        public SimpleScrollSnap panels;
        public LoadList Load;
        private bool mode=true;

    
        void Start()
        {
            Application.targetFrameRate=240;
            nowbgm.clip=allbgm[0].clip;
            Load.Starting();
            Load.loadcsv();
            changepanel();
            GameObject.DontDestroyOnLoad(nowbgm);
        }
        public void choosemap(string diff)
        {
            mapnow.BGM=nowbgm;
            mapnow.title=title.text;
            mapnow.difficulty=diff;
            mapnow.mapid=Load.songs[panels.CurrentPanel].id;
            mapnow.BPM=Load.songs[panels.CurrentPanel].bpm;
            SceneManager.LoadScene("Play");
        }
        public void changepanel()
        {
            nowbgm.Stop();
            nowbgm.clip=allbgm[panels.CurrentPanel].clip;
            nowbgm.Play();
            title.text=Load.songs[panels.CurrentPanel].title;
            bscore.text=Load.songs[panels.CurrentPanel].bestscore;
            easy.text=Load.songs[panels.CurrentPanel].diff_e;
            normal.text=Load.songs[panels.CurrentPanel].diff_n;
            hard.text=Load.songs[panels.CurrentPanel].diff_h;
        }
        public void openset()
        {
            offset.text=(mapnow.offset*1000).ToString();
            settings.SetActive(true);
        }
        public void storeset()
        {
            float a=mapnow.offset;
            if(float.TryParse(offset.text,out mapnow.offset))mapnow.offset/=1000;
            else mapnow.offset=a;
            settings.SetActive(false);
        }
        public void cancelset()
        {
            settings.SetActive(false);
        }
        public void selmode()
        {
            if(mode)
            {
                mode=false;
                auto.SetActive(false);
                read.SetActive(true);
            }
            else
            {
                mode=true;
                auto.SetActive(true);
                read.SetActive(false);
            }
        }
    }