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
using UnityEngine.Serialization;

public class GhostPlayerManager : Singleton<GhostPlayerManager>
{
    [HideInInspector]
    public bool readyToBegin = false;
    public GameObject pressPlayToBeginMsg;
    public GameObject needTwoPhonePlayersMsg;

    public TMPro.TMP_Text ghostText;

    public List<GhostPlayer>  ghostPlayers;

    public List<Goal> allSharedGoals;

    public string[] spiritNames;

    [FormerlySerializedAs("ghostsToFade")]
    public SpriteRenderer[] spiritsToFade;
    public GameObject exit;

    public void CreateAllGoals(){
        allSharedGoals = new List<Goal>
        {
            new Goal {
                goalString = "I hate the smell of burning torches.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Torch) == 0,
                spiritIndex = 0,
            },
            new Goal {
                goalString = "But I can't stand the dark.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Candle) >= 1,
                spiritIndex = 0,
            },
            new Goal {
                goalString = "Ooooh that vase reminds me of limes!",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Vase, InteractableObject.InteractableColor.Green) >= 1,
                spiritIndex = 0,
            },


            new Goal {
                goalString = "I always loved pottery.",
                goalAction = () => InteractableObject.allInteractables
                    .Where( (a) => a.interactableType == InteractableObject.InteractableType.Vase )
                    .Where( a => a.interactableColor != InteractableObject.InteractableColor.Blue )
                    .All( a => a.hasBeenEnabledByPlayer ),
                spiritIndex = 1,
            },
            new Goal {
                goalString = "That blue vase is an embarrassment.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Vase, InteractableObject.InteractableColor.Blue) == 0,
                spiritIndex = 1,
            },
            new Goal {
                goalString = "I do love blue though!",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Torch, InteractableObject.InteractableColor.Blue) >= 1,
                spiritIndex = 1,
            },


            new Goal {
                goalString = "Ewww, purple!",
                goalAction = () => InteractableObject.allInteractables
                    .Where( (a) => a.interactableType == InteractableObject.InteractableType.Vase )
                    .Where( a => a.interactableColor != InteractableObject.InteractableColor.Purple )
                    .All( a => a.hasBeenEnabledByPlayer ),
                spiritIndex = 2,
            },
            new Goal {
                goalString = "I didn't know there was a painting of me.",
                goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Vase, InteractableObject.InteractableColor.Blue) == 0,
                spiritIndex = 2,
            },
            new Goal {
                goalString = "These vases are all hideous!",
                goalAction = () => InteractableObject.allInteractables
                    .Where( (a) => a.interactableType == InteractableObject.InteractableType.Vase )
                    .All( a => a.hasBeenEnabledByPlayer ),
                spiritIndex = 2,
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
        int goalsPerPlayer = (int) Mathf.Ceil(9.0f / actors.Count);

        foreach(var phonePlayer in actors){
            var ghost = new GhostPlayer
            {
                name = phonePlayer.Nickname,
                actor = phonePlayer
            };

            ghost.ClearHints();
            for(int i = 0; i < goalsPerPlayer; i ++){
                if(allSharedGoals.Count > 0){
                    var goal = allSharedGoals[Random.Range(0, allSharedGoals.Count)];
                    ghost.AssignGoal(goal);
                    allSharedGoals.Remove(goal);
                }
            }

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
        hint.Message = GhostPlayerManager.i.spiritNames[goal.spiritIndex] + ": " +  goal.goalString;

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
    /// <summary>
    /// spirit index of -1 means it controls the exit.
    /// </summary>
    public int spiritIndex;

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
