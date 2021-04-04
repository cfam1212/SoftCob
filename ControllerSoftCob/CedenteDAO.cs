namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    public class CedenteDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SoftCobEntities _dtb = new SoftCobEntities();
        string _mensaje = "";

        #endregion

        #region Procedimientos y Funciones Arbol
        public DataSet FunGetCiuadesCedentes()
        {
            try
            {
                var query = (from Cedente in _dtb.SoftCob_CEDENTE
                             join Ciudad in _dtb.SoftCob_CIUDAD on Cedente.cede_ciudcod equals Ciudad.CIUD_CODIGO
                             where Cedente.cede_estado == true
                             select new CatalogosDTO
                             {
                                 Descripcion = Ciudad.ciud_nombre,
                                 Codigo = Ciudad.CIUD_CODIGO.ToString()
                             }).Distinct();

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public DataSet FunGetCedentesporIDCiudad(int _idciudad)
        {
            try
            {
                var query = from Cedente in _dtb.SoftCob_CEDENTE
                            where Cedente.cede_ciudcod.Equals(_idciudad) && Cedente.cede_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = Cedente.cede_nombre,
                                Codigo = Cedente.CEDE_CODIGO.ToString()
                            };
                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetProductosporIDCedente(int _idcedente)
        {
            try
            {
                var query = from ProductoCedente in _dtb.SoftCob_PRODUCTOS_CEDENTE
                            where ProductoCedente.CEDE_CODIGO.Equals(_idcedente) && ProductoCedente.prce_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = ProductoCedente.prce_producto,
                                Codigo = ProductoCedente.PRCE_CODIGO.ToString()
                            };
                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetCatalogoProductosporIDProducto(int _idproducto)
        {
            try
            {
                var query = from CatalogoProducto in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            where CatalogoProducto.PRCE_CODIGO.Equals(_idproducto) && CatalogoProducto.cpce_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = CatalogoProducto.cpce_producto,
                                Codigo = CatalogoProducto.CPCE_CODIGO.ToString()
                            };
                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SoftCob_CEDENTE FunGetCedentePorID(int _codcedente)
        {
            SoftCob_CEDENTE _cedente = new SoftCob_CEDENTE();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _cedente = _db.SoftCob_CEDENTE.Where(e => e.CEDE_CODIGO == _codcedente).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _cedente;
        }
        public string FunGetNameCatalogoporID(int _codigocatalogo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _mensaje = _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE.Where(c => c.CPCE_CODIGO == _codigocatalogo).FirstOrDefault().cpce_producto;
                }
            }
            catch
            {
                _mensaje = "";
            }
            return _mensaje;
        }
        public DataSet FunGetCatalogoPorCiudad(int _codigociudad)
        {
            try
            {
                var query = from CTP in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            join PRC in _dtb.SoftCob_PRODUCTOS_CEDENTE on CTP.PRCE_CODIGO equals PRC.PRCE_CODIGO
                            join CED in _dtb.SoftCob_CEDENTE on PRC.CEDE_CODIGO equals CED.CEDE_CODIGO
                            where CED.cede_ciudcod.Equals(_codigociudad) && CTP.cpce_estado == true && PRC.prce_estado == true
                            && CED.cede_estado == true
                            orderby CTP.cpce_producto
                            select new CatalogosDTO
                            {
                                Descripcion = CTP.cpce_producto,
                                Codigo = CTP.CPCE_CODIGO.ToString(),
                                Nivel = CED.cede_auxi1
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
