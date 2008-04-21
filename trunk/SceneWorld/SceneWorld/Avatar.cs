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

/// <summary>
/// A mesh that moves.  The navigatable avatar is a MovableMesh.
/// Other MovableMeshes could be in the scene as animations (not implemented).
/// Has two Cameras:  firstPerson and follow. 
/// Camera avatarCamera references the currently used camera {firstPerson || follow}
/// Follow camera shows the MovableMesh from behind and up.
/// </summary>
public class Avatar : MovableMesh3D {
   protected Camera avatarCamera, firstPerson, follow;
   // use these as constructor arguments ?
   protected Vector3 firstLocation =  new Vector3(0.0f, 15.0f, 0.0f);     
   protected Vector3 followLocation = new Vector3(0.0f, 50.0f, -100.0f);

   // Constructor
   public Avatar(SceneWorld sw, string label, Vector3 pos, Vector3 orientAxis, 
      float radians, string meshFile)
      : base(sw, label, pos, orientAxis, radians, meshFile) 
      {
      firstPerson = new Camera(sw,  "First   ", firstLocation,
         orientationAxis, orientationRadians);
      avatarCamera = firstPerson;
      follow = new Camera(sw, "Follow  ", followLocation,
         orientationAxis, orientationRadians);     
      } 
   
   // Properties  
   
   public Camera AvatarCamera {
      set { avatarCamera = value;}}

   public Camera FirstPerson {
      get { return firstPerson; }}

   public Camera FollowCamera {
      get { return follow; }}
            
   // Methods

   public override string ToString() {
      return Name;
      }
      
   public void updateCameras() {
      Vector3 tFirst, tFollow;
      tFirst = firstLocation;
      firstPerson.Orientation = Orientation;
      tFirst.TransformCoordinate(Orientation); 
      firstPerson.Location = tFirst;
      tFollow = followLocation;
      follow.Orientation = Orientation;
      tFollow.TransformCoordinate(Orientation);
      follow.Location = tFollow;
      }
      
   // Avatars use MovableMesh's move()
   public override void move() {
      base.move();     
      updateCameras();      
      }     
}}
