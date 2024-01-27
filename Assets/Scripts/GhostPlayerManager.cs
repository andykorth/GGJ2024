using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;

public class GhostPlayerManager : MonoBehaviour
{

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
            }
            
            
        };
    }


    public void Start(){
        ghostPlayers = new List<GhostPlayer>();
        var testGhost = new GhostPlayer{name = "TestGhost"};

        CreateAllGoals();

        // TODO: get all the players that have connected to the Futz here. Assign them goals.

        Goal testGoal = new Goal {
            goalString = "Touch the sphere-vase",
            goalType = Goal.GoalType.TouchTestVase
        };



        testGhost.activeGoals.Add(testGoal);
        ghostPlayers.Add(testGhost);
    }

    public void Update(){

        foreach(GhostPlayer g in ghostPlayers){
            g.EvaluateGoals();
        }

        string s = $"Ghosts: {ghostPlayers.Count}\n";
        foreach(var ghost in ghostPlayers){
            s += "   " + ghost.ToString() + "\n";
        }

        ghostText.text = s;
    }
    
}

public class GhostPlayer {

    public string name;
    public int goalsComplete = 0;
    public List<Goal> activeGoals = new List<Goal>();

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
