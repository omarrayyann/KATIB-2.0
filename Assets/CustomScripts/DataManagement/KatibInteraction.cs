using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text;

public class KatibInteraction : MonoBehaviour
{
    public string port = "COM7";
    public int baudRate = 115200;

    private SerialPort sp;

    private bool isStreaming = false;

    public List<KatibInformation> katibInformation = new List<KatibInformation>();
    public int currentSet = 0;
    public int currentPoint = 0;
    public bool calibrated { get; set; }
    [SerializeField]
    public int xs { get; set; }
    [SerializeField]
    public int xl { get; set; }
    [SerializeField]
    public int ys { get; set; }
    [SerializeField]
    public int yl { get; set; }

    /// <summary>
    /// Opens the serial port connection with the Katib device and calls the SetUp function
    /// </summary>
    public void Open()
    {
        isStreaming = true;
        sp = new SerialPort(port, baudRate);
        sp.Open();

        sp.ReadTimeout = 100;
        // Flush how?
        //sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$X\n")));
        //sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$H\n")));
        StartCoroutine(Calibrate());
        Debug.Log("Here");

    }

    /// <summary>
    /// Callibration procedure
    /// </summary>
    /// <returns></returns>
    IEnumerator Calibrate()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Start");
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$X\n")));
        Debug.Log(sp.ReadLine());
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$H\n")));
        
        yield return new WaitForSeconds(30);

        yield return new WaitForSeconds(1);
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G10 P1 L20 X0 Y0 Z0\n")));
        Debug.Log(sp.ReadLine());
        yield return new WaitForSeconds(0.1f);
        // sp.WriteLine("G10 P1 L20 \n");
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G21 X80 Y-10 Z0 F4000\n")));
        //Debug.Log(sp.ReadLine());
        yield return new WaitForSeconds(0.1f);
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G10 P1 L20 X0 Y0 Z0\n")));
        //Debug.Log(sp.ReadLine());
        yield return new WaitForSeconds(0.1f);
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G21 X0 Y0 Z72.5 F4000\n")));
        yield return new WaitForSeconds(2);
        // sp.WriteLine("$X\n");
        // Debug.Log(sp.ReadLine());
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G21 X0 Y-95 F4000\n")));
        // Debug.Log(sp.ReadLine());
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$X\n")));
        // yield return new WaitForSeconds(0.1f);
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G21 X143 Y-95 F4000\n")));
        // Debug.Log(sp.ReadLine());
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$X\n")));
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G21 X0 Y0 F4000\n")));
        // Debug.Log(sp.ReadLine());
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$X\n")));
        // yield return new WaitForSeconds(0.1f);
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("G21 X0 Y0 Z72 F4000\n")));
        // Debug.Log(sp.ReadLine());
        // yield return new WaitForSeconds(0.1f);
        // sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes("$X\n")));
        // yield return new WaitForSeconds(2f);
    }

    /// <summary>
    /// Sends the next point through the open port
    /// </summary>
    public void SendNextPoint()
    {
        float xp0 = katibInformation[currentSet].points[currentPoint][0];
        float yp0 = katibInformation[currentSet].points[currentPoint][1];
        Vector2 vc = GetCoords(xp0, yp0);
        Debug.Log("pixels x " + xp0 + " y " + yp0);
        Debug.Log(vc);
        string gcodeString = "G21 X" + vc.x.ToString("n3") + " Y" + vc.y.ToString("n3") + " F4000\n";
        print(gcodeString);
        sp.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes(gcodeString)));
        if (currentPoint < katibInformation[currentSet].points.Count - 1)
        {
            currentPoint++;
        }
        else if (currentSet < katibInformation.Count - 1)
        {
            currentSet++;
        }
    }


    /// <summary>
    /// Transforms the cm coordinates into pixel coordinates
    /// </summary>
    /// <param name="xn">Original x coordinate</param>
    /// <param name="yn">Original y coordinate</param>
    /// <returns>Updated position Vector2</returns>
    private Vector2 GetCoords(float xn, float yn)
    {
        Debug.Log(" xN: " + xn + " yN: " + yn);
        float fx, fy;
        if (xn < xs + xl && xn >= xs)
        {
            xn = (xn - xs) / xl;
            fx = 143 * xn;
        }
        else
            return new Vector2(xs, ys);
        if (yn < ys + yl && yn >= ys)
        {
            yn = (yn - ys) / yl;
            fy = -95 * yn;
        }
        else
            return new Vector2(xs, ys);
        return new Vector2(fx, fy);
    }

    /// <summary>
    /// Closes the connection with the Katib device
    /// </summary>
    public void EndConnection()
    {
        sp.Close();
    }
}
