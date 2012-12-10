﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace ChqPrint
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class ChqDatabase1Entities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new ChqDatabase1Entities object using the connection string found in the 'ChqDatabase1Entities' section of the application configuration file.
        /// </summary>
        public ChqDatabase1Entities() : base("name=ChqDatabase1Entities", "ChqDatabase1Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new ChqDatabase1Entities object.
        /// </summary>
        public ChqDatabase1Entities(string connectionString) : base(connectionString, "ChqDatabase1Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new ChqDatabase1Entities object.
        /// </summary>
        public ChqDatabase1Entities(EntityConnection connection) : base(connection, "ChqDatabase1Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Cheques> Cheques
        {
            get
            {
                if ((_Cheques == null))
                {
                    _Cheques = base.CreateObjectSet<Cheques>("Cheques");
                }
                return _Cheques;
            }
        }
        private ObjectSet<Cheques> _Cheques;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Formatos> Formatos
        {
            get
            {
                if ((_Formatos == null))
                {
                    _Formatos = base.CreateObjectSet<Formatos>("Formatos");
                }
                return _Formatos;
            }
        }
        private ObjectSet<Formatos> _Formatos;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Cheques EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCheques(Cheques cheques)
        {
            base.AddObject("Cheques", cheques);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Formatos EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToFormatos(Formatos formatos)
        {
            base.AddObject("Formatos", formatos);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ChqDatabase1Model", Name="Cheques")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Cheques : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Cheques object.
        /// </summary>
        /// <param name="idCheque">Initial value of the idCheque property.</param>
        /// <param name="nroCheque">Initial value of the nroCheque property.</param>
        public static Cheques CreateCheques(global::System.Int32 idCheque, global::System.String nroCheque)
        {
            Cheques cheques = new Cheques();
            cheques.idCheque = idCheque;
            cheques.nroCheque = nroCheque;
            return cheques;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 idCheque
        {
            get
            {
                return _idCheque;
            }
            set
            {
                if (_idCheque != value)
                {
                    OnidChequeChanging(value);
                    ReportPropertyChanging("idCheque");
                    _idCheque = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("idCheque");
                    OnidChequeChanged();
                }
            }
        }
        private global::System.Int32 _idCheque;
        partial void OnidChequeChanging(global::System.Int32 value);
        partial void OnidChequeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String nroCheque
        {
            get
            {
                return _nroCheque;
            }
            set
            {
                OnnroChequeChanging(value);
                ReportPropertyChanging("nroCheque");
                _nroCheque = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("nroCheque");
                OnnroChequeChanged();
            }
        }
        private global::System.String _nroCheque;
        partial void OnnroChequeChanging(global::System.String value);
        partial void OnnroChequeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> Fecha
        {
            get
            {
                return _Fecha;
            }
            set
            {
                OnFechaChanging(value);
                ReportPropertyChanging("Fecha");
                _Fecha = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Fecha");
                OnFechaChanged();
            }
        }
        private Nullable<global::System.DateTime> _Fecha;
        partial void OnFechaChanging(Nullable<global::System.DateTime> value);
        partial void OnFechaChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Monto
        {
            get
            {
                return _Monto;
            }
            set
            {
                OnMontoChanging(value);
                ReportPropertyChanging("Monto");
                _Monto = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Monto");
                OnMontoChanged();
            }
        }
        private Nullable<global::System.Int32> _Monto;
        partial void OnMontoChanging(Nullable<global::System.Int32> value);
        partial void OnMontoChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String PagueseOrdenDe
        {
            get
            {
                return _PagueseOrdenDe;
            }
            set
            {
                OnPagueseOrdenDeChanging(value);
                ReportPropertyChanging("PagueseOrdenDe");
                _PagueseOrdenDe = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("PagueseOrdenDe");
                OnPagueseOrdenDeChanged();
            }
        }
        private global::System.String _PagueseOrdenDe;
        partial void OnPagueseOrdenDeChanging(global::System.String value);
        partial void OnPagueseOrdenDeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String MontoEnLetras
        {
            get
            {
                return _MontoEnLetras;
            }
            set
            {
                OnMontoEnLetrasChanging(value);
                ReportPropertyChanging("MontoEnLetras");
                _MontoEnLetras = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("MontoEnLetras");
                OnMontoEnLetrasChanged();
            }
        }
        private global::System.String _MontoEnLetras;
        partial void OnMontoEnLetrasChanging(global::System.String value);
        partial void OnMontoEnLetrasChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> Anulado
        {
            get
            {
                return _Anulado;
            }
            set
            {
                OnAnuladoChanging(value);
                ReportPropertyChanging("Anulado");
                _Anulado = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Anulado");
                OnAnuladoChanged();
            }
        }
        private Nullable<global::System.Boolean> _Anulado;
        partial void OnAnuladoChanging(Nullable<global::System.Boolean> value);
        partial void OnAnuladoChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ChqDatabase1Model", Name="Formatos")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Formatos : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Formatos object.
        /// </summary>
        /// <param name="idFormato">Initial value of the idFormato property.</param>
        public static Formatos CreateFormatos(global::System.Int32 idFormato)
        {
            Formatos formatos = new Formatos();
            formatos.idFormato = idFormato;
            return formatos;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 idFormato
        {
            get
            {
                return _idFormato;
            }
            set
            {
                if (_idFormato != value)
                {
                    OnidFormatoChanging(value);
                    ReportPropertyChanging("idFormato");
                    _idFormato = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("idFormato");
                    OnidFormatoChanged();
                }
            }
        }
        private global::System.Int32 _idFormato;
        partial void OnidFormatoChanging(global::System.Int32 value);
        partial void OnidFormatoChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Path
        {
            get
            {
                return _Path;
            }
            set
            {
                OnPathChanging(value);
                ReportPropertyChanging("Path");
                _Path = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Path");
                OnPathChanged();
            }
        }
        private global::System.String _Path;
        partial void OnPathChanging(global::System.String value);
        partial void OnPathChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Descripcion
        {
            get
            {
                return _Descripcion;
            }
            set
            {
                OnDescripcionChanging(value);
                ReportPropertyChanging("Descripcion");
                _Descripcion = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Descripcion");
                OnDescripcionChanged();
            }
        }
        private global::System.String _Descripcion;
        partial void OnDescripcionChanging(global::System.String value);
        partial void OnDescripcionChanged();

        #endregion
    
    }

    #endregion
    
}
