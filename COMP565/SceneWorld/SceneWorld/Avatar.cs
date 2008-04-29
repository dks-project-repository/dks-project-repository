using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    /// <summary>
    /// A mesh that moves.  The navigatable avatar is a MovableMesh.
    /// Other MovableMeshes could be in the scene as animations (not implemented).
    /// Has two Cameras:  firstPerson and follow. 
    /// Camera avatarCamera references the currently used camera {firstPerson || follow}
    /// Follow camera shows the MovableMesh from behind and up.
    /// </summary>
    public class Avatar : MovableMesh3D
    {
        
        protected Camera avatarCamera, firstPerson, follow, top;
        // use these as constructor arguments ?
        protected Vector3 firstLocation = new Vector3(0.0f, 15.0f, 0.0f);
        protected Vector3 followLocation = new Vector3(0.0f, 25.0f, -60.0f);
        protected Vector3 topLocation = new Vector3(0.0f, 495.0f, 0.0f);
        

        // Constructor
        public Avatar(SceneWorld sw, string label, Vector3 pos, Vector3 orientAxis,
           float radians, string meshFile)
            : base(sw, label, pos, orientAxis, radians, meshFile)
        {
            firstPerson = new Camera(sw, "First   ", firstLocation,
               orientationAxis, orientationRadians);
            avatarCamera = firstPerson;
            follow = new Camera(sw, "Follow  ", followLocation,
               orientationAxis, orientationRadians);
            top = new Camera(sw, "Top     ", topLocation, orientationAxis, orientationRadians);
        }

        // Properties  

        public Camera AvatarCamera
        {
            set { avatarCamera = value; }
        }

        public Camera FirstPerson
        {
            get { return firstPerson; }
        }

        public Camera FollowCamera
        {
            get { return follow; }
        }

        public Camera TopCamera
        {
            get { return top; }
        }



        // Methods

        public override string ToString()
        {
            return Name;
        }

        public void updateCameras()
        {
            Vector3 tFirst, tFollow, tTop;
            tFirst = firstLocation;
            firstPerson.Orientation = Orientation;
            tFirst.TransformCoordinate(Orientation);
            firstPerson.Location = tFirst;
            tFollow = followLocation;
            follow.Orientation = Orientation;
            tFollow.TransformCoordinate(Orientation);
            follow.Location = tFollow;
            tTop = topLocation;
            top.Orientation = Matrix.RotationX((float)Math.PI / 2f) * Orientation;
            tTop.TransformCoordinate(Orientation);
            top.Location = tTop;
        }

        // Avatars use MovableMesh's move()
        public override void move()
        {
            if (Vector3.Length(location - scene.npAvatar.Location) < 30 && name.Equals("Chaser"))
                winner = true;

            if (autoMove && !(this is NPAvatar))
                automove();
            else
            {
                onPath = false;
                base.move();
            }
            updateCameras();
        }

        public override void draw()
        {
            //if (autoMove && !(this is NPAvatar))
            scene.NavGraph.drawPath(path);
            base.draw();
        }

        /* Stuff for automovement */
        public bool autoMove = true;
        protected List<Vector3> path = new List<Vector3>();
        public void clearPath() { path.Clear(); }
        protected bool wander = false;
        protected int currStep = 0;
        protected IndexPair dest;
        // threading stuff
        public bool findingPath = false;
        public int findingPathWaitCount = 0;
        public bool stopFindingPath = false;
        protected int pathIndex = 0, pathDir = 1;
        protected bool onPath = false;

        protected void automove()
        {
            if (++currStep == 12)
            {
                currStep = 0;
                pathIndex += pathDir;
                //findPath();
                lock (this)
                {
                    int c;
                    lock (path)
                        c = path.Count;
                    if (c < 7 && !findingPath)
                    {
                        // if not currently finding a path (or near end of it), find one
                        findingPathWaitCount = 0;
                        stopFindingPath = false;
                        findingPath = true;
                        ThreadPool.QueueUserWorkItem(Avatar.findPath, this);
                    }
                    else
                    {
                        findingPathWaitCount++;
                        if (findingPathWaitCount == 20)
                        {
                            // interrupt current pathfinding attempt if it's taking too long
                            lock (path)
                                path.Clear();
                            stopFindingPath = true;
                        }
                    }
                }
                wander = false; //TODO
                if (wander)
                {
                    steps = 1;
                    yaw = random.Next(0, 2);
                    if (yaw == 0) yaw = -1;
                }
            }

            if (wander)
            {
                if (!onPath)
                {
                    pathIndex = scene.hilbert.closetIndexTo(location);
                    onPath = true;
                }
                followPath(scene.hilbert.Path, pathIndex, ref pathDir);
            }
            else
            {
                onPath = false;
                followPath();
            }
        }

        protected void followPath()
        {
            int dir = 42;
            followPath(path, 0, ref dir);
        }

        public void followPath(List<Vector3> path, int index, ref int dir)
        {
            lock (path)
            {
                if (dir == 42 && currStep == 0 && path.Count > 0)
                    path.RemoveAt(0);

                if (dir > 0 && path.Count >= index + 4 || dir < 0 && index >= 3)
                {
                    Vector3 v;
                    if (dir > 0)
                        v = Vector3.CatmullRom(path[index], path[index + 1], path[index + 2], path[index + 3], currStep / 12f);
                    else
                        v = Vector3.CatmullRom(path[index], path[index - 1], path[index - 2], path[index - 3], currStep / 12f);
                    Vector3 newAt = v - location;
                    if (newAt.LengthSq() != 0)
                    {
                        At = Vector3.Normalize(v - location);
                        Right = Vector3.Cross(Up, At);
                    }
                    Location = v;
                }
                else
                {
                    // we ran out of path
                    if (dir == 42)
                    {
                        path.Clear();
                        wander = true;
                    }
                    else
                        dir = -dir;
                }
            }

            if (!name.Contains("flock"))
            {
                IndexPair temp;
                if ((temp = scene.Treasures.treasureWithin(Location, 30)) != null)
                {
                    scene.Treasures.collectTreasure(temp);
                    scene.Trace = name + " collected a treasure: " + ++treasureCount + " collected out of 4";

                    lock (path)
                    path.Clear();

                    if (treasureCount > scene.Treasures.TreasureCount / 2)
                        winner = true;
                }
            }
        }

        public Vector3 getWithin(Vector3 loc, float dist)
        {
            if ((location - loc).Length() <= dist)
                return location;
            return Vector3.Empty;
        }

        // A* setup
        protected static void findPath(object o)
        {
            Avatar a = (Avatar)o;

            IndexPair dest = null;

            float yon = 500;

            // First check to see if we're near the other avatar
            if (a == a.scene.avatar)
            {
                Vector3 targetLoc = a.scene.npAvatar.getWithin(a.location, yon);
                if (targetLoc != Vector3.Empty)
                    dest = NavGraph.indexFromLocation(targetLoc);
            }

            // If not, check to see if we're near a treasure
            if (dest == null)
            {
                IndexPair ip = a.scene.Treasures.treasureWithin(a.location, yon);
                if (ip != null)
                    dest = ip;
            }

            // If not, give up
            if (dest == null)
            {
                // target not within yon. don't even try finding it.
                lock (a)
                {
                    lock (a.path)
                        a.path.Clear();
                    a.wander = true;
                    a.findingPath = false;
                }
                return;
            }

            // Otherwise, do pathfinding
            AStar(a, dest, yon);
        }

        protected static void AStar(Avatar a, IndexPair dest, float yon)
        {
            SortedList<IndexPair, bool> open = new SortedList<IndexPair, bool>(new IndexPair.Comparer());
            SortedList<IndexPair, bool> closed = new SortedList<IndexPair, bool>(new IndexPair.Comparer());
            List<Vector3> path = new List<Vector3>();

            // convert yon to index coords
            yon /= 10;

            IndexPair curr = NavGraph.indexFromLocation(a.location), next;
            curr.cost = 0;
            curr.dir = NavGraph.directionFromVector(a.At);
            IndexPair source = curr;
            open.Add(curr, true);

            while (open.Count > 0)
            {
                // allow pathfinding to be interrupted by parent thread
                lock (a)
                    if (a.stopFindingPath)
                    {
                        open.Clear();
                        closed.Clear();
                        path.Clear();
                        a.findingPath = false;
                        return;
                    }

                // get first indexpair in the list
                curr = open.Keys[0];
                /*if (curr.sourceCost > yon)
                {
                    // no path found! wander aimlessly
                    lock (a)
                    {
                        a.wander = true;
                        a.findingPath = false;
                    }
                    open.Clear();
                    closed.Clear();
                    path.Clear();
                    return;
                }
                else*/
                if (curr.Equals(dest))
                {
                    // we now have a path

                    // stick it in a list
                    curr.backtrack(path);

                    lock (a.path)
                    {
                        if (a.path.Count == 0 || a.path.Count > path.Count)
                        {
                            //a.path.Clear();
                            //a.path = path;
                            if (a.path.Count != 0)
                            {
                                // If we're already following a path we can just append to it, so delete uneeded first/last nodes
                                a.path.RemoveAt(a.path.Count - 1);
                                path.RemoveAt(0);
                            }
                            else
                            {
                                // add Catmull-Rom first & last points
                                path.Insert(0, a.location);
                                path.Add(path[path.Count - 1] - (path[path.Count - 1] - path[path.Count - 2]));
                            }
                            a.path.AddRange(path);
                        }
                    }
                    lock (a)
                    {
                        a.findingPath = false;
                        a.wander = false;
                    }
                    open.Clear();
                    closed.Clear();

                    return;
                }
                else
                {
                    // keeping looking
                    open.RemoveAt(0);
                    closed.Add(curr, true);
                    int nextDir = curr.dir - 1;
                    if (nextDir < 0)
                        nextDir += 8;
                    int nodesAdded = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        next = NavGraph.indexAt(curr, nextDir);
                        if (a.scene.NavGraph.isTraversable(next))
                        {
                            // source cost
                            next.cost = next.sourceCost = curr.sourceCost +
                                    (nextDir % 2 == 0 ? 1 : 1.41421356237f);

                            // Don't add node if too far away
                            if (IndexPair.dist(source, next) > yon)
                                continue;

                            // heuristic
                            next.cost += IndexPair.dist(dest, next);
                            if (!open.ContainsKey(next) && !closed.ContainsKey(next))
                            {
                                open.Add(next, true);
                                nodesAdded++;
                            }
                        }

                        nextDir++;
                        if (nextDir >= 8)
                            nextDir -= 8;

                        if (i == 2 && curr == source && nodesAdded == 0)
                        {
                            // If stuck, allow to go backwards
                            i--;
                            nextDir = (curr.dir + 4) % 8;
                        }
                    }
                }
            }

            // no path found! wander aimlessly
            lock (a)
            {
                a.wander = true;
                a.findingPath = false;
            }
            closed.Clear();
            path.Clear();
        }
    }
}
