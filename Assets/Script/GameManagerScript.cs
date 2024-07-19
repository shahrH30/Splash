using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Device;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CharactersScript;

public class GameManagerScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Image fadeImg;
    //משתנים
    [Header("start")]// אלמנטים למסך התחלה ובחירת דמויות
    public List<CharacterData> characters; // רשימה שמגדירים בממשק יוניטי עם נתוני 4 הדמויות 
    public TMP_InputField GameInputFieldCodeTextStarte;// למסך הזנת קוד תיבת טקסט
    [SerializeField] private TMP_InputField ChoosePlayerInputField;// להזהת שם למסך בחירת משתמשים
    [SerializeField] private Button BTNGameCodeStart;// כפתור למסך התחלתי אחרי הזנת הקוד הוא יפעל 
    [SerializeField] private Button BTNChoosePlayers;// כפתור למסך בחירת דמויות אחרי בחירת שם לדמות ודמות הוא יפעל
    public TextMeshPro TextErrorCode;// טעות של אם הקוד שהוזן שגוי
    [SerializeField] private GameObject StartScreen;// אובייקט שפותח את המסכים של תחילת המשחק
    [SerializeField] private GameObject logo;//  אובייקט בו יש את הלוגו של מסך ההתחלה
	[SerializeField] private GameObject starttextabouve;// אובייקט בו יש את הטקסט העליון של מסך ההתחלה
    [SerializeField] private GameObject starttextabelow;//  אובייקט בו יש את הטקסט התחתון של מסך ההתחלה
	[SerializeField] private GameObject ChoosePlayers;// אובייקט המייצג את כל האובייקים במסך בחירת הדמויות 
    [SerializeField] private GameObject ParticipantLotteryScreen;// אובייקט המייצג את מסך ההגרלה עם כל הדברים שנמצאים על המסך
    [SerializeField] private GameObject startCanvas;// הכפתור והתיבת טקסט ש למסך ההתחלה
    [SerializeField] private GameObject BgSoundForStart;// הכפתור והתיבת טקסט ש למסך ההתחלה
    [SerializeField] private GameObject ChoosePlayersCanvas;// הכפתור והתיבת טקסט של מסך בחירת הדמויות
    [SerializeField] CharactersScript characterPF;//פריפאב של הדמות משתנה שיצור את היאנסטנסים של הדמות במשחק
    [SerializeField] TextMeshPro TheNumberOfTheStartingPlayer;// מציג את מספר השחקן שמתחיל להזין את הנתונים שלו

    [SerializeField] private GameObject parkAnimationContainer;
    private Vector3 parkAnimationContainerStartingScale;
    [SerializeField] private Animator parkAnimator;
    [SerializeField] private Animator spinningWheel;
    [SerializeField] private Animator kidsAnim;
    [SerializeField] private Animator Flag;
    [SerializeField] private Animator chair2;
    [SerializeField] private Animator chair3;
    [SerializeField] private Animator chair4;
    [SerializeField] private Animator chair5;
    [SerializeField] private Animator Boy1InBarrowStart;
    [SerializeField] private SpriteRenderer parkBG;

    [Header("character selectio")]// אלמנטים של הדמוית שהמשתמש בוחר מהם
    bool checkClickChooseCarecter;// בוליאני שבוחר האם נבחרה הדמות
    [SerializeField] private List<Button> CharactersButton;//כפתורים שהם בעצם בחירת דמויות
    [SerializeField] private GameObject TextErrorChoosePlayers;// הצגת שגיאה במסך בחירת הדמויות
    [SerializeField] private GameObject TextErrorChoosePlayersSpase;// הצגת שגיאה במסך בחירת הדמויות של רווח


    [Header("Lottery screen")]// אלמנטים למסך הגרלה
    [SerializeField] private GameObject PositionPlayer1;// מיקום השחקן הראשון על מסך בהגרלה
    [SerializeField] private GameObject PositionPlayer2;// מיקום השחקן השני על מסך בהגרלה
    [SerializeField] private Button BTNRandomWheel;// כפתור להגרלת דמות 
    [SerializeField] private TextMeshPro WheelWinnerTitle;// הצגת שם השחקן שמתחיל
    [SerializeField] private GameObject RandomWheelCanvas;//הכפתור של הגרלת המשחק
    [SerializeField] private Collider2D arrowWheelCollider;// קוליידר לחצ שמצביע על הדמות שמנצחת בהגרלה
    [SerializeField] private TextMeshProUGUI wheelSliceText;// השם לגלגל -שם אחד מתוך השמות שנמצאים 
	[SerializeField] private ParticleSystem Confety; // קונפטי

	[Header("The game screen")]// מסך שאלות 
    [SerializeField] private GameObject GameQuestionsCanvas;// האלמנטים שנמצאים בקאנבס המשחק
    [SerializeField] private GameObject GameQuestionsScreen;// האלמנטים שנמצאים במסך של הדמות הרגיל
    [SerializeField] private GameObject PlaceForPlayer1;// מקום לשחקן 1 על המסך
    [SerializeField] private GameObject PlaceForPlayer2;// מקום לשחקן 2 על המסך
    [SerializeField] private GameObject woodenSignP1;// שלט עץ של שחקן 1
    [SerializeField] private GameObject woodenSignP2;// שלט עץ של שחקן 2
    [SerializeField] private TextMeshPro NamePlayer1;// השם של שחקו 1 שיוצג
    [SerializeField] private TextMeshPro NamePlayer2;// שם של שחקן 2 שיוצג
    [SerializeField] private TextMeshProUGUI timerTextPlayer1;// הטקסט שמציג את השעון של שחקן 1
    [SerializeField] private TextMeshProUGUI timerTextPlayer2;// הטקסט שמציג את השעון של שחקן 2
    [SerializeField] private GameObject timerSpotPlayer1;//מיקום השעון של שחקן 1
    [SerializeField] private GameObject timerSpotPlayer2;//מיקום השעון של שחקן 2 
    [SerializeField] private Image QuestionImage;//תמונה לשאלה 
    [SerializeField] private GameObject questionImagePanel;// התמונה וזכוכית המגדלת ביחד
    [SerializeField] private GameObject MagnifyingGlassImage;//הצגת אייקון זכוית מגדלת
    private float magnificationAmount = 1.7f; //בכמה מגדילים את התמונה
    private float animationTime = 0.25f; // משך זמן האנימציה




    [Header("Question Creation and Feedback")]// יצירת שאלות ומשובים במסך שאלות
    [SerializeField] private TextMeshPro questionText;// להצגת השאלה
    [SerializeField] private TextMeshPro TitleGameSub;// להצגת הנושא
    [SerializeField] AnswerBoxScript AnswerScript;// שמירת הסקרפט של התשובות כדי ליצור את הפריפאבס
    [SerializeField] private RectTransform answersGrid;// גריד שמוקמות עליו השאלות בהתאם לכמות השאלות 
    [SerializeField] private HorizontalLayoutGroup answersGridGroupPrefab;
    [SerializeField] private GameObject nextQuesBTN;//כפתור לשאלה הבאה
    [SerializeField] private float distance;// משתנה לקביעת מרחק בין התשובות
    [SerializeField] private RuntimeAnimatorController questionAminController;
    private int questionNumber; //מעקב באיזה שאלה אני מתוך המשחק
    private TimeLimitTimer questionTimer = new TimeLimitTimer();//כמה זמן עבר מתחילת השאלה
    [SerializeField] private GameData game;// הפניה למחלקה שניצור בה את השאלות ותשובות מחוץ לקוד ונשתמש בה בקוד
    private List<AnswerBoxScript> answerOnScreen = new List<AnswerBoxScript>();// רשימה של כל האפשרויות של התשובות שיש על המסך


    [Header("Ball")]//
    //private GameObject Ball;
    [SerializeField] private Transform ThrowBallStartingPos;// מיקום של איפה הכדור ממוקם ראשון ומתחילה האנימציה
    [SerializeField] private GameObject ThrowBallP1;// אובייקט שמייצגת את הכדור של שחקן 1
    [SerializeField] private GameObject ThrowBallP2;// אובייקט שמייצג את הכדור של שחקן 2
    //[SerializeField] private GameObject PlaseBtnPopup;

    [Header("Feedback: The color of the balls")]
    [SerializeField] private Image answerBallGray;//הצגת הכדור האפור למטה בגריד
    [SerializeField] private Image answerBallBlue;// הצגת הכדור הכחול של משתמש 2 בגריד
    [SerializeField] private Image answerBallRed;// הצגת הכדור האדום של משתמש 1 בגריד
    [SerializeField] private Transform answersBallsGrid;// המקום שמאורגנים הכדורים בהתאם לכמות השאלות

    [Header("SpeechBubb game")]// משוב להצלחה או אי הצלחה בבועות דיבור
    [SerializeField] private GameObject SpeechBubble2;//בועת דיבור של שחקן 2
    [SerializeField] private GameObject SpeechBubble1;//בועת דיבור של שחקן 1
    [SerializeField] private TextMeshPro TextBubble2;// טקסט לבועת דיבור של שחקן 2
    [SerializeField] private TextMeshPro TextBubble1;//טקסט לבועת דיבור של שחקן 1

    [Header("before the end")]// הכנות לפני סוף משחק תהיה פה אנימציה
    [SerializeField] private GameObject BtnToTheEnd;// כפתור שלוחצים עליו מראה את המפסיד והמנצח
    [SerializeField] private GameObject ResultsBTN;// כפתור ששולח למסך סיכום
    [SerializeField] private GameObject BRforTheFall;// כפתור ששולח למסך סיכום
    [SerializeField] private TextMeshProUGUI TextToTheEnd;// טקסט שמשתנה בסוף המשחק מבקש מהמשתמש להפיך את המפסיד למים

    [Header("location of the fall")]// מיקום הנפילה אחרי לחיצה על הכפתור
    [SerializeField] private Transform PlaseForMachP2;// מיקום חדש השחקן 2 שנפל
    [SerializeField] private Transform PlaseForMachP1;//מיקום חדש השחקן 1 שנפל

    [Header("End Screen")]// מעבר למסך סיכום
    [SerializeField] private GameObject EndScreen;// מסך הסיכום עם כל האלמנטים
    [SerializeField] private GameObject EndGameCanvas;// תוכן שנמצא בקאנבס
    [SerializeField] Transform player1EndPosition;// מיקום של שחקן 1
    [SerializeField] Transform player2EndPosition;// מיקום של שחקן 2
    [SerializeField] TextMeshPro TitleGameSubForEnd;// הכותרת של המשחק בסיום
    [SerializeField] private Button resetGameButton;// כפתור התחלה מחדש כולל קוד
    [SerializeField] private Button AnotherRoundButton;// כפתור איתחול משחק


    [Header("SpeechBubb to the end")]// משובים לסיכום המשחק
    [SerializeField] private GameObject EndSpeechBubble2;// בועת דיבור לסיכום של שחקן 2
    [SerializeField] private GameObject EndSpeechBubble1;//בועת דיבור לסיכום של שחקן 1
    [SerializeField] private GameObject EndSpeechBubble3;//בועת דיבור לסיכום של תיקו
    [SerializeField] private TextMeshPro TwoWinText;//טקסט של שניים שבתיקו
    [SerializeField] private TextMeshPro OnlyOneWinText;// טקסט אם רק אחד ניצח
    [SerializeField] private TextMeshProUGUI SummaryTextP1;// טקסט סיכום של שחקן 1
    [SerializeField] private TextMeshProUGUI SummaryTextP2;// טקסט סיכום של שחקן 2
    [SerializeField] private TextMeshProUGUI SummaryTextWiner;// כותר אמצעית של מי שניצח


    [Header("Popups timeup")]//מסך נגמר הזמן 
    //[SerializeField] private Transform timeEndBTNPose; //האם צריך? מיקום כפתור חדש
    [SerializeField] private GameObject TimeUpCanvas;// פןפ אפ של נגמר הזמן

    [Header("pause Popup")]// מסך עצירה
    [SerializeField] private GameObject stopBTN;// כפתור עצירת משחקמשחק
    [SerializeField] private Button stopBTNAsBtn;// כפתור עצירת משחקמשחק
    //private float savedTimeRemaining;
    [SerializeField] private GameObject pausePopup;// פופ אפ של עצירת משחק
    [SerializeField] private Button resumeButton; // כפתור להמשך המשחק מהפופ אפ
    bool isBtnNextQuestionActiv { get => !nextQuesBTN.activeSelf; }// בדיקה אם אנחנו לא במשוב של סוף שאלה ואז מפעילים א=רק חלק מהדברים
    bool ResumeGameWasActiv = false;// בדיקה אם כפתור הפאוז הביא אותנו למסך יצירת שאלות

    [Header("sound")]//שמע במשחק
    [SerializeField] AudioMixer mixer;// שולט בסאונד 
    [SerializeField] private AudioSource musicAudioSource; // אודיה בשביל להשמיע את המוזיקה של המשחק
    [SerializeField] private AudioSource SFXAudioSource; //אודיו בשביל להשמיע את המוזיקות רקע
    [SerializeField] private Image soundOffImage; // תמונה של הכיבוי מוזיקה
    [SerializeField] private Image soundOnImage; // תמונה של מוזיקה פועלת
    [SerializeField] private Button soundToggleButton; // הכפתור שמפעיל את המוזיקה
    [SerializeField] private AudioClip victoryClipWillWin; // מוזיקה של ניצחון בגלגל
    [SerializeField] private AudioClip DramsClipWill; // מוזיקת רקע לגלג
    [SerializeField] private AudioClip fallWaterSound; // מוזיקה של נפילה למים
    [SerializeField] private AudioClip ChoosePlayerSound; //בלחיצה מוזיקה של בחירת שחקן
    [SerializeField] private AudioClip clapsForEndSound; //מחיאות כפיים בסוף למנצחים
    public static bool IsSoundMuted = false;// מעקב אחרי ההשתקה גם מסקריפט אחר

    [Header("stand1")]//1תקן של השחקן
    [SerializeField] private GameObject standPlayer1;//כל האובייקט
    [SerializeField] private GameObject StickStand1;//מקל
    [SerializeField] private GameObject chairStand1;// כיסא
    [SerializeField] private GameObject ChairStick1;// כיסא
    [SerializeField] private GameObject bathStand1;// Tמבטיה
    [SerializeField] private Animator standAnimator1;
    [SerializeField] private GameObject SplashWhenWrong1GameObject;

    private Vector3 chair1StartingPlace;
    private Vector3 chair2StartingPlace;

    [Header("splash")]//1תקן של השחקן
    //public ParticleSystem waterDrops; // אסיגנים לזה את ה- Particle System שלך דרך האינספקטור
    [SerializeField] private Animator splashAnimator1;
    [SerializeField] private Animator splashAnimator2;
    [SerializeField] private Animator SplashWhenWrong1;
    [SerializeField] private Animator SplashWhenWrong2;
    [SerializeField] private GameObject splashPlayer1;// מיקום של אנימציית המים
    [SerializeField] private GameObject splashPlayer2;// מיקום של אנימציית המים
    [SerializeField] private GameObject splastransperent1;// 
    //[SerializeField] private GameObject Puddle1;
    //[SerializeField] private GameObject Puddle2; 



    [Header("stand2")]//1תקן של השחקן
    [SerializeField] private GameObject standPlayer2;//כל האובייקט
    [SerializeField] private GameObject StickStand2;//מקל
    [SerializeField] private GameObject chairStand2;// כיסא
    [SerializeField] private GameObject ChairStick2;// כיסא
    [SerializeField] private GameObject bathStand2;// אמבטיה
    [SerializeField] private GameObject splastransperent2;// 
    [SerializeField] private Animator standAnimator2;
    [SerializeField] private GameObject SplashWhenWrong2GameObject;



    [SerializeField] private RuntimeAnimatorController endingAnimatorController;
    [Space]
    [SerializeField] private MonoBehaviour objectInvoker;
    public static MonoBehaviour ObjectInvoker;



    //private Vector3 initialScale; // גודל התחלתי של התמונה
    List<TextMeshProUGUI> wheelTexts = new List<TextMeshProUGUI>();// הקסט של הגלגל עם השמות בסוף אנחנו הורסים את הרשימה כדי ליצור שמות חדשים
    List<Image> activeAnswerBalls = new List<Image>();//רשימה לשמירת הכדורים במשחק שמציגים את השאלות
    CharactersScript player1;// משתנה לשמירת הנתונים של השחקנים
    CharactersScript player2;//משתנה לשמירת הנתונים של השחקנים
    CharactersScript playerTurn;//מיצג את השחקן שזה התור שלו
    string chosenCharacterName;// סטרינג לשמירת שם הדמות
                               
    LTDescr lt;// משתנה לשמירת האנימציה של LeanTween לשימוש עתידי או ביטולה



    [SerializeField] GameObject splshAnimasion;
    private Vector3 originalScale; // לשמירת הגודל המקורי של התמונה
    private bool isMouseOver = false; // לבדוק אם העכבר מעל האובייקט


    public void Start()// נקראת ברגע שהמשחק מופעל משמשת לאתחול המשחק בתחילתו ושמירת מצבים קבועים של אובייקטים במשחק 
    {
        // חיבור האירוע onValueChanged לפונקציה CheckInput
        ChoosePlayerInputField.onValueChanged.AddListener(CheckInput);

        splshAnimasion.SetActive(false);
        originalScale = QuestionImage.transform.localScale; // שמירת הגודל המקורי של התמונה
        parkAnimationContainerStartingScale = parkAnimationContainer.transform.localScale;
        BTNRandomWheel.image.alphaHitTestMinimumThreshold = 0.1f;
        CharactersButton.ForEach(b =>
        {
            b.image.rectTransform.sizeDelta = b.image.sprite.textureRect.size;
            b.image.alphaHitTestMinimumThreshold = 0.01f;// תתעלם משטחים שקופים שנמצאים בכפתורין
        });// פונקציה שעוברת על כל כפתורי השחקנים ומתאימים את גודל הכפתור לתמונה שלהם

        //startingPosNextQuesBTN = nextQuesBTN.transform.position;// שמירת ערך התחלתי של מיקום הכפתור כדי שנוכל לשנות אותו בהמשך ולחזור אליו
        chair1StartingPlace = chairStand1.transform.position;
        chair2StartingPlace = chairStand2.transform.position;

        if (objectInvoker == null)
            objectInvoker = this;
        ObjectInvoker = objectInvoker;
 
        EnterCodeGameScreen();// קריאה לפונקציה שמתחילה את המשחק

    }

    public void GetGame(/*GameData gameFromServer*/)
    {

        //game = gameFromServer;
        questionNumber = 0;

        //foreach (var question in game.questionList)
        //{
        //    Debug.Log("Question: " + question.content);
        //    foreach (var answer in question.answersList)
        //    {
        //        Debug.Log("Answer: " + (answer.isCorrect ? "(Correct) " : "") + (answer.textContent ?? "No Text") + (answer.imageContent != null ? " (Image)" : ""));
        //    }
        //}

        //InitializeQuestions();
        TextErrorCode.text = ""; // משוב שנכתב על המסך
        StartCoroutine(SwitchToChooseCharactersScreen());

    }
    //private void InitializeQuestions()
    //{
    //    foreach (var question in game.questionList)
    //    {
    //        question.content = ReverseNumbersInText(question.content); // היפוך מספרים בשאלה

    //        foreach (var answer in question.answersList)
    //        {
    //            answer.textContent = ReverseNumbersInText(answer.textContent); // היפוך מספרים בכל תשובה
    //        }

    //        //// אם רוצים לערבב תשובות בשלב זה, ניתן להוסיף כאן
    //        //question.answersList = RandomizeAnswersList(question.answersList);
    //    }
    //}


    public void ToggleSound()// פונקציה שמאפשרת למשתמש להפעיל או להשתיק את הסאונד
    {

        IsSoundMuted = !IsSoundMuted;// הפיכת מצב ההשתקה אם הסאונד מושתק הוא ישתנה ללא מושתק ולהפך

        if (IsSoundMuted)// אם הסאונד מושתק אז מורידים את את הדציבלים ל80 - כי זה הכי נמוך בשמע וזה מעלים את הסאונד 
        {
            mixer.SetFloat("Volume", -80);
        }
        else// אם הסאונד מופעל מגדירים את המיקסר לסאונד השמיעה שרצינו
        {
            mixer.SetFloat("Volume", -20);
        }

        // עדכון ויזואליות של הכפתור ההשתקה 
        ShowCorrectSoundBTN();
    }


    void ChangeSoundOn(bool on)//  פונקציה לשינוי מצב הסאונד  במשחק לפי הבוליאני שהתקבל 
    {
        if (on && IsSoundMuted) // אם הפרמטר שהתקבל הוא טרו והסאונד מושתק
        {
            ToggleSound();// קוראים לפונקציה כדי לשנות את מצב הסאונד
        }
        if (!on && !IsSoundMuted)// אם קיבלנו פרמטר פולס והסאונד  פעיל
        {
            ToggleSound();//
        }
    }


    private void ShowCorrectSoundBTN()// מעדכנת את התצוגה של הכפתור  לפי מצב של ההשתקה או לא השתקה 
    {
        soundOffImage.enabled = IsSoundMuted;// אם הסאונד מושתק מפעילים את התשוצה של התמונה המושתקת ומסתירים את  תמונת הסאונד הפעיל
        soundOnImage.enabled = !IsSoundMuted;// אם הסאונד פעיל מפעילים את תצוגת התמונה של הסאונד טהסתרת השניה
    }



    public void EnterCodeGameScreen()// מטפלת בהכנות הראשונות להצגה של מסך הקוד 
    {
        BgSoundForStart.SetActive(false);
        parkBG.color = new Color(parkBG.color.r, parkBG.color.g, parkBG.color.b,0);
        parkAnimationContainer.transform.localScale = parkAnimationContainerStartingScale;

        parkAnimator.Play("Empty");
        spinningWheel.Play("Empty"); 
        kidsAnim.Play("Empty");
        Flag.Play("Empty");
        chair2.Play("Empty");
        chair3.Play("Empty");
        chair4.Play("Empty");
        chair5.Play("Empty");
        Boy1InBarrowStart.Play("Empty");

        stopBTN.gameObject.SetActive(false);// כיבוי כפתור הסטופ של המשחק
        soundToggleButton.gameObject.SetActive(false); // הסתר את הכפתור לשליטה על הסאונד

        if (!IsSoundMuted)// אם הסאונד לא מושתק מפעילים את הפונקציה שמשנה את מצב הסאונד למצב הפוק
        {
            ToggleSound();
        }

        startCanvas.SetActive(true);// הפעלת קאנבס של תחילת המשחק
        StartScreen.SetActive(true);// הפעלת אלמנטים של תחילת המשחק

        GameInputFieldCodeTextStarte.text = "";// איפוס שדה הטקסט של קוד המשחק
        BTNGameCodeStart.interactable = false;   //שינוי צבע הכפתור הצבע לא יהיה פעיל עד שהמשתמש יזין תוכן

    }

    

    IEnumerator SwitchToChooseCharactersScreen()
        {
            startCanvas.SetActive(false);// סגירת הקאנבס של ההתחלה
            soundToggleButton.gameObject.SetActive(true); // הפעל את הכפתור לשליטה על הסאונד
            starttextabouve.SetActive(false);
            starttextabelow.SetActive(false);
            logo.SetActive(false);
            BgSoundForStart.SetActive(true);
            ChangeSoundOn(true);//מפעיל את הסאונד במי;דה והיה מושתק
            musicAudioSource.Play();// מתחיל לנגן את המוזיקה ברקע

            //ChoosePlayers.SetActive(false);// הפעלת מסך בחירת דמויות
            //ChoosePlayersCanvas.SetActive(false);// הפעלת קאנבס של בחירת דמויות

            parkAnimationContainer.LeanScale(Vector3.one, 1f);

            parkAnimator.Play("StartScreenAnim");
            spinningWheel.Play("Spin");
            kidsAnim.Play("StartboyInBarrow");
            Flag.Play("FlagMove");
            chair2.Play("Chair2");
            chair3.Play("Chair3");
            chair4.Play("Chair4");
            chair5.Play("Chair5");
            Boy1InBarrowStart.Play("Boy1InBarrowStartNew");


        yield return new WaitForSeconds(4f);

       

            parkAnimationContainer.LeanScale(Vector3.one * 2.5f, 2f);

            SFXAudioSource.PlayOneShot(ChoosePlayerSound, 4f);// הפעלת מוזיקת רקע בלחיצה על אחד השחקנים

        yield return new WaitForSeconds(4f);

        OnEvent e = new OnEvent(() => { 
                StartScreen.SetActive(false);// סגירת המסך של ההתחלה 
                ChoosePlayerScreen();// הפעלת פונקציה שמתחילה מסך בחירת דמויות
            });
            StartCoroutine( FadeOut(e));


    }

    void ChoosePlayerScreen()
    {
        StartChoosePlayerScreen();
        StartCoroutine( FadeIn(new OnEvent(() => { })));
    }
    void StartChoosePlayerScreen()// התחלת מסך בחירת דמויות
    {
        //soundToggleButton.gameObject.SetActive(true); // הפעל את הכפתור לשליטה על הסאונד

        //ChangeSoundOn(true);//מפעיל את הסאונד במידה והיה מושתק

        //musicAudioSource.Play();// מתחיל לנגן את המוזיקה ברקע

        ChoosePlayers.SetActive(true);// הפעלת מסך בחירת דמויות
        ChoosePlayersCanvas.SetActive(true);// הפעלת קאנבס של בחירת דמויות
        BTNChoosePlayers.interactable = false;//  הכפתור לבחירת דמויות יהיה לא פעיל עד שהמשתמש ילחץ על כל הנתונים הדרושים
        TheNumberOfTheStartingPlayer.text = "שחקן ראשון";// טקסט שרשום מי מוגדר כשחקן ראשון או שני שמזינים נתונים



    }

    void ChangeButtonCharacterSprite(Button button, Sprite sprite)// מתאימה את גודל הכפתור לגודל של הדמות
    {
        button.image.sprite = sprite;// שינוי התמונה שמוצגת על הכפתור
        //button.image.rectTransform.sizeDelta = button.image.sprite.textureRect.size;// שינוי הגודל 
        button.image.rectTransform.sizeDelta = button.image.sprite.textureRect.size;// שינוי הגודל 
    }

    public void ChooseCharacter() //פונקציה של בחירת דמויות לשני השחקנים
    {
        Button Character = CharactersButton.Find(Character => Character.gameObject.name.Contains(chosenCharacterName)); // עוברים על כל אחד מהכפתורים של השחקנים ומחפשים את שם הדמות שנלחצה 
        Character.interactable = false; // משביתים אותו כדי שלא יוכלו ללחוץ עליו שוב

        ChangeButtonCharacterSprite(Character, characters.Find(character => character.name == chosenCharacterName).emotions.Find(e => e.emotion == CharacterStates.StandRegularForStart).sprite); // משנה את הספרייטי של השחקן שיהיה תואם לספריט של הדמות במסך הגרלה

        if (player1 == null) // אם שחקן 1 עדיין לא נבחר
        {
            player1 = CreateCharacter(); //שומר את המידע כשחקן 1
            player1.Init(timerTextPlayer1, ThrowBallP1, answerBallRed.sprite); //מופע של הקלאס  שולחים נתונים ספציפים של שחקן 1 שדרושים למהלך המשחק כמו השם צבע הכדור והטיימר
            chosenCharacterName = ""; //מנקה את השם של הכפתור שנלחץ
            TheNumberOfTheStartingPlayer.text = "שחקן שני"; // טקסט שאומר שעכשיו התור של שחקן 2
            BTNChoosePlayers.interactable = false; // מחזירים את הכפתור למצב לא פעיל
            ChoosePlayerInputField.text = string.Empty; // מנקה את תיבת הטקסט
        }
        else if (player2 == null) // אם השחקן השני מוגדר כנול
        {
            if (ChoosePlayerInputField.text == player1.playerName) // בודק אם השם שנבחר זהה לשם של השחקן הקודם
            {
                TextErrorChoosePlayers.gameObject.SetActive(true); // מציג שגיאה
                Character.interactable = true; // מחזיר את הכפתור של הדמות למצב פעיל
                ChoosePlayerInputField.text = string.Empty; // מנקה את התיבת טקסט
                return; //לא ממשיך את הפונקציה כדי שהמשתמש יזין שוב נתונים
            }

            // אם הזין שם שונה
            player2 = CreateCharacter(); //שומר את המידע כשחקן 2
            player2.Init(timerTextPlayer2, ThrowBallP2, answerBallBlue.sprite); //מופע של הקלאס  שולחים נתונים ספציפים של שחקן2 שדרושים למהלך המשחק כמו השם צבע הכדור והטיימר
            BTNChoosePlayers.GetComponentInChildren<TextMeshProUGUI>().text = "להגרלה"; // משנים את טקסט הכפתור
        }

        checkClickChooseCarecter = false; // מבטלים את הכפתור
        if (player1 != null && player2 != null) // אם שני השחקנים נבחרו מעבירים עם אותו כפתור את השחקנים להגרלה
        {
            StartParticipantLotteryScreen(); // התחלת ההגרלה
        }
    }

   

    // פונקציה לבדיקת הקלט
    private void CheckInput(string input)
    {
        if (input.StartsWith(" "))
        {
            // אם התו הראשון הוא רווח, הצגת שגיאה והסרת הרווח
            ChoosePlayerInputField.text = input.TrimStart();
            TextErrorChoosePlayersSpase.gameObject.SetActive(true);
            TextErrorChoosePlayers.SetActive(false);

        }
        else
        {
            TextErrorChoosePlayersSpase.gameObject.SetActive(false);
        }
    }

    void StartParticipantLotteryScreen()// התחלת מסך הגרלה
    {
        ChoosePlayers.SetActive(false);// כיבוי מסך של בחירת דמויות
        ChoosePlayersCanvas.SetActive(false);// כיבוי קאנבס של בחירת דמויות
        ParticipantLotteryScreen.SetActive(true);// הפעלת מסך של הגרלה
        RandomWheelCanvas.SetActive(true);// הפעלת הקאנבס 


        //עדכון המיקום של השחקים על מסך ההגרלה לפי אובייקט ריק שמיקמנו ביוניטי
        player1.transform.SetParent(PositionPlayer1.transform);
        player2.transform.SetParent(PositionPlayer2.transform);
        player1.transform.position = new Vector3(PositionPlayer1.transform.position.x, PositionPlayer1.transform.position.y, player1.transform.position.z);
        player2.transform.position = new Vector3(PositionPlayer2.transform.position.x, PositionPlayer2.transform.position.y, player2.transform.position.z);
        player1.ChangeEmotion(CharacterStates.StandRegular);//!!!!שינוי מצב הדמות
        player2.ChangeEmotion(CharacterStates.StandRegular);//!!!!שינוי מצב הדמות
        GenerateWheelNames(player1);

    }


    //יצירת הגרלת השחקנים בצורה ויזואלית
    void GenerateWheelNames(CharactersScript currentCharacter) // הפונקציה מחשבת את המיקום החדש של הטקסט בהתאם לזווית ומייצרת אובייקט חדש של טקסט שימוקם במיקום שחושב
    {
        for (int i = 0; i < 8; i++)// עוברים על 8 נקודות סביב הגלגל
        {
            // חישוב של מיקום השם הנוכחי על הגלגל פתחנו מחלקה למטה עם פונקציה שמחשבת את המרחק בעיגול
            Vector2 offset = Circle.AngleToPoint((float)(i * 45));
            // ממירים את זה לוקטור 3 כדי שנוכל להשתמש ביוניטי
            Vector3 offset3 = new Vector3(offset.x, offset.y, 0);

            // יצירת האובייקט עם הטקסט במיקום של החישוב
            //  מסובבים אותו בהתאם לזווית שהוא נדרש להיות בה במעגל
            TextMeshProUGUI text = Instantiate(wheelSliceText, BTNRandomWheel.transform.position + offset3, Quaternion.Euler(0, 0, i * 45), BTNRandomWheel.transform);

            wheelTexts.Add(text);// הוספת השם לרשימת השמות

            if (currentCharacter == player1)  // תנאי שבודק איזה שחקן כרגע והשמת השם שלו בטקסט
            {
                text.text = currentCharacter.playerName;// הכנסת שם שחקן 1
                currentCharacter = player2;//  מעבר בין השחקנים
                continue;// מדלג לשלב הבא בלולאה לא ממשיך הלאה
            }
            if (currentCharacter == player2)
            {
                text.text = currentCharacter.playerName;// הכנסת שם שחקן 2
                currentCharacter = player1;//  מעבר בין השחקנים
                continue;// מדלג לשלב הבא בלולאה לא ממשיך הלאה
            }

        }
    }


	// פונקציה להתחלת סיבוב הגלגל ולבדיקת התוצאה
	public void startRandomWheel()
	{
		// הפעלת סאונד רקע של הגרלה
		SFXAudioSource.PlayOneShot(DramsClipWill, 10f);

		// הגדרת סך הסיבובים ומשך הזמן לסיבוב
		float spinDuration = 5f; // סה"כ זמן הסיבוב בחמש שניות
		int totalRotations = 5; // מספר סיבובים מלאים
		float finalAngle = UnityEngine.Random.Range(0, 360); // זווית אקראית לעצירה
		float totalAngle = (totalRotations * 360) + finalAngle;

		// סיבוב הגלגל
		LeanTween.rotateZ(BTNRandomWheel.gameObject, totalAngle, spinDuration)
			.setEaseInOutQuad()
			.setOnComplete(() =>
			{
				// קביעת המנצח
				DetermineWheelWinner();
			});
	}

	// פונקציה לקביעת המנצח לאחר עצירת הגלגל
	private void DetermineWheelWinner()
	{
		// יצירת מערך של Collider2D שיכול להכיל עד 10 תוצאות
		Collider2D[] results = new Collider2D[10];

		// בדיקת הקוליידרים בהם החץ נגע
		arrowWheelCollider.OverlapCollider(new ContactFilter2D() { useTriggers = true }, results);

		for (int i = 0; i < results.Length; i++)
		{
			if (results[i] != null && results[i].gameObject.TryGetComponent(out TextMeshProUGUI weelTextNames))
			{
				// שיוך השם למי שמתחיל ראשון
				playerTurn = GetPlayerByName(weelTextNames.text);

				// הצגת שם השחקן המנצח
				WheelWinnerTitle.text = weelTextNames.text;

				// שינוי מצב הדמות
				playerTurn.ChangeEmotion(CharacterStates.StandHappy);

				// עצירת סאונד רקע והפעלת סאונד ניצחון
				SFXAudioSource.Stop();
				SFXAudioSource.PlayOneShot(victoryClipWillWin, 10f);

				// הפעלת הקונפטי
				Confety.Play();

				// הפיכת הכפתור ללא לחיץ
				BTNRandomWheel.interactable = false;

				// הפעלת פונקציה להתחלת המשחק לאחר זמן קצוב
				StartCoroutine(StartGameAfterDelay());
				return;
			}
		}

		// אם לא נמצא מנצח, להתחיל שוב
		startRandomWheel();
	}

	CharactersScript GetPlayerByName(string name) // מחזירה את המופעה של השחקן בהתאם לשם שהוזן בגלגל ונבחר
    {
        if (player1.playerName == name)// בודקים אם שם השחקן הראשון תואם לשם שהתקבל
        {
            return player1; // אם יש התאמה היא מחזירה את האובייקט של שחקן 1 אם כל הפרטים שלו
        }
        if (player2.playerName == name)// בדיקה האם השחקן השני תואם לשם שהתקבל
        {
            return player2;//  אם יש התאמה היא מחזירה את האובייקט של שחקן 2 אם כל הפרטים שלו
        }
        UnityEngine.Debug.LogError("couldnot find any player that matches the name " + name);
        return null;//אם לא הוחזר שחקן עם שם שהוזן  מחזירים נול כדי להפעיל את הגלגל שוב

    }

    IEnumerator StartGameAfterDelay()//הפעלת פונקציה שמחכה כמה שניות
    {
        yield return new WaitForSeconds(2.5f); // ממתין שתיים וחצי שניות
        startGamAndQuestions(); // קורא לפונקציה לאחר ההשהייה
    }

    CharactersScript CreateCharacter()// בלחיצה על דמות יוצרת דמות חדשה במשחק
    {

        CharactersScript player = Instantiate(characterPF);//  characterPFיוצרים מופע חדש של פריפאב שמוגדר כ 
        List<VisualEmotion> emotionsToAssign = new List<VisualEmotion>();// מכין רשימה שלתוכה מכניס את כל ההבעות השונות שיבצרו

        for (int i = 0; i < characters.Count; i++) // עובר על כל הדמויות שמוגדרות במערכת כדי למצוא את זו שנבחרה
        {
            if (characters[i].name == chosenCharacterName)//מחפשים את הדמות שנלחצה 
            {
                emotionsToAssign = characters[i].emotions;// מציב במשתנה את רשימת ההבעות של הדמות שנבחרה
                Debug.Log("found the chosen character");
                break; // יוצא מהלולאה לאחר מציאת הדמות המתאימה
            }

        }

        player.CreationPlayers(ChoosePlayerInputField.text, emotionsToAssign);// לוקח את שם השחקן והרשימה שיצרנו של המצבים של הדמות ושולח לפונקציה
        return player;// מחזיר את הדמות שנוצרה
    }

    //מופעלת כאשר יש בחירת דמות  מעדכנת שבוצעה בחיר ומפעילה מוזיקה בהתאם ומשנה את מצב הדמות שנלחצה
    public void ChangeChosenname(string name) //שולח את שם הכפתור של השחקן שנלחץ
    {
        SFXAudioSource.PlayOneShot(ChoosePlayerSound, 10f);// הפעלת מוזיקת רקע בלחיצה על אחד השחקנים
        checkClickChooseCarecter = true;//מסמנים שבוצעה לחיצה על דמות
        chosenCharacterName = name;// עדכון של שם הדמות שנבחרה
        // עוברם על כל הכפתורים שמייצגים דמויות
        CharactersButton.ForEach(b =>
        {
            // בודקים אם שם הכפתור מכיל את שם השמות שנבחרה
            if (b.gameObject.name.Contains(chosenCharacterName))
            {
                // משנה את ספרייט הכפתור להבעת הפנים ה שמחה של הדמות הנבחרת
                ChangeButtonCharacterSprite(b, b.image.sprite = characters.Find(c => c.name == chosenCharacterName).emotions.Find(emotion => emotion.emotion == CharacterStates.StandHappyForStart).sprite);
            }
            else
            {
                // לכל שאר הכפתורים, משנה את הספרייט להבעת הפנים ה רגילה
                ChangeButtonCharacterSprite(b, characters.Find(c => b.gameObject.name.Contains(c.name)).emotions.Find(emotion => emotion.emotion == CharacterStates.StandRegularForStart).sprite);
            }
        });//
    }

    void Update()// הפונקציה  מתבצעת בכל פריים ובודקת מצבים שונים במשחק


    {
        if (GameInputFieldCodeTextStarte.text != "")// בודקים אם השדה של הזנת קוד לא ריק
        {

            BTNGameCodeStart.interactable = true; // משנים את הכפתור לפעיל

        }
        else// אם לא
        {
            BTNGameCodeStart.interactable = false;// משנים את הכפתור ללא פעיל
        }

        if (checkClickChooseCarecter && ChoosePlayerInputField.text != "")// בודק אם נבחרה דמות והשחקן הזין טקסט
        {

            BTNChoosePlayers.interactable = true;// מפעיל כפתור
        }
        else// אם לא מכבה כפתור
        {
            BTNChoosePlayers.interactable = false;

        }


        if (questionTimer.IsRunning)// ספירה לאחור של הזמן של המשתמש
        {

            print("timer active");
            playerTurn.timerText.text = FormatTime(Mathf.RoundToInt(game.questionTime - (float)questionTimer.QuestionTime));

            //playerTurn.timerText.text = FormatTime((int)questionTime+1);// עדכון ויזואלי של טקסט הזמן לשחקן הנוכחי


        }

    }

    private void OnValidate()// מתרחש תמיד גם אם לא הפעלנו את המשחק
    {
        if (game.gameName.Length > 10)// יפעל אם אתחיל להקליד את התו אקסטרה
        {
            char[] content = game.gameName.ToCharArray();// הופך לרשימה של תוים
            List<char> contentList = content.ToList();//הופכת את התוים לרשימה חדשה
            contentList.RemoveRange(game.gameName.Length - game.gameName.Length % 10, game.gameName.Length % 10);//מוריד את השארית של הרשימה 
            game.gameName = new string(contentList.ToArray());//יש סטרינג חדש ומכניסים את זה לשם משחק
        }

        // הגדרת אורך מקסימלי לשאלות והתשובות
        game.questionList.ForEach(question =>
        {
            if (question.content.Length > 75)// אם אורך השאלה גדול מ76 תוים
            {
                char[] content = question.content.ToCharArray(); // הופך לרשימה של תוים
                List<char> contentList = content.ToList();// הופך את התוים לרשימה חדשה
                contentList.RemoveRange(question.content.Length - question.content.Length % 75, question.content.Length % 75);// מוריד את השארית של הרשימה
                question.content = new string(contentList.ToArray()); // עדכון תוכן השאלה
            }

            // אותו דבר פה על התשובות
            question.answersList.ForEach(answer =>
            {
                if (answer.textContent.Length > 25)
                {
                    char[] content = answer.textContent.ToCharArray();
                    List<char> contentList = content.ToList();
                    contentList.RemoveRange(answer.textContent.Length - answer.textContent.Length % 25, answer.textContent.Length % 25);
                    answer.textContent = new string(contentList.ToArray());
                }
            });
        });
    }

    void startGamAndQuestions()// פונקצייה שמתחילה את מסך השאלות
    {
        nextQuesBTN.gameObject.SetActive(false);// העלמת כפתור שאלה הבאה אם במקרה פעיל 
        Confety.Stop();
		SplashWhenWrong2GameObject.SetActive(false);
        SplashWhenWrong1GameObject.SetActive(false);

        stopBTN.gameObject.SetActive(true);// הפעלת כפתור עצירה
        //musicAudioSource.Play();
        ParticipantLotteryScreen.SetActive(false);// סגירת מסך ההגרלה
        RandomWheelCanvas.SetActive(false);// סגירת הקאנבס של ההגרלה
        GameQuestionsCanvas.SetActive(true);// פתיחת קאנבס השאלות
        GameQuestionsScreen.SetActive(true);// פתיחת מסך שאלות
        standPlayer1.SetActive(true);// הופעת מתקן של שחקן 1
        standAnimator1.runtimeAnimatorController = questionAminController;
        standPlayer2.SetActive(true);// הופעת מתקן של שחקן 2
        standAnimator2.runtimeAnimatorController = questionAminController;

        player1.ChangeEmotion(CharacterStates.SittingRegular);//!!!!שינוי מצב הדמות 
        player2.ChangeEmotion(CharacterStates.SittingRegular);//!!!!שינוי מצב הדמות
        // מיקום הדמויות שישבו מעל המתקן
        player1.transform.position = new Vector3(PlaceForPlayer1.transform.position.x, PlaceForPlayer1.transform.position.y, player1.transform.position.z);
        player1.transform.SetParent(PlaceForPlayer1.transform);
        player2.transform.position = new Vector3(PlaceForPlayer2.transform.position.x, PlaceForPlayer2.transform.position.y, player2.transform.position.z);
        player2.transform.SetParent(PlaceForPlayer2.transform);
        chairStand1.transform.position = chair1StartingPlace;
        chairStand2.transform.position = chair2StartingPlace;
        ChairStick1.transform.SetParent(chairStand1.transform);
        ChairStick2.transform.SetParent(chairStand2.transform);
        // הוספת שמות השחקנים על המתקן
        NamePlayer2.text = player2.playerName;
        NamePlayer1.text = player1.playerName;

        CreateBalls();// פונקציה שמייצרת כדורים כדי  לעקוב אחרי מצב התשובות של המשתמש
        HandleVisualOfCurrentTurn(); // פונקציה שמשנה את המצב הויזואלי של השחקנים בהצאם לתור
        CreateQuestion(); // פונקציה שיוצרת שאלות

    }

    void CreateBalls()// פונקציה מנקה את כל הכדורים שקיימים  ואז מייצרת כדורים לפי מספר השאלות הקיימות
    {
        for (int i = activeAnswerBalls.Count - 1; i >= 0; i--)// לולאה שרצה על הרשימה של הכדורים הפעילים מהסוף אל ההתחלה
        {
            Image currentBall = activeAnswerBalls[i];// מייצר כדור ושומר אותו במשתנה
            activeAnswerBalls.Remove(currentBall);// מסיר את הכדור הנוכחי מהרשימה
            Destroy(currentBall.gameObject); // מוחקים את הכדור מהסצנה ביוניטי
        }
        // יוצרת כדור חדש לכל שאלה ברשימת השאלות
        game.questionList.ForEach(que => activeAnswerBalls.Add(Instantiate(answerBallGray, answersBallsGrid)));
    }

    private void HandleVisualOfCurrentTurn()// מעדכנת אלמנטים וויזואלים שקשורים לתור הנוכחי במשחק
    {
        SetupTurnVisuals(playerTurn == player1);// בודק בצורה בוליאנית אם תור המשחק הוא של שחקן ראשון
        ChangeCharacterEmotions();// פונקציה שמשנה מצב לדמות
    }

    private void SetupTurnVisuals(bool isPlayerOneTurn)// מגדירה את הויזואליזציות של המשחק לפי תור השחקן הנוכחי

    {
        // מפעיל את הכדור בהתאם לתור השחקן
        ThrowBallP1.gameObject.SetActive(isPlayerOneTurn);
        ThrowBallP2.gameObject.SetActive(!isPlayerOneTurn);

        //GameObject activeBall = isPlayerOneTurn ? ThrowBallP1 : ThrowBallP2;

        GameObject activeBall; // יוצר משתנה לכדור הפעיל ומגדיר אותו לפי התור הנוכחי

        if (isPlayerOneTurn)// בוחרים איזה כדור להציג על פי התור הנוכחי
        {
            activeBall = ThrowBallP1;
        }
        else
        {
            activeBall = ThrowBallP2;
        }

        activeBall.transform.position = ThrowBallStartingPos.position;// מזיזים את הכדור למקום ההתחלתי

        // הפעלת וכיבוי הטיימרים לפי התור

        if (game.questionTime > 0)
        {
            timerTextPlayer1.gameObject.SetActive(isPlayerOneTurn);
            timerTextPlayer2.gameObject.SetActive(!isPlayerOneTurn);

            timerSpotPlayer1.SetActive(isPlayerOneTurn);
            timerSpotPlayer2.SetActive(!isPlayerOneTurn);
        }

        
            // הגדרת נראות הדמויות
            if (isPlayerOneTurn)
            {
                SetCharacterVisuals(player2, true);
            }
            else
            {
                SetCharacterVisuals(player1, false);
            }
        
      
    }


    private void SetCharacterVisuals(CharactersScript otherPlayer, bool isPlayerOneTurn)// אחראית למראה השחקנים
    {
        SplashWhenWrong1GameObject.SetActive(false);
        SplashWhenWrong2GameObject.SetActive(false);


        SetCharacterToGray(otherPlayer.gameObject); // משנה את הדמות שלא משחקת לאפור
        GameObject StickStand;//מקל
        GameObject chairStand;// כיסא
        GameObject chairStic;// מקל כיסא
        GameObject bathStand;// כיסא
        GameObject SplashWhenWrongGameObject;// אנימציית ספלאש
        //GameObject otherStand; // לוקחת את הסטנד של השחקן שלא משחק כדי לשנות את הצבע שלו
        GameObject otherWoodenSign; // לוקחת את השלט עץ של השחקן שלא משחק כדי לשנות את הצבע שלו
        //GameObject currentStand; // לוקחת את הסטנד של השחקן שמשחק כרגע כדי לשנות את הצבע שלו
        GameObject currentWoodenSign;//לוקח את השלט של השחקן שלא משחק 
        CharactersScript currentPlayer; // מגדיר את השחקן הנוכחי שמשחק
        GameObject currentStickStand;//מקל
        GameObject currentchairStand;// כיסא
        GameObject currentchairstic;// מקל כיסא
        GameObject currentbathStand;// כיסא
        GameObject currentSplashTransparent;// 
        GameObject currentSplashWhenWrongGameObject;// 
        GameObject splashTransparent;// 
;// כיסא

        if (isPlayerOneTurn)
        {
            currentSplashTransparent = splastransperent1;
            splashTransparent = splastransperent2;
            StickStand = StickStand2;
            currentStickStand = StickStand1;
            chairStand= chairStand2;
            currentchairStand = chairStand1;
            bathStand=bathStand2;
            currentbathStand = bathStand1;
            chairStic = ChairStick2;
            currentchairstic = ChairStick1;
            SplashWhenWrongGameObject = SplashWhenWrong2GameObject;
            currentSplashWhenWrongGameObject = SplashWhenWrong1GameObject;
            //otherStand = standPlayer2; // אם זה תור שחקן 1, הסטנד האחר יהיה של שחקן 2
            otherWoodenSign = woodenSignP2;
            currentPlayer = player1; // אם זה תור שחקן 1, השחקן הנוכחי הוא שחקן 1
            currentWoodenSign = woodenSignP1;
            //currentStand = standPlayer1; // אם זה תור שחקן 1, הסטנד הנוכחי הוא של שחקן 1
        }
        else
        {
            currentSplashTransparent =splastransperent2;
            splashTransparent =splastransperent1;

            StickStand = StickStand1;
            currentStickStand = StickStand2;
            chairStand = chairStand1;
            currentchairStand = chairStand2;
            bathStand = bathStand1;
            currentbathStand = bathStand2;
            chairStic = ChairStick1;
            currentchairstic = ChairStick2;
            SplashWhenWrongGameObject = SplashWhenWrong1GameObject;
            currentSplashWhenWrongGameObject = SplashWhenWrong2GameObject;

            //otherStand = standPlayer1; // אם לא, הסטנד האחר יהיה של שחקן 1
            otherWoodenSign = woodenSignP1;
            currentPlayer = player2; // אם לא, השחקן הנוכחי הוא שחקן 2
            currentWoodenSign = woodenSignP2;
            //currentStand = standPlayer2; // אם לא, הסטנד הנוכחי הוא של שחקן 2
        }

        //SetCharacterToGray(otherStand); // משנה את הסטנד של הדמות שלא משחקת לאפור
        SetCharacterToGray(chairStand); 
        SetCharacterToGray(StickStand); 
        SetCharacterToGray(bathStand); 
        SetCharacterToGray(otherWoodenSign); // משנה את השלט עץ של הדמות שלא משחקת לאפור
        SetCharacterToGray(chairStic);
        SetCharacterToOriginalColor(splashTransparent, new Color(0.5f, 0.5f, 0.5f, splashTransparent.GetComponent<SpriteRenderer>().color.a));
        SetCharacterToOriginalColor(SplashWhenWrongGameObject, new Color(0.5f, 0.5f, 0.5f, SplashWhenWrongGameObject.GetComponent<SpriteRenderer>().color.a)); 
        SetCharacterToOriginalColor(currentPlayer.gameObject, Color.white); // משנה את הדמות שמשחקת למצב רגיל
        //SetCharacterToOriginalColor(currentStand, Color.white); // משנה את הסטנד של הדמות שמשחקת למצב רגיל
        SetCharacterToOriginalColor(currentchairStand, Color.white); 
        SetCharacterToOriginalColor(currentStickStand, Color.white); 
        SetCharacterToOriginalColor(currentbathStand, Color.white); 
        SetCharacterToOriginalColor(currentWoodenSign, Color.white); // משנה את השלט עץ של הדמות שמשחקת למצב רגיל
        SetCharacterToOriginalColor(currentchairstic, Color.white); 
        SetCharacterToOriginalColor(currentSplashTransparent, new Color(1f,1f,1f, currentSplashTransparent.GetComponent<SpriteRenderer>().color.a)); 
        SetCharacterToOriginalColor(currentSplashWhenWrongGameObject, new Color(1, 1, 1, currentSplashWhenWrongGameObject.GetComponent<SpriteRenderer>().color.a)); 
    }


    private void ChangeCharacterEmotions()// משנים את המצב של הדמות
    {
        player1.ChangeEmotion(CharacterStates.SittingRegular);// מחזירים את הדמויות למצב ישיבה רגיל
        player2.ChangeEmotion(CharacterStates.SittingRegular);

        // ניקוי הבועות דיבור וכיבוי שלהן
        SpeechBubble1.SetActive(false);
        SpeechBubble2.SetActive(false);
        TextBubble1.text = "";
        TextBubble2.text = "";
    }


    public void SetCharacterToGray(GameObject character)// פונקציה שמשנה את הצבע של האובייקט לגוון אפור
    {
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray; //  לקבלת הצבע אפור
    }

    // ואם את רוצה לשנות חזרה לצבע מקורי
    public void SetCharacterToOriginalColor(GameObject character, Color originalColor)// פונקציה שמחזירה את הצבע של המשתמש לגוון רגיך
    {
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        spriteRenderer.color = originalColor; // הצבע המקורי של הדמות
    }




    [System.Serializable]//מאפשר לנו להזים את הנתונים מהממשק של היוניטי
    public class CharacterData// מגדירים את המצבים של הדמות והשם שלהם מתוך היוניטי באמצעות המחלקה הזאת
    {
        public string name;
        public List<VisualEmotion> emotions;
    }
    void ClearPreviousAnswers()
    {
        foreach (var group in answersLayoutGroups)
        {
            Destroy(group.gameObject);
        }
        answersLayoutGroups.Clear();
        answerOnScreen.Clear();
    }

   
    private void CreateQuestion() // יצירת שאלה
    {
        ClearPreviousAnswers();

        questionNumber = 0; // איפוס מספר השאלה

        game.questionList = RandomizeQuestions(game.questionList); // ערבוב רשימת השאלות בצורה אקראית

        // מציאת השאלה שנענתה בצורה שגויה והעברתה לסוף הרשימה
        for (int i = 0; i < game.questionList.Count; i++)
        {
            if (game.questionList[i].wrongUser)
            {
                QuestionData wrongQuestion = game.questionList[i];
                game.questionList.RemoveAt(i);
                game.questionList.Add(wrongQuestion);
                game.questionList[game.questionList.Count - 1].wrongUser = false;
                break;
            }
        }

        TitleGameSub.text = game.gameName; // כותרת של נושא המשחק
        QuestionImage.gameObject.transform.localScale = originalScale;

        // מציאת השאלה הראשונה שלא נענתה
        for (int i = 0; i < game.questionList.Count; i++)
        {
            if (!game.questionList[i].isAnswered)
            {
                questionNumber = i;
                break;
            }
        }

        QuestionData currentQuestion = game.questionList[questionNumber]; // הבאת השאלה הנוכחית מתוך רשימת השאלות

        if (currentQuestion.imageContent != null) // בדיקה אם יש תמונה
        {
            questionImagePanel.SetActive(true); // אם כן אז פתיחת הפאנל של התמונה
            QuestionImage.sprite = currentQuestion.imageContent; // הצגת התמונה של השאלה
        }
        else
        {
            questionImagePanel.SetActive(false); // הסתרת התמונה וזכוכית המגדלת אם אין תמונה
        }

        questionText.text = currentQuestion.content; // הצגת שאלה במסך
        currentQuestion.answersList = RandomizeAnswersList(currentQuestion.answersList); // ערבוב התשובות בצורה רנדומלית

        // בחירת איך להציג את התשובות בהתאם לכמותן
        if (currentQuestion.answersList.Count == 4) // אם יש רק 4 תשובות
        {
            CreateAnswersGrid(currentQuestion, 2); // שליחה לפונקציה את כמות התשובות שצריך ליצור בשורה וגם את השאלה הנוכחית
        }
        else
        {
            CreateAnswersGrid(currentQuestion, 3);
        }

        
            Debug.Log("Resuming game: Starting question timer."); // Log when resuming the game

            OnEvent e = new OnEvent(
                    delegate
                    {
                        //Console.("Question timer ended."); // Log when question timer ends
                        AnswerTimeUp();
                        questionImagePanel.SetActive(false);// הזזת התמונות שלא 
                        timerSpotPlayer1.SetActive(false);//כיבוי של מקום לשעון 
                        timerSpotPlayer2.SetActive(false);//כיבוי של מקום לשעון 
                        stopBTNAsBtn.interactable = false;
                    }); // שולח לפונקציה של סיון זמן בשאלה

            if (game.questionTime > 0)
                questionTimer.Start(game.questionTime, e);
            else
                questionTimer.ResetTimer();

        endingQuestion = false;
    }




    List<HorizontalLayoutGroup> answersLayoutGroups = new List<HorizontalLayoutGroup>();//  רשימה ששומרת את התשובות  כדי שנוכל למחוק אותן בעתיד כדי להתחיל מחדש 
    void CreateAnswersGrid(QuestionData currentQuestion, int rowSize)// יצירת הגריד של התשובות בהתאם לכמות התשובות שצריכות להיות בשורה
    {
        HorizontalLayoutGroup currentGroup = null;// מגדירים את הרשימה ריקה
        for (int i = 0; i < currentQuestion.answersList.Count; i++)// עוברים על הליסט של התשובות 
        {
            if (i % rowSize == 0)// בדיקה אם הגענו לכמות התשובות שאמורה להיות בשורה חדשה 
            {

                currentGroup = Instantiate(answersGridGroupPrefab, answersGrid);// אם הגענו לכמות התשובות בשורה יוצרים שורה עבור התשובות
                answersLayoutGroups.Add(currentGroup);//  הוספת העותק של של השורות לרשימה של השורות כדי שכל פעם מחדש נוכל למחוק אותן שלא יווצרו כפילויות 
            }
            CreateAnswer(currentQuestion.answersList[i], currentGroup.gameObject);//קריאה לפונקציה שיוצרת תשובה בודדת הפונקציה מקבלת את הנתונים של התשובה ואת האובייקט הגדול שמנהל אותה שאליו מוסיפים את התשובה
        }

    }

    void CreateAnswer(AnswerData answerData, GameObject parent)//  פונקציה שיוצרת תשובות  
    {
        //// הפוך מספרים בטקסט של התשובה לפני האתחול
        //answerData.textContent = ReverseNumbersInText(answerData.textContent);

        AnswerBoxScript newAns = Instantiate(AnswerScript, parent.transform); //יצירת תשובה והכנסת למסך התשובות
        newAns.Init(this, answerData);//אתחול האלמנט של תשובה ספציפית על כל מה שאמור להיות שם
        answerOnScreen.Add(newAns);// מוסיפים את התשובה שנוצרה לרשימת התשובות במסך 

    }



    void DestroyAnswers()// פונקציה זו עוברת על רשימת התשובות שמוצגות על המסך, מסירה כל אחת מהן מהרשימה ולאחר מכן הורסת את האובייקט הגרפי גם
    {
        // מעבר בלולאה על רשימת התשובות מהסוף להתחלה

        for (int i = answerOnScreen.Count - 1; i >= 0; i--)
        {
            AnswerBoxScript answerBoxScript = answerOnScreen[i];// שמירת התשובה הנוכחית
            answerOnScreen.Remove(answerBoxScript);// מחיקת תשובה מהרשימה
            Destroy(answerBoxScript.gameObject);// השמדת האובייקטים של התשובות שיצרנו
        }
        for (int i = 0; i < answersLayoutGroups.Count; i++)
        {
            Destroy(answersLayoutGroups[i].gameObject);
        }
        answersLayoutGroups.Clear();

    }
    private List<QuestionData> RandomizeQuestions(List<QuestionData> originalQuestions) // פונקציה שיוצרת רשימה לשאלות שעוברות רנדו
    {
        List<QuestionData> randomizedQuestions = new List<QuestionData>(); // יצירת רשימת שאלות חדשה
        int count = originalQuestions.Count;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, originalQuestions.Count);

            // בחירת מיקום שאלה אקראית מהרשימה המקורית
            QuestionData randomQuestion = originalQuestions[randomIndex]; // שליפת השאלה
            randomizedQuestions.Add(randomQuestion); // הוספת השאלה לרשימת השאלות המעורבבות
            originalQuestions.RemoveAt(randomIndex); // מחיקת השאלה מהרשימה
        }

        return randomizedQuestions; // מחזירים רשימה חדשה מעורבבת
    }


    private List<AnswerData> RandomizeAnswersList(List<AnswerData> originalList) // מחזירה רשימה רנדומלית
    {
        List<AnswerData> randomList = new List<AnswerData>(); // יצירת רשימה חדשה לאחסון תשובות
        int count = originalList.Count;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, originalList.Count); // יוצר הגרלה באורך הרשימה
            AnswerData randomAnswer = originalList[randomIndex]; // שמירת הנתונים
            randomList.Add(randomAnswer); // הכנסת האובייקט למיקום החדש
            originalList.RemoveAt(randomIndex); // הסרת האובייקט מהרשימה הישנה
        }

        return randomList; // החזרה
    }



    

    bool answeredAnimIsPlaying = false;
    public void answerClicked(AnswerBoxScript chosenAnswer)// הפונקציה שמטפלת באירוע של לחיצה על  תשובה
    {
        answerOnScreen.ForEach(ans => ans.DisableAnsweringOption());// מנטרל את אפשרות התשובה לכל התשובות המוצגות על המסך
        answeredAnimIsPlaying = true;
        float timeElapsed = (float)questionTimer.QuestionTime;
        //לין טווין שמזיז את הכדור ליעד התשובה שנבחר יוצר את האנימציה
        lt = LeanTween.move(playerTurn.throwBall, Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(chosenAnswer.BallTarget.position)), 1f).setOnComplete(() =>
        {
            StartCoroutine(PresentAnswer(chosenAnswer, timeElapsed));// פונקציה שמפעילה את המשוב רק לאחר סיום האנימציה!!
            lt = null;// איפוס משתנה האנימציה
        });
        questionTimer.Stop();
    }

    //קורוטינות ביוניטי הן כלי  לביצוע פעולות שדורשות זמן, כמו השהיות או פעולות אסינכרוניות, באופן שלא יעצור את זרימת המשחק. הן מאפשרות לכתוב קוד שמתבצע במשך מספר פריימים או זמן מסוים, מבלי לגרום למשחק להיתקע או להיות חסר תגובה.

    // משמש ליצירת קורנטינות אשר מאפשר לבצע פעולות במשך זמן  

    private int index = 0;
    IEnumerator PresentAnswer(AnswerBoxScript chosenAnswer, float timePassed)// הגדרת קורוטינה שמציגה משוב על התשובה שנבחרה
    {
        yield return new WaitForSeconds(1f);// המתנה של שנייה אחת לפני המשך הפעולה

        if (chosenAnswer != null) // בדיקה אם נבחרה תשובהchosenAnswer
        {
            chosenAnswer.Feedback();// הפעלת משוב על התשובה שנבחרה

            playerTurn.AddScore(chosenAnswer.answerData.isCorrect, timePassed);// עדכון הניקוד 

            if (chosenAnswer.answerData.isCorrect)
            {
                GiveCorrectAnswer();// משוב לתשובה נכונה
            }
            else
            {
                GiveIncorrectAnswer();// משוב לתשובה לא נכונה
                
            }

        }

        playerTurn.throwBall.SetActive(false);// מעלים את הכדור שזורקים
        //answeredAnimIsPlaying = false;

        if (game.questionList.TrueForAll(q => q.isAnswered))// אם עברנו על כל השאלות וכולן נענו וקיבלו תשובה
        {
            stopBTNAsBtn.interactable = false;// הופכים את כפתור בפאוז ללא לחיץ
            yield return new WaitForSeconds(3);// מחכים 4 שניות ורק אז ממשיכים לשאר הפונקציות
            PrepareForEndGame();// הכנות למסך סיום שמפילים מישהו למים

        }
        else // אם יש עוד שאלות שלא נענו
        {
            yield return new WaitForSeconds(0.5f);// המתנה של שנייה אחת לפני המשך הפעולה

            nextQuesBTN.gameObject.SetActive(true);// הפעלת הכפתור לשאלה הבאה

        }

    }
    bool endingQuestion = false;
    void GiveIncorrectAnswer()// פידבק אם טועים בתשובה
    {
        playerTurn.ChangeEmotion(CharacterStates.SittingUnHappy);// השחקן שמשחק לא שמח
        game.questionList[questionNumber].wrongUser = true;// משנים את מצה התשובה לנענתה כדי שלא תחזור שוב לשחקנים

        if (playerTurn == player1)// משווים איזה תור זה של שחקן 1 או 2 ובהתאם יהה משוב
        {
            // אם זה שחקן 1
            SpeechBubble1.SetActive(true);// משנים את הבועת דיבור למצב פעיל
            TextBubble1.text = "אוף איזו טעות, ניפגש בסיבוב הבא";// המשוב של השחקן לטעות
            player1.ChangeEmotion(CharacterStates.SittingUnHappy);// מצב הדמות משתנה לעצובה
        }
        else // אותו דבר רק הפוך
        {
            SpeechBubble2.SetActive(true);
            TextBubble2.text = "אוף איזו טעות, ניפגש בסיבוב הבא";
            player2.ChangeEmotion(CharacterStates.SittingUnHappy);
        }
        answeredAnimIsPlaying = false;
        endingQuestion = true;
    }
    
    
    // הגדרת הזווית שהסטנד יזוז ימינה ושמאלה
    public float rotateAngle = 10.0f;

    // הגדרת הזמן שלוקח לזוז מצד לצד
    public float rotateTime = 0.4f;

    public void MoveStandCharacter(GameObject standObject, Animator SplashWhenWrong)
    {
        // שמירת הזווית ההתחלתית
        float originalAngle = standObject.transform.eulerAngles.z;

        Animator barrowAnim = standObject.GetComponent<Animator>();
        barrowAnim.Play("BarrowRotateStart");
        OnEvent onEndSplashAnim = new OnEvent(() =>
        {
            SplashWhenWrong1GameObject.SetActive(false);
            SplashWhenWrong2GameObject.SetActive(false);
            answeredAnimIsPlaying = false;
        });
        if (SplashWhenWrong == SplashWhenWrong1)
        {
            SplashWhenWrong1GameObject.SetActive(true);

            SplashWhenWrong1.Play("SplashWhenWrong");
            StartCoroutine(WaitForAnimToEnd(SplashWhenWrong1.GetCurrentAnimatorClipInfo(0)[0].clip.length, onEndSplashAnim));


        }
        else
        {
            SplashWhenWrong2GameObject.SetActive(true);

            SplashWhenWrong2.Play("SplashWhenWrong");
            StartCoroutine(WaitForAnimToEnd(SplashWhenWrong2.GetCurrentAnimatorClipInfo(0)[0].clip.length, onEndSplashAnim));

        }


        
    }

    IEnumerator WaitForAnimToEnd(float time, OnEvent e)
    {
        yield return new WaitForSeconds(time);
        e.Invoke();
    }

    //void StopParticleSystem()
    //{
    //    // עצירת ה- Particle System
    //    if (waterDrops.isPlaying)
    //    {
    //        waterDrops.Stop();
    //    }
    //}


    void GiveCorrectAnswer()// משוב אם יש תשובה כונה
    {
        game.questionList[questionNumber].isAnswered = true;// משנים את מצה התשובה לנענתה כדי שלא תחזור שוב לשחקנים
        GetBallToChange().sprite = playerTurn.answerBallSprite;// הופכים את אחד הכדורים למטה לפי הסדר לצבע של הדמות
        playerTurn.ChangeEmotion(CharacterStates.SittingHappy);// הדמות משנה את המצב שלה ויושבת שמחה
        if (playerTurn == player1)// בודקים אם שחקן 1 או 2 נצחו
        {
            MoveStandCharacter(chairStand2 , SplashWhenWrong2);

            // אם שחקן 1 ניצח
            player2.ChangeEmotion(CharacterStates.SittingUnHappy);// משנים את המצב של שחקן 2 לעוב
            SpeechBubble1.SetActive(true);// הפעלת הבועת דיבור של השחקן שניצח
            TextBubble1.text = "איזה כיף!  \n " + player2.playerName + ", " + "זהירות רק לא ליפול!";// משוב של שחקן 1
        }
        else// אותו דבר רק הפוך
        {
            MoveStandCharacter(chairStand1 , SplashWhenWrong1);

            player1.ChangeEmotion(CharacterStates.SittingUnHappy);
            SpeechBubble2.SetActive(true);
            TextBubble2.text = "איזה כיף!  \n" + player1.playerName + ", " + "זהירות רק לא ליפול!";/// לא 
        }
        endingQuestion = true;
    }
    void PrepareForEndGame()// הכנה למסך אנימציה העתידי שהשחקן שהפסיד נופל
    {
        SplashWhenWrong1GameObject.SetActive(false);
        SplashWhenWrong2GameObject.SetActive(false);

        SetCharacterToOriginalColor(standPlayer1, Color.white);// משנים את הצבע של הסטנדים לרגיל
        SetCharacterToOriginalColor(woodenSignP1, Color.white);// משנים את הצבע של הסטנדים לרגיל
        SetCharacterToOriginalColor(standPlayer2, Color.white);
        SetCharacterToOriginalColor(woodenSignP2, Color.white);
        SetCharacterToOriginalColor(player1.gameObject, Color.white);// משנים את הצבע של הסטנדים לרגיל
        SetCharacterToOriginalColor(player2.gameObject, Color.white);
        SetCharacterToOriginalColor(chairStand2, Color.white);
        SetCharacterToOriginalColor(chairStand1, Color.white);
        SetCharacterToOriginalColor(ChairStick2, Color.white);
        SetCharacterToOriginalColor(ChairStick1, Color.white);
        SetCharacterToOriginalColor(bathStand2, Color.white);
        SetCharacterToOriginalColor(bathStand1, Color.white);
        SetCharacterToOriginalColor(StickStand1, Color.white);
        SetCharacterToOriginalColor(StickStand2, Color.white);
        SetCharacterToOriginalColor(splastransperent1, new Color(1,1,1,0.4f));
        SetCharacterToOriginalColor(splastransperent2, new Color(1,1,1,0.4f));
        SetCharacterToOriginalColor(SplashWhenWrong1GameObject, new Color(1,1,1,0.4f));
        SetCharacterToOriginalColor(SplashWhenWrong2GameObject, new Color(1,1,1,0.4f));


        SpeechBubble2.SetActive(false);// סוגרים את הבועות דיבור
        SpeechBubble1.SetActive(false);
        MagnifyingGlassImage.SetActive(false);//סוגרים את הזכוכית מגדלת 

        TextToTheEnd.gameObject.SetActive(true);// מפעילים את טקסט השאלה לפני הנפילה למים

        timerSpotPlayer1.SetActive(false);// מכבים את המקום הטיימרים
        timerSpotPlayer2.SetActive(false);

        timerTextPlayer1.gameObject.SetActive(false);// מכבים את הטקסט של  הטיימרים
        timerTextPlayer2.gameObject.SetActive(false);

        questionImagePanel.gameObject.SetActive(false);// מכבים את השאלות


        TextBubble2.text = "";// מנקים את הטקסט במשובים בזמן המשחק
        TextBubble1.text = "";

        questionText.text = "מי המנצח שלנו ויזכה להישאר יבש?"; // מציגים שאלה על המסך
        player1.ChangeEmotion(CharacterStates.SittingUnHappy);// משנים את מצב הדמויות
        player2.ChangeEmotion(CharacterStates.SittingUnHappy);

        standAnimator1.runtimeAnimatorController = endingAnimatorController;
        standAnimator2.runtimeAnimatorController = endingAnimatorController;

        BtnToTheEnd.gameObject.SetActive(true);// הפעלת כפתור
        TextToTheEnd.text = "לחצו כדי להפיל את המפסיד למים";
        print("player 1 score:\n " + "name: " + player1.playerName + "\ntime: " + player1.time + "\ncorrect answers: " + player1.correctAnswers + "\nincorrect answers: " + player1.incorrectAnswers + "\n\n" +
              "player 2 score:\n " + "name: " + player2.playerName + "\ntime: " + player2.time + "\ncorrect answers: " + player2.correctAnswers + "\nincorrect answers: " + player2.incorrectAnswers);
        DestroyAnswers();// פונקציה להרס השאלה
    }

    public void AnswerTimeUp()//פונקציה שפועלת ברגע שנגמר הזמן למשתמש
    {
        Debug.Log("Answer time is up."); // Log when answer time is up

        playerTurn.AddScore(false, (float)questionTimer.QuestionTime);// מוסיפים את הזמן שעבר לשחקן ושהוא טעה אוטומטית בשאלה לפונקצית הציון
        TimeUpCanvas.SetActive(true);// מפעילים פופ אפ של נגמר הזמן
        DestroyAnswers();// הורסים את התשובות כדי ליצור אותן מחדש

        //ניקוי השעונים של השחקנים
        timerTextPlayer1.text = "";
        timerTextPlayer2.text = "";

        // להפעיל את המסך
        //nextQuesBTN.transform.position = timeEndBTNPose.position;

        nextQuesBTN.SetActive(false);// סגירת כפתור של שאלה הבאה 

        //העלמת הגריד של הכדורים
        ShowBalls(false);
        Debug.Log("Completed AnswerTimeUp execution."); // Log at the end of AnswerTimeUp execution

    }

    Image GetBallToChange()//מזהה איזה כדור צריך לשנות ברשימת הכדורים
    {
        // מחשב את כמות השאלות שכבר ענו ומוריד אחד כדי שנדע איזה כדור לשנות בהתאם למשתמש שצדק 
        return activeAnswerBalls[game.questionList.Count(q => q.isAnswered) - 1];
    }

    public void EndQuestion(bool resumed)// מנהלת את תהליך סיום השאלה והתגובה והכנה לשאלה הבאה
    {
        stopBTNAsBtn.interactable = true;// פעיל את הפאוז שוב אם כיבינו אותו
        questionImagePanel.SetActive(true);// מדליק את הפאנל של השאלות אם סגרנו בטעות שאלה לפני בפופ אפ
        answersBallsGrid.gameObject.SetActive(true);// מראה את הניקוד לפי כדורים

        if (game.questionList.TrueForAll(q => q.isAnswered))// בודק אם ענו על כל השאלות במשחק כבר
        {
            return;// חוזרת כי אין צורך להכין את המסך לשאלה הבאה
        }

        DestroyAnswers();// הורס את התשובות

        if(!resumed)
            PassNextTurn();// מעביר תור לשחקן הבא
        else
            HandleVisualOfCurrentTurn();

        CreateQuestion();// יוצר שאלות

    }


    public void EndGame()// מעדכנת את נתוני המשחק הסופיים
    {
        BtnToTheEnd.SetActive(false);// הסתרת כפתור לסיום משחק
        BRforTheFall.SetActive(true);
        //ResultsBTN.SetActive(true);// הצגת כפתור תוצאות
        standAnimator1.SetTrigger("EndAnim");
        standAnimator2.SetTrigger("EndAnim");


        // בודק אם יש תיקו בין שני השחקנים
        if (player1.Finalgradecalculation() == player2.Finalgradecalculation())
        {
            // אם יש תיקו, כל השחקנים נחשבים למנצחים ואין מפסיד
            StartCoroutine( UpdateEndGameState(null, null, true));
        }
        else
        {
            // אם לא היה תיקו, יש לבדוק מי מבין השחקנים קיבל ציון גבוה יותר
            CharactersScript winner;
            CharactersScript loser;

            // קובעים מי המנצח על בסיס חישוב הציון הסופי
            if (player1.Finalgradecalculation() > player2.Finalgradecalculation())
            {
                winner = player1;
                loser = player2;
            }
            else
            {
                winner = player2;
                loser = player1;
            }

            // עדכן את מצב המשחק לקראת סיום על בסיס מי המנצח ומי המפסיד
            StartCoroutine( UpdateEndGameState(winner, loser, false));
        }
    }


    private IEnumerator UpdateEndGameState(CharactersScript winner, CharactersScript loser, bool isTie) // פונקציה שמעדכנת את מצב המשחק בסוף
    {
        NamePlayer2.text = "";//ניקוי המקום לשמות השחקנים
        NamePlayer1.text = "";
        woodenSignP1.SetActive(false);// כיבוי שני המתקנים של השמות
        woodenSignP2.SetActive(false);
        yield return new WaitForSeconds( standAnimator1.GetCurrentAnimatorClipInfo(0).Length * 1.5f );
        ChairStick1.transform.SetParent(ChairStick1.transform.parent.parent, true);
        ChairStick2.transform.SetParent(ChairStick2.transform.parent.parent, true);

        if (!isTie)// אם אין תיקו
        {
            winner.ChangeEmotion(CharacterStates.SittingHappy); // שינוי מצב המנצח

            //loser.transform.position = (loser == player1) ? PlaseForMachP1.position : PlaseForMachP2.position;
            
            // הסתרה או הצגה של הדמויות על בסיס מי המפסיד
            //standPlayer1.SetActive(loser != player1);
            //standPlayer2.SetActive(loser != player2);

            if (loser == player1)// בודקים אם המפסיד הוא שחקן 1
            {
                
                //loser.transform.position = PlaseForMachP1.position;//אם כן משנים לו את המיקום על המסך
                splashPlayer1.gameObject.SetActive(true);
                standAnimator1.SetTrigger("Fall");

                //SplashWhenWrong1.gameObject.SetActive(true) ;
                //SplashWhenWrong1.Play("SplashWhenWrong");

                yield return new WaitForSeconds(SplashWhenWrong1.GetCurrentAnimatorClipInfo(0).Length);

               

                splashAnimator1.Play("Splash");
                SplashWhenWrong1.gameObject.SetActive(false);

            }
            else
            {
                //loser.transform.position = PlaseForMachP2.position;
                splashPlayer2.gameObject.SetActive(true);

                standAnimator2.SetTrigger("Fall");

                //SplashWhenWrong2.gameObject.SetActive(true);
                //SplashWhenWrong2.Play("SplashWhenWrong");

                yield return new WaitForSeconds(SplashWhenWrong2.GetCurrentAnimatorClipInfo(0).Length);

                splashAnimator2.Play("Splash");
                SplashWhenWrong2.gameObject.SetActive(false);

            }

            //loser.ChangeEmotion(CharacterStates.SadInBarrow);// שינוי מצב המפסיד



            questionText.text = "הניצחון הוא של " + winner.playerName + "!";// טקסט של מי ניצח
            TextToTheEnd.text = loser.playerName + " " + "אנחנו מקווים שהמים לא קרים מידי...";// טקסט של מי שהפסיד
        }
        else// אם זה תיקו
        {
            splashPlayer2.gameObject.SetActive(true);
            splashPlayer1.gameObject.SetActive(true);

            standAnimator1.SetTrigger("Fall");
            standAnimator2.SetTrigger("Fall");

            //SplashWhenWrong2.gameObject.SetActive(true);
            //SplashWhenWrong1.gameObject.SetActive(true);

            //SplashWhenWrong2.Play("SplashWhenWrong");
            //SplashWhenWrong1.Play("SplashWhenWrong");

            yield return new WaitForSeconds(SplashWhenWrong1.GetCurrentAnimatorClipInfo(0).Length);
            //splashPlayer1.SetActive(true);
            //splashPlayer2.SetActive(true);


            splashAnimator1.Play("Splash");
            splashAnimator2.Play("Splash");

            SplashWhenWrong2.gameObject.SetActive(false);
            SplashWhenWrong1.gameObject.SetActive(false);

            //player1.ChangeEmotion(CharacterStates.SadInBarrow);
            //player2.ChangeEmotion(CharacterStates.SadInBarrow);


            //player1.transform.SetParent(player1.transform.root.root.root, true);
            //player2.transform.SetParent(player2.transform.root.root.root, true);

            //// כיבוי הסטנד של הדמויות
            //standPlayer1.SetActive(false);
            //standPlayer2.SetActive(false);
            //StickStand1.SetActive(false);
            //StickStand2.SetActive(false);

            //שינוי המצב של הדמות
            //player1.ChangeEmotion(CharacterStates.SadInBarrow);
            //player2.ChangeEmotion(CharacterStates.SadInBarrow);
            //// שינוי מיקום השחקן על הבמה
            //player1.transform.position = PlaseForMachP1.position;
            //player2.transform.position = PlaseForMachP2.position;
            // משובים על המסך

            questionText.text = "יש פה תיקו!";
            TextToTheEnd.text = player1.playerName + " " + player2.playerName + " " + "אנחנו מקווים שהמים לא קרים מידי...";
        }
        // השמעת סאונד רקע של נפילה למים
        SFXAudioSource.PlayOneShot(fallWaterSound, 10f);

        yield return new WaitForSeconds(1f);// המתנה של שנייה אחת לפני המשך הפעולה
        // הצגת הכפתור לאחר סיום האנימציה
        ResultsBTN.SetActive(true);

    }

    // פונקציה שמכבה את כל הילדים של אובייקט מסוים
    private void DeactivateChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void SwitchToSummaryScreen()// החלפנ שלמסך של סיכום המשחק
    {
        BRforTheFall.SetActive(false);

        DeactivateChildren(splashAnimator2.gameObject);
        DeactivateChildren(splashAnimator1.gameObject);

        splashPlayer1.SetActive(false);
        splashPlayer2.SetActive(false);

        GameQuestionsCanvas.SetActive(false);//  סגירת האנבס של המשחק
        GameQuestionsScreen.SetActive(false);// סגירת המסך של השאלות

        //איפוס היררכיה לעמוד ולשחקנים
        
        player1.transform.SetParent(PositionPlayer1.transform);
        player2.transform.SetParent(PositionPlayer2.transform);

        musicAudioSource.Stop();// עצירת מוזיקת רקע

        EndScreen.SetActive(true);// הפעלת מסך סיום
        EndGameCanvas.SetActive(true);// הפעלת קאנבס סיום
        ResultsBTN.SetActive(false);// סגיירת כפתור לתוצאות
        TitleGameSubForEnd.gameObject.SetActive(true);// הפעלת כותר סיום למשחק
        TitleGameSubForEnd.text = game.gameName;// הפעלת כותר נושא המשחק

        SFXAudioSource.PlayOneShot(clapsForEndSound, 10f);// הפעלת מוזיקת מחיאות כפיים בסוף
    }


    private void UpdateCharacterStates()
    {
        // מעביר את השחקנים למיקומים הסופיים שלהם על המסך
        player1.transform.SetParent(player1EndPosition.transform);
        player2.transform.SetParent(player2EndPosition.transform);
        player1.transform.position = player1EndPosition.position;
        player2.transform.position = player2EndPosition.position;

        // בדיקה אם ציון הסוף של שחקן 1 גבוה משל שחקן 2
        if (player1.Finalgradecalculation() > player2.Finalgradecalculation())
        {
            // שחקן 1 מוגדר כמנצח ושחקן 2 כמפסיד, ומעדכנים את המצבים בהתאם
            SetWinLoseState(player1, player2);
        }
        // במידה וציון הסוף של שחקן 2 גבוה משל שחקן 1
        else if (player1.Finalgradecalculation() < player2.Finalgradecalculation())
        {
            // שחקן 2 מוגדר כמנצח ושחקן 1 כמפסיד, ומעדכנים את המצבים בהתאם
            SetWinLoseState(player2, player1);
        }
        // במקרה שהציונים שווים
        else
        {
            // מעדכנים את המצב לתיקו, ללא מנצח ומפסיד 
            SetTieState();
        }
    }

    private void SetWinLoseState(CharactersScript winner, CharactersScript loser)//
    {
        //שינוי מצב הדמות של המנצח
        winner.ChangeEmotion(CharacterStates.StandHappy);
        // שינוי מצב הדמות של המפסיד
        loser.ChangeEmotion(CharacterStates.SadInBarrow);

        // אם המנצח הוא השחקן הראשון
        if (winner == player1)//
        {
            EndSpeechBubble1.SetActive(true);// הפעלת בועת דיבור של שחקן ראשון 
        }
        else
        {
            EndSpeechBubble2.SetActive(true);// הצגת בועת דיבור של שחקן שני
        }

        OnlyOneWinText.gameObject.SetActive(true);// הפעלת הטקסט שמודיע על ניצחון של שחקן אחד בלבד

        // עדכון הטקסט של סיכום המשחק עם שמות השחקנים והודעה על הניצחון
        SummaryTextWiner.text = winner.playerName + " הצליח/ה" + '\n' + "להפיל את " + loser.playerName + '\n' + "למים!";
    }

    private void SetTieState()// עידכון מצב תיקו
    {
        player1.ChangeEmotion(CharacterStates.HappyInBarrow);// שינוי מצב הדמות
        player2.ChangeEmotion(CharacterStates.HappyInBarrow);// שינוי מצב הדמות

        EndSpeechBubble3.SetActive(true);// פתיחת בועת דיבור של תיקו
        TwoWinText.gameObject.SetActive(true);// פתיחת טקסט של שני שחקנים

        SummaryTextWiner.text = player1.playerName + " " + player2.playerName + '\n' + "סיימו את " + '\n' + "המשחק בתיקו";// טקסט סיכום של תיקו
    }


    private void UpdateSummaryTexts()// עדכון ציונים של המשתתפים
    {
        SummaryTextP1.text = "<b>" + player1.playerName + "</b>" + '\n' + "ציון: " + new string(player1.Finalgradecalculation().ToString().Reverse().ToArray()) + '\n' + "מספר שגיאות: " + new string(player1.incorrectAnswers.ToString().Reverse().ToArray()) + '\n' + "זמן כולל: " + new string(FormatTime((int)player1.time).Reverse().ToArray());

        SummaryTextP2.text = "<b>" + player2.playerName + "</b>" + '\n' + "ציון: " + new string(player2.Finalgradecalculation().ToString().Reverse().ToArray()) + '\n' + "מספר שגיאות: " + new string(player2.incorrectAnswers.ToString().Reverse().ToArray()) + '\n' + "זמן כולל: " + new string(FormatTime((int)player2.time).Reverse().ToArray());

        Debug.Log(player1.time +"שחקן 1");
        Debug.Log(player2.time+"שחקן 2");

    }

    public void ToSummary()// פונקציה שמפעילה את מסך הסיכום מכפתור
    {
        //Puddle1.SetActive(false);//סגירת השלוליות שהופכות להיות נראות בסוף האנימציה
       //Puddle2.SetActive(false);
        stopBTN.SetActive(false);// הסתרת הכפתור לעצירת המשחק
        SwitchToSummaryScreen();// החלפה למסך סיום
        UpdateCharacterStates();// עדכון מצב הדמות
        UpdateSummaryTexts();// עדכון טקסט סיום
    }


    // פונקציה שפולינה הביאה לנו ועשינו התאמות שיעבדו עבורינו
    public string FormatTime(int time)// פונקציה שמציגה את הספירת זמן כמו שעון 
    {
        int newTime = Mathf.Abs(time);
        int minutes = Mathf.FloorToInt(newTime / 60); // חישוב מספר הדקות
        int seconds = Mathf.FloorToInt(newTime % 60);// חישוב מספר השניות
        string secondsStr = "";// הכנת מחרוזת לדקות ושניות
        string minutesStr = "";//
        if (seconds < 10)// הוספת 0 בהתחלה אם השניות פחות מ10
        {
            secondsStr = "0" + seconds.ToString();
        }
        else
        {

            secondsStr = seconds.ToString();
        }

        // הוספת אפס בתחילה אם הדקות פחות מ-10
        if (minutes < 10)
        {
            minutesStr = "0" + minutes.ToString();
        }
        else
        {
            minutesStr = minutes.ToString();
        }

        // שילוב הדקות והשניות לפורמט מלא והחזרתו
        return minutesStr + ":" + secondsStr;
    }


    // פונקציה המגיבה לכניסת העכבר לתחום האובייקט
    public void OnPointerEnter()// התמונה של השאלה גדלה
    {
        if (!isMouseOver)//אם התמונה קטנה והמשתנה מוגדר כפאלס אז יהיה אפשר להיכנס לפונקצית ההגדלה של התמונה
        {
            EnlargeImage();// מגדיל את התמונה
            isMouseOver = true;//מעדכן שהאובייקטי סיים לגדול ורק אז אפשר יהיה להקטין אותו כדי שלא יהיה מצב של בעיות בתהליך ההגדלה וההקטנה אם המשתמש יעבור על התמונה במהירות 
        }
    }

    // כל התהליך של הפונקציה של ההגדלה רק בההפך בשביל הקטנה
    public void OnPointerExit()
    {

        if (isMouseOver)
        {
            ShrinkImage();
            isMouseOver = false;
        }
    }

    private void EnlargeImage()//פונקציה להגדלת התמונה
    {
        // מבצע אנימציה שמגדילה את התמונה לפי גודל מוגדר וזמן אנימציה 
        LeanTween.scale(QuestionImage.gameObject, originalScale * magnificationAmount, animationTime);//  אנימציה שמגדילה את התמונה
        MagnifyingGlassImage.gameObject.SetActive(false);// סוגר את הזכוכית מגדלת ליד
    }

    private void ShrinkImage()//פונקציה לכיווץ התמונה
    {
        LeanTween.scale(QuestionImage.gameObject, originalScale, animationTime);// אנימציה שמקווצת את התמונה
        MagnifyingGlassImage.gameObject.SetActive(true);// מפעיל את הזכוכית מגדלת ליד
    }



    public void PassNextTurn()// פונקציה להחלפת התורים של השחקנים
    {
        if (playerTurn == player1)// אם תור הנוכחי הוא של שחקן 1
        {
            playerTurn = player2; // החלף את התור לשחקן 2
        }
        else if (playerTurn == player2)// ההפך
        {
            playerTurn = player1;
        }
        HandleVisualOfCurrentTurn();
    }

    public void PauseGame()
    {
        if (answeredAnimIsPlaying)// אם האנימציה של הכדור פועלת זה לא נותן לו להיכנס לפופ אפ פאוז
        {
            return;
        }

        game.questionList[questionNumber].wrongUser = true;// משנים את מצה התשובה לנענתה כדי שלא תחזור שוב לשחקנים

        //סגירת התמונה שלא תופיע למעלה בשאלה
        //questionImagePanel.gameObject.SetActive(false);
        stopBTNAsBtn.interactable = false;

        Time.timeScale = 0f; // הקפאת הזמן במשחק, עוצר את כל האנימציות והזמן
        questionTimer.Pause();
        ShowPausePopup(); // הצגת פופאפ השהייה
        

        if (nextQuesBTN.activeSelf)// אם כפתור השאלה הבאה פעיל
        {
            nextQuesBTN.SetActive(false); // הפסקת הצגת כפתור השאלה הבאה
            //answersGrid.gameObject.SetActive(false);// הסתרת גריד התשובות
            //answersBallsGrid.gameObject.SetActive(false);// הסתרת גריד כדורים
        }
        else // אם כפתור השאלה הבאה לא פעיל
        {
            if (!endingQuestion) { 
            //timerActive = false; // הפסקת הטיימר של השאלה
            DestroyAnswers(); // השמדת תשובות השאלה הנוכחית
            playerTurn.throwBall.gameObject.transform.position = ThrowBallStartingPos.position;// החזרת כדור שזורקים למיקום התחלתי
            ShowBalls(false);// הסתרת הכדור של התשובה והגריד של הכדורים 
            if (lt != null) // אם קיימת אנימציה פעילה
            {
                lt.reset();// איפוס האנימציה
                lt = null; // איפוס האובייקט של האנימציה
            }}
        }


    }


    public void ShowPausePopup()// פתיחת הקאנבס של הפופ אפ
    {
        pausePopup.SetActive(true); // הצגת פופ אפ

    }


    public void ResumeGame()
    {
        ResumeGameWasActiv = true;

        stopBTNAsBtn.interactable = true;
        Time.timeScale = 1; // הפעלת הזמן במשחק שוב
        pausePopup.SetActive(false); // הסתרת פופאפ העצירה
        questionImagePanel.gameObject.SetActive(true); // הצגת הפאנל של התמונה

        if (endingQuestion) // אם כפתור השאלה הבאה היה פעיל בתחילת העצירה
        {
            nextQuesBTN.SetActive(true); // הפעלת כפתור השאלה הבאה
            answersGrid.gameObject.SetActive(true); // הצגת גריד התשובות
        }
        if(!endingQuestion) 
        {
            EndQuestion(true);
        }
        ResumeGameWasActiv = false;
    }

    private void ShowBalls(bool show)// פונקציה שמשמשת להפעיל או לכבוד את גריד הכדורים והכדורים ברגע שפופ אפ נסגר או נפתח
    {

        answersBallsGrid.gameObject.SetActive(show); // מצב הצגה או הסתרה של גריד הכדורים
        playerTurn.throwBall.SetActive(show);// מעלים את הכדור 

    }

    public void StartNewRound()// איפוס והתחלה של סבב של אותו משחק
    {
        SFXAudioSource.Stop();

        ResetEndScreen();// איפוס מסך סופי
        ResetActiveAnswerBalls();//ניקוי הכדורים של סימום תשובות נכונות
        ResetGameQuestio(); // איפוס מסך שאלות
        ResetWheel();// איפוס מסך הגלגל
        ResetGrades();// איפוס ציון לכל שחקן

        //ChangeSoundOn(true);//מפעיל את הסאונד במידה והיה מושתק
        musicAudioSource.Play();// מתחיל לנגן את המוזיקה ברקע

        StartParticipantLotteryScreen(); // חזרה למסך הגרלב


    }


    public void ResetAndStartNewGame()// איפוס והתחלה של משחק חדש
    {
		// עצירצת הסאונד
		//musicAudioSource.Stop();
		pausePopup.SetActive(false);// עצירת מסך הפופ אפ למקרה שהמסך הזה שלח אותנו 
        questionTimer.Stop();
		SFXAudioSource.Stop();
		ResetEndScreen();// איפוס מסך סופי
        ResetWheel();// איפוס הכפתורים והגלגל
        ResetChoicePlayer();// איפוס של מסך בחירת דמויות 
        ResetActiveAnswerBalls();//ניקוי הכדורים של סימום תשובות נכונות
        ResetSartCod(); // איפוס מסך התחלה
        ResetGameQuestio(); // איפוס מסך שאלות
        
        EnterCodeGameScreen();  // חזרה למסך הראשון שמזינים קוד
    }

    public void ResumeAndStartNewGame()
    {
        //nextQuesBTN.gameObject.SetActive(false);// העלמת כפתור שאלה הבאה אם במקרה פעיל 
        ResumeGame();
        ResetAndStartNewGame();
    }
    private void ResetGrades()
    {
        player1.ResetScore();
        player2.ResetScore();
    }


    private void ResetEndScreen()// איפווס מסך הסיום
    {
        splshAnimasion.SetActive(false);

        // סגירת כל המסכים וחזרה למסך התחלתי
        EndScreen.SetActive(false);
        EndGameCanvas.SetActive(false);
        EndSpeechBubble3.SetActive(false);
        EndSpeechBubble2.SetActive(false);
        EndSpeechBubble1.SetActive(false);
        SummaryTextWiner.text = "";
        OnlyOneWinText.gameObject.SetActive(false);
        TwoWinText.gameObject.SetActive(false);
    }

    private void ResetSartCod()// איפוס מסך הזנת הקוד
    {
        startCanvas.SetActive(true);// פתיחת הקאנבס של המסך התחלה
        BTNGameCodeStart.gameObject.SetActive(true);
        GameInputFieldCodeTextStarte.gameObject.SetActive(true);
        StartScreen.SetActive(true);//פתיחת מסך התחלה
        logo.SetActive(true);//סוגר לוגו
        starttextabouve.SetActive(true);
        starttextabelow.SetActive(true);
        // איפוס טקסטים ושדות קלט
        GameInputFieldCodeTextStarte.text = "";
        TextErrorCode.text = "";
    }

    private void ResetWheel()// איפוס מסך הגלגל
    {
        // מעבר על כל הטקסטים המוצגים על הגלגל ומחיקתם
        foreach (var text in wheelTexts)
        {
            if (text != null)// אם האובייקט של הטקסט אינו null
                Destroy(text.gameObject);// מחזיר את האובייקט והורס אותו
        }

        wheelTexts.Clear(); // ניקוי הרשימה של טקסטים על הגלגל

        WheelWinnerTitle.text = "לחצו על הגלגל להגרלת \nהשחקן הראשון";//איפוס טקסט כותרת
        WheelWinnerTitle.fontSize = 3.8f;// הגדלת גודל הטקסט של הכותרת

        BTNRandomWheel.transform.rotation = Quaternion.identity; //  איפוס סיבוב הגלגל למצב התחלתי
        BTNRandomWheel.interactable = true;// הפיכת הכפתור לאינטראקטיבי, כך שניתן יהיה ללחוץ עליו שוב
        RandomWheelCanvas.SetActive(false);// הפעלת קאנבס
        ParticipantLotteryScreen.SetActive(false);// הפעלת מסך

    }

    private void ResetChoicePlayer() //איפוס מסך בחירת השחקן
    {
        // איפוס מסכים
        ChoosePlayers.SetActive(false);
        ChoosePlayersCanvas.SetActive(false);

        // הסרת השחקנים מהמשחק
        if (player1 != null)
        {
            Destroy(player1.gameObject);
            player1 = null;
        }
        if (player2 != null)
        {
            Destroy(player2.gameObject);
            player2 = null;
        }
        playerTurn = null;

        //איפוס כפתורי דמויות שלא יהיו כהים
        foreach (Button button in CharactersButton)
        {
            button.interactable = true;// הפיכת הכפתור לאינטראקטיבי

        }

        //איפוס משובים
        TextErrorChoosePlayers.gameObject.SetActive(false);
        ChoosePlayerInputField.text = "";
        TheNumberOfTheStartingPlayer.text = "שחקן ראשון";
        BTNChoosePlayers.GetComponentInChildren<TextMeshProUGUI>().text = "הבא";
        chosenCharacterName = "";

    }

    private void ResetActiveAnswerBalls() // פונקציה לאיפוס כדורי התשובות הפעילים
    {
        foreach (var ball in activeAnswerBalls)
        {
            if (ball != null)
            {
                Destroy(ball.gameObject);// השמדת אובייקטי כדורי התשובות
            }

        }
        activeAnswerBalls.Clear();// ניקוי הרשימה
    }

    private void ResetGameQuestio()//  שאלות המשחק
    {
       
        woodenSignP1.SetActive(true);// הדלקת שלטי העץ של השחקן
        woodenSignP2.SetActive(true);
        GameQuestionsCanvas.SetActive(false);// הסתרת הקאנבס של השאלות
        GameQuestionsScreen.SetActive(false);// הסתרת המסך שאלות
        QuestionImage.gameObject.SetActive(true);// מדליקים את השאלות
        MagnifyingGlassImage.gameObject.SetActive(true);// הפעלת הזכוכית מגדלת 
        TextToTheEnd.gameObject.SetActive(false);// מכבים את הטקסט שלפני הסוף
        stopBTNAsBtn.interactable = true;
        foreach (QuestionData Question in game.questionList)// איפוס הסימונים של השאלות שנענו במשחק
        {
            Question.isAnswered = false;
        }
        TimeUpCanvas.SetActive(false);
        DestroyAnswers();
    }

    IEnumerator FadeOut(OnEvent OnComplete)
    {
        float t = 0;
        while (fadeImg.color.a < 1f)
        {
            fadeImg.color = Color.Lerp(new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 0), new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1), t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        OnComplete.Invoke();

    }
    IEnumerator FadeIn(OnEvent OnComplete)
    {
        float t = 0;
        while (fadeImg.color.a > 0f)
        {
            fadeImg.color = Color.Lerp(new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1), new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 0), t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        OnComplete.Invoke();

    }

}
    public delegate void OnEvent();
public static class Circle// מחלקה שמכילה פונקציה לחישוב זוית ונקודות במעגל
{
    public static Vector2 AngleToPoint(float angle)// ממיר זוית במעלות לנקודה במעגל
    {
        angle *= Mathf.Deg2Rad;// המרת זוית
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));// חישוב נקודת מעגל לפי זוית
    }

    // פונקציה הממירה נקודה על מעגל לזווית במעלות
    public static float PointToAngle(Vector2 point)
    {
        return Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg; // חישוב הזווית על פי הנקודה
    }

    public static float ConvertAngleToEuler(float angle)
    {
        angle = (angle > 180) ? angle - 360 : angle;
        return angle;
    }
    public static float ConvertEulerToAngle(float angle)
    {
        angle = (angle < 0) ? angle + 360 : angle;
        return angle;
    }

}

//מחלקות שמשמשות להגדרה של השאלות במשחק

[System.Serializable]
public class GameData
{
    public string gameName;// שם המשחק
    public float questionTime;// זמן השמחק
    public List<QuestionData> questionList;//רשימת שאלות
}

[System.Serializable]
public class QuestionData// נתונים על שאלות במשחק
{
    public bool isAnswered = false;// האם היא נענתה
    public bool wrongUser = false;// האם היא נענתה
    public string content;// תוכן השאלה
    public Sprite imageContent; // הוספתי אופציה שיהיה אפשר להוסיף תמונה
    public List<AnswerData> answersList;// רשימת תשובות
}

[System.Serializable]
public class AnswerData// נתונים של תשובות
{
    public string textContent;// טקסט תשובה
    public Sprite imageContent;// מקום לתמונה
    public bool isCorrect;// האם היא נכונה
}



