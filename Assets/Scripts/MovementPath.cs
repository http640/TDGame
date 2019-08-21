using UnityEngine;

public class MovementPath : MonoBehaviour
{
    public Transform[] PathSequence; //Array of all points in the path
 
    public void OnDrawGizmos()
    {
        if(PathSequence == null || PathSequence.Length < 2)
        {
            return; //Exits OnDrawGizmos if no line is needed
        }

        //Loop through all of the points in the sequence of points
        for(var i=1; i < PathSequence.Length; i++)
        {
            //Draw a line between the points
            Gizmos.DrawLine(PathSequence[i - 1].position, PathSequence[i].position);
        }
    }
}
