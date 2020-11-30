using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ZilchCheats
{
    public partial class Form1 : Form
    {




        public int bClient = 0;

        public VAMemory vam;
        public Dictionary d;

        public Task[] tasks = new Task[3];

        public string process = "csgo";

        public int localPlayer;
        public int bEngine = 0;

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);


        public Form1()
        {
            InitializeComponent();
            Init();
        }

        public void DefineDictionary()
        {
            d.aLocalPlayer = bClient + Signatures.dwLocalPlayer;
            d.bClient = bClient;
            d.bEngine = bEngine;
            d.pEngine = vam.ReadInt32((IntPtr)d.bEngine+Signatures.dwClientState);
            d.aEntityList = bClient + Signatures.dwEntityList;

            
            d.fJump = bClient + Signatures.dwForceJump;

            // redefine all of these in CHangePlayer()
            d.aFlags = localPlayer + Netvars.m_fFlags; 
            d.aTeam = localPlayer + Netvars.m_iTeamNum;
            d.aId = localPlayer + Netvars.m_iAccountID;
            d.aCrossHairID = localPlayer + Netvars.m_iCrosshairId;
            d.aHealth = localPlayer + Netvars.m_iHealth;

            d.aVecPosition = localPlayer + Netvars.m_vecOrigin;
            d.aVecView = localPlayer + Netvars.m_vecViewOffset;
            // END

            d.fAttack1 = bClient + Signatures.dwForceAttack;
            d.oEntityLoopDistance = 0x10; // this is the distance of bytes between memory of the players

            d.aViewAngle = d.pEngine +Signatures.dwClientState_ViewAngles;
            
            
            // dont define aVecEyePos
            

            d.eTrigger = false;
            d.eBhop = false;
            d.eWall = false;
            d.eAim = false;

            d.coords = new string[3] { "x", "y", "z" };


            //WallHacks
            d.enemy.r = 1;
            d.enemy.g = 0;
            d.enemy.b = 0;
            d.enemy.a = 1;
            d.enemy.rwuo = false;
            d.enemy.rwo = true;

            d.team.r = 0;
            d.team.g = 0;
            d.team.b = 1;
            d.team.a = 1;
            d.team.rwuo = false;
            d.team.rwo = true;

            d.oGlowDistance = 0x38;

            d.keyTrigger = 4;
            d.keyAim = 18;
        }

        public struct GlowStruct
        {
            public float r;
            public float g;
            public float b;
            public float a;

            public bool rwo;
            public bool rwuo;             
        }
        
        public struct Vector
        {
            public float x;
            public float y;
            public float z;


        }

        public struct Angle
        {
            public float x;
            public float y;
        }

        public struct Dictionary
        {
            // for all
            public int aLocalPlayer;
            public int bClient;
            public int localPlayer;
            public int aEntityList;
            public int bEngine;
            public int pEngine;

            public int myTeam;
            public int myId;

            // bhop
            public int aFlags;
            public int fJump;

            //wallhack

            public int oGlowDistance;
            public GlowStruct enemy;
            public GlowStruct team;
            
            

            //trigger

    
            public int aCrossHairID;
            public int aTeam;
            public int aHealth;

            public int fAttack1;
            public int oEntityLoopDistance; // this is the distance of bytes between memory of the players

            public int aViewAngle;
            public int pViewAngle;

            public int aId;
            

            // aimbot

            public int aVecPosition;
            public int aVecView;
            public int aVecEyePos;
            public string[] coords;


            // enablign
            public bool eBhop;
            public bool eTrigger;
            public bool eWall;
            public bool eAim;

            // hotkeys 

            public int keyTrigger;
            public int keyAim;
        }

        public Vector VectorSubtract(Vector vec1, Vector vec2)
        {
            Vector nVec;
            nVec.x = vec1.x - vec2.x;
            nVec.y = vec1.y - vec2.y;
            nVec.z = vec1.z - vec2.z;

            return nVec;
        }

        public Vector VectorAdd(Vector vec1, Vector vec2)
        {
            Vector nVec;
            nVec.x = vec1.x + vec2.x;
            nVec.y = vec1.y + vec2.y;
            nVec.z = vec1.z + vec2.z;

            return nVec;
        }

        public Angle CalcAngle(Vector src, Vector dst)
        {
            Vector delta; // enemy position relative to you
            delta.x = src.x - dst.x;
            delta.y = src.y - dst.y;
            delta.z = src.z - dst.z; 

        
            double hyp = Math.Sqrt((delta.x * delta.x) + (delta.y * delta.y)); // get distance to enemy on 2d scale

            Angle angles;
            // soh cah toa
            // If you try to visualize it you will realize that once you find the hypotenuse of y and x. The hypotenuse is really the base of the angle for looking up
            // Tan of the hypotenuse and z is really how high you are looking.
            // Tan of y and x will serve as horizontally where you need to aim.
            angles.x = (float)(Math.Atan(delta.z / hyp) * 57.295779513082f); // convert to radians and find x angle. X angle is pitch. Getting how high up you need to aim
            angles.y = (float)(Math.Atan(delta.y / delta.x) * 57.295779513082f); // convert to radians and find y angle. Y angle is yaw. Getting where horizontally you need to aim.
            
            if (delta.x >= 0.0) { angles.y += 180.0f; }

            return angles;
        }

        

        public Vector GetPlayerPosition()
        {
            Vector player;


            player.x = vam.ReadFloat((IntPtr)d.localPlayer + Netvars.m_vecOrigin + 0x0)+ vam.ReadFloat((IntPtr)d.localPlayer + Netvars.m_vecViewOffset + 0x0);
            player.y = vam.ReadFloat((IntPtr)d.localPlayer + Netvars.m_vecOrigin + 0x4) + vam.ReadFloat((IntPtr)d.localPlayer + Netvars.m_vecViewOffset + 0x4);
            player.z = vam.ReadFloat((IntPtr)d.localPlayer + Netvars.m_vecOrigin + 0x8) + vam.ReadFloat((IntPtr)d.localPlayer + Netvars.m_vecViewOffset + 0x8);
            


            return player;
        }


        public Vector GetEnemyPosition(int ePointer)
        {
            Vector enemy;
            enemy.x = vam.ReadFloat((IntPtr)ePointer + Netvars.m_vecOrigin + 0x0) + vam.ReadFloat((IntPtr)ePointer + Netvars.m_vecViewOffset + 0x0);
            enemy.y = vam.ReadFloat((IntPtr)ePointer + Netvars.m_vecOrigin + 0x4) + vam.ReadFloat((IntPtr)ePointer + Netvars.m_vecViewOffset + 0x4);
            enemy.z = vam.ReadFloat((IntPtr)ePointer + Netvars.m_vecOrigin + 0x8) + vam.ReadFloat((IntPtr)ePointer + Netvars.m_vecViewOffset + 0x8);
            return enemy;
        }

        public Vector GetEnemyBonePosition(int ePointer)
        {
            int address;
            address = ePointer + Netvars.m_dwBoneMatrix;
            int bonePointer = vam.ReadInt32((IntPtr)address);
            Vector bone = GetBonePosition( bonePointer, 8);
            return bone;
        }

        public Vector GetBonePosition(int boneBase, int boneIndex)
        {
            Vector bonePos;
            bonePos.x = vam.ReadFloat((IntPtr)boneBase + (0x30 * boneIndex) + 0x0C); // bone base, multiply boneIndex by offset to get a bone, then add 0x_C to get a coordinate as it's a matrix
            bonePos.y = vam.ReadFloat((IntPtr)boneBase + (0x30 * boneIndex) + 0x1C);
            bonePos.z = vam.ReadFloat((IntPtr)boneBase + (0x30 * boneIndex) + 0x2C);
            return bonePos;
        }

        public float GetDistance(Vector src, Vector dst)
        {
            Vector delta; // enemy position relative to you
            delta.x = src.x - dst.x;
            delta.y = src.y - dst.y;
            delta.z = src.z - dst.z;

            double number = (delta.x * delta.x) + (delta.y * delta.y) + (delta.z * delta.z);
            return (float)Math.Sqrt(number);
        }

        public Vector GetClosestEnemy(Vector pPosition)
        {
            Vector closest;
            closest.x = 0;
            closest.y = 0;
            closest.z = 0;
            
            
            float closestDistance = 0;

            int address;

            int myTeam = d.myTeam;
            
            for (int i = 0; i < 64; i += 1)
            {
                address = d.aEntityList + (i * d.oEntityLoopDistance);
                int ptrToPerson = vam.ReadInt32((IntPtr)address); // pointer to enemy 

                address = Signatures.m_bDormant + ptrToPerson;
                bool eDormant = vam.ReadBoolean((IntPtr)address);

                address = Netvars.m_lifeState + ptrToPerson;
                int eLifeState = vam.ReadInt32((IntPtr) address);
                if (!eDormant && eLifeState == 0) // If enemy is not dormant
                {

                    address = Netvars.m_iTeamNum + ptrToPerson;
                    int pTeam = vam.ReadInt32((IntPtr)address); // Enemy's team

                    if (pTeam > 1 && ptrToPerson != 0 && myTeam != pTeam)
                    {
                        Vector ePosition = GetEnemyBonePosition(ptrToPerson);
                        float compare = GetDistance(pPosition, ePosition);
                        //
                        if (closestDistance > compare || closestDistance == 0)
                        {
                            closestDistance = compare;
                            closest = ePosition;
                        }
                    }
                }
                

            }
            return closest;
        }

        public Vector GetClosestEnemyFOV(Vector pPosition)
        {
            Vector closest = new Vector();


            float closestAngleSum = 720f;

            int address;

            int myTeam = d.myTeam;

            for (int i = 0; i < 64; i += 1)
            {
                address = d.aEntityList + (i * d.oEntityLoopDistance);
                int ptrToPerson = vam.ReadInt32((IntPtr)address); // pointer to enemy 

                address = Signatures.m_bDormant + ptrToPerson;
                bool eDormant = vam.ReadBoolean((IntPtr)address);

                address = Netvars.m_lifeState + ptrToPerson;
                int eLifeState = vam.ReadInt32((IntPtr)address);
                if (!eDormant && eLifeState == 0) // If enemy is not dormant
                {

                    address = Netvars.m_iTeamNum + ptrToPerson;
                    int pTeam = vam.ReadInt32((IntPtr)address); // Enemy's team

                    if (pTeam > 1 && ptrToPerson != 0 && myTeam != pTeam)
                    {
                        Vector ePosition = GetEnemyBonePosition(ptrToPerson);

                        Vector viewAngles;
                        viewAngles.x = vam.ReadFloat((IntPtr) d.aViewAngle);
                        viewAngles.y = vam.ReadFloat((IntPtr) d.aViewAngle + 0x4);

                        

                        Angle eAngle = CalcAngle(pPosition, ePosition);
                        eAngle = NormalizeAngle(eAngle);

                        float angleSum = Math.Abs(viewAngles.x - eAngle.x) + Math.Abs(viewAngles.y - eAngle.y);

                        if(angleSum < closestAngleSum)
                        {
                            closest = ePosition;
                            closestAngleSum = angleSum;
                        }

                    }
                }
                if(i == 63 && closestAngleSum == 720f)
                {
                    closest.x = 6969f; // This means that there is no enemy player found to lock into
                    /*
                     Couldn't find a good way to say that there is no player  found, so will use a naive method to say that there's no player.
                     See the Float structure next time.
                    */
                }

            }
            return closest;

            
        }




        public void Init()
        {
            

            vam = new VAMemory(process);

            if (GetModuleAddy())
            {

                localPlayer = vam.ReadInt32((IntPtr) bClient + Signatures.dwLocalPlayer);

                DefineDictionary();

                d.localPlayer = localPlayer;

                Executor();

            }
            
        }

        public void Executor ()
        {
            
            tasks[0] = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    AimBot();
                    TriggerBot();
                    Bunnyhop();
                
                    Thread.Sleep(1);
                }
            });
            
            tasks[1] = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    WallHack();
                    ChangePlayer(); // probably need to change mroe stuff for this aswell
                    Thread.Sleep(17);
                }
            });
            /*
            tasks[2] = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Bunnyhop();
                }
                    
                
            });
            */
            
                
            
        }

        public void AimBot()
        {
            if (d.eAim && GetAsyncKeyState(d.keyAim) != 0)
            {

                Vector pPosition = GetPlayerPosition();
         
                Vector ePosition = GetClosestEnemyFOV(pPosition);
                
                if(ePosition.x != 6969f)
                {
                    Angle aimAt = CalcAngle(pPosition, ePosition);


                    if (!float.IsNaN(aimAt.x) && !float.IsNaN(aimAt.y))
                    {
                        Angle newAim = NormalizeAngle(aimAt);
                        vam.WriteFloat((IntPtr)d.aViewAngle, newAim.x);
                        vam.WriteFloat((IntPtr)d.aViewAngle + 0x4, newAim.y);
                    }
                }
        
            }
        }

       

        public void WallHack()
        {
            if (d.eWall)
            {
                int address;
                int calculation;

              
                
                int myTeam = d.myTeam;
                

                for (int i =0; i < 64; i+=1)
                {
                    address = d.aEntityList + (i * d.oEntityLoopDistance);
                    int ptrToPerson = vam.ReadInt32((IntPtr)address);

                    address = Netvars.m_iTeamNum + ptrToPerson;
                    int pTeam = vam.ReadInt32((IntPtr) address);

                    address = Signatures.m_bDormant + ptrToPerson;
                    if (!vam.ReadBoolean((IntPtr)address))
                    {
                        
                        address = Netvars.m_iGlowIndex + ptrToPerson;
                        int glowIndex = vam.ReadInt32((IntPtr)address);

                        address = Signatures.dwGlowObjectManager + bClient;
                        int glowObject = vam.ReadInt32((IntPtr)address);

                        int current;
                        if(pTeam > 1 && ptrToPerson != 0)
                        {
                            GlowStruct targetTeam;
                            if (pTeam == myTeam)
                            {
                                targetTeam = d.team;
                            }
                            else
                            {
                                targetTeam = d.enemy;
                            }
                            
                                
                                // 0x4 0x8 0xC 0x10 0x24 25

                            calculation = glowIndex * d.oGlowDistance + 0x4;
                            current = glowObject + calculation;
                            vam.WriteFloat((IntPtr)current, targetTeam.r);

                            calculation = glowIndex * d.oGlowDistance + 0x8;
                            current = glowObject + calculation;
                            vam.WriteFloat((IntPtr)current, targetTeam.g);

                            calculation = glowIndex * d.oGlowDistance + 0xC;
                            current = glowObject + calculation;
                            vam.WriteFloat((IntPtr)current, targetTeam.b);

                            calculation = glowIndex * d.oGlowDistance + 0x10;
                            current = glowObject + calculation;
                            vam.WriteFloat((IntPtr)current, targetTeam.a);

                            calculation = glowIndex * d.oGlowDistance + 0x24;
                            current = glowObject + calculation;
                            vam.WriteBoolean((IntPtr)current, targetTeam.rwo);

                            calculation = glowIndex * d.oGlowDistance + 0x25;
                            current = glowObject + calculation;
                            vam.WriteBoolean((IntPtr)current, targetTeam.rwuo);
                            
                            
                        }
                        

                    }
                    
                }

               
                
            }
        }

        public void TriggerBot()
        {
            if (d.eTrigger)
            {
                if (GetAsyncKeyState(d.keyTrigger) != 0)
                {


                    int myTeam = d.myTeam;
                    int playerInCross = vam.ReadInt32((IntPtr)d.aCrossHairID);

                    if (playerInCross > 0 && playerInCross < 65)
                    {
                        int address = bClient + Signatures.dwEntityList + ((playerInCross - 1) * d.oEntityLoopDistance); // look for the player in your crosshair
                        int ptrToEnemy = vam.ReadInt32((IntPtr)address);

                        address = ptrToEnemy + Netvars.m_iHealth; // health of enemy
                        int enemyHealth = vam.ReadInt32((IntPtr)address);

                        address = ptrToEnemy + Netvars.m_iTeamNum;
                        int enemyTeam = vam.ReadInt32((IntPtr)address);

                        if ((enemyTeam != myTeam) && (enemyTeam > 1) && (enemyHealth > 0))
                        {
                            vam.WriteInt32((IntPtr)d.fAttack1, 1);
                            Thread.Sleep(1);
                            vam.WriteInt32((IntPtr)d.fAttack1, 4);
                        }
                    }
                }
            }
            
        }

        public void Bunnyhop()
        {
           
                if (d.eBhop)
                {
                    if (GetAsyncKeyState(32) != 0)
                    {

                        int calculation = d.localPlayer + Netvars.m_vecVelocity;
                        float x = vam.ReadFloat((IntPtr) calculation);
                        float y = vam.ReadFloat((IntPtr)calculation + 0x4);
                        
                        if(!(x == 0f && y == 0f))
                        {
                            int flags = vam.ReadInt32((IntPtr)d.aFlags);
                            if (flags >= 257 && flags <= 263 && flags % 2 == 1)
                            {
                                vam.WriteInt32((IntPtr)d.fJump, 5);
                                Thread.Sleep(80);
                                vam.WriteInt32((IntPtr)d.fJump, 4);
                            }
                        }
                        

                    }
                }
            
           
        }

        public Angle NormalizeAngle(Angle angle)
        {
            Angle vec = angle;

            if (vec.x > 89.0f && vec.x <= 180.0f)
            {
                vec.x = 89.0f;
            }
            while (vec.x > 180f)
            {
                vec.x -= 360f;
            }
            while (vec.x < -89.0f)
            {
                vec.x = -89.0f;
            }
            while (vec.y > 180)
            {
                vec.y -= 360f;
            }
            while (vec.y < -180)
            {
                vec.y += 360f;
            }


            return vec;
        }

        public void ChangePlayer()
        {
           
            int state = vam.ReadInt32((IntPtr)(d.pEngine + Signatures.dwClientState_State));
            
            if (state == 6 || state == 7)
            {
                
                d.localPlayer = vam.ReadInt32((IntPtr) d.aLocalPlayer);
                d.aTeam = d.localPlayer + Netvars.m_iTeamNum;
                d.aId = d.localPlayer + Netvars.m_iAccountID;
                d.aHealth = d.localPlayer + Netvars.m_iHealth;
                d.aFlags = d.localPlayer + Netvars.m_fFlags;
                d.aCrossHairID = d.localPlayer + Netvars.m_iCrosshairId;

                // define optimizers
                d.myTeam = vam.ReadInt32((IntPtr)d.aTeam);
//d.myId = vam.ReadInt32((IntPtr)d.aId);
                
            }
        }

        

        public bool GetModuleAddy()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(process);

                if(p.Length > 0)
                {
                  
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if(m.ModuleName == "client_panorama.dll")
                        {
                            
                            bClient = (int)m.BaseAddress;
                            
                        }
                        if(m.ModuleName == "engine.dll")
                        {
                            bEngine = (int)m.BaseAddress;
                        }
                        if (bEngine != 0 && bClient != 0) return true;
                    }
                }
                 
            } catch (Exception e)
            {
                Debug.WriteLine("Exception in module");
            }
            return false;
        }
        
        private void EnableBunnyhop_CheckedChanged(object sender, EventArgs e)
        {
            d.eBhop = !d.eBhop;
            
        }

        private void EnableTrigger_CheckedChanged(object sender, EventArgs e)
        {
            d.eTrigger = !d.eTrigger;
        }

        private void EnableWall_CheckedChanged(object sender, EventArgs e)
        {
            d.eWall = !d.eWall;
        }

        private void EnableAim_CheckedChanged(object sender, EventArgs e)
        {
            d.eAim = !d.eAim;
        }
    }
}
