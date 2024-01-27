using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;
using System.Linq;
using Foundational;
using futz.ActGhost;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class GhostPlayerManager : Singleton<GhostPlayerManager>
{
    [HideInInspector]
    public bool readyToBegin = false;
    public GameObject pressPlayToBeginMsg;
    public GameObject needTwoPhonePlayersMsg;

    public TMPro.TMP_Text ghostText;

    public List<GhostPlayer>  ghostPlayers;

    public List<Goal> allSharedGoals;

    public SpriteRenderer[] ghostsToFade;

    public void CreateAllGoals(){
        allSharedGoals = new List<Goal>
        {
            new Goal {
                goalString = "Flip one more vases upside down.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Vase) > 0
            },
            new Goal {
                goalString = "Light all torches.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Torch) > 4
            },
            new Goal {
                goalString = "Extinguish all candles.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Candle) <= 0
            },
            new Goal {
                goalString = "Straighten one portrait.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Portrait) <= 0
            },
            new Goal {
                goalString = "I hate crooked paintings!",
                goalAction = () => InteractableObject.allInteractables.All(
                    (a) => a.interactableType != InteractableObject.InteractableType.Portrait
                    || a.hasBeenEnabledByPlayer )
                    // that means all the portraits must be "enabled".
            },
            
        };
    }


    public void Start(){
        ghostPlayers = new List<GhostPlayer>();
        CreateAllGoals();

        // TODO: get all the players that have connected to the Futz here. Assign them goals.

        pressPlayToBeginMsg.SetActive(false);
        needTwoPhonePlayersMsg.SetActive(true);
        readyToBegin = false;
        Player.i.gameObject.SetActive(false);

    }

    public void Update(){

        if(readyToBegin){
            needTwoPhonePlayersMsg.SetActive(false);
            pressPlayToBeginMsg.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space)){
                GameStarted();
                
            }
        }

        foreach(GhostPlayer g in ghostPlayers){
            g.EvaluateGoals();
        }

        string s = $"Ghosts: {ghostPlayers.Count}\n";
        foreach(var ghost in ghostPlayers){
            s += "   " + ghost.ToString() + "\n";
        }

        ghostText.text = s;
    }

    public void GameStarted(){
        readyToBegin = false;
        Player.i.gameObject.SetActive(true);
        pressPlayToBeginMsg.SetActive(false);
        GhostLogic.BeginRoom(GameSysClip.I.GhostAct.Current).Forget();
        
        var actors = GameSysClip.I.GhostAct.Current.Actors.Current;

        ghostPlayers = new List<GhostPlayer>();
        foreach(var phonePlayer in actors){
            var ghost = new GhostPlayer
            {
                name = phonePlayer.Nickname,
                actor = phonePlayer
            };
            var goal = allSharedGoals[Random.Range(0, allSharedGoals.Count)];

            ghost.ClearHints();
            ghost.AssignGoal(goal);
            ghostPlayers.Add(ghost);
        }
   

    }
    
}

public class GhostPlayer {

    public string name;
    public int goalsComplete = 0;
    public List<Goal> activeGoals = new List<Goal>();
    internal GhostActor actor;

    public void EvaluateGoals(){
        List<Goal> toRemove = new List<Goal>();

        foreach(Goal g in activeGoals){
            if(g.Evaluate()){
                goalsComplete += 1;
                toRemove.Add(g);
            }
        }

        if(toRemove.Count > 0){
            activeGoals.RemoveAll(x => toRemove.Contains(x));
        }
    }
    
    public override string ToString(){
        return $"{name} score: {goalsComplete}";
    }

    internal void AssignGoal(Goal goal)
    {
        activeGoals.Add(goal);
        var hint = new Hint();
        hint.Message = goal.goalString;

        actor.AssignedHints.Add(hint);
    }

    internal void ClearHints()
    {
        actor.AssignedHints.Clear();
    }
}

public class Goal {
    public string goalString;
    public GoalType goalType;
    public System.Func<bool> goalAction;

    public enum GoalType {
        TouchTestVase
    }

    public bool Evaluate(){
        switch(goalType){
            case GoalType.TouchTestVase:
                return Player.i.InteractCount(InteractableObject.InteractableType.Vase) > 0;

            default:
                return false;
        }
    }

}
