using System.Collections;
using Main;
using UnityEngine;
using Zenject;

namespace WorkerBot
{
    public class WorkerBotView : MonoBehaviour
    {
        private IEnumerator _actualMove;
        private SignalBus _signalBus;
        
        public float moveSpeed;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void MoveTo(Vector3 position)
        {
            if (_actualMove != null) StopCoroutine(_actualMove);
            _actualMove = Move(position);
            StartCoroutine(_actualMove);
        }


        private IEnumerator Move (Vector3 targetPos)
        {
            while (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    moveSpeed * Time.deltaTime);
                
                yield return new WaitForFixedUpdate();
            }

            _signalBus.Fire(new WorkerBotMoveFinishedSignal(this));
        }


        public class Factory : PlaceholderFactory<WorkerBotView>
        {
        }

    }
}