using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace EckTechGames.CleanClothes
{
    public class RepairApparelWorker : RecipeWorker
    {
        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
        {
            // Intentionally NOT calling the base class so the ingredient doesn't get consumed
            // base.ConsumeIngredient(ingredient, recipe, map);

            RepairApparelRecipeDef repairApparelRecipeDef = recipe as RepairApparelRecipeDef;

            // Get a random amount of health restored
            float repairPercent = Rand.Range(repairApparelRecipeDef.minRepairPercent, repairApparelRecipeDef.maxRepairPercent);
            int restoredHP = (int)Math.Ceiling(ingredient.MaxHitPoints * repairPercent);

           
            ingredient.HitPoints = Math.Min(ingredient.HitPoints + restoredHP, ingredient.MaxHitPoints);


            string translatedMessage;
            if(ingredient.HitPoints < ingredient.MaxHitPoints)
                translatedMessage = TranslatorFormattedStringExtensions.Translate("ECK_CleanClothes_RepairApparel", ingredient.Label, repairPercent.ToString("P"));
            else
                translatedMessage = TranslatorFormattedStringExtensions.Translate("ECK_CleanClothes_FullyRepaired", ingredient.Label);

            MoteMaker.ThrowText(ingredient.Position.ToVector3(), map, translatedMessage, 5f);

            //Apparel clothingItem = ingredient as Apparel;

            //System.Reflection.FieldInfo wornByCorpseIntField = clothingItem.GetType().GetField("wornByCorpseInt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //wornByCorpseIntField.SetValue(clothingItem, false);
        }
    }
}
