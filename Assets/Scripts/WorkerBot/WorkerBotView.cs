using UnityEngine;
using Zenject;

namespace WorkerBot
{
    public class WorkerBotView : MonoBehaviour
    {


        public class Factory : PlaceholderFactory<InitArgs, WorkerBotView>
        {
        }

        public class InitArgs
        {
            public Vector3 Position { get; private set; }
            public float MoveSpeed { get; private set; }

            public InitArgs(Vector3 position, float moveSpeed)
            {
                Position = position;
                MoveSpeed = moveSpeed;
            }
        }
    }
}