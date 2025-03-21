using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public List<int[]> PlaceEnemyShips()
    {
        List<int[]> enemyShips = new List<int[]>
        {
            new int[] { -1, -1, -1, -1, -1 },
            new int[] { -1, -1, -1, -1 },
            new int[] { -1, -1, -1, -1, -1, -1 },
            new int[] { -1, -1, -1, -1 },
            new int[] { -1, -1, -1, -1 }
        };
        int[] gridNumbers = Enumerable.Range(1, 100).ToArray();
        bool taken = true;

        foreach (int[] tileNumArray in enemyShips)
        {
            taken = true;
            while (taken == true)
            {
                taken = false;
                int shipNose = UnityEngine.Random.Range(0, 99);
                int rotateBool = UnityEngine.Random.Range(0, 2);
                int minusAmount = rotateBool == 0 ? 10 : 1;

                for (int i = 0; i < tileNumArray.Length; i++)
                {

                    if ((shipNose - (minusAmount * i)) < 0 || gridNumbers[shipNose - i * minusAmount] < 0)
                    {
                        taken = true;
                        break;
                    }
                    else if (minusAmount == 1 && shipNose / 10 != ((shipNose - i * minusAmount) / 10))
                    {
                        taken = true;
                        break;
                    }
                }
                if (taken == false)
                {
                    for (int j = 0; j < tileNumArray.Length; j++)
                    {
                        tileNumArray[j] = gridNumbers[shipNose - j * minusAmount];
                        gridNumbers[shipNose - j * minusAmount] = -1;
                    }
                }
            }
        }
        foreach (int[] numArray in enemyShips)
        {
            string temp = "";
            for (int i = 0; i < numArray.Length; i++)
                temp += ", " + numArray[i];
            Debug.Log(temp);
        }

        return enemyShips;
    }
}
