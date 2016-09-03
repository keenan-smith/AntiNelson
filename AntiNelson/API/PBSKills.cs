using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Enumerables;

namespace PointBlank.API
{
    public class PBSKills
    {
        #region Skills
        public static PBSKills Overkill = new PBSKills("Overkill", 0, ESkill.OVERKILL, ESkillSet.OFFENSE, 7);
        public static PBSKills SharpShooter = new PBSKills("Sharp Shooter", 1, ESkill.SHARPSHOOTER, ESkillSet.OFFENSE, 7);
        public static PBSKills Dexterity = new PBSKills("Dexterity", 2, ESkill.DEXTERITY, ESkillSet.OFFENSE, 5);
        public static PBSKills Cardio = new PBSKills("Cardio", 3, ESkill.CARDIO, ESkillSet.OFFENSE, 5);
        public static PBSKills Exercise = new PBSKills("Exercise", 4, ESkill.EXERCISE, ESkillSet.OFFENSE, 5);
        public static PBSKills Diving = new PBSKills("Diving", 5, ESkill.DIVING, ESkillSet.OFFENSE, 5);
        public static PBSKills Parkour = new PBSKills("Parkour", 6, ESkill.PARKOUR, ESkillSet.OFFENSE, 5);

        public static PBSKills SneakyBeaky = new PBSKills("Sneaky Beaky", 0, ESkill.SNEAKYBEAKY, ESkillSet.DEFENSE, 7);
        public static PBSKills Vitality = new PBSKills("Vitality", 1, ESkill.VITALITY, ESkillSet.DEFENSE, 5);
        public static PBSKills Immunity = new PBSKills("Immunity", 2, ESkill.IMMUNITY, ESkillSet.DEFENSE, 5);
        public static PBSKills Toughness = new PBSKills("Toughness", 3, ESkill.TOUGHNESS, ESkillSet.DEFENSE, 5);
        public static PBSKills Strength = new PBSKills("Strength", 4, ESkill.STRENGTH, ESkillSet.DEFENSE, 5);
        public static PBSKills WarmBlooded = new PBSKills("Warm Blooded", 5, ESkill.WARMBLOODED, ESkillSet.DEFENSE, 5);
        public static PBSKills Survival = new PBSKills("Survival", 6, ESkill.SURVIVAL, ESkillSet.DEFENSE, 5);

        public static PBSKills Healing = new PBSKills("Healing", 0, ESkill.HEALING, ESkillSet.SUPPORT, 7);
        public static PBSKills Crafting = new PBSKills("Crafting", 1, ESkill.CRAFTING, ESkillSet.SUPPORT, 3);
        public static PBSKills Outdoors = new PBSKills("Outdoors", 2, ESkill.OUTDOORS, ESkillSet.SUPPORT, 5);
        public static PBSKills Cooking = new PBSKills("Cooking", 3, ESkill.COOKING, ESkillSet.SUPPORT, 3);
        public static PBSKills Fishing = new PBSKills("Fishing", 4, ESkill.FISHING, ESkillSet.SUPPORT, 5);
        public static PBSKills Agriculture = new PBSKills("Agriculture", 5, ESkill.AGRICULTURE, ESkillSet.SUPPORT, 5);
        public static PBSKills Mechanic = new PBSKills("Mechanic", 6, ESkill.MECHANIC, ESkillSet.SUPPORT, 5);
        public static PBSKills Engineer = new PBSKills("Engineer", 7, ESkill.ENGINEER, ESkillSet.SUPPORT, 3);
        #endregion

        #region Variables
        private byte _maxLevel;
        private ESkill _skill;
        private string _name;
        private int _ID;
        private ESkillSet _skillset;
        #endregion

        #region Properties
        public string name
        {
            get
            {
                return _name;
            }
        }

        public ESkill skill
        {
            get
            {
                return _skill;
            }
        }

        public byte maxLevel
        {
            get
            {
                return _maxLevel;
            }
        }

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public ESkillSet skillset
        {
            get
            {
                return _skillset;
            }
        }
        #endregion

        public PBSKills(string name, int ID, ESkill skill, ESkillSet skillset, byte maxLevel)
        {
            _name = name;
            _ID = ID;
            _skill = skill;
            _skillset = skillset;
            _maxLevel = maxLevel;
        }

        #region Functions
        public void setMaxLevel(PBPlayer player)
        {
            player.player.skills.skills[(int)skillset][ID].level = maxLevel;
        }

        public void setMinLevel(PBPlayer player)
        {
            player.player.skills.skills[(int)skillset][ID].level = 0;
        }

        public void setLevel(PBPlayer player, byte level)
        {
            player.player.skills.skills[(int)skillset][ID].level = level;
        }
        #endregion
    }
}
