using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "AI/States/Attacks/BlightBreath")]
    public class BlightBreath : BaseAttackState
    {
        Transform bossTransform;

        [SerializeField]
        float exhaleRange = 10f;
        [SerializeField]
        float exhaleAngle = 90f;
        [SerializeField]
        float hazardDuration = 5f; 
        [SerializeField]
        LayerMask targetLayer;

        /// <summary>
        /// Reference to the toxic gas particle system prefab
        /// </summary>
        [SerializeField]
        GameObject gasParticlesPrefab;

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();

            if (!isOnCooldown() && !isActive)
            {
                Activate();
                visualizeAbility();
            }

            if (checkCooldownStateChange())
            {
                Debug.Log("Cooldown done: Toxic Exhale");
                isActive = false;
                stateToReturn = nextState == null ? stateToReturn : nextState;
            }

            checkIfCooldownNeedReset(stateToReturn);
            return stateToReturn;
        }

        public override void Activate()
        {
            Debug.Log($"{abilityName} activated!");

            Collider[] hits = Physics.OverlapSphere(bossTransform.position, exhaleRange, targetLayer);
            foreach (var hit in hits)
            {
                Vector3 directionToTarget = (hit.transform.position - bossTransform.position).normalized;
                float angle = Vector3.Angle(bossTransform.forward, directionToTarget);

                if (angle <= exhaleAngle / 2)
                {
                    // Apply toxic gas effect or damage
                    Debug.Log($"{hit.name} is in the toxic gas!");
                    applyHazardEffect(hit.gameObject);
                }
            }

            isActive = true;
            startCooldown();
        }

        private void applyHazardEffect(GameObject target)
        {
            // TODO: Add logic to apply damage-over-time or debuff to the player
            Debug.Log($"Applying toxic effect to {target.name}");
        }

        protected override void visualizeAbility()
        {
            spawnGasCloudEffect();
        }

        private void spawnGasCloudEffect()
        {
            if (gasParticlesPrefab)
            {
                GameObject gasEffect = Instantiate(gasParticlesPrefab);
                gasEffect.transform.position = bossTransform.position + bossTransform.forward * (exhaleRange / 2);
                gasEffect.transform.rotation = bossTransform.rotation;
                gasEffect.transform.localScale = new Vector3(exhaleRange, exhaleRange / 2, exhaleRange);

                Destroy(gasEffect, hazardDuration);
            }
            else
            {
                Debug.LogError("GasParticles prefab is null!");
            }
        }

        public override void resetValues()
        {
            resetCooldown();
        }
    }
}
