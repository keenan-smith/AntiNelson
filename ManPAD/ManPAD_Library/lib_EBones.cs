using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Library
{
    public class EBones : MonoBehaviour
    {
        public static Dictionary<int, string> bones = new Dictionary<int, string>();
        
        void Start()
        {
            bones.Add((int)ELimb.LEFT_FOOT, "Left_Foot");
            bones.Add((int)ELimb.LEFT_LEG, "Left_Leg");
            bones.Add((int)ELimb.RIGHT_FOOT, "Right_Foot");
            bones.Add((int)ELimb.RIGHT_LEG, "Right_Leg");
            bones.Add((int)ELimb.LEFT_HAND, "Left_Hand");
            bones.Add((int)ELimb.LEFT_ARM, "Left_Arm");
            bones.Add((int)ELimb.RIGHT_HAND, "Right_Hand");
            bones.Add((int)ELimb.RIGHT_ARM, "Right_Arm");
            bones.Add((int)ELimb.LEFT_BACK, "Left_Back");
            bones.Add((int)ELimb.RIGHT_BACK, "Right_back");
            bones.Add((int)ELimb.LEFT_FRONT, "Left_Front");
            bones.Add((int)ELimb.RIGHT_FRONT, "Right_Front");
            bones.Add((int)ELimb.SPINE, "Spine");
            bones.Add((int)ELimb.SKULL, "Skull");
        }

        public static string GetBone(ELimb limb)
        {
            string bone_string = bones[(int)limb];
            return bone_string;
        }
    }
}
