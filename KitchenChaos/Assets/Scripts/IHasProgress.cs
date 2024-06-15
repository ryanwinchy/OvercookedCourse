using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;                    //Event declaration. Implementing interfaces fire this.
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;                //as progress bars use float not int. So convert.
    }

}
