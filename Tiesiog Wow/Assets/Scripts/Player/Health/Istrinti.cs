using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Istrinti : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
