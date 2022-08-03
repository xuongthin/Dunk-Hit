using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSChecker : MonoBehaviour
{
    Text display;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 1f;

    private void Start()
    {
        display = GetComponent<Text>();
    }

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            display.text = m_frameCounter.ToString();
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
    }
}
