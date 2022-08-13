using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace EckTechGames.CleanClothes
{
    /// <summary>
    /// Worker class that handles the clean clothes recipe
    /// </summary>
    public class CleanClothesWorker : RecipeWorker
    {
        /// <summary>
        /// Consume Ingredient gets called at the end of the process. When it does, we will
        /// consume the soap, but leave the apparel in-tact and clean it.
        /// </summary>
        /// <param name="ingredient">The ingredient we want to consume.</param>
        /// <param name="recipe">The recipe that was used</param>
        /// <param name="map">Reference to the map</param>
        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
        {
            // If the ingredient is a clothing item, clean it.
            Apparel clothingItem = ingredient as Apparel;
            if (clothingItem != null)
            {
                System.Reflection.FieldInfo wornByCorpseIntField = clothingItem.GetType().GetField("wornByCorpseInt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                wornByCorpseIntField.SetValue(clothingItem, false);

                // Show a messages saying the clothes are clean.
                string translatedMessage = TranslatorFormattedStringExtensions.Translate("ECK_CleanClothes_Cleaned", ingredient.Label);
                MoteMaker.ThrowText(ingredient.Position.ToVector3(), map, translatedMessage, 5f);
            }
            else
            {
                // ONLY consuming the soap, not consuming the apparel.
                base.ConsumeIngredient(ingredient, recipe, map);
            }
        }
    }
}
