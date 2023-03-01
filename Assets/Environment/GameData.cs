using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo {

    [System.Serializable]
    public class GameData{
        public int goldAmount = 0;

        public bool item1 = false;

        public int gameProgress = 0;
    }

    public class Item {
        public int goldAmount;

        public bool item1;
    }
}
