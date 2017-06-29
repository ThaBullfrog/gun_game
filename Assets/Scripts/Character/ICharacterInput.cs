using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterInput {

	float move { get; }
    bool jump { get; }
    bool shooting { get; }
    Vector2 aimLocation { get; }
    bool grapple { get; }

}
