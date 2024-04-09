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
        public static bool operator ==(Subscription a, Subscription b)
        {
            return a.userLogin==b.userLogin
                & a.subscriptionStart==b.subscriptionStart
                & a.subscriptionEnd==b.subscriptionEnd
                & a.productID==b.productID
                & a.productAmount==b.productAmount;
        }
        public static bool operator !=(Subscription a, Subscription b)
        {
            return !(a == b);
        }
    }
    
}
