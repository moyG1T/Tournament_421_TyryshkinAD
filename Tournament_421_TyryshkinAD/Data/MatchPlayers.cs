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
    
    public partial class MatchPlayers
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public Nullable<int> Rating { get; set; }
    
        public virtual Match Match { get; set; }
        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
    }
}
