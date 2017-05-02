using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTreeSystem : MonoBehaviour {

    // Player Controller Script

    public TPSCharacterController tpsCharacterController;

    // UI Elements

    public GameObject UISkillTree;
    public Button[] skillButtons;

    // Determining where a skill is located in the skill tree

    private bool skillTreeTeir1;
    private bool skillTreeTeir2;
    private bool skillTreeTeir3;

    // Skill values - increase player attribiutes by skill values

    [HideInInspector]
    public float maxHealthLvl1 = 40; //min health 20 - max health 60
    [HideInInspector]
    public float maxAttackLvl1 = 10;
    [HideInInspector]
    public float maxMoveSpeedLvl1 = 20;

    // Amount of EPs to unlock skill

    private int healthLvl1SkillPrice = 30;
    private int attackLvel1SkillPrice = 40;
    private int movementLvl1SkillPrice = 40;

    // Determining if skill has been unlocked by the player or not

    private bool isHealthSkillUnlocked = false;
    private bool isAttackSkillUnlocked = false;
    private bool isMovementSkillUnlocked = false;


	void SetInitialReferences()
    {
        UISkillTree.SetActive(false);
        skillTreeTeir1 = true;
        skillTreeTeir2 = false;
    }

    // Use this for initialization
	void Start () {

        SetInitialReferences();      		
	}
	
	// Update is called once per frame
	void Update () {

        CheckUnlockedSkills();
        GetInput();
    }

    // Checking if skill has already been unlocked by the player or not

    void CheckUnlockedSkills()
    {
        if (tpsCharacterController.EP >= healthLvl1SkillPrice && !isHealthSkillUnlocked && skillTreeTeir1)
        {
            skillButtons[0].gameObject.SetActive(true);
        }

        else
        {
            skillButtons[0].gameObject.SetActive(false);
        }

        if (tpsCharacterController.EP >= movementLvl1SkillPrice && !isMovementSkillUnlocked && skillTreeTeir2)
        {
            skillButtons[1].gameObject.SetActive(true);
        }

        else
        {
            skillButtons[1].gameObject.SetActive(false);
        }

        if (tpsCharacterController.EP >= attackLvel1SkillPrice && !isAttackSkillUnlocked && skillTreeTeir2)
        {
            skillButtons[2].gameObject.SetActive(true);
        }

        else
        {
            skillButtons[2].gameObject.SetActive(false);
        }
    }

    // Getting input from the player to activate Skill Tree UI

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UISkillTree.SetActive(true);
            Cursor.lockState = CursorLockMode.None; //Unlock cursor
        }
    }

    public void ChangeHealthValue(float changeHealthValue)
    {
        tpsCharacterController.EP -= healthLvl1SkillPrice;
        tpsCharacterController.health = changeHealthValue;

        skillTreeTeir1 = false;
        skillTreeTeir2 = true;

        isHealthSkillUnlocked = true;
        skillButtons[0].gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        UISkillTree.SetActive(false);
    }

    public void ChangeMovementValue(float changeMovementValue)
    {
        tpsCharacterController.EP -= movementLvl1SkillPrice;
        tpsCharacterController.speed = changeMovementValue;
        skillButtons[1].gameObject.SetActive(false);

        isMovementSkillUnlocked = true;
        skillTreeTeir3 = true;

        Cursor.lockState = CursorLockMode.Locked;
        UISkillTree.SetActive(false);
    }

    public void ChangeAttackValue(float changeAttackValue)
    {
        tpsCharacterController.EP -= attackLvel1SkillPrice;
        tpsCharacterController.attack = changeAttackValue;
        skillButtons[2].gameObject.SetActive(false);

        isAttackSkillUnlocked = true;
        skillTreeTeir3 = true;

        Cursor.lockState = CursorLockMode.Locked;
        UISkillTree.SetActive(false);
    }
}

