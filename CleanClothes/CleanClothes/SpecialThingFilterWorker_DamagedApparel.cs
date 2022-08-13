using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace EckTechGames.CleanClothes
{
    public class SpecialThingFilterWorker_DamagedApparel : SpecialThingFilterWorker
	{
		public override bool Matches(Thing t)
		{
			if (t is Apparel apparel)
			{
				return apparel.HitPoints < apparel.MaxHitPoints;
			}
			return false;
		}

		public override bool CanEverMatch(ThingDef def)
		{
			if (def.IsApparel)
			{
				return true;
			}
			return false;

		}
	}
}

