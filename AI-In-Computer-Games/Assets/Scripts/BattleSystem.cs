using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START,PLAYERTURN,ENEMYTURN,WON,LOST}
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Unit playerUnit;
    public Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    MinMaxAlgorithm neededScript;
    //MinMaxAlgorithm neededscript;

    // public float healPoint;
    private float playerUnitattackingPoint, enemyUnitattackingPoint;
    private float playerUnitSituationValue, enemyUnitSituationValue;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        neededScript = GetComponent<MinMaxAlgorithm>();
    }

    IEnumerator SetupBattle()
    {
        // Instantiate(playerPrefab,new Vector3(playerBattleStation.position.x,playerBattleStation.position.y+0.75f,playerBattleStation.position.z) );
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "Be prepare! " + enemyUnit.unitName + " is coming!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = "The attack is successful!";
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        yield return new WaitForSeconds(2f);
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " is calculating possible moves...";
        yield return new WaitForSeconds(2f);
        //neededScript.PrintMinMaxTree();

        /// Dialogue update
        dialogueText.text = enemyUnit.unitName + " is preparing for move!";

        yield return new WaitForSeconds(1f);

          if(SituationCalc())
        {
            // Deciding Attack
            playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.currentHP);             // UI updating after attacking
            dialogueText.text = "Your enemy attacked you!";    // UI updating after attacking
            Debug.Log("Your enemy attacked you!");
        }          // NOT CRASHES ANYMORE, ALREADY SOLVED;
          else
        {
            // Deciding Heal
            enemyUnit.Heal(enemyUnit.healPoint);
            enemyHUD.SetHP(enemyUnit.currentHP);              // UI updating after healing
            dialogueText.text = "Your enemy healed itself!";    // UI updating after healing
            Debug.Log("Your enemy healed itself!");
        }

        /// Damaging and dead control
       

        /// Updating HUD
        //playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (playerUnit.currentHP<=0 /*isDead*/)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST) {
            dialogueText.text = "You were defeated!";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an option:";

    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(playerUnit.healPoint);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You healed yourself!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

     public bool SituationCalc() {

        //Debug.Log("Entered situation calc function");

        /// Evaluating formula = (player.hp + (Attacking point x Level) * 100)
        /// Next optional formula = ( (attackingPoint*Rand(0,1)* Level) * (HealthPoint/100) = playerMove1;
        /// (((attackingPoint*Rand(0,1))*0.5) + (currentHP*0.5)= situationPoint

        /*  for (int iteration=1;iteration<=3;iteration++) {

             //Random.seed = System.DateTime.Now.Millisecond;

             playerUnitattackingPoint = Random.Range((playerUnit.damage + playerUnit.unitLevel) + 3, (playerUnit.damage + playerUnit.unitLevel) - 3);
             enemyUnitattackingPoint = Random.Range((enemyUnit.damage + enemyUnit.unitLevel) + 3, (enemyUnit.damage + enemyUnit.unitLevel) - 3);


             playerUnitattackingPoint = ((playerUnitattackingPoint * (float)Random.Range(0, 1)) * 0.5f);
             enemyUnitattackingPoint = ((enemyUnitattackingPoint * (float)Random.Range(0, 1)) * 0.5f);


             playerUnitSituationValue = playerUnitattackingPoint + (playerUnit.currentHP * 0.5f);
             enemyUnitSituationValue = enemyUnitattackingPoint + (enemyUnit.currentHP * 0.5f);
             Debug.Log("iteration number" + iteration + "  Player minmax:" + playerUnitSituationValue + "---------Enemy minmax:" + enemyUnitSituationValue);
         }  */


        //  Debug.Log("Current Situation: Player minmax:"+ playerUnitSituationValue + "---------Enemy minmax:"+ enemyUnitSituationValue);

        // GetComponent<MinMaxAlgorithm>().MinMaxCalc();

        if (neededScript.MinMaxCalc() /* playerUnitSituationValue >= enemyUnitSituationValue*/)
        {
            return true;
        } 

        else
        {
            return false;
        } 


       /* if (playerUnit.currentHP >= enemyUnit.currentHP) {
            return true; }
        else
        {
            return false;
        } */

      /*  yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());    */
        }

    

         /*  
         /// Creating a formula for calculating a game situation /// STILL PROGRESSING
         ///There will be determining code with min-max        /// STILL PROGRESSING
         ///Whether attack or heal (Even maybe another option) /// DONE
         /// Also it will consider level of opponent.          /// DONE
         /// AI should consider the current situation of the game, and tend to be more agressive player is about to lose. /// TO-DO
         /// Improving UI     /// TO-DO
         /// Player can attack couple times before enemy turn, solve it! /// TO-DO
         }   */

}
