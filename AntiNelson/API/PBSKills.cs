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
        private ESkill _skillType;
        private string _name;
        private int _ID;
        private ESkillSet _skillset;
        private Skill _skill;
        #endregion

        #region Properties
        public string name
        {
            get
            {
                return _name;
            }
        }

        public ESkill skillType
        {
            get
            {
                return _skillType;
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

        public Skill skill
        {
            get
            {
                return _skill;
            }
        }
        #endregion

        public PBSKills(string name, int ID, ESkill skillType, ESkillSet skillset, byte maxLevel)
        {
            _name = name;
            _ID = ID;
            _skillType = skillType;
            _skillset = skillset;
            _maxLevel = maxLevel;
        }

        public PBSKills(PBSKills pbskill, Skill skill)
        {
            _name = pbskill.name;
            _ID = pbskill.ID;
            _skillType = pbskill.skillType;
            _skillset = pbskill.skillset;
            _maxLevel = pbskill.maxLevel;
            _skill = skill;
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

        public byte getLevel(PBPlayer player)
        {
            return player.player.skills.skills[(int)skillset][ID].level;
        }

        public static PBSKills[] getSkills(PBPlayer player)
        {
            List<PBSKills> skills = new List<PBSKills>();
            PlayerSkills skill = player.player.skills;

            skills.Add(new PBSKills(PBSKills.Agriculture, skill.skills[(byte)PBSKills.Agriculture.skillset][(int)PBSKills.Agriculture.ID]));
            skills.Add(new PBSKills(PBSKills.Cardio, skill.skills[(byte)PBSKills.Cardio.skillset][(int)PBSKills.Cardio.ID]));
            skills.Add(new PBSKills(PBSKills.Cooking, skill.skills[(byte)PBSKills.Cooking.skillset][(int)PBSKills.Cooking.ID]));
            skills.Add(new PBSKills(PBSKills.Crafting, skill.skills[(byte)PBSKills.Crafting.skillset][(int)PBSKills.Crafting.ID]));
            skills.Add(new PBSKills(PBSKills.Dexterity, skill.skills[(byte)PBSKills.Dexterity.skillset][(int)PBSKills.Dexterity.ID]));
            skills.Add(new PBSKills(PBSKills.Diving, skill.skills[(byte)PBSKills.Diving.skillset][(int)PBSKills.Diving.ID]));
            skills.Add(new PBSKills(PBSKills.Engineer, skill.skills[(byte)PBSKills.Engineer.skillset][(int)PBSKills.Engineer.ID]));
            skills.Add(new PBSKills(PBSKills.Exercise, skill.skills[(byte)PBSKills.Exercise.skillset][(int)PBSKills.Exercise.ID]));
            skills.Add(new PBSKills(PBSKills.Fishing, skill.skills[(byte)PBSKills.Fishing.skillset][(int)PBSKills.Fishing.ID]));
            skills.Add(new PBSKills(PBSKills.Healing, skill.skills[(byte)PBSKills.Healing.skillset][(int)PBSKills.Healing.ID]));
            skills.Add(new PBSKills(PBSKills.Immunity, skill.skills[(byte)PBSKills.Immunity.skillset][(int)PBSKills.Immunity.ID]));
            skills.Add(new PBSKills(PBSKills.Mechanic, skill.skills[(byte)PBSKills.Mechanic.skillset][(int)PBSKills.Mechanic.ID]));
            skills.Add(new PBSKills(PBSKills.Outdoors, skill.skills[(byte)PBSKills.Outdoors.skillset][(int)PBSKills.Outdoors.ID]));
            skills.Add(new PBSKills(PBSKills.Overkill, skill.skills[(byte)PBSKills.Overkill.skillset][(int)PBSKills.Overkill.ID]));
            skills.Add(new PBSKills(PBSKills.Parkour, skill.skills[(byte)PBSKills.Parkour.skillset][(int)PBSKills.Parkour.ID]));
            skills.Add(new PBSKills(PBSKills.SharpShooter, skill.skills[(byte)PBSKills.SharpShooter.skillset][(int)PBSKills.SharpShooter.ID]));
            skills.Add(new PBSKills(PBSKills.SneakyBeaky, skill.skills[(byte)PBSKills.SneakyBeaky.skillset][(int)PBSKills.SneakyBeaky.ID]));
            skills.Add(new PBSKills(PBSKills.Strength, skill.skills[(byte)PBSKills.Strength.skillset][(int)PBSKills.Strength.ID]));
            skills.Add(new PBSKills(PBSKills.Survival, skill.skills[(byte)PBSKills.Survival.skillset][(int)PBSKills.Survival.ID]));
            skills.Add(new PBSKills(PBSKills.Toughness, skill.skills[(byte)PBSKills.Toughness.skillset][(int)PBSKills.Toughness.ID]));
            skills.Add(new PBSKills(PBSKills.Vitality, skill.skills[(byte)PBSKills.Vitality.skillset][(int)PBSKills.Vitality.ID]));
            skills.Add(new PBSKills(PBSKills.WarmBlooded, skill.skills[(byte)PBSKills.WarmBlooded.skillset][(int)PBSKills.WarmBlooded.ID]));

            return skills.ToArray();
        }
        #endregion
    }
}
