/*Daniel Frankel
 * Kevin Yedlin
 * Comp 565 - Project 1
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld {

public class NPAvatar : Avatar {
   private int remoteX, remoteY, remoteTurns = 0;
   
   // Constructor

   public NPAvatar(SceneWorld sw, string label, Vector3 pos, Vector3 orientAxis, 
      float radians, string meshFile)
      : base(sw, label, pos, orientAxis, radians, meshFile)
      {  // change names for on-screen display of current camera
      firstPerson.Name = "npFirst ";
      follow.Name = "npFollow";
      }   

   // Methods 
   
   /// <summary>
   /// Set Steps and Yaws randomly for remote players. 
   /// This is a "temporary defintion" for NPC phases
   /// </summary>
   public override void move() {
      remoteTurns++;
      if (remoteTurns > 25) {
         remoteTurns = 0;
         remoteX =  random.Next(2);       // 0..1 move forward only;
         remoteY = -1 + random.Next(3);   // turn left or right ;
         }
      steps += remoteX;
      yaw += remoteY;         // always turn
      base.move();            // now use MovableMesh's move via Avatar's move();
      }
   }
}
