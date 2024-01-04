using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Amazon;

 namespace Amazon
{
    public class ThingDef_AmazonArrow : ThingDef
    {
        public float AddHediffChance = 0.9f;
    }

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

    }
}
