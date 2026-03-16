using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : Singleton<LevelsManager>
{
    //variables
    public List<SOlevel> levels = new List<SOlevel>();
    [SerializeField] Image image;
    bool pressed;

    public Level currentNextLevel;
    [SerializeField] CarouselUI carousel;

    //properties
    protected override bool persistent => true;
    public int LevelsCount {get => levels.Count; }
    public bool Pressed {  get => pressed;}
    //methods

    public void NextLevel()
    {
        if(pressed) return;
        if (levels.Count == 0)
        {
            SceneManager.instance.LoadScene("FinalScene");
            return;
        }
        carousel.gameObject.SetActive(true);
        carousel.StartScrolling();
        //StartCoroutine(ChooseLevel());
    }

    public void ChargeLevel()
    {
        StartCoroutine(ChooseLevel());
    }

    IEnumerator ChooseLevel()
    {
        pressed = true;
        //int index = 0;

        //while (index < 3)
        //{
        //    foreach (SOlevel level in levels)
        //    {
        //        image.sprite = level.icon;
        //        yield return new WaitForSeconds(.3f);
        //    }
        //    index++;
        //}
        

        //int value = Random.Range(0, levels.Count);
        //SOlevel _level = levels[value];
        //image.sprite = _level.icon;

        yield return new WaitForSeconds(2f);
        SceneManager.instance.LoadScene(currentNextLevel.soLevel.levelName);
        yield return new WaitForSeconds(1.5f);
        foreach(SOlevel level in levels) { if(level == currentNextLevel.soLevel) { levels.Remove(level); break; } }
        Destroy(currentNextLevel.gameObject);
        carousel.gameObject.SetActive(false);
        pressed = false;

    }
}
