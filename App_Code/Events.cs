using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// List of various events in pshr.mast_event
/// </summary>
public abstract class EVENTS
{
    public enum POTR
    {
        CTRP = 36,      //Transfer - Public  interest 
        CPRO = 28,      //Promotion
    };
    public enum RETD
    {
        RMIN = 11,      //Retirement-Medical Invalidation 
        RSUP = 12,      //Retirement-Superannuation 
        RVOR = 13,      //Retirement-Voluntary Retirement 
        RPRB = 14,      //Premature Retirement by Board 
        REGN = 15,      //Resignation 
        REXP = 16,      //Expired 
        RMIS = 89,      //MISSING 
    };
    public enum LEAVE
    {
        LELS = 2,       //Proceding on Sanctioned Earned Leave
        LMTL = 3,       //Proceding on Sanctioned Maternity leave
        LJON = 10,      //Ending of any kind of  leave
        LJONRC = 72,    //ENDING OF ANY KIND OF LEAVE DUE TO RECALL
        LJONTR = 73,    //ENDING OF ANY KIND OF LEAVE DUE TO TRANSFER
        LJONPR = 74,    //ENDING OF ANY KIND OF LEAVE DUE TO PROMOTION
        LJONSP = 75,    //ENDING OF ANY KIND OF LEAVE DUE TO SUSPENSION
        LJONRT = 76,    //ENDING OF ANY KIND OF LEAVE DUE TO RETIREMENT
        LJONEX = 77,    //ENDING OF ANY KIND OF LEAVE DUE TO EXPIRY
        JELWS = 87,     //JOINING AFTER AVAILING LEAVE WITH SUBSTITUTE
    }
}