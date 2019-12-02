using UnityEngine;
using System.Collections;

public class ToyCar : MonoBehaviour
{
    private int m_LastIndex;

    public void PlayToyCarAnim()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            if (m_LastIndex == 0)
            {
                GetComponent<Animation>().Play("ToyCar");
                m_LastIndex = 1;
            }
            else
            {
                GetComponent<Animation>().Play("ToyCar");
                m_LastIndex = 0;
            }
        }
    }
}