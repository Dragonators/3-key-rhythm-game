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
        private string newPath;
        public void Starting()
        {
            newPath=Application.persistentDataPath+"/"+"Songlist.txt";
            if(!File.Exists(newPath))
            {
                Debug.Log(1);
                File.WriteAllText(newPath,Resources.Load<TextAsset>("Songlist").ToString());
                Resources.UnloadUnusedAssets();
            }
        }
        public void loadcsv()
        {
            string[] fileData=File.ReadAllLines(newPath);
            for(int i=1;i<fileData.Length;i++)
            {
                string[] _fileData=fileData[i].Split(',');
                song.id=_fileData[0];
                song.title=_fileData[1];
                song.bpm=float.Parse(_fileData[2]);
                song.bestscore=_fileData[3];
                song.artist=_fileData[4];
                song.diff_e=_fileData[5];
                song.diff_n=_fileData[6];
                song.diff_h=_fileData[7];
                songs.Add(song);
            }
        }
        public void scorewrite()
        {
            int index_=-1;
            string[] fileData=File.ReadAllLines(newPath);
            for(int i=1;i<fileData.Length;i++)
            {
                string[] _fileData=fileData[i].Split(',');
                if(_fileData[0]!=mapnow.mapid)continue;
                if(int.Parse(_fileData[3])>=mapnow.score)
                {
                    mapnow.highscore=int.Parse(_fileData[3]);
                    break;
                }
                else 
                {
                    mapnow.highscore=mapnow.score;
                    _fileData[3]=mapnow.score.ToString();
                    index_=i;
                    fileData[index_]=_fileData[0]+","+_fileData[1]+","+_fileData[2]+","+_fileData[3]+","+_fileData[4]+","+_fileData[5]+","+_fileData[6]+","+_fileData[7];
                    break;
                }
            }
            if(index_!=-1)
            {
                File.WriteAllLines(newPath,fileData);
            }
        }
    }
    public struct songinform
    {
        public string id;
        public string title;
        public float bpm;
        public string bestscore;
        public string artist;
        public string diff_e;
        public string diff_n;
        public string diff_h;
    }