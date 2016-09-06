using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Enumerables;

namespace PointBlank.API.Server
{
    public class PBSkills
    {
        #region Skills
        public static PBSkills Overkill = new PBSkills("Overkill", 0, ESkill.OVERKILL, ESkillSet.OFFENSE, 7);
        public static PBSkills SharpShooter = new PBSkills("Sharp Shooter", 1, ESkill.SHARPSHOOTER, ESkillSet.OFFENSE, 7);
        public static PBSkills Dexterity = new PBSkills("Dexterity", 2, ESkill.DEXTERITY, ESkillSet.OFFENSE, 5);
        public static PBSkills Cardio = new PBSkills("Cardio", 3, ESkill.CARDIO, ESkillSet.OFFENSE, 5);
        public static PBSkills Exercise = new PBSkills("Exercise", 4, ESkill.EXERCISE, ESkillSet.OFFENSE, 5);
        public static PBSkills Diving = new PBSkills("Diving", 5, ESkill.DIVING, ESkillSet.OFFENSE, 5);
        public static PBSkills Parkour = new PBSkills("Parkour", 6, ESkill.PARKOUR, ESkillSet.OFFENSE, 5);

        public static PBSkills SneakyBeaky = new PBSkills("Sneaky Beaky", 0, ESkill.SNEAKYBEAKY, ESkillSet.DEFENSE, 7);
        public static PBSkills Vitality = new PBSkills("Vitality", 1, ESkill.VITALITY, ESkillSet.DEFENSE, 5);
        public static PBSkills Immunity = new PBSkills("Immunity", 2, ESkill.IMMUNITY, ESkillSet.DEFENSE, 5);
        public static PBSkills Toughness = new PBSkills("Toughness", 3, ESkill.TOUGHNESS, ESkillSet.DEFENSE, 5);
        public static PBSkills Strength = new PBSkills("Strength", 4, ESkill.STRENGTH, ESkillSet.DEFENSE, 5);
        public static PBSkills WarmBlooded = new PBSkills("Warm Blooded", 5, ESkill.WARMBLOODED, ESkillSet.DEFENSE, 5);
        public static PBSkills Survival = new PBSkills("Survival", 6, ESkill.SURVIVAL, ESkillSet.DEFENSE, 5);

        public static PBSkills Healing = new PBSkills("Healing", 0, ESkill.HEALING, ESkillSet.SUPPORT, 7);
        public static PBSkills Crafting = new PBSkills("Crafting", 1, ESkill.CRAFTING, ESkillSet.SUPPORT, 3);
        public static PBSkills Outdoors = new PBSkills("Outdoors", 2, ESkill.OUTDOORS, ESkillSet.SUPPORT, 5);
        public static PBSkills Cooking = new PBSkills("Cooking", 3, ESkill.COOKING, ESkillSet.SUPPORT, 3);
        public static PBSkills Fishing = new PBSkills("Fishing", 4, ESkill.FISHING, ESkillSet.SUPPORT, 5);
        public static PBSkills Agriculture = new PBSkills("Agriculture", 5, ESkill.AGRICULTURE, ESkillSet.SUPPORT, 5);
        public static PBSkills Mechanic = new PBSkills("Mechanic", 6, ESkill.MECHANIC, ESkillSet.SUPPORT, 5);
        public static PBSkills Engineer = new PBSkills("Engineer", 7, ESkill.ENGINEER, ESkillSet.SUPPORT, 3);
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

        public PBSkills(string name, int ID, ESkill skillType, ESkillSet skillset, byte maxLevel)
        {
            _name = name;
            _ID = ID;
            _skillType = skillType;
            _skillset = skillset;
            _maxLevel = maxLevel;
        }

        public PBSkills(PBSkills pbskill, Skill skill)
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

        public static PBSkills[] getSkills(PBPlayer player)
        {
            List<PBSkills> skills = new List<PBSkills>();
            PlayerSkills skill = player.player.skills;

            skills.Add(new PBSkills(PBSkills.Agriculture, skill.skills[(byte)PBSkills.Agriculture.skillset][(int)PBSkills.Agriculture.ID]));
            skills.Add(new PBSkills(PBSkills.Cardio, skill.skills[(byte)PBSkills.Cardio.skillset][(int)PBSkills.Cardio.ID]));
            skills.Add(new PBSkills(PBSkills.Cooking, skill.skills[(byte)PBSkills.Cooking.skillset][(int)PBSkills.Cooking.ID]));
            skills.Add(new PBSkills(PBSkills.Crafting, skill.skills[(byte)PBSkills.Crafting.skillset][(int)PBSkills.Crafting.ID]));
            skills.Add(new PBSkills(PBSkills.Dexterity, skill.skills[(byte)PBSkills.Dexterity.skillset][(int)PBSkills.Dexterity.ID]));
            skills.Add(new PBSkills(PBSkills.Diving, skill.skills[(byte)PBSkills.Diving.skillset][(int)PBSkills.Diving.ID]));
            skills.Add(new PBSkills(PBSkills.Engineer, skill.skills[(byte)PBSkills.Engineer.skillset][(int)PBSkills.Engineer.ID]));
            skills.Add(new PBSkills(PBSkills.Exercise, skill.skills[(byte)PBSkills.Exercise.skillset][(int)PBSkills.Exercise.ID]));
            skills.Add(new PBSkills(PBSkills.Fishing, skill.skills[(byte)PBSkills.Fishing.skillset][(int)PBSkills.Fishing.ID]));
            skills.Add(new PBSkills(PBSkills.Healing, skill.skills[(byte)PBSkills.Healing.skillset][(int)PBSkills.Healing.ID]));
            skills.Add(new PBSkills(PBSkills.Immunity, skill.skills[(byte)PBSkills.Immunity.skillset][(int)PBSkills.Immunity.ID]));
            skills.Add(new PBSkills(PBSkills.Mechanic, skill.skills[(byte)PBSkills.Mechanic.skillset][(int)PBSkills.Mechanic.ID]));
            skills.Add(new PBSkills(PBSkills.Outdoors, skill.skills[(byte)PBSkills.Outdoors.skillset][(int)PBSkills.Outdoors.ID]));
            skills.Add(new PBSkills(PBSkills.Overkill, skill.skills[(byte)PBSkills.Overkill.skillset][(int)PBSkills.Overkill.ID]));
            skills.Add(new PBSkills(PBSkills.Parkour, skill.skills[(byte)PBSkills.Parkour.skillset][(int)PBSkills.Parkour.ID]));
            skills.Add(new PBSkills(PBSkills.SharpShooter, skill.skills[(byte)PBSkills.SharpShooter.skillset][(int)PBSkills.SharpShooter.ID]));
            skills.Add(new PBSkills(PBSkills.SneakyBeaky, skill.skills[(byte)PBSkills.SneakyBeaky.skillset][(int)PBSkills.SneakyBeaky.ID]));
            skills.Add(new PBSkills(PBSkills.Strength, skill.skills[(byte)PBSkills.Strength.skillset][(int)PBSkills.Strength.ID]));
            skills.Add(new PBSkills(PBSkills.Survival, skill.skills[(byte)PBSkills.Survival.skillset][(int)PBSkills.Survival.ID]));
            skills.Add(new PBSkills(PBSkills.Toughness, skill.skills[(byte)PBSkills.Toughness.skillset][(int)PBSkills.Toughness.ID]));
            skills.Add(new PBSkills(PBSkills.Vitality, skill.skills[(byte)PBSkills.Vitality.skillset][(int)PBSkills.Vitality.ID]));
            skills.Add(new PBSkills(PBSkills.WarmBlooded, skill.skills[(byte)PBSkills.WarmBlooded.skillset][(int)PBSkills.WarmBlooded.ID]));

            return skills.ToArray();
        }
        #endregion
    }
}
