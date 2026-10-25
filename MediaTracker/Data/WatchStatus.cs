using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTracker.Data
{
    public enum WatchStatus
    {
        PlanToWatch,
        CurrentlyWatching,
        Completed,
        OnHold,
        Dropped,
        Rewatching
    }
}
