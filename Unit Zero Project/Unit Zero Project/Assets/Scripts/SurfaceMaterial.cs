using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMaterial : MonoBehaviour {
    public enum Surface {
        grass,
        concrete,
        metal, 
        wood
    }

    public Surface thisSurface = Surface.grass;
}
