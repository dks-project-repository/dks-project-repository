using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game465P3
{
    public class Avatar : Movable
    {
        public enum Team { Red, Blue };
        public float yaw = MathHelper.PiOver2, pitch = MathHelper.PiOver2;
        public Vector3 actualAt;
        public float jetFuel = 1;
        protected bool jetting = false;
        public LinkedCamera camera;
        protected LinkedList<Type> weapons;

        protected float health = 1;
        public float Health
        {
            get
            {
                return health;
            }
            set
            {
                if (health > 0)
                {
                    if (value <= 0)
                    {
                        health = 0;
                        jetFuel = 0;
                        skiing = false;
                        dead = true;
                        deadCounter = 0;
                        game.setDamage(1);
                        acceleration = -gravity;
                        tractionForce = Vector3.Zero;
                        game.oct.Remove(this);
                    }
                    else
                    {
                        game.setDamage(health - value);
                        health = value;
                    }
                }
            }
        }
        public float getHealth() { return health; }
        public float getJet() { return jetFuel; }

        protected bool dead;
        protected int deadCounter;
        protected Vector3 origLocation;

        private static readonly float jetHorizontal = Settings.jetPackLateralProportion * Settings.jetPackAccel;
        private static readonly float jetVertical = (float)Math.Sqrt(1 - Settings.jetPackLateralProportion * Settings.jetPackLateralProportion) * Settings.jetPackAccel;

        Team team;

        public Avatar(World game, Vector3 location, Model model)
            : base(game, location, model)
        {
            camera = new LinkedCamera(game, this);

            location.Y = game.terrain.GetHeight(location) + Settings.collisionHeightDisplacement;
            transform.Translation = location;
            update();
            //createBoundingBox();
            origLocation = transform.Translation;

            weapons = new LinkedList<Type>();
            weapons.AddLast(typeof(StraightProjectile));
            weapons.AddLast(typeof(LobProjectile));
        }

        public override void update()
        {
            if (dead)
            {
                actualAt = game.input.handleRotation(ref yaw, ref pitch);
                camera.zoom = Settings.minZoom * 2;

                base.update();

#if XBOX360
                if (++deadCounter > Settings.deadMinTicks && game.input.IsButtonPressed(Settings.fireButton))
#else
                if (++deadCounter > Settings.deadMinTicks && game.input.LeftMouseClick)
#endif
                {
                    respawn();
                }

                return;
            }

            if (game.input.IsKeyPressed(Settings.killSelf))
                Health = 0;

            // Switch weapons
            if (game.input.IsKeyPressed(Settings.switchWeapon) || game.input.IsButtonPressed(Settings.switchWeaponButton))
            {
                Type t = weapons.First.Value;
                weapons.RemoveFirst();
                weapons.AddLast(t);
            }


            // Shooting
#if XBOX360
            if (game.input.IsButtonPressed(Settings.fireButton))
#else
            if (game.input.LeftMouseClick || game.input.IsButtonPressed(Settings.fireButton))
#endif
            {
                Type t = weapons.First.Value;
                if (t == typeof(StraightProjectile))
                    new StraightProjectile(game, this);
                else if (t == typeof(LobProjectile))
                    new LobProjectile(game, this);
            }

            // TODO: eventually remove this testing code
            if (game.input.IsKeyPressed(Keys.F8))
            {
                Vector3 w = transform.Translation;
                Vector3 v = transform.Translation;
                v.Y += 100;
                Vector3 aAt = actualAt;
                actualAt = Vector3.Normalize(Vector3.Down + aAt);
                for (int i = -5; i < 5; i++)
                {
                    for (int j = -5; j < 5; j++)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            v.X = w.X + i * 300;
                            v.Y = w.Y + k * 300;
                            v.Z = w.Z + j * 300;
                            transform.Translation = v;
                            new LobProjectile(game, this);   
                        }
                    }
                }
                transform.Translation = w;
                actualAt = aAt;
            }

            // yaw is handled in Avatar's matrix
            // pitch is handled in LinkedCamera's matrix

            Vector3 location = transform.Translation;
            bool onGround = game.terrain.onGround(location);

            // Jetting
            Vector3 translation = game.input.handleTranslation(transform.Forward);
            acceleration = new Vector3(0, Settings.gravity, 0);
#if XBOX360
            if (game.input.IsButtonDown(Settings.jet) && (jetting && jetFuel > Settings.jetExpend || jetFuel > Settings.jetMinFuel))
#else
            if ((game.input.RightMouseDown || game.input.IsButtonDown(Settings.jet)) && (jetting && jetFuel > Settings.jetExpend || jetFuel > Settings.jetMinFuel))
#endif
            {
                jetting = true;
                jetFuel -= Settings.jetExpend;

                if (translation == Vector3.Zero)
                    acceleration.Y += Settings.jetPackAccel;
                else
                {
                    acceleration.Y += jetVertical;
                    acceleration += translation * jetHorizontal;
                }

                if (onGround)
                {
                    // Make sure we get above the onGround detection height
                    acceleration.Y += Settings.collisionHeightAboveTerrain;
                }
            }
            else
            {
                jetting = false;
                jetFuel += Settings.jetRefuel;
            }
            jetFuel = MathHelper.Clamp(jetFuel, 0, 1);

            // Rotation
            actualAt = game.input.handleRotation(ref yaw, ref pitch);
            transform = Matrix.CreateWorld(Vector3.Zero, orthogonalize(actualAt, Vector3.Up), Vector3.Up);
            transform.Translation = location;

            // Skiing
            if (game.input.IsKeyDown(Settings.ski) || game.input.IsButtonDown(Settings.skiButton))
                skiing = true;
            else
                skiing = false;

            // Normal movement
            tractionForce = Vector3.Zero;
            if (onGround)
            {
                Vector3 walking = game.input.handleTranslation(Vector3.Forward);

                if (!skiing && walking.Z != 0)
                {
                    Vector3 tractionZ = transform.Backward * walking.Z;
                    tractionForce += tractionZ;
                }

                if (walking.X != 0)
                {
                    Vector3 tractionX = transform.Right * walking.X;
                    tractionForce += tractionX;
                }
            }

            base.update();
        }

        protected void respawn()
        {
            yaw = pitch = MathHelper.PiOver2;
            transform.Translation = origLocation;
            game.oct.Add(this);
            velocity = Vector3.Zero;
            camera.zoom = camera.targetZoom;
            game.setDamage(0);
            health = 1;
            jetFuel = 1;
            dead = false;
        }

        public override bool handleCollision(Vector3 normal)
        {
            Vector3 oldVelocity = velocity;
            velocity = Object3D.orthogonalize(velocity, normal);
            if (oldVelocity.Length() > 2 * velocity.Length())
                velocity *= 1 - (skiing ? Settings.frictionSki : Settings.frictionKinetic);

            if (!dead)
            {
                float vaLen = velocity.Length(), vbLen = oldVelocity.Length();
                float velocDiff = (vbLen - vaLen);
                if (velocDiff > Settings.damageMinSpeed)
                {
                    velocDiff -= Settings.damageMinSpeed;
                    float damage = velocDiff * velocDiff * Settings.damageSpeedMultiplier;
                    Health -= damage;
                }
            }
            return false;
        }
    }
}
