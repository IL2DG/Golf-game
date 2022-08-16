using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorScript : MonoBehaviour
{
    public GameObject levelObject;
    public GameObject ballPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject roadPrefab;
    public GameObject turnPrefab;
    public GameObject prepPrefabV1;
    public GameObject prepPrefabV2;
    public GameObject prepPrefabV3;
    private string[,] level;
    private Vector3 pos;
    private int lvlWidth;
    private int lvlHeight;
    private void Start() {
        if(PlayerPrefs.GetInt("lvlWidth") < 3){
            PlayerPrefs.SetInt("lvlWidth", 3);
            PlayerPrefs.SetInt("lvlHeight", 3);
        }
        lvlWidth = PlayerPrefs.GetInt("lvlWidth");
        lvlHeight = PlayerPrefs.GetInt("lvlHeight");
        GenerateLevel(lvlWidth, lvlHeight);
    }  

    private void OnApplicationQuit() {
        PlayerPrefs.SetInt("lvlWidth", 3);
        PlayerPrefs.SetInt("lvlHeight", 3);
        PlayerPrefs.SetInt("Currentlvl", 1);
        PlayerPrefs.SetInt("CurrentPoints", 0);
    }

    void GenerateLevel(int w, int h){
        level = new string[h, w];
        for(int i = 0; i < h; i++){
            for(int j = 0; j < w; j++){
                level[i, j] = "N";
            }
        }

        int randStart = Random.Range(0,w-1);
        bool genCycle = true;

        level[0,randStart] = "S";

        int prevX = randStart;
        int prevY = 1;
        level[prevY, prevX] = "Rv";

        while(genCycle){
            int randDirection = (int)Random.Range(1,6);

            if(randDirection == 5){
                if(prevY != level.GetUpperBound(0)){
                    if(prevX != 0 && level[prevY, prevX-1] != "N"){
                        level[prevY, prevX] = "Trd";
                    } else if(prevX != level.GetUpperBound(1) && level[prevY, prevX+1] != "N"){
                        level[prevY, prevX] = "Tld";
                    }
                    prevY += 1;
                    level[prevY, prevX] = "Rv";
                } else {
                    level[prevY, prevX] = "F";
                    genCycle = false;
                    //break;
                }
            } else if(randDirection > 2){
                if(prevX != level.GetUpperBound(1) && level[prevY, prevX + 1] == "N"){
                    if(prevX == 0){
                        level[prevY, prevX] = "Tur";
                    } else if(level[prevY, prevX - 1] == "N"){
                        level[prevY, prevX] = "Tur";
                    }
                    prevX += 1;
                    level[prevY, prevX] = "Rh";
                } else if(prevX != 0 && level[prevY, prevX - 1] == "N") {
                    if(prevX == level.GetUpperBound(1)){
                        level[prevY, prevX] = "Tul";
                    } else if(level[prevY, prevX + 1] == "N"){
                        level[prevY, prevX] = "Tul";
                    }
                    prevX -= 1;
                    level[prevY, prevX] = "Rh";
                } else if(prevY != level.GetUpperBound(0)){
                    if(prevX == 0){
                        level[prevY, prevX] = "Tld";
                    } else {
                        level[prevY, prevX] = "Trd";
                    }
                    prevY += 1;
                    level[prevY, prevX] = "Rv";
                } else {
                    level[prevY, prevX] = "F";
                    genCycle = false;
                    //break; 
                }
            } else {
                if(prevX != 0 && level[prevY, prevX - 1] == "N"){
                    if(prevX == level.GetUpperBound(1)){
                        level[prevY, prevX] = "Tul";
                    } else if(level[prevY, prevX + 1] == "N"){
                        level[prevY, prevX] = "Tul";
                    }
                    prevX -= 1;
                    level[prevY, prevX] = "Rh";
                } else if(prevX != level.GetUpperBound(1) && level[prevY, prevX + 1] == "N"){
                    if(prevX == 0){
                        level[prevY, prevX] = "Tur";
                    } else if(level[prevY, prevX - 1] == "N"){
                        level[prevY, prevX] = "Tur";
                    }
                    prevX += 1;
                    level[prevY, prevX] = "Rh";
                } else if(prevY != level.GetUpperBound(0)){
                    if(prevX == 0){
                        level[prevY, prevX] = "Tld";
                    } else {
                        level[prevY, prevX] = "Trd";
                    }
                    prevY += 1;
                    level[prevY, prevX] = "Rv";
                } else {
                    level[prevY, prevX] = "F";
                    genCycle = false;
                    //break;
                }
            }
            
        }

        string strLog = "";
        for(int i = 0; i <= level.GetUpperBound(0); i++){
            for(int j = 0; j <= level.GetUpperBound(1); j++){
                strLog += level[i, j];
            }
            strLog += "\n";
        }
        Debug.Log(strLog);

        for(int i = 0; i <= level.GetUpperBound(0); i++){
            if(level[level.GetUpperBound(0), i] == "F"){
                if(i != 0 && level[level.GetUpperBound(0), i-1] != "N"){
                    level[level.GetUpperBound(0), i] = "Fl";
                } else if(i != level.GetUpperBound(1) && level[level.GetUpperBound(0), i+1] != "N"){
                    level[level.GetUpperBound(0), i] = "Fr";
                } else {
                    level[level.GetUpperBound(0), i] = "Fu";
                }
            }
        }

        for(int i = 0; i <= level.GetUpperBound(0); i++){
            for(int j = 0; j <= level.GetUpperBound(1); j++){
                pos = Vector3.zero;
                pos.x = i * 6;
                pos.z = j * 6;
                if(level[i, j] == "S"){
                    Instantiate(startPrefab, pos, Quaternion.identity).transform.SetParent(levelObject.transform);
                    pos.y = 1f;
                    Instantiate(ballPrefab, pos, Quaternion.identity);
                }
                    
                if(level[i, j] == "Rv"){
                    int randVar = Random.Range(0,8);
                    if(randVar < 5){
                        Instantiate(roadPrefab, pos, Quaternion.identity).transform.SetParent(levelObject.transform);
                    } else if(randVar < 6){
                        Instantiate(prepPrefabV1, pos, Quaternion.identity).transform.SetParent(levelObject.transform);
                    } else if(randVar < 7){
                        Instantiate(prepPrefabV2, pos, Quaternion.identity).transform.SetParent(levelObject.transform);
                    } else {
                        Instantiate(prepPrefabV3, pos, Quaternion.identity).transform.SetParent(levelObject.transform);
                    }
                }
                if(level[i, j] == "Rh"){
                    int randVar = Random.Range(0,8);
                    if(randVar < 5){
                        Instantiate(roadPrefab, pos, Quaternion.Euler(0, 90f, 0)).transform.SetParent(levelObject.transform);
                    } else if(randVar < 6){
                        Instantiate(prepPrefabV1, pos, Quaternion.Euler(0, 90f, 0)).transform.SetParent(levelObject.transform);
                    } else if(randVar < 7){
                        Instantiate(prepPrefabV2, pos, Quaternion.Euler(0, 90f, 0)).transform.SetParent(levelObject.transform);
                    } else {
                        Instantiate(prepPrefabV3, pos, Quaternion.Euler(0, 90f, 0)).transform.SetParent(levelObject.transform);
                    }
                }
                if(level[i, j] == "Tul")
                    Instantiate(turnPrefab, pos, Quaternion.Euler(0, 180f, 0)).transform.SetParent(levelObject.transform);
                if(level[i, j] == "Tur")
                    Instantiate(turnPrefab, pos, Quaternion.Euler(0, 270f, 0)).transform.SetParent(levelObject.transform);
                if(level[i, j] == "Tld")
                    Instantiate(turnPrefab, pos, Quaternion.identity).transform.SetParent(levelObject.transform);
                if(level[i, j] == "Trd")
                    Instantiate(turnPrefab, pos, Quaternion.Euler(0, 90f, 0)).transform.SetParent(levelObject.transform);
                if(level[i, j] == "Fu")
                    Instantiate(endPrefab, pos, Quaternion.Euler(0, 180f, 0)).transform.SetParent(levelObject.transform);
                if(level[i, j] == "Fl")
                    Instantiate(endPrefab, pos, Quaternion.Euler(0, 90f, 0)).transform.SetParent(levelObject.transform);
                if(level[i, j] == "Fr")
                    Instantiate(endPrefab, pos, Quaternion.Euler(0, 270f, 0)).transform.SetParent(levelObject.transform);
            }
        }
    }  

}
