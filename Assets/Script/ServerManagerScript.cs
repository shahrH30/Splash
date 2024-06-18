using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Runtime.Serialization.Json;
using System.IO;
using System;

public class ServerManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMP_InputField codeInput;
    [SerializeField] GameObject startButton;
    [SerializeField] GameManagerScript gameManager;
    [SerializeField] Sprite tempImage;

    string projectURL = "https://localhost:7022/"; //����� ��������
    string apiURL = "api/Unity/"; //����� ��������� ������
    //string imagesURL = "uploadedFiles/"; //����� ������� �������


    public async void CheckCode()
    {

       
        string code = codeInput.text;

        if (int.TryParse(code, out int parsedCode) && parsedCode <= 100)
        {
            gameManager.TextErrorCode.text = "���� ���� ����� ���� �-100.";
            return;
        }

        Debug.Log("loading");

        GameData UnityGame = await GetDataFromServer(code);

        if (UnityGame != null && UnityGame.questionList.Count > 0)
        {
            UnityGame = ReverseNumbersInGameData(UnityGame);

            startButton.SetActive(false);
            codeInput.gameObject.SetActive(false);
            gameManager.GetGame(UnityGame);
        }
        else
        {
            if (UnityGame == null)
            {
                // ����� �� ������ ����� ������ ������ �� ����
                string errorMessage = await GetServerErrorMessage(code);

                if (errorMessage.Contains("����� �� ����"))
                {
                    gameManager.TextErrorCode.text = "����� �� ���� ������";
                }
                else if (errorMessage.Contains("����� �� �����"))
                {
                    gameManager.TextErrorCode.text = "����� ���� �� �� �����";
                }
              
            }

            Debug.LogError("content returns empty or null");
            gameManager.GameInputFieldCodeTextStarte.text = string.Empty;
            UnityGame = null;
            return;
        }
    }


    async Task<string> GetServerErrorMessage(string code)
    {
        string endpoint = projectURL + apiURL + code;
        using var http = UnityWebRequest.Get(endpoint);
        var get = http.SendWebRequest();
        while (!get.isDone)
        {
            await Task.Yield();
        }
        return http.downloadHandler.text;
    }

    async Task<GameData> GetDataFromServer(string code)
    {
        string endpoint = projectURL + apiURL + code;
        using var http = UnityWebRequest.Get(endpoint); //����� ����
        var get = http.SendWebRequest(); //�����
        while (get.isDone == false) //�� ��� �� ����
        {
            await Task.Yield(); //������ �� ���� ����
        }
        if (http.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = http.downloadHandler.text;
            ServerGame serverGame = JsonUtility.FromJson<ServerGame>(jsonResponse);
            //Game serverGame = JsonReaderWriterFactory.CreateJsonReader(StreamReader jsonResponse, System.Text.Encoding.Unicode);
            GameData UnityGame = new GameData();

            UnityGame.gameName = serverGame.gameName;
            UnityGame.questionTime = (float)serverGame.timeLimitPerQues;
            UnityGame.questionList = new List<QuestionData>();

            foreach (ServerQuestion question in serverGame.questions)
            {
                QuestionData UnityQuestion = new QuestionData();
                UnityQuestion.content = question.questionsText;
                //UnityQuestion.isAnswered = question.isAnswered;

                if (question.questionsImage != "DefaultName")
                {
                    UnityQuestion.imageContent = await LoadImage(question.questionsImage);

                }
                UnityQuestion.answersList = new List<AnswerData>();
                foreach (ServerAnswer answer in question.answers)
                {
                    AnswerData UnityAnswer = new AnswerData();
                    UnityAnswer.isCorrect = answer.isCorrect;
                    if (answer.isPicture == true)
                    {
                        UnityAnswer.imageContent = await LoadImage(answer.content);

                    }
                    else
                    {
                        UnityAnswer.textContent = answer.content;
                    }
                    UnityQuestion.answersList.Add(UnityAnswer);
                }
                UnityGame.questionList.Add(UnityQuestion);
            }
            return UnityGame;
        }

        else //�� �� �����
        {
            string errorMessage = http.downloadHandler.text;
            Debug.LogError(errorMessage);
            return null;
        }
    }

    async Task<Sprite> LoadImage(string imageName)
    {

        if (imageName == "DefaultName")
        {
            return null;
        }

        string endpoint = projectURL + imageName;
        //string endpoint = projectURL + imagesURL + imageName;
        using var http = UnityWebRequestTexture.GetTexture(endpoint);
        var get = http.SendWebRequest();
        while (get.isDone == false)
        {
            await Task.Yield();
        }
        if (http.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(http);
            if (texture == null)
            {
                
                Debug.LogError("No image");
                return tempImage;

            }
            Rect spriteRect = new Rect(0, 0, texture.width, texture.height);
            Vector2 spritePivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(texture, spriteRect, spritePivot);
            return sprite;

        }
        else
        {
            Debug.LogError("No texture found at the endpoint: " + endpoint);
            Debug.LogError("No image");
            return tempImage;
        }
    }


    private GameData ReverseNumbersInGameData(GameData gameData)
    {
        gameData.gameName = ReverseNumbersInText(gameData.gameName);
        for (int i = 0; i < gameData.questionList.Count; i++)
        {
            gameData.questionList[i].content = ReverseNumbersInText(gameData.questionList[i].content);
            for (int j = 0; j < gameData.questionList[i].answersList.Count; j++)
            {
                if (!string.IsNullOrEmpty(gameData.questionList[i].answersList[j].textContent))
                {
                    gameData.questionList[i].answersList[j].textContent = ReverseNumbersInText(gameData.questionList[i].answersList[j].textContent);
                }
            }
        }
        return gameData;
    }


    public static string ReverseNumbersInText(string input)
    {
        char[] chars = input.ToCharArray();
        int start = -1;

        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsDigit(chars[i]))
            {
                if (start == -1)
                    start = i;
            }
            else
            {
                if (start != -1)
                {
                    Array.Reverse(chars, start, i - start);
                    start = -1;
                }
            }
        }

        if (start != -1)
        {
            Array.Reverse(chars, start, chars.Length - start);
        }

        return new string(chars);
    }



}

[System.Serializable]
public class ServerGame
{
    public int id;
    //public string code;
    public string gameName;
   
    public int timeLimitPerQues;
    public List<ServerQuestion> questions;
}

[System.Serializable]
public class ServerQuestion
{
    //public int id;
    public string questionsText;
    public string questionsImage;
    public int gameID;
    //public bool isAnswered;
    public List<ServerAnswer> answers;
}

[System.Serializable]
public class ServerAnswer
{
    //public int id;
    //public int questionID;
    public string content;
    public bool isPicture;
    public bool isCorrect;
}

