using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public List<GameObject> projectilesVariants;
    public int currentVariant = 0;

    public void Fire() {
        GameObject proj = Instantiate(projectilesVariants[currentVariant], transform.position, transform.rotation);
    }
}
