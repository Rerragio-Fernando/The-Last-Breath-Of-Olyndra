using System.Collections.Generic;
using UnityEngine;
using System.Linq;





namespace NPC.Brain.Test
{
    public class Replay
    {
        public List<double> states;
        public double reward;

        public Replay(double xr, double ballz, double ballvx, double r)
        {
            states = new List<double>();
            states.Add(xr);
            states.Add(ballz);
            states.Add(ballvx);
            reward = r;
        }
    }

    public class BalanceBallBrain : MonoBehaviour
    {
        //object to monitor
        public GameObject ball; 

        
        ANN ann;
        // reward associated with actions
        float reward = 0.0f;
        // memory - list of past actions and rewards
        List<Replay> replayMemory = new List<Replay>();
        // memory capacity
        int memoryCapacity = 1000;

        // how much future states affects rewards
        float discount = 0.99f;
        // chance of picking random action
        float exploreRate = 100.0f;
        // max chance value
        float maxExplorerRate = 100.0f;
        // min chance value
        float minExploreRate = 0.01f;
        //chance decay amount for each update
        float exploreDecay = 0.0001f;

        // record of the ball start position
        Vector3 ballStartPos;
        // count of the number of time the ball was dropped
        int failCount = 0;
        // max angle to apply to tilting each update, needs to be large enough so that the q value multiplied 
        // by it is enough to recover balance
        float tiltSpeed = 0.5f;

        // keep track of the time the ball is kept on balance
        float timer = 0;
        // keep track of the the max time the ball was kept on balance
        float maxBalanceTime = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ann = new ANN(3, 2, 1, 6, 0.2f);
            ballStartPos = ball.transform.position;
            Time.timeScale = 5.0f;
        }

        GUIStyle guiStyle = new GUIStyle();
        private void OnGUI()
        {
            guiStyle.fontSize = 25;
            guiStyle.normal.textColor = Color.white;
            GUI.BeginGroup(new Rect(10, 10, 600, 150));
            GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
            GUI.Label(new Rect(10, 25, 500, 30), "Fails: "+failCount, guiStyle);
            GUI.Label(new Rect(10, 50, 500, 30), "Decay Rate: " + exploreRate, guiStyle);
            GUI.Label(new Rect(10, 75, 500, 30), "Last Best Balance: " + maxBalanceTime, guiStyle);
            GUI.Label(new Rect(10, 100, 500, 30), "This Balance: " + timer, guiStyle);
            GUI.EndGroup();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown("space"))
            {
                resetBall();
            }
        }

        private void FixedUpdate()
        {
            timer += Time.deltaTime;
            List<double> states = new List<double>();
            List<double> qs = new List<double>();

            states.Add(this.transform.rotation.x);
            states.Add(ball.transform.position.z);
            states.Add(ball.GetComponent<Rigidbody>().angularVelocity.x);

            qs = softMax(ann.calcOutput(states));
            double maxQ = qs.Max();
            int maxQIndex = qs.ToList().IndexOf(maxQ);
            exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExplorerRate);

            //if(Random.Range(0,100) < exploreRate)
            //	maxQIndex = Random.Range(0,2);

            if (maxQIndex == 0)
            {
                this.transform.Rotate(Vector3.right, tiltSpeed * (float)qs[maxQIndex]);
            }
            else if (maxQIndex == 1)
            {
                this.transform.Rotate(Vector3.right, -tiltSpeed * (float)qs[maxQIndex]);
            }
                

            if (ball.GetComponent<BallState>().dropped)
            {
                reward = -1.0f;
            }
            else
            {
                reward = 0.1f;
            }    
                

            Replay lastMemory = new Replay(this.transform.rotation.x,
                                    ball.transform.position.z,
                                    ball.GetComponent<Rigidbody>().angularVelocity.x,
                                    reward);

            if (replayMemory.Count > memoryCapacity)
            {
                replayMemory.RemoveAt(0);
            }
                

            replayMemory.Add(lastMemory);

            if (ball.GetComponent<BallState>().dropped)
            {
                for (int i = replayMemory.Count - 1; i >= 0; i--)
                {
                    List<double> toutputsOld = new List<double>();
                    List<double> toutputsNew = new List<double>();
                    toutputsOld = softMax(ann.calcOutput(replayMemory[i].states));

                    double maxQOld = toutputsOld.Max();
                    int action = toutputsOld.ToList().IndexOf(maxQOld);

                    double feedback;
                    if (i == replayMemory.Count - 1 || replayMemory[i].reward == -1)
                    {
                        feedback = replayMemory[i].reward;
                    }
                    else
                    {
                        toutputsNew = softMax(ann.calcOutput(replayMemory[i + 1].states));
                        maxQ = toutputsNew.Max();
                        feedback = (replayMemory[i].reward +
                            discount * maxQ);
                    }

                    toutputsOld[action] = feedback;
                    ann.Train(replayMemory[i].states, toutputsOld);
                }

                if (timer > maxBalanceTime)
                {
                    maxBalanceTime = timer;
                }

                timer = 0;

                ball.GetComponent<BallState>().dropped = false;
                this.transform.rotation = Quaternion.identity;
                resetBall();
                replayMemory.Clear();
                failCount++;
            }
        }

        void resetBall()
        {
            ball.transform.position = ballStartPos;
            ball.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
            ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }

        List<double> softMax(List<double> values)
        {
            double max = values.Max();

            float scale = 0.0f;
            for (int i = 0; i < values.Count; ++i)
            {
                scale += Mathf.Exp((float)(values[i] - max));
            }
                

            List<double> result = new List<double>();
            for (int i = 0; i < values.Count; ++i)
            {
                result.Add(Mathf.Exp((float)(values[i] - max)) / scale);
            }

            return result;
        }
    }
}
