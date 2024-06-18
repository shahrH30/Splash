using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // מיועד לניהול תשובות המשחק, ממשקים הנוספים מאפשרים לסקריפ להגיב לפעולות העכבר של הגדלה והקטנה
{
    [Header("It shouldn't be used publicly for an admin game")]
    public TextMeshProUGUI answerText;// האם יש טקסט לתשובה
    public Image answerImage;// האם יש תמונה לתשובה
    public AnswerData answerData;// בדיקה האם זה נכון או לא נכון 
    public GameManagerScript gameManager; //תקשורת בין שני הסקריפטים שלנו ושהתשובה תגיד חמנהל שהיוזר לחץ עלי ואני תשובה נכונה או לא נכונה
    [SerializeField] private Button clickAnswer;//כפתור שהוא התשובה שנלחצת 
    [SerializeField] private GameObject Vobject; // המטרה עם משוב של הצלחה
    [SerializeField] private GameObject Xobject;// מטרה עם משוב להצלחה
    [SerializeField] private RectTransform ballTarget;// כיוון שנשלח הכדור
    public RectTransform BallTarget { get => ballTarget;}// המיקום שהכדור אמור לפגוע בו בסוף האנימציה מיקום קבוע

    // משתנים להפעלת סאונדים שונים בהתאם לסוג התשובה (נכונה או שגויה)
    [SerializeField] private AudioSource feedbackAudio;//אובייקט על המסך שמפעיל את הסאונד
    [SerializeField] private AudioClip correctSound;// קובץ סאונד
    [SerializeField] private AudioClip InCorrectSound;// קובץ סאונד
    [SerializeField] private GameObject MagnifyingGlass;//הצגת אייקון זכוית מגדלת

    // ערכים להגדלה ואנימציה של התמונה בעת העברת העכבר מעליה
    public static float magnificationAmount = 2f; // גודל ההגדלה
    public static float animationTime = 0.25f; // זמן האנימציה בשניות
    private Vector2 originalSizeDelta;// משתנה ששומר את הגודל ההתחלתח של התמונה

    




    void Start()
    {
        if (answerImage != null)// אם יש תמונה בתשובה אז שומרים את גודל התמונה ה ראשוני כמשתנה כדי שנוכל להגיל ולהקטין אותו והוא יחזור לגודל המקורי שלו
        {
            // שמור את הגודל המקורי של התמונה
            originalSizeDelta = answerImage.rectTransform.sizeDelta;
        }

        clickAnswer.image.alphaHitTestMinimumThreshold = 0.1f;// תתעלם משטחים שקופים שנמצאים בכפתורים
    }

    //אתחול תשובות בדיקה אם יש טקסט או תמונה 
    public void Init(GameManagerScript gameManager, AnswerData answerData)// אתחול של הפריפאב
    {
        this.gameManager = gameManager;
        this.answerData = answerData;
        if (answerData.imageContent != null)//האם יש תמונה בתשובה
        {
            answerImage.sprite = answerData.imageContent;//הצגת תמונה אם קיימת
            MagnifyingGlass.SetActive(true);
            answerImage.gameObject.SetActive(true);
            answerText.gameObject.SetActive(false);
            //newAns.answerImage.rectTransform.sizeDelta = Vector2.one;// מרכז את התשובה בתוך התיבה
            answerText.text = "";// הסתרת טקסט אם יש תמונה

        }
        else if (answerData.textContent != "")//אם אין תמונה יש טקסט
        {
            answerText.text = answerData.textContent;//הצגת הטקסט של התשובה בתוך האובייקט  
            answerImage.gameObject.SetActive(false);// כיבוי התמונה
            answerText.gameObject.SetActive(true);// הפעלת האובייקט של הטקסט

        }
        else
        {
            Debug.Log("no content");//  הדפסת הודעת שגיאה אם אין תוכן כלל.
        }
    }


    // פונקציה שמופעלת כאשר לוחצים על התשובה
    public void AnswerClicked()
    {
        gameManager.answerClicked(this);//הפונקציה של הלחיצת תשובה תופעל בסקריפט של מנהל המשחק עם המידע של התשובה שנלחצה 
    }

    // לא נאפשר את האופציה ללחוץ שוב על התשובה
    public void DisableAnsweringOption()
    {
        clickAnswer.interactable = false;
    }


    public bool Feedback()//פידבק של אחרי לחיצה
    {
        
        if (answerData.isCorrect) // אם זה נכון 
        {
            Vobject.SetActive(true);// האוביקט של ה תשובה נכונה מופיע
        }
        else// ההפך
        {
            Xobject.SetActive(true);
        }
        checkSound();
        return answerData.isCorrect;
    }


    //public enum AnswerType { Image, Text}

    // מופעל כאשר המשתמש מעביר את העכבר
    public void OnPointerEnter(PointerEventData eventData) // הגדרת הפונקציה של אירוע עכבר שהמשתמש נגעה בתמונה
    {
        // הגדלת התמונה כאשר העכבר עובר מעליה בודק אם התמונה פעילה בסצנה הנוכחית
        //אם הי לא קיימת אז אין למה להגדיל
        if (answerImage.gameObject.activeSelf)
        {
            MagnifyingGlass.SetActive(false); // מסתיר את הזכוכית מגדלת

            // הגדלת התמונה עם אנימציה מקבלת שלושה פרמטרים הגודל של התמונה שרוצים לשנות ,הגודל החדש שרוצים להגדיל אליו,וזמן האנימציה
            LeanTween.size(answerImage.rectTransform, originalSizeDelta * magnificationAmount, animationTime);
        }
    }

    //אותו דבר כמו ההגדלה רק הפוך
    // מופעל כאשר המשתמש מזיז את העכבר
    public void OnPointerExit(PointerEventData eventData)
    {
        // חזרה לגודל המקורי
        if (answerImage.gameObject.activeSelf)
        {
            MagnifyingGlass.SetActive(true); // מסתיר את הזכוכית מגדלת
            // הקטנת התמונה עם אנימציה החזרה לגודל המקורי שהיא הוצגה
            LeanTween.size(answerImage.rectTransform, originalSizeDelta, animationTime);
        }
    }


    //הפעלה והשתקה של סאונדים בהתאם לבחירה של המשתמש נשלח מהגיים מנגר
    public void checkSound()
    {
        if (GameManagerScript.IsSoundMuted)
        {
            // אם הסאונד מושתק, אל תנגן את הסאונד וחזור מהפונקציה.
            return;
        }
        
        if (answerData.isCorrect)
        {
            // סאונד אם התשובה נכונה
            feedbackAudio.PlayOneShot(correctSound, 10f); // הסאונד יושמע בעוצמה פי 1.5 מהעוצמה הרגילה
        }
        else
        {
            // סאונד אם התשובה לא נכונה
            feedbackAudio.PlayOneShot(InCorrectSound, 10f); // הסאונד יושמע בעוצמה פי 1.5 מהעוצמה הרגילה
        }
        // הפעלת הסאונד
        feedbackAudio.Play();
    }


}

