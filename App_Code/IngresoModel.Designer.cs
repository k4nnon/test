﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
#region Metadatos de relaciones en EDM

[assembly: EdmRelationshipAttribute("IngresoModel", "FK_Registros_Cliente", "Cliente", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(IngresoModel.Cliente), "Registros", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(IngresoModel.Registros), true)]
[assembly: EdmRelationshipAttribute("IngresoModel", "FK_Registros_Codigo", "Codigo", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(IngresoModel.Codigo), "Registros", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(IngresoModel.Registros), true)]
[assembly: EdmRelationshipAttribute("IngresoModel", "FK_Registros_Cursos", "Cursos", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(IngresoModel.Cursos), "Registros", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(IngresoModel.Registros), true)]

#endregion

namespace IngresoModel
{
    #region Contextos
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    public partial class IngresoEntitie : ObjectContext
    {
        #region Constructores
    
        /// <summary>
        /// Inicializa un nuevo objeto IngresoEntitie usando la cadena de conexión encontrada en la sección 'IngresoEntitie' del archivo de configuración de la aplicación.
        /// </summary>
        public IngresoEntitie() : base("name=IngresoEntitie", "IngresoEntitie")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto IngresoEntitie.
        /// </summary>
        public IngresoEntitie(string connectionString) : base(connectionString, "IngresoEntitie")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto IngresoEntitie.
        /// </summary>
        public IngresoEntitie(EntityConnection connection) : base(connection, "IngresoEntitie")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Métodos parciales
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Propiedades de ObjectSet
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<Cliente> Cliente
        {
            get
            {
                if ((_Cliente == null))
                {
                    _Cliente = base.CreateObjectSet<Cliente>("Cliente");
                }
                return _Cliente;
            }
        }
        private ObjectSet<Cliente> _Cliente;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<Codigo> Codigo
        {
            get
            {
                if ((_Codigo == null))
                {
                    _Codigo = base.CreateObjectSet<Codigo>("Codigo");
                }
                return _Codigo;
            }
        }
        private ObjectSet<Codigo> _Codigo;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<Cursos> Cursos
        {
            get
            {
                if ((_Cursos == null))
                {
                    _Cursos = base.CreateObjectSet<Cursos>("Cursos");
                }
                return _Cursos;
            }
        }
        private ObjectSet<Cursos> _Cursos;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<Registros> Registros
        {
            get
            {
                if ((_Registros == null))
                {
                    _Registros = base.CreateObjectSet<Registros>("Registros");
                }
                return _Registros;
            }
        }
        private ObjectSet<Registros> _Registros;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<sysdiagrams> sysdiagrams
        {
            get
            {
                if ((_sysdiagrams == null))
                {
                    _sysdiagrams = base.CreateObjectSet<sysdiagrams>("sysdiagrams");
                }
                return _sysdiagrams;
            }
        }
        private ObjectSet<sysdiagrams> _sysdiagrams;

        #endregion

        #region Métodos AddTo
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet Cliente. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToCliente(Cliente cliente)
        {
            base.AddObject("Cliente", cliente);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet Codigo. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToCodigo(Codigo codigo)
        {
            base.AddObject("Codigo", codigo);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet Cursos. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToCursos(Cursos cursos)
        {
            base.AddObject("Cursos", cursos);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet Registros. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToRegistros(Registros registros)
        {
            base.AddObject("Registros", registros);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet sysdiagrams. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddTosysdiagrams(sysdiagrams sysdiagrams)
        {
            base.AddObject("sysdiagrams", sysdiagrams);
        }

        #endregion

    }

    #endregion

    #region Entidades
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="IngresoModel", Name="Cliente")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Cliente : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto Cliente.
        /// </summary>
        /// <param name="rut">Valor inicial de la propiedad rut.</param>
        /// <param name="nombres">Valor inicial de la propiedad nombres.</param>
        /// <param name="apellidoP">Valor inicial de la propiedad apellidoP.</param>
        /// <param name="apellidoM">Valor inicial de la propiedad apellidoM.</param>
        public static Cliente CreateCliente(global::System.Int32 rut, global::System.String nombres, global::System.String apellidoP, global::System.String apellidoM)
        {
            Cliente cliente = new Cliente();
            cliente.rut = rut;
            cliente.nombres = nombres;
            cliente.apellidoP = apellidoP;
            cliente.apellidoM = apellidoM;
            return cliente;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 rut
        {
            get
            {
                return _rut;
            }
            set
            {
                if (_rut != value)
                {
                    OnrutChanging(value);
                    ReportPropertyChanging("rut");
                    _rut = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("rut");
                    OnrutChanged();
                }
            }
        }
        private global::System.Int32 _rut;
        partial void OnrutChanging(global::System.Int32 value);
        partial void OnrutChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String nombres
        {
            get
            {
                return _nombres;
            }
            set
            {
                OnnombresChanging(value);
                ReportPropertyChanging("nombres");
                _nombres = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("nombres");
                OnnombresChanged();
            }
        }
        private global::System.String _nombres;
        partial void OnnombresChanging(global::System.String value);
        partial void OnnombresChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String apellidoP
        {
            get
            {
                return _apellidoP;
            }
            set
            {
                OnapellidoPChanging(value);
                ReportPropertyChanging("apellidoP");
                _apellidoP = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("apellidoP");
                OnapellidoPChanged();
            }
        }
        private global::System.String _apellidoP;
        partial void OnapellidoPChanging(global::System.String value);
        partial void OnapellidoPChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String apellidoM
        {
            get
            {
                return _apellidoM;
            }
            set
            {
                OnapellidoMChanging(value);
                ReportPropertyChanging("apellidoM");
                _apellidoM = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("apellidoM");
                OnapellidoMChanged();
            }
        }
        private global::System.String _apellidoM;
        partial void OnapellidoMChanging(global::System.String value);
        partial void OnapellidoMChanged();

        #endregion

    
        #region Propiedades de navegación
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("IngresoModel", "FK_Registros_Cliente", "Registros")]
        public Registros Registros
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Registros>("IngresoModel.FK_Registros_Cliente", "Registros").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Registros>("IngresoModel.FK_Registros_Cliente", "Registros").Value = value;
            }
        }
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Registros> RegistrosReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Registros>("IngresoModel.FK_Registros_Cliente", "Registros");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Registros>("IngresoModel.FK_Registros_Cliente", "Registros", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="IngresoModel", Name="Codigo")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Codigo : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto Codigo.
        /// </summary>
        /// <param name="codigoA">Valor inicial de la propiedad codigoA.</param>
        /// <param name="rut">Valor inicial de la propiedad rut.</param>
        public static Codigo CreateCodigo(global::System.String codigoA, global::System.Int32 rut)
        {
            Codigo codigo = new Codigo();
            codigo.codigoA = codigoA;
            codigo.rut = rut;
            return codigo;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String codigoA
        {
            get
            {
                return _codigoA;
            }
            set
            {
                if (_codigoA != value)
                {
                    OncodigoAChanging(value);
                    ReportPropertyChanging("codigoA");
                    _codigoA = StructuralObject.SetValidValue(value, false);
                    ReportPropertyChanged("codigoA");
                    OncodigoAChanged();
                }
            }
        }
        private global::System.String _codigoA;
        partial void OncodigoAChanging(global::System.String value);
        partial void OncodigoAChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 rut
        {
            get
            {
                return _rut;
            }
            set
            {
                OnrutChanging(value);
                ReportPropertyChanging("rut");
                _rut = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("rut");
                OnrutChanged();
            }
        }
        private global::System.Int32 _rut;
        partial void OnrutChanging(global::System.Int32 value);
        partial void OnrutChanged();

        #endregion

    
        #region Propiedades de navegación
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("IngresoModel", "FK_Registros_Codigo", "Registros")]
        public EntityCollection<Registros> Registros
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Registros>("IngresoModel.FK_Registros_Codigo", "Registros");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Registros>("IngresoModel.FK_Registros_Codigo", "Registros", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="IngresoModel", Name="Cursos")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Cursos : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto Cursos.
        /// </summary>
        /// <param name="curso">Valor inicial de la propiedad curso.</param>
        public static Cursos CreateCursos(global::System.String curso)
        {
            Cursos cursos = new Cursos();
            cursos.curso = curso;
            return cursos;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String curso
        {
            get
            {
                return _curso;
            }
            set
            {
                if (_curso != value)
                {
                    OncursoChanging(value);
                    ReportPropertyChanging("curso");
                    _curso = StructuralObject.SetValidValue(value, false);
                    ReportPropertyChanged("curso");
                    OncursoChanged();
                }
            }
        }
        private global::System.String _curso;
        partial void OncursoChanging(global::System.String value);
        partial void OncursoChanged();

        #endregion

    
        #region Propiedades de navegación
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("IngresoModel", "FK_Registros_Cursos", "Registros")]
        public EntityCollection<Registros> Registros
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Registros>("IngresoModel.FK_Registros_Cursos", "Registros");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Registros>("IngresoModel.FK_Registros_Cursos", "Registros", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="IngresoModel", Name="Registros")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Registros : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto Registros.
        /// </summary>
        /// <param name="rut">Valor inicial de la propiedad rut.</param>
        /// <param name="curso">Valor inicial de la propiedad curso.</param>
        /// <param name="fechaCurso">Valor inicial de la propiedad fechaCurso.</param>
        /// <param name="fechaIngreso">Valor inicial de la propiedad fechaIngreso.</param>
        /// <param name="estado">Valor inicial de la propiedad estado.</param>
        public static Registros CreateRegistros(global::System.Int32 rut, global::System.String curso, global::System.String fechaCurso, global::System.String fechaIngreso, global::System.String estado)
        {
            Registros registros = new Registros();
            registros.rut = rut;
            registros.curso = curso;
            registros.fechaCurso = fechaCurso;
            registros.fechaIngreso = fechaIngreso;
            registros.estado = estado;
            return registros;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 rut
        {
            get
            {
                return _rut;
            }
            set
            {
                if (_rut != value)
                {
                    OnrutChanging(value);
                    ReportPropertyChanging("rut");
                    _rut = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("rut");
                    OnrutChanged();
                }
            }
        }
        private global::System.Int32 _rut;
        partial void OnrutChanging(global::System.Int32 value);
        partial void OnrutChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String curso
        {
            get
            {
                return _curso;
            }
            set
            {
                OncursoChanging(value);
                ReportPropertyChanging("curso");
                _curso = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("curso");
                OncursoChanged();
            }
        }
        private global::System.String _curso;
        partial void OncursoChanging(global::System.String value);
        partial void OncursoChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String fechaCurso
        {
            get
            {
                return _fechaCurso;
            }
            set
            {
                OnfechaCursoChanging(value);
                ReportPropertyChanging("fechaCurso");
                _fechaCurso = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("fechaCurso");
                OnfechaCursoChanged();
            }
        }
        private global::System.String _fechaCurso;
        partial void OnfechaCursoChanging(global::System.String value);
        partial void OnfechaCursoChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String fechaIngreso
        {
            get
            {
                return _fechaIngreso;
            }
            set
            {
                OnfechaIngresoChanging(value);
                ReportPropertyChanging("fechaIngreso");
                _fechaIngreso = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("fechaIngreso");
                OnfechaIngresoChanged();
            }
        }
        private global::System.String _fechaIngreso;
        partial void OnfechaIngresoChanging(global::System.String value);
        partial void OnfechaIngresoChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String codigoA
        {
            get
            {
                return _codigoA;
            }
            set
            {
                OncodigoAChanging(value);
                ReportPropertyChanging("codigoA");
                _codigoA = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("codigoA");
                OncodigoAChanged();
            }
        }
        private global::System.String _codigoA;
        partial void OncodigoAChanging(global::System.String value);
        partial void OncodigoAChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String estado
        {
            get
            {
                return _estado;
            }
            set
            {
                OnestadoChanging(value);
                ReportPropertyChanging("estado");
                _estado = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("estado");
                OnestadoChanged();
            }
        }
        private global::System.String _estado;
        partial void OnestadoChanging(global::System.String value);
        partial void OnestadoChanged();

        #endregion

    
        #region Propiedades de navegación
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("IngresoModel", "FK_Registros_Cliente", "Cliente")]
        public Cliente Cliente
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Cliente>("IngresoModel.FK_Registros_Cliente", "Cliente").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Cliente>("IngresoModel.FK_Registros_Cliente", "Cliente").Value = value;
            }
        }
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Cliente> ClienteReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Cliente>("IngresoModel.FK_Registros_Cliente", "Cliente");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Cliente>("IngresoModel.FK_Registros_Cliente", "Cliente", value);
                }
            }
        }
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("IngresoModel", "FK_Registros_Codigo", "Codigo")]
        public Codigo Codigo
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Codigo>("IngresoModel.FK_Registros_Codigo", "Codigo").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Codigo>("IngresoModel.FK_Registros_Codigo", "Codigo").Value = value;
            }
        }
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Codigo> CodigoReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Codigo>("IngresoModel.FK_Registros_Codigo", "Codigo");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Codigo>("IngresoModel.FK_Registros_Codigo", "Codigo", value);
                }
            }
        }
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("IngresoModel", "FK_Registros_Cursos", "Cursos")]
        public Cursos Cursos
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Cursos>("IngresoModel.FK_Registros_Cursos", "Cursos").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Cursos>("IngresoModel.FK_Registros_Cursos", "Cursos").Value = value;
            }
        }
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Cursos> CursosReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Cursos>("IngresoModel.FK_Registros_Cursos", "Cursos");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Cursos>("IngresoModel.FK_Registros_Cursos", "Cursos", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="IngresoModel", Name="sysdiagrams")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class sysdiagrams : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto sysdiagrams.
        /// </summary>
        /// <param name="name">Valor inicial de la propiedad name.</param>
        /// <param name="principal_id">Valor inicial de la propiedad principal_id.</param>
        /// <param name="diagram_id">Valor inicial de la propiedad diagram_id.</param>
        public static sysdiagrams Createsysdiagrams(global::System.String name, global::System.Int32 principal_id, global::System.Int32 diagram_id)
        {
            sysdiagrams sysdiagrams = new sysdiagrams();
            sysdiagrams.name = name;
            sysdiagrams.principal_id = principal_id;
            sysdiagrams.diagram_id = diagram_id;
            return sysdiagrams;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String name
        {
            get
            {
                return _name;
            }
            set
            {
                OnnameChanging(value);
                ReportPropertyChanging("name");
                _name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("name");
                OnnameChanged();
            }
        }
        private global::System.String _name;
        partial void OnnameChanging(global::System.String value);
        partial void OnnameChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 principal_id
        {
            get
            {
                return _principal_id;
            }
            set
            {
                Onprincipal_idChanging(value);
                ReportPropertyChanging("principal_id");
                _principal_id = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("principal_id");
                Onprincipal_idChanged();
            }
        }
        private global::System.Int32 _principal_id;
        partial void Onprincipal_idChanging(global::System.Int32 value);
        partial void Onprincipal_idChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 diagram_id
        {
            get
            {
                return _diagram_id;
            }
            set
            {
                if (_diagram_id != value)
                {
                    Ondiagram_idChanging(value);
                    ReportPropertyChanging("diagram_id");
                    _diagram_id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("diagram_id");
                    Ondiagram_idChanged();
                }
            }
        }
        private global::System.Int32 _diagram_id;
        partial void Ondiagram_idChanging(global::System.Int32 value);
        partial void Ondiagram_idChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> version
        {
            get
            {
                return _version;
            }
            set
            {
                OnversionChanging(value);
                ReportPropertyChanging("version");
                _version = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("version");
                OnversionChanged();
            }
        }
        private Nullable<global::System.Int32> _version;
        partial void OnversionChanging(Nullable<global::System.Int32> value);
        partial void OnversionChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.Byte[] definition
        {
            get
            {
                return StructuralObject.GetValidValue(_definition);
            }
            set
            {
                OndefinitionChanging(value);
                ReportPropertyChanging("definition");
                _definition = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("definition");
                OndefinitionChanged();
            }
        }
        private global::System.Byte[] _definition;
        partial void OndefinitionChanging(global::System.Byte[] value);
        partial void OndefinitionChanged();

        #endregion

    
    }

    #endregion

    
}
