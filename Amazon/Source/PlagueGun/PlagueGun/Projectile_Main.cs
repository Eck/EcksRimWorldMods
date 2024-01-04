using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Amazon
{
    public class MeleeWeapon_ManCatcher : ThingWithComps
    {
        public override void Tick()
        {
            base.Tick();

            // Check if the weapon is equipped by a pawn
            if (this.ParentHolder is Pawn_EquipmentTracker equipmentTracker && equipmentTracker.pawn != null)
            {
                Pawn wielderPawn = equipmentTracker.pawn;

                // Check for targets in melee range
                foreach (var potentialTarget in wielderPawn.Position.GetThingList(wielderPawn.Map))
                {
                    if (potentialTarget is Pawn targetPawn && targetPawn != wielderPawn && targetPawn.Position.AdjacentTo8WayOrInside(wielderPawn.Position))
                    {
                        // Perform your custom effect on the target pawn here
                        ApplyCustomEffect(targetPawn);
                    }
                }
            }
        }

        private void ApplyCustomEffect(Pawn targetPawn)
        {
            if (Amazon.bool_Debug == true) { Log.Message($"Effect applied"); }
            HediffDef customHediffDef = HediffDef.Named("Stun");
            Hediff customHediff = HediffMaker.MakeHediff(customHediffDef, targetPawn);
            targetPawn.health.AddHediff(customHediff);
        }
    }

    public class Projectile_AmazonArrow : Bullet
    {
        //I know these should be in a constructor somewhere... but I am inexperienced!
        public static bool bool_agetweak = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_AlterAge;
        public static bool bool_NeuroToxin = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_NeuroToxin;
        public static bool bool_MoodDebuff = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_NegativeMood;

        #region Properties
        //
        public ThingDef_AmazonArrow Def
        {
            get
            {
                return this.def as ThingDef_AmazonArrow;
            }
        }
        #endregion Properties

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing);
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn) //Fancy way to declare a variable inside an if statement. - Thanks Erdelf.
            {
                if (hitPawn.gender == Gender.Male)
                {
                    if (bool_agetweak == true)
                    {
                        long ageTicks = hitPawn.ageTracker.AgeBiologicalTicks;
                        int ageYears = (int)(ageTicks / GenDate.TicksPerYear);
                        if (ageYears >= 0 && ageYears < 9999)
                        {
                            // Add 300 days or one year (in game time)
                            long ticksToAdd = 120 * GenDate.TicksPerDay;
                            hitPawn.ageTracker.AgeBiologicalTicks += ticksToAdd;
                        }
                    }

                    if (bool_MoodDebuff == true)
                    {
                        ThoughtDef joyfuzzDef = ThoughtDef.Named("AmazonToxGun");
                        Thought_Memory joyfuzzMemory = (Thought_Memory)ThoughtMaker.MakeThought(joyfuzzDef);
                        hitPawn.needs.mood.thoughts.memories.TryGainMemory(joyfuzzMemory);
                    }
                }
            }
        }
    }

    public class Projectile_PlagueBullet : Bullet
    {
        #region Properties
        //
        public ThingDef_PlagueBullet Def
        {
            get
            {
                return this.def as ThingDef_PlagueBullet;
            }
        }
        #endregion Properties


        #region Overrides
        public float durationDays;
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing);

            if (Def != null && hitThing != null && hitThing is Pawn hitPawn) //Fancy way to declare a variable inside an if statement. - Thanks Erdelf.
            {
                var rand = Rand.Value; // This is a random percentage between 0% and 100%
                if (rand <= Def.AddHediffChance) // If the percentage falls under the chance, success!
                {

                    //string translatedMessage = TranslatorFormattedStringExtensions.Translate("ECK_PlagueBullet_SuccessMessage", launcher.Label, hitPawn.Label, Def.HediffToAdd.label);
                    //Messages.Message(translatedMessage, MessageTypeDefOf.NegativeHealthEvent);

                    //This checks to see if the character has a heal differential, or hediff on them already.
                    if (hitPawn.gender == Gender.Male)
                    {
                        var plagueOnPawn = hitPawn?.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                        var randomSeverity = Rand.Range(0.15f, 0.30f);
                        long ageTicks = hitPawn.ageTracker.AgeBiologicalTicks;
                        int ageYears = (int)(ageTicks / GenDate.TicksPerYear);

                        if (ageYears > 16 && ageYears < 9999999)
                        {
                            // Add 60 days or one year (in game time)
                            long ticksToAdd = 60 * GenDate.TicksPerDay;
                            hitPawn.ageTracker.AgeBiologicalTicks += ticksToAdd;
                        }

                        if (plagueOnPawn != null)
                        {
                            //If they already have plague, add a random range to its severity.
                            //If severity reaches 1.0f, or 100%, plague kills the target.
                            plagueOnPawn.Severity += randomSeverity;
                        }
                        else
                        {
                            //These three lines create a new health differential or Hediff,
                            //put them on the character, and increase its severity by a random amount.
                            Hediff hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn, null);
                            hediff.Severity = randomSeverity;
                            hitPawn.health.AddHediff(hediff, null, null);
                            ThoughtDef joyfuzzDef = ThoughtDef.Named("AmazonToxGun");
                            Thought_Memory joyfuzzMemory = (Thought_Memory)ThoughtMaker.MakeThought(joyfuzzDef);
                            hitPawn.needs.mood.thoughts.memories.TryGainMemory(joyfuzzMemory);
                        }
                    }
                }
                else //failure!
                {

                }
            }
        }
        #endregion Overrides
    }

    public class CompTargetEffect_mypower : CompTargetEffect
    {
        public override void DoEffectOn(Pawn user, Thing target)
        {
            // Check if target is a pawn
            if (target is Pawn pawn)
            {
                // Display message on screen
                Find.LetterStack.ReceiveLetter("My Power", "My power was used on " + pawn.Name.ToStringShort, LetterDefOf.PositiveEvent, pawn);
            }
        }
    }

    public class CompAbilityEffect_AgeUp : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                base.Apply(target, dest);
                if (target.Thing is Pawn pawn)
                {
                    base.Apply(target, dest);
                    Pawn targetPawn = target.Pawn;
                    if (pawn != null)
                    {
                        targetPawn.needs.rest.CurLevel = 0f;
                        Job job = JobMaker.MakeJob(JobDefOf.LayDown, targetPawn.Position);
                        //job.forceSleep = true;
                        pawn.jobs.StartJob(job, JobCondition.InterruptForced);
                    }
                    //pawn.stances.stunner.StunFor(GetDurationSeconds(pawn).SecondsToTicks(), parent.pawn, addBattleLog: false);
                }
            }
        }
    }

    public class CompAbilityEffect_Stun : CompAbilityEffect_WithDuration
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                base.Apply(target, dest);
                if (target.Thing is Pawn pawn)
                {
                    pawn.stances.stunner.StunFor(GetDurationSeconds(pawn).SecondsToTicks(), parent.pawn, addBattleLog: false);
                }
            }
        }
    }


    internal class Projectile_Main
    {
    }
}
