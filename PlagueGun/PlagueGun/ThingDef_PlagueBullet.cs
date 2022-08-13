using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;

namespace EckTechGames.PlagueGun
{
    /// <summary>
    /// Plague Bullet stores the data for the infection chance and disease to add.
    /// 
    /// This should probable me named "DiseaseBullet" since we could change which 
    /// Hediff gets applied by chainging HediffToAdd in the XML. I kept it as
    /// PlagueBullet for people following along in the video tutorial.
    /// </summary>
    public class ThingDef_PlagueBullet : ThingDef
    {
        /// <summary>
        /// This field controls the chance that the Hediff will be applied.
        /// 
        /// 0.5f is the default chance of adding the hediff if nothing is specified in the XML. 
        /// If it IS specified in the XML, we will be using that value instead.
        /// </summary>
        public float AddHediffChance = 0.5f; 

        /// <summary>
        /// This hediff will be the health differential we apply to the pawn when 
        /// the random chance successfully infects the target.
        /// </summary>
        public HediffDef HediffToAdd; // Not putting a default here. 


        // Note, we don't have to define a field for the "thingClass" in the xml data: 
        //     <thingClass>EckTechGames.PlagueGun.Projectile_PlagueBullet</thingClass>
        // This is because thingClass is defined in our parent class: ThingDef and we inherit those fields.
        // Our ThingDef_PlagueBullet inherits from ThingDef on line 15 above.


        // We no longer need this ResolveReferences code since we aren't hardcoding the HediffToAdd above. 
        // I left this code here for those following along in the video tutorial
        //public override void ResolveReferences()
        //{
        //    base.ResolveReferences();
        //    HediffToAdd = HediffDefOf.Plague; // Instead of hardcoding this to plague going to drive it solely from the data.
        //}
    }
}
