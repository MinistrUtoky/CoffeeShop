using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace productRelated {
    [Serializable] public struct Subscription
    {
        public string userLogin;
        public string subscriptionStart;
        public string subscriptionEnd;
        public int productID;
        public int productAmount;
    }
}
