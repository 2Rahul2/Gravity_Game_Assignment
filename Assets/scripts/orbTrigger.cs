using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class orbTrigger : MonoBehaviour
{
    public int totalCubes;
    public float totalTime;
    public Text timeText;

    public bool winMatch;
    public GameObject RetryButton;
    public GameObject winMessage;

    public GameObject player;

    void Update()
    {
        if(winMatch){
            winMessage.SetActive(true);
            player.GetComponent<player>().enabled = false;

        }else{
            if(totalTime<=0){
                // if(winMatch){
                //     winMessage.SetActive(true);
                // }else{
                RetryButton.SetActive(true);
                
                player.GetComponent<player>().enabled = false;
                timeText.text = "Time Remaining: 0";
            }else{
                totalTime -= Time.deltaTime;
                totalTime = Mathf.Round(totalTime * 100f) / 100f;
                timeText.text = ("Time Remaining: " + totalTime.ToString());
            }

        }
    }
    public void RetryFunction(){
         SceneManager.LoadScene("SampleScene");
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "orb"){
            print("detected");
            Destroy(other.gameObject);
            totalCubes -= 1;
        }

        if(totalCubes<=0){
            print("game completed");
            winMatch = true;
        }   
    }
}
