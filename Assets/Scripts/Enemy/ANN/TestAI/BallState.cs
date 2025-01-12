using UnityEngine;

namespace NPC.Brain.Test
{
    public class BallState : MonoBehaviour
    {

        public bool dropped = false;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<testBallDrop>())
            {
                dropped = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<testBallDrop>())
            {
                dropped = true;
            }
        }
    }
}
