using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GetDustFromPlayer : MonoBehaviour
{

    public VisualEffect effect;
    public Player player;

    private Vector2 prevPos;
    private Vector2 smoothedVel;

    void Update()
    {
        Vector2 pos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 delta = (prevPos - pos) * Time.deltaTime;

        smoothedVel = Vector2.Lerp(delta, smoothedVel, 0.5f);

        Vector2 output = -smoothedVel.normalized * 0.3f;
        if(delta == Vector2.zero){
            output = Vector2.zero;
        }
        effect.SetVector2("PlayerVel", output);
        effect.SetVector2("PlayerPos", pos);
        
        prevPos = pos;
    }
}
