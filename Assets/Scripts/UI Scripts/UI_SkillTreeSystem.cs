//using UnityEngine;
//using UnityEngine.UI;

//public class UI_SkillTreeSystem : MonoBehaviour {

//    // Player Controller Script

//    public TPSCharacterController tpsCharacterController;

//    // UI Elements

//    public GameObject UISkillTree;
//    public Button[] skillButtons;

//    // Determining where a skill is located in the skill tree

//    private bool skillTreeTeir1;
//    private bool skillTreeTeir2;
//    private bool skillTreeTeir3;

//    // Skill values - increase player attribiutes by skill values

//    [HideInInspector]
//    public float maxHealthLvl1 = 40;
//    [HideInInspector]
//    public float maxAttackLvl1 = 10;
//    [HideInInspector]
//    public float maxMoveSpeedLvl1 = 20;
//    [HideInInspector]
//    public float maxHealthLvl2 = 60; 

//    // Amount of EPs to unlock skill

//    private int healthLvl1SkillPrice = 30;
//    private int attackLvel1SkillPrice = 40;
//    private int movementLvl1SkillPrice = 40;
//    private int healthLvl2SkillPrice = 50;

//    // Determining if skill has been unlocked by the player or not

//    private bool isHealthLvl1SkillUnlocked = false;
//    private bool isAttackSkillUnlocked = false;
//    private bool isMovementSkillUnlocked = false;
//    private bool isHealthLvl2SkillUnlocked = false;


//	void SetInitialReferences()
//    {
//        UISkillTree.SetActive(false);

//        skillTreeTeir1 = true;
//        skillTreeTeir2 = false;
//        skillTreeTeir3 = false;
//    }

//    // Use this for initialization
//	void Start () {

//        SetInitialReferences();      		
//	}
	
//	// Update is called once per frame
//	void Update () {

//        CheckUnlockedSkills();
//        GetInput();
//    }

//    // Checking if skill has already been unlocked by the player or not

//    void CheckUnlockedSkills()
//    {
//        if (tpsCharacterController.EP >= healthLvl1SkillPrice && tpsCharacterController.EP >= healthLvl2SkillPrice && !isHealthLvl1SkillUnlocked || !isHealthLvl2SkillUnlocked && skillTreeTeir1 || skillTreeTeir3)
//        {
//            skillButtons[0].gameObject.SetActive(true);
//        }

//        else
//        {
//            skillButtons[0].gameObject.SetActive(false);
//        }

//        if (tpsCharacterController.EP >= movementLvl1SkillPrice && !isMovementSkillUnlocked && skillTreeTeir2)
//        {
//            skillButtons[1].gameObject.SetActive(true);
//        }

//        else
//        {
//            skillButtons[1].gameObject.SetActive(false);
//        }

//        if (tpsCharacterController.EP >= attackLvel1SkillPrice && !isAttackSkillUnlocked && skillTreeTeir2)
//        {
//            skillButtons[2].gameObject.SetActive(true);
//        }

//        else
//        {
//            skillButtons[2].gameObject.SetActive(false);
//        }

//        if (isMovementSkillUnlocked && isAttackSkillUnlocked)
//        {
//            skillTreeTeir2 = false;
//        }

//        if (skillTreeTeir1)
//        {
//            skillButtons[0].GetComponentInChildren<Text>().text = "Health Lvl1";
//        }

//        if (skillTreeTeir3)
//        {
//            skillButtons[0].GetComponentInChildren<Text>().text = "Health Lvl2";
//        }
//    }

//    // Getting input from the player to activate Skill Tree UI

//    void GetInput()
//    {
//        if (Input.GetKeyDown(KeyCode.Tab))
//        {
//            if (skillTreeTeir1 || skillTreeTeir2 || skillTreeTeir3)
//            {
//                UISkillTree.SetActive(true);
//                Cursor.lockState = CursorLockMode.None; //Unlock cursor
//                Debug.Log("A new talent is available!");
//            }

//            else
//            {
//                UISkillTree.SetActive(false);
//                Cursor.lockState = CursorLockMode.Locked; //Lock cursor
//                Debug.Log("No new talents are available!");
//            }
//        }
//    }

//    public void ChangeHealthValue(float changeHealthValue)
//    {
//        if (skillTreeTeir1)
//        {
//            changeHealthValue = maxHealthLvl1;
//            tpsCharacterController.EP -= healthLvl1SkillPrice;
//            tpsCharacterController.health = changeHealthValue;

//            skillButtons[0].gameObject.SetActive(false);
//            isHealthLvl1SkillUnlocked = true;
//            skillTreeTeir2 = true;
//            skillTreeTeir1 = false;

//        }

//        if (skillTreeTeir3)
//        {
//            changeHealthValue = maxHealthLvl2;
//            tpsCharacterController.EP -= healthLvl2SkillPrice;
//            tpsCharacterController.health = changeHealthValue;

//            isHealthLvl2SkillUnlocked = true;
//            skillTreeTeir2 = true;
//            skillTreeTeir1 = false;
//        }

//        UISkillTree.SetActive(false);
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    public void ChangeMovementValue(float changeMovementValue)
//    {
//        tpsCharacterController.EP -= movementLvl1SkillPrice;
//        tpsCharacterController.speed = changeMovementValue;

//        skillButtons[1].gameObject.SetActive(false);
//        UISkillTree.SetActive(false);
//        Cursor.lockState = CursorLockMode.Locked;

//        isMovementSkillUnlocked = true;
//        skillTreeTeir3 = true;
//        skillTreeTeir2 = true;
//    }

//    public void ChangeAttackValue(float changeAttackValue)
//    {
//        tpsCharacterController.EP -= attackLvel1SkillPrice;
//        tpsCharacterController.attack = changeAttackValue;

//        skillButtons[2].gameObject.SetActive(false);
//        UISkillTree.SetActive(false);
//        Cursor.lockState = CursorLockMode.Locked;

//        isAttackSkillUnlocked = true;
//        skillTreeTeir3 = true;
//        skillTreeTeir2 = true;
//    }
//}

