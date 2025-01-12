using UnityEngine;
using UnityEngine.VFX;

public class EnemyAnimationAndEffects : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnSwipeEffect(GameObject slashParticles, float attackRadius = 5f)
    {
        if (slashParticles)
        {
            GameObject tmpSlash = GameObject.Instantiate(slashParticles);
            tmpSlash.transform.position = transform.position;

            float effectScale = attackRadius / 5f;
            tmpSlash.transform.localScale = new Vector3(effectScale, effectScale, effectScale);

            VisualEffect visualEffect = tmpSlash.GetComponentInChildren<VisualEffect>();
            if (visualEffect != null)
            {
                float effectDuration = 0.3f;
                Destroy(tmpSlash, effectDuration + 1);
            }
            else
            {
                Destroy(tmpSlash, 5f);
            }
        }
        else
        {
            Debug.LogError("SlashParticles prefab is null!");
        }
    }

    public void spawnSwipingAttackVfx(GameObject slashParticles)
    {
        spawnSwipeEffect(slashParticles);
    }
}
