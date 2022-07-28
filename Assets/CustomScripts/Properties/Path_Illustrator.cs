using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Illustrator : MonoBehaviour
{
      [SerializeField]
    public LineRenderer lineRenderer;


    // Update is called once per frame
    void Update()
    {
        DrawLine(Data.collectables[Data.collectables.Count - 1].path);
    }

    void DrawLine(List<Vector3> vertexPositions)
    {
        
        lineRenderer.positionCount = vertexPositions.Count;
        lineRenderer.SetPositions(vertexPositions.ToArray());
    }

   /* public void AnimatePoints()
    {
        GameObject PreviousAnimator = GameObject.FindGameObjectWithTag("Animator");
        if (PreviousAnimator == null)
        {
            GameObject Animator = new GameObject("Animator");
            AnimatedWriting animatedWriting = Animator.AddComponent<AnimatedWriting>();
            Animator.tag = "Animator";
            animatedWriting.brushPrefab = animatorBrushPrefab;
            animatedWriting.startPos = new Vector2(-this.gameObject.transform.localScale[0] / 2 + padding, this.gameObject.transform.localScale[1] / 2 - padding);
            animatedWriting.LoadWithPoints(strokes);
        }
        else
        {
            PreviousAnimator.GetComponent<AnimatedWriting>().TogglePause();
        }
    }*/

}
