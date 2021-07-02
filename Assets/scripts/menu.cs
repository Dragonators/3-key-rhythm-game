using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleScrollSnap;

    public class menu : MonoBehaviour
    {
        public Text title;
        public Text bscore;
        public int selectid;
        public AudioSource[] allbgm=new AudioSource[6];
        public AudioClip turn;
        public SimpleScrollSnap scrollSnap;

    
        void Start()
        {
        
        }
        void Update()
        {
        
        }
        public void tested()
        {
            Debug.Log(scrollSnap.CurrentPanel);
        }
    }
//}
