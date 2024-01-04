using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Verse;
using UnityEngine;
using Verse.AI;
using System.Runtime;

namespace Amazon
{

    public class Amazon : GameComponent
    {
        // The constructor is not explicitly defined here to use the parameterless one
        public static bool bool_Debug = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_Debug;
        public static float AM_IntervalHours = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_IntervalHours;
        public static float AM_MaleThresholdx = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_MaleThresholdx;
        public static float AM_RelationIntx = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_RelationIntx;
        public static float AM_RelationDecx = LoadedModManager.GetMod<AmazonMod>().GetSettings<ExampleSettings>().AM_RelationDecx;

        public Amazon(Game game) : base()
        {
            // Initialization logic, if needed
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            // Initialization logic here
        }

        private int countHours = 1;
        private float lastHour = -1;
        //Messages.Message("TestMod counter reset.", MessageTypeDefOf.PositiveEvent);
        public override void GameComponentTick()
        {
            base.GameComponentTick();

            // Get the current in-game hour
            int currentHour = Find.TickManager.TicksGame / 2500;

            // Calculate the time in hours
            float currentTimeHours = currentHour;

            // Check if the interval has passed
            if (currentTimeHours >= lastHour + AM_IntervalHours)
            {
                lastHour = currentTimeHours;
                CountColonists();
            }
        }


        private void CountColonists()
        {
            int maleCount = 0;
            int femaleCount = 0;

            // Iterate through all pawns in the colony
            foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
            {
                // Check if the pawn is a colonist
                if (pawn.IsColonist)
                {
                    // Check the gender of the colonist
                    if (pawn.gender == Gender.Male)
                    {
                        if (pawn?.IsPrisoner == false)
                        {
                            maleCount++;
                        }
                    }
                    else if (pawn.gender == Gender.Female)
                    {
                        femaleCount++;
                    }
                }
            }

            if (maleCount > 0 || femaleCount > 0)
            {
              
                if (femaleCount >= 1 && maleCount < AM_MaleThresholdx) { SetAmazonFactionRelations(false); }
                if (maleCount >= AM_MaleThresholdx) { SetAmazonFactionRelations(true); }
                // Log the counts
                if (bool_Debug == true)
                {
                    //Log.Message($"Number of male colonists: {maleCount}");
                    //Log.Message($"Number of female colonists: {femaleCount}");
                }
            }
        }

        public void SetAmazonFactionRelations(bool bool_hostile = true)
        {
            FactionDef amazonFactionDef = DefDatabase<FactionDef>.GetNamed("Amazon");
            Faction amazonFaction = Find.FactionManager.AllFactions.FirstOrDefault(f => f.def == amazonFactionDef);

            if (bool_Debug) { Log.Message("Attempting to set relations"); }

            // Check if the faction exists and is not the player's faction
            if (amazonFaction != null && amazonFaction != Faction.OfPlayer)
            {
                int goodwillAdjustment = bool_hostile ? (int)AM_RelationDecx : (int)AM_RelationIntx;
                amazonFaction.TryAffectGoodwillWith(Faction.OfPlayer, goodwillAdjustment);
                if (bool_Debug) { Log.Message($"Adjusting relations by {goodwillAdjustment}"); }
            }
        }
    }

    public class ExampleSettings : ModSettings
    {
        /// <summary>
        /// The three settings our mod has.
        /// </summary>
        public bool AM_AlterAge = false;
        public bool AM_NeuroToxin = true;
        public bool AM_NegativeMood = true;
        public bool AM_IdeoDoubt = false;
        public bool AM_Debug = false;
        public float AM_MaleThresholdx = 2f;
        public float AM_IntervalHours = 4f;
        public float AM_RelationIntx = 1f;
        public float AM_RelationDecx = -5f;

        public float exampleFloat = 200f;
        public List<Pawn> exampleListOfPawns = new List<Pawn>();

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref AM_AlterAge, "AM_AlterAge", false);
            Scribe_Values.Look(ref AM_NeuroToxin, "AM_NeuroToxin", false);
            Scribe_Values.Look(ref AM_NegativeMood, "AM_NegativeMood", false);
            Scribe_Values.Look(ref AM_MaleThresholdx, "AM_MaleThresholdx", 2f);
            Scribe_Values.Look(ref AM_IntervalHours, "AM_IntervalHours", 4f);
            Scribe_Values.Look(ref AM_RelationIntx, "AM_RelationIntx", 1f);
            Scribe_Values.Look(ref AM_RelationDecx, "AM_RelationDecx", -5f);
            Scribe_Values.Look(ref AM_Debug, "AM_Debug", false);
        }
    }

    public class AmazonMod : Mod
    {
        /// <summary>
        /// A reference to our settings.
        /// </summary>
        ExampleSettings settings;
        

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public AmazonMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<ExampleSettings>();
        }

        //listingStandard.Label($"Increase Relations By: {newValue:F1}");
        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Do Amazon weapons alter the targets age?", ref settings.AM_AlterAge, "By default, the Amazon weapons increase the age of any male pawn which they hit.");
            listingStandard.CheckboxLabeled("Do Amazon weapons inflict a neuro toxin?", ref settings.AM_NeuroToxin, "By default, the Amazon weapons apply a neuro toxin on targetted male pawns.");
            listingStandard.CheckboxLabeled("Do Amazon weapons cause mood debuffs?", ref settings.AM_NegativeMood, "By default, the Amazon any male pawn who is hit by an Amazonian weapon incurs a mood debuff temporarily.");
            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Label("<b><color=gray>Boundaries</color></b>",-1f, "How many male colonists are permitted, restart the game after altering");

            float malethresholdval = listingStandard.Slider(settings.AM_MaleThresholdx, 1f, 10f);
            listingStandard.Label($"<b><color=orange>Male colonist amount: {Mathf.Round(malethresholdval)}</color></b>", -1f, "Minimum number of males tolerated, default is 2, changing requires a game restart");
            settings.AM_MaleThresholdx = malethresholdval;

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Label("<b><color=orange>Interval(s)</color></b>", -1f, null);
            float intervalHoursVal = listingStandard.Slider(settings.AM_IntervalHours, 1f, 72f);
            settings.AM_IntervalHours = Mathf.Round(intervalHoursVal);
            listingStandard.Label($"<b>Interval: {Mathf.Round(intervalHoursVal)} hour(s)</b>", -1f, "Interval in hours before checking colonist composition, changing requires a game restart");
            listingStandard.Gap();
            listingStandard.Gap();

            listingStandard.Label("<b><color=orange>Relations</color></b>", -1f, null);
            float newValue = listingStandard.Slider(settings.AM_RelationIntx, 1f, 10f);
            settings.AM_RelationIntx = newValue;
            listingStandard.Label($"<b>Increase Relations By: {Mathf.Round(newValue)}</b>", -1f, "Increase the relations each interval if the colony contains mostly female colonists, changing requires a game restart");
            listingStandard.Gap();

            float newDValue = listingStandard.Slider(settings.AM_RelationDecx, -20f, -1f);
            settings.AM_RelationDecx = newDValue;
            listingStandard.Label($"<b>Decrease Relations By: {Mathf.Round(newDValue)}</b>", -1f, "Decrease the relation by x if the colony contains x number of male colonists, changing requires a game restart");
            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Label("<b><color=orange>Other settings</color></b>", -1f, "Other settings and debug output");
            listingStandard.CheckboxLabeled("Enable Debug Mode?", ref settings.AM_Debug, "Is Debug mode enabled?");
            //listingStandard.CheckboxLabeled("Do Amazon weapons increase doubt in the targets idealogy?", ref settings.AM_IdeoDoubt, "Do Amazon weapons increase doubt in whatever target pawn that they hit? (Experimental).");
            
            // settings.Write();
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localisation.
        /// </summary>
        /// <returns>The (translated) mod name.</returns>
        public override string SettingsCategory()
        {
            return "AM_AmazonMod".Translate();
        }
    }



    public class CompTargetEffect_PsychicMindbreak : CompTargetEffect
    {
        public override void DoEffectOn(Pawn user, Thing target)
        {
            Pawn pawn = (Pawn)target;
            if (pawn.Dead)
            {
                return;
            }
            //Log.Message("CompTargetEffect_PsychicMindbreak called");
        }
    }

    public class CompAbilityEffect_Paralyze : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            //Log.Message("CompAbilityEffect_Paralyze.Apply() called");
            if (target.Thing is Pawn pawn)
            {
                //Log.Message("CompAbilityEffect_Paralyze.Apply(): target is a pawn");
                // ...
            }
            else
            {
                //Log.Message("CompAbilityEffect_Paralyze.Apply(): target is not a pawn");
            }
        }

    }

    public class CompAbilityEffect_PsychicRage : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Pawn != null && !target.Pawn.Dead && !target.Pawn.Downed)
            {
                target.Pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk);
                //Messages.Message("PsychicRage power used on " + target.Pawn.Label, MessageTypeDefOf.PositiveEvent);
            }
            else
            {
                //Messages.Message("Invalid target for PsychicRage power", MessageTypeDefOf.RejectInput);
            }
            return;
        }
    }

}
