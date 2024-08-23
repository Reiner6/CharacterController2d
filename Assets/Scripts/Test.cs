using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
   async void Start()
    {
        await Execute();
    }

    private static async Task Execute()
    {
        Debug.Log("before");

        RunLogCycle();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"2 {i}");

            await Task.Delay(10);
        }
        Debug.Log("after");

    }

    private static async Task RunLogCycle()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"1 {i}");

            await Task.Delay(10);
        }
    }

}
