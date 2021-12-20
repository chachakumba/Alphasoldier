using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeapon : Weapon
{
    public override void Shoot()
    {
        logic.SetWaitTime(reloadTime);
        Manager.instance.ShakeSkreen(shakePower, shakeDuration);
        source.clip = shootSound;
        source.Play();


        LineRenderer line;
        RaycastHit2D[] hit = new RaycastHit2D[1];
        //hit = Physics2D.Raycast(shootPoint.position, -shootPoint.up, maxDistance, Manager.instance.floorLayer + logic.enemyLayer);
        line = Manager.instance.GetShootLine();
        line.SetPosition(0, shootPoint.position);
        StartCoroutine(ChangeLine(line));
        if (Physics2D.Raycast(shootPoint.position, -shootPoint.up, logic.floorAndEnemyFilter, hit, maxDistance) > 0)
        {
            line.SetPosition(1, shootPoint.position + -shootPoint.up * hit[0].distance);
            if (hit[0].collider.gameObject.GetComponent<IHaveHP>() != null)
            {
                hit[0].collider.gameObject.GetComponent<IHaveHP>().GetDamage(damage);
            }

            if (hit[0].collider.gameObject.CompareTag("Wall"))
            {
                ParticleSystem particle = Instantiate(Manager.instance.particlePrefab);
                particle.transform.position = hit[0].point;
                Destroy(particle.gameObject, particle.main.duration);
            }
            else if (hit[0].collider.gameObject.CompareTag("Entity"))
            {
                ParticleSystem particle = Instantiate(Manager.instance.particlePrefab);
                particle.startColor = Color.red;
                particle.transform.position = hit[0].point;
                Destroy(particle.gameObject, particle.main.duration);
            }
        }
        else
        {
            line.SetPosition(1, shootPoint.position + shootPoint.up * -maxDistance);
        }

        Manager.instance.ReturnLine(line, timeToFade);
    }
    IEnumerator ChangeLine(LineRenderer line)
    {
        if (line != null)
        {
            //line.startWidth = 0.7f; lazer weapon
            for (int i = 0; i < 9; i++)
            {
                if (line != null)
                {
                    line.endWidth += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                else break;
            }
            for (int i = 0; i < 9; i++)
            {
                if (line != null)
                {
                    line.endWidth -= 0.02f;
                    yield return new WaitForSeconds(0.01f);
                }
                else break;
            }
            line.endWidth = 0;
        }
    }
}
