using PTSC.Nameservice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTSC.Interfaces
{
    public interface IModel : INotifyPropertyChanged
    {
        [JsonIgnore]
        EntityState State { get; }
        void ResetState();
    }
}
