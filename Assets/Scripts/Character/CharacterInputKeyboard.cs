using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputKeyboard : MonoBehaviour, ICharacterInput {

    public float move
    {
        get
        {
            if(Time.timeScale == 0f)
            {
                return 0f;
            }
            float move = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                move -= 1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += 1f;
            }
            return move;
        }
    }
    public bool jump { get { return Time.timeScale == 0f ? false : Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space); } }
    public bool shooting { get { return Time.timeScale == 0f ? false : Input.GetMouseButton(0); } }
    public Vector2 aimLocation { get { return Time.timeScale == 0f ? (transform.position + transform.right).Vector2() : Camera.main.ScreenToWorldPoint(Input.mousePosition).Vector2(); } }
    public bool grapple { get { return Time.timeScale == 0f ? false : Input.GetMouseButtonDown(1); } }

}
