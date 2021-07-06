using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public Text title;
    public Text score;
    public Text P;
    public Text Gd;
    public Text Gr;
    public Text miss;
    public Text highscore;
    public LoadList Load;
    void Start()
    {
        title.text=mapnow.title;
        score.text=mapnow.score.ToString();
        P.text=mapnow.n_P.ToString();
        Gd.text=mapnow.n_Gd.ToString();
        Gr.text=mapnow.n_Gr.ToString();
        miss.text=mapnow.n_miss.ToString();
        Load.Starting();
        Load.scorewrite();
        highscore.text=mapnow.highscore.ToString();
    }
    public void backto()
    {
        GameObject.DestroyImmediate(mapnow.BGM.gameObject);
        SceneManager.LoadScene("Selsong");
    }
    public void retry()
    {
        SceneManager.LoadScene("Play");
    }
}
