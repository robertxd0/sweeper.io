using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create hexagon helper instance", fileName = "HexagonHelperInstance")]
public class HexagonHelper : ScriptableObject
{
   public GameObject hexagonPrefab = null;
   
   internal float? hexagonWidth;

   internal float? hexagonDepth;

   public GameObject SpawnHexagon()
   {
      var hexagonInstance = Instantiate(hexagonPrefab);

      if(hexagonWidth == null || hexagonDepth == null)
      {
         BoxCollider hexagonCollider = hexagonInstance.GetComponent<BoxCollider>();

         hexagonWidth = hexagonCollider.bounds.size.x;
         hexagonDepth = hexagonCollider.bounds.size.y;
      }

      return Instantiate(hexagonInstance);
   }
}