using ModdingUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnboundLib.Extensions;

namespace CardShopMod.Extentions
{
    [Serializable]
    public class CharacterStatsModifiersAdditionalData
    {
        public int coins;
    }

    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data = new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();


        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers statModifiers)
        {
            return data.GetOrCreateValue(statModifiers);
        }

        public static void AddData(this CharacterStatModifiers statModifiers, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(statModifiers, value);
            } 
            catch (Exception) { }
        }
    }
}