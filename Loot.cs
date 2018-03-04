using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.Collections;
using Fougerite;

namespace ExtraLoot
{
    public class Loot : MonoBehaviour
    {
        public static float AjusteEjeY = 1.6f;
        
        void Start()
        {
            StartCoroutine(ZonasExtraLoot());
        }
        IEnumerator ZonasExtraLoot()
        {
            //ESPARER ANTES COMENZAR
            yield return new WaitForSeconds(120); // 20mins * 60 = 1800

            Logger.Log("Adding Loot ----- Start Loop IEnumerator");
            foreach (var x in ExtraLoot.AreasDeLoot)
            {
                var pos = Util.GetUtil().ConvertStringToVector3(x);
                var posfinal = new Vector3(pos.x, pos.y - AjusteEjeY, pos.z);
                //Borrar caja para evitar que se superpongan
                var obj = Physics.OverlapSphere(posfinal, 2);
                foreach (var xs in obj)
                {
                    if (xs.name.ToLower().Contains("box"))
                    {
                        Util.GetUtil().DestroyObject(xs.gameObject);
                    }
                }
                //Poner Caja Neva
                yield return new WaitForSeconds(1);
                World.GetWorld().Spawn("BoxLoot", posfinal);
            }

            Logger.Log("Adding Loot ----- END");
            //ESPARER ANTES COMENZAR REPETIR
            yield return new WaitForSeconds(1800); // 20mins * 60 = 1800
            //REPETIR RUTINA
            StartCoroutine(ZonasExtraLoot());
        }
    }
}
