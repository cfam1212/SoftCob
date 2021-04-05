namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    public class CatalogosDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        List<CatalogosDTO> _catalogo = new List<CatalogosDTO>();
        //List<ParametroDetalle> _detalle = new List<ParametroDetalle>();
        #endregion

        #region Procedimientos y Funciones
        public DataSet FunGetDepartamento()
        {
            List<SoftCob_DEPARTAMENTO> _depar = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _depar = _db.SoftCob_DEPARTAMENTO.Where(d => d.depa_estado == true).OrderBy(d => d.depa_descripcion).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Departamento--",
                Codigo = "0"
            });

            foreach (SoftCob_DEPARTAMENTO _dep in _depar)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _dep.depa_descripcion,
                    Codigo = _dep.DEPA_CODIGO.ToString()
                });
            }

            _dts = new FuncionesDAO().FunCambiarDataSet(_catalogo);
            return _dts;
        }
        public DataSet FunGetPerfil()
        {
            List<SoftCob_PERFIL> _perfil = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _perfil = _db.SoftCob_PERFIL.Where(p => p.perf_estado == true).OrderBy(p => p.perf_descripcion).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Perfil--",
                Codigo = "0"
            });

            foreach (SoftCob_PERFIL _per in _perfil)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _per.perf_descripcion,
                    Codigo = _per.PERF_CODIGO.ToString()
                });
            }

            _dts = new FuncionesDAO().FunCambiarDataSet(_catalogo);
            return _dts;
        }
        public DataSet FunGetParametroDetalle(string _parametro, string _descripcionini)
        {
            try
            {
                List<SoftCob_PARAMETRO_DETALLE> _pdetalle = null;

                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _pdetalle = _db.SoftCob_PARAMETRO_DETALLE.Where(pd => pd.pade_estado && pd.PARA_CODIGO == 
                    pd.SoftCob_PARAMETRO_CABECERA.PARA_CODIGO && pd.SoftCob_PARAMETRO_CABECERA.para_nombre == _parametro && 
                    pd.SoftCob_PARAMETRO_CABECERA.para_estado).OrderBy(pd => pd.pade_nombre).ToList();
                }

                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _descripcionini,
                    Codigo = "0"
                });

                foreach (SoftCob_PARAMETRO_DETALLE _pdet in _pdetalle)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _pdet.pade_nombre,
                        Codigo = _pdet.pade_valorI.ToString()
                    });
                }

                _dts = new FuncionesDAO().FunCambiarDataSet(_catalogo);
                return _dts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
