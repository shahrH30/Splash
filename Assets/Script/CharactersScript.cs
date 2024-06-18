using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharactersScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;// רכיב להצגת ספרייט (תמונה) של הדמות

    List<VisualEmotion> emotions; //רשימה של מצבים של הדמות כל מצב קשור לספרייט מסוים

    public string playerName;// שם השחקן
    public int correctAnswers;//מספר תשובות נכונות
    public int incorrectAnswers;//מספר תשובות לא נכונות

    public float time;// זמן כולל שהשחקן שיחק
    public TextMeshProUGUI timerText;// טקסט להצגת הזמן בצורה ויזואלית
    public GameObject throwBall;// הכדור שהשחקן זורק
    public Sprite answerBallSprite;//ספרייט של הכדור שמשמש לתשובות


    public int AllAnswers { get => correctAnswers + incorrectAnswers;}// חישוב כלל התשובות של השחקן

    [System.Serializable]
    public class VisualEmotion// מחלקה שדרכה משנים את מצב הדמות
    {
        public CharacterStates emotion = CharacterStates.StandRegularForStart;// מצב הדמות
        public Sprite sprite;
    }

    //איתחול של הדמות עם טיימר כדור לזריקה והספרייט של הכדור
    public bool Init(TextMeshProUGUI timerText, GameObject throwBall, Sprite answerBallSprite)
    {
        this.timerText = timerText;
        this.throwBall = throwBall;
        this.answerBallSprite = answerBallSprite;

        return true;
    }

    // יצירת שחקן חדש עם שם ורשימת מצבים של הדמות
    public void CreationPlayers(string name, List<VisualEmotion> emotions)//מגדיר שיטה להגדרת דמויות שחקנים עם שמותיהם ורגשותיהם המתאימים.
    {
        this.playerName = name;
        this.emotions = emotions;
        ChangeEmotion(CharacterStates.Empty);// במקום לקבוע ישירות את הספרייט, נשתמש בפונקציה ChangeEmotion כדי לקבוע את המראה ההתחלתי
    }


    public void ChangeEmotion(CharacterStates newState) // החלפת ספרייט לדמות
    {
        VisualEmotion emotionToDisplay = emotions.Find(e => e.emotion == newState); // מחפש את המצב הרלוונטי
        if (emotionToDisplay != null)
        {
            renderer.sprite = emotionToDisplay.sprite; // שינוי הספרייט של הדמות לספרייט שמתאים למצב החדש
        }
    }

    // פונקציה להוספת נקודות או זמן לשחקן
    public void AddScore(bool isCorrect, float time)
    {
        // שליחה לפונקציה של הוספת זמן
        SaveTheTime(time);

        // בדיקה אם השחקן צדק או טעה
            if (isCorrect)
            {
                correctAnswers++;
            }
            else
            {
                incorrectAnswers++;
            }
        
    }

    // שמירת הזמן הכולל
    public void SaveTheTime(float time)
    {

        this.time += time;
    }

    // חישוב הציון הסופי של השחקן
    public int Finalgradecalculation()
    {

        float score = ((float)(AllAnswers - incorrectAnswers) / AllAnswers) * 100;
        Debug.Log(score);
        return (int)score;
    }

}
 
public enum CharacterStates { HappyInBarrow, SadInBarrow, SittingHappy,SittingRegular,SittingSmile,SittingSuprise,SittingUnHappy,StandHappy,StandRegular,StandSmile,Empty, StandRegularForStart,StandHappyForStart }//יצירת רשימת מצבים שאחריה נקשר את המצבים של הדמות אליהם




