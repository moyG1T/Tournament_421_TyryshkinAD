//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tournament_421_TyryshkinAD.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class MatchViewed
    {
        public int Id { get; set; }
        public int ViewerId { get; set; }
        public int MatchId { get; set; }
        public System.DateTime Timestamp { get; set; }
    
        public virtual Match Match { get; set; }
        public virtual Viewer Viewer { get; set; }
    }
}
