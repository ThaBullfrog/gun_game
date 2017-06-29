using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputKeyboard : MonoBehaviour, ICharacterInput {

    public float move
    {
        get
        {
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
    public bool jump { get { return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space); } }
    public bool shooting { get { return Input.GetMouseButton(0); } }
    public Vector2 aimLocation { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition).Vector2(); } }
    public bool grapple { get { return Input.GetMouseButtonDown(1); } }

}
