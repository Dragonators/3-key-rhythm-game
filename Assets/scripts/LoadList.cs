using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

    public class LoadList : MonoBehaviour
    {
        public List<songinform> songs=new List<songinform>();
        private songinform song;
        private TextAsset songlist;
        public void loadcsv()
        {
            songlist=Resources.Load<TextAsset>("Songlist");
            string[] fileData=songlist.text.Split('\n');
            for(int i=1;i<fileData.Length;i++)
            {
                string[] _fileData=fileData[i].Split(',');
                song.id=_fileData[0];
                song.title=_fileData[1];
                song.bpm=_fileData[2];
                song.bestscore=_fileData[3];
                song.artist=_fileData[4];
                songs.Add(song);
            }
        }
        void Start()
        {
            loadcsv();
        }
    }
    public struct songinform
    {
        public string id;
        public string title;
        public string bpm;
        public string bestscore;
        public string artist;
    }