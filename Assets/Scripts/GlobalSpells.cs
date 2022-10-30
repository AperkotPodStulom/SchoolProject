using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalThings
{
    public enum Spells
    {
        Dragging
    }

    public class GlobalSpells
    {
        //variables
        private static Spells globalSpellState = new Spells();
        
        //access to variables
        public static Spells GlobalSpellState
        {
            get { return GlobalSpells.globalSpellState; }
            set { GlobalSpells.globalSpellState = value; }
        }
    }
}