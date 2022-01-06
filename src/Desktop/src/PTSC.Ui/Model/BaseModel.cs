using MvvmGen;
using PTSC.Interfaces;
using PTSC.Nameservice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTSC.Ui.Model
{

    [ViewModel]
    public abstract partial class BaseModel : IModel
    {


        public delegate void OnChangedHandler(string propertyName);
        public event OnChangedHandler OnChanged;

        public EntityState State { get; protected set; }

        [JsonIgnore]
        public Guid Id { get; protected set; }

        partial void OnInitialize()
        {
            Id = Guid.NewGuid();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if(State == EntityState.None)
                State = EntityState.Changed;

            base.OnPropertyChanged(e);
            OnChanged?.Invoke(e.PropertyName);
        }

        public virtual void ResetState()
        {
            State = EntityState.None;
        }
    }
}
