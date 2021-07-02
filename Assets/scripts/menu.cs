using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleScrollSnap;
using System.IO;

    public class menu : MonoBehaviour
    {
        public Text title;
        public Text bscore;
        public int selectid;
        public AudioSource[] allbgm=new AudioSource[6];
        private AudioSource nowbgm;
        public AudioClip turn;
        public SimpleScrollSnap panels;
        public LoadList Load;

    
        void Start()
        {
            nowbgm=allbgm[0];
            nowbgm.Play();
            nowinform();
        }
        void Update()
        {
        
        }
        public void changebgm()
        {
            nowbgm.Stop();
            nowbgm=allbgm[panels.CurrentPanel];
            nowbgm.Play();
        }
        public void nowinform()
        {
            title.text=Load.songs[panels.CurrentPanel].title;
            bscore.text=Load.songs[panels.CurrentPanel].bestscore;
        }
    }
//}
